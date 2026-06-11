namespace SampleForElastic.Domain.Enums
{
    public enum OutboxStatus : byte
    {
        Pending = 0,
        Processed = 1,
        Failed = 2
    }
}
