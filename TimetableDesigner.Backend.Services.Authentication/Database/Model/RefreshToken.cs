namespace TimetableDesigner.Backend.Services.Authentication.Database.Model;

public class RefreshToken
{
    public Guid Token { get; set; }
    public long AccountId { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public bool IsExtendable { get; set; }
    public uint Version { get; set; }
    
    public virtual Account Account { get; set; } = null!;
}