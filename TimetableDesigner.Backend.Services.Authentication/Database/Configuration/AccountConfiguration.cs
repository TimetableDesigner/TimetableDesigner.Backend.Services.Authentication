using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimetableDesigner.Backend.Services.Authentication.Database.Model;

namespace TimetableDesigner.Backend.Services.Authentication.Database.Configuration;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired()
               .UseIdentityAlwaysColumn();
        
        builder.Property(x => x.Email)
               .HasMaxLength(320)
               .IsRequired();
        
        builder.Property(x => x.Password)
               .HasMaxLength(1000)
               .IsRequired();
        
        builder.Property(x => x.PasswordSalt)
               .HasMaxLength(20)
               .IsRequired();
        
        builder.Property(b => b.Version)
               .IsRowVersion();
    }
}