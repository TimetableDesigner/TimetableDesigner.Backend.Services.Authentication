namespace TimetableDesigner.Backend.Services.Authentication.Database.Model;

public class Event
{
    public Guid Id { get; set; }
    public required string Payload { get; set; }
    public required string PayloadType { get; set; }
    public DateTimeOffset OccuredOn { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? ProcessedOn { get; set; }
    public uint RetryCount { get; set; }
    public uint Version { get; set; }
}