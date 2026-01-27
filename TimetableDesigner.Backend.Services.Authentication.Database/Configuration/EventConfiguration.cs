using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimetableDesigner.Backend.Services.Authentication.Database.Model;

namespace TimetableDesigner.Backend.Services.Authentication.Database.Configuration;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired();
        
        builder.Property(x => x.Payload)
               .IsRequired();
        
        builder.Property(x => x.PayloadType)
               .IsRequired();
        
        builder.Property(x => x.OccuredOn)
               .IsRequired();

        builder.Property(x => x.ProcessedOn);
        
        builder.Property(x => x.RetryCount)
               .IsRequired();
        
        builder.Property(b => b.Version)
               .IsRowVersion();
    }
}