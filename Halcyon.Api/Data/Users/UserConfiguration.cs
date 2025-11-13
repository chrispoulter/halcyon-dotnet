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
        builder.Property(u => u.FirstName).HasColumnName("first_name").IsRequired();
        builder.Property(u => u.LastName).HasColumnName("last_name").IsRequired();
        builder.Property(u => u.DateOfBirth).HasColumnName("date_of_birth").IsRequired();
        builder.Property(u => u.Roles).HasColumnName("roles").HasColumnType("text[]");
        builder.Property(u => u.IsLockedOut).HasColumnName("is_locked_out").HasDefaultValue(false);
        builder.Property(u => u.Version).IsRowVersion();

        builder.HasIndex(u => u.EmailAddress, "users_email_address_unique").IsUnique();

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
    }
}
