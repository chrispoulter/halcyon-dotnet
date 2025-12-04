using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Halcyon.Api.Data.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(u => u.Id).HasColumnName("id");
        builder.Property(u => u.EmailAddress).HasColumnName("email_address").IsRequired();
        builder.Property(u => u.Password).HasColumnName("password");
        builder.Property(u => u.PasswordResetToken).HasColumnName("password_reset_token");
        builder
            .Property(u => u.IsTwoFactorEnabled)
            .HasColumnName("is_two_factor_enabled")
            .HasDefaultValue(false);
        builder.Property(u => u.TwoFactorSecret).HasColumnName("two_factor_secret");
        builder.Property(u => u.TwoFactorTempSecret).HasColumnName("two_factor_temp_secret");
        builder
            .Property(u => u.TwoFactorRecoveryCodes)
            .HasColumnName("two_factor_recovery_codes")
            .HasColumnType("text[]");
        builder.Property(u => u.FirstName).HasColumnName("first_name").IsRequired();
        builder.Property(u => u.LastName).HasColumnName("last_name").IsRequired();
        builder.Property(u => u.DateOfBirth).HasColumnName("date_of_birth").IsRequired();
        builder.Property(u => u.Roles).HasColumnName("roles").HasColumnType("text[]");
        builder.Property(u => u.IsLockedOut).HasColumnName("is_locked_out").HasDefaultValue(false);
        builder.Property(u => u.SearchVector).HasColumnName("search_vector");

        builder.HasGeneratedTsVectorColumn(
            u => u.SearchVector,
            "english",
            u => new
            {
                u.FirstName,
                u.LastName,
                u.EmailAddress,
            }
        );

        builder.HasKey(u => u.Id).HasName("pk_users");
        builder.HasIndex(u => u.EmailAddress, "ix_users_email_address").IsUnique();
        builder.HasIndex(u => u.SearchVector, "ix_users_search_vector").HasMethod("gin");
    }
}
