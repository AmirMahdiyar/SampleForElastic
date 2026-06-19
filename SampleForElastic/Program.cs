using SampleForElastic;
using SampleForElastic.Application.Contracts;
using SampleForElastic.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDI(builder.Configuration);
builder.Services.AddHostedService<OutboxProcessorBackgroundService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var userReadRepository = scope.ServiceProvider.GetRequiredService<IUserReadRepository>();
    await userReadRepository.InitializeIndexAsync(CancellationToken.None);
}
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
