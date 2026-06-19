# Dual-DB Event-Driven Sync Sandbox (SQL Server & Elasticsearch)

This repository is a comprehensive, educational sandbox designed to demonstrate **Domain-Driven Design (DDD)**, the **CQRS pattern** split across two isolated database engines, and an asynchronous data synchronization mechanism powered by the **Outbox Pattern** and **MediatR In-Memory Pub/Sub**.

---

## 🚀 Purpose of the Project

The primary goal of this test project is to gain hands-on experience in:
* Designing rich domain models using strict **DDD principles** (Aggregate Roots, Entities, and Immutability).
* Implementing **CQRS** by separating the write-store (**SQL Server**) from the read-store (**Elasticsearch v8+**).
* Configuring the modern, strongly-typed official **Elastic.Clients.Elasticsearch** client without relying on raw string-based scripts.
* Building a resilient transaction-safe **Outbox Worker** to capture changes and stream them dynamically to the read-model.

---

### The Synchronization Lifecycle
1.  **Write Phase:** A state mutation occurs on the `User` Aggregate Root (e.g., `AddCar`, `UpdateAbout`). The aggregate raises a specific rich domain event.
2.  **Intercept & Commit:** Upon calling `SaveChanges()` in the Unit of Work, an `EventsInterceptor` intercepts the domain events, serializes their payloads to JSON, and inserts them into the `OutboxMessages` table within the **same SQL Server database transaction**.
3.  **Poll & Fetch:** A hosted `OutboxProcessorBackgroundService` running inside the WebApi layer polls the database every 1 second using explicit **`WITH (UPDLOCK, READPAST)`** hints to fetch pending rows safely.
4.  **In-Memory Dispatch:** The background service deserializes the payload using the stored metadata `EventType` and triggers an in-memory broadcast via MediatR's `_mediator.Publish()`.
5.  **Read-Model Sync:** The corresponding `INotificationHandler<T>` inside the Application layer catches the notification and executes highly optimized **Strongly-Typed Partial Updates via LINQ** to sync the flat, denormalized read-model inside Elasticsearch.

---

## 🛠️ Tech Stack & Key Configurations

* **.NET 9.0 SDK**
* **Entity Framework Core** (With interceptors for implicit outbox tracking)
* **SQL Server** (Write database)
* **Elasticsearch v8+ / v9.3.3** (Read-model search cluster)
* **MediatR** (For CQRS routing and In-Memory event broadcasting)

### Advanced Elasticsearch Mapping Definitions
On application startup, the infrastructure layer automatically provisions the read index (`users-read-index`) with strict performance settings:
* `NumberOfShards(1)` and `NumberOfReplicas(0)` optimized for single-node development environments.
* Explicit data types mapping: Identifiers (Guids, usernames, and license plates) mapped as `Keyword` for precise, lightning-fast filtering, while descriptions are mapped as `Text` to allow fuzzy full-text searching.

---
## 🔍 Elasticsearch Architecture & Mapping Strategy

A key focus of this project is demonstrating how to properly configure and synchronize data with **Elasticsearch v8+ / v9.x** using the modern, strongly-typed `Elastic.Clients.Elasticsearch` client. 

Instead of treating Elasticsearch as a simple Key-Value store, we implement a **Flat Denormalized Read-Model** strategy optimized for sub-millisecond full-text searches.

### 1. Document Mapping Configuration
On application startup, the `InitializeIndexAsync` method checks for index existence. If missing, it provisions the index schema dynamically using the Fluent API. The field types are strategically selected to optimize search indices:

* **`Id` & `Username` (Keyword):** Mapped as `Keyword`. This tells Elasticsearch to index the exact string without tokenizing it, enabling lightning-fast exact matches and aggregations.
* **`About` (Text):** Mapped as `Text` to enable analyzer tokenization. This allows full-text fuzzy queries, matching terms even with minor typos or partial words.
* **`Cars` (Nested Object Array):** Mapped as an embedded object containing sub-properties (`id`, `name`, `code`, `color`). The `name` property uses a **Multi-Field** strategy—it is indexed as both `Text` (for fuzzy search) and a `Keyword` sub-field (for exact sorting/filtering).

### 2. High-Performance Synchronous Updates (No String-Scripts)
A typical anti-pattern when syncing sub-collections (like adding a Car to a User) is querying the SQL database again to fetch the entire aggregate, or using unsafe, untyped string-based Painless Scripts (`ctx._source.cars.add(...)`).

This project implements **Strongly-Typed Partial Updates via LINQ**:
1.  The sync handler captures the event (e.g., `UserCarAdded` or `UserCarRemoved`).
2.  The handler fetches the existing flat document from Elasticsearch via `GetAsync`.
3.  Standard C# LINQ modifies the local collection in-memory.
4.  The application pushes **only the modified array slice** back using an anonymous object inside `UpdateAsync`:

---

## ⚠️ Architectural Limitations & Why This is NOT Production-Ready

> **CRITICAL NOTE:** This project is explicitly built as a **functional sandbox for learning and experimentation**. It attempts to highlight core functionality rather than production-grade stability. Several architectural choices are intentionally simplified and constitute production anti-patterns.

### 1. In-Memory Processing Risk & Scale-Out Failures
We are currently using MediatR's `_mediator.Publish()` inside the background worker. This relies heavily on **In-Memory** processing. 
* **The Issue:** If multiple instances of this WebApi are spun up behind a load balancer (horizontal scaling), each instance's background worker will poll the same SQL table. Although `READPAST` prevents lock deadlocks, once the event enters memory, if that instance crashes prematurely, the event is lost forever from the pipeline. 
* **The Production Best Practice:** Replace MediatR Pub/Sub with a distributed message broker using **MassTransit** backed by **RabbitMQ** or **Apache Kafka**. This ensures messages are safe from memory-wipes and allows concurrent processing via real distributed consumers.

### 2. Lack of Event Ordering (The Chronological Split)
In an event-driven system syncing state changes, the sequence of execution is everything.
* **The Issue:** If a user modifies their username (`UserUpdated`) and immediately adds a car (`UserCarAdded`), the Outbox service might grab them together. If a network blip occurs during the execution of the first event, and the second event processes first, the read-model will drift into an inaccurate state. MediatR notifications cannot enforce sequential partition ordering across asynchronous boundaries.
* **The Production Best Practice:** Implement an explicit message ordering sequence, or switch to event brokers that support partitioned/ordered consumption keys (like Kafka Partition Keys or RabbitMQ Consistent Hash Exchange).

### 3. Hydration vs. Enrichment Tradeoffs
While effort has been made to keep domain events rich (containing nested data), any modification that alters historical arrays forces the repository to query state or handle collection updates. In a live system, asynchronous replication should rely on independent delta streams.

---

## 📂 Custom Exception Architecture
The project implements a strict, decoupled, multi-level business exception hierarchy. No infrastructure exceptions leak out of the data layers. The repository layer remains completely silent, returning clean results, while the application layer acts as the absolute validator of responses.

SampleForElastic.Common.Base.ApplicationException (Level 1)
└── SampleForElastic.Common.Base.KeyNotFoundApplicationException (Level 2)
└── SampleForElastic.Application.Exceptions.UserNotFoundInElasticsearchException (Level 3)


<p align="center">
  <b>Happy Coding! 🚀 Built with passion to master the craft of Event-Driven Systems.</b>
</p>
