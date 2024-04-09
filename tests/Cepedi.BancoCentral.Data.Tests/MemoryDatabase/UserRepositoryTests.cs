using Cepedi.BancoCentral.Data.Repositories;
using Cepedi.BancoCentral.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cepedi.BancoCentral.Data.Tests.MemoryDatabase;

public class UserRepositoryTests
{
    [Fact]
    public async Task Can_Get_Users_From_Database()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            context.Usuario.Add(new UsuarioEntity { Id = 1, Nome = "user1" });
            context.Usuario.Add(new UsuarioEntity { Id = 2, Nome = "user2" });
            context.SaveChanges();
        }

        // Act
        using (var context = new ApplicationDbContext(options))
        {
            var userRepository = new UsuarioRepository(context);
            var users = await userRepository.GetUsuariosAsync();

            // Assert
            Assert.Equal(2, users.Count);
            Assert.Equal("user1", users[0].Nome);
            Assert.Equal("user2", users[1].Nome);
        }
    }

}
