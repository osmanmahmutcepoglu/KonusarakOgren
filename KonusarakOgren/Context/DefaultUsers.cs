using KonusarakOgren.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KonusarakOgren.Context
{
    public class DefaultUsers : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = "1",
                    UserName = "Test",
                    PasswordHash = "Test1234"
                },
                new User
                {
                    Id = "2",
                    UserName = "Test2",
                    PasswordHash = "Test1234"
                });
        }
    }
}
