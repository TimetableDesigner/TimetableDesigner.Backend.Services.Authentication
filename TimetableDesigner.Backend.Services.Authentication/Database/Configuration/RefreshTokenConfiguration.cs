using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimetableDesigner.Backend.Services.Authentication.Database.Model;

namespace TimetableDesigner.Backend.Services.Authentication.Database.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(x => x.Token);
        builder.HasIndex(x => x.Token)
               .IsUnique();
        builder.Property(x => x.Token)
               .IsRequired();
        
        builder.HasOne(x => x.Account)
               .WithMany(x => x.RefreshTokens)
               .HasForeignKey(x => x.AccountId)
               .IsRequired();
        builder.Property(x => x.AccountId)
               .IsRequired();
        
        builder.Property(x => x.ExpirationDate)
               .IsRequired();
        
        builder.Property(x => x.IsExtendable)
               .IsRequired();
        
        builder.Property(b => b.Version)
               .IsRowVersion();
    }
}