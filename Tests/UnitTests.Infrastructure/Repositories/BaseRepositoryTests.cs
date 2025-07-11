using FluentAssertions;
using Books.Hub.Domain.Entities;
using Books.Hub.Infrastructure.Repositories;
using UnitTests.Infrastructure.InMemoryData;

namespace UnitTests.Infrastructure.Repositories
{
    public class BaseRepositoryTests
    {

        [Fact]
        public async Task AddAsync_ShouldAddEntity()
        {
            // Arrange
            using var context = new InMemoryDbContext();
            var repository = new BaseRepository<Author>(context);
            var author = new Author { Name = "Test Author" };
            var token = CancellationToken.None;

            // Act
            var result = await repository.AddAsync(author, token);

            // Assert
            result.Should().NotBeNull();
            context.Set<Author>().Should().Contain(author);
        }



        [Fact]
        public async Task EditAsync_ShouldUpdateEntity()
        {
            // Arrange
            using var context = new InMemoryDbContext();
            var author = new Author { Name = "Old Name" };
            context.Add(author);
            await context.SaveChangesAsync();

            var repository = new BaseRepository<Author>(context);
            author.Name = "Updated Name";
            var token = CancellationToken.None;

            // Act
            var updated = await repository.EditAsync(author, token);

            // Assert
            updated.Should().BeTrue();
            context.Set<Author>().First().Name.Should().Be("Updated Name");
        }



        [Fact]
        public async Task DeleteAsync_ShouldRemoveEntity()
        {
            // Arrange
            using var context = new InMemoryDbContext();
            var author = new Author { Name = "To Delete" };
            context.Add(author);
            await context.SaveChangesAsync();

            var repository = new BaseRepository<Author>(context);
            var token = CancellationToken.None;

            // Act
            var deleted = await repository.DeleteAsync(author, token);

            // Assert
            deleted.Should().BeTrue();
            context.Set<Author>().Should().NotContain(author);
        }



        [Fact]
        public async Task RemoveRange_ShouldRemoveMultipleEntities()
        {
            // Arrange
            using var context = new InMemoryDbContext();
            var authors = new List<Author>
            {
                new Author { Name = "A" },
                new Author { Name = "B" }
            };
            context.AddRange(authors);
            await context.SaveChangesAsync();

            var repository = new BaseRepository<Author>(context);

            // Act
            await repository.RemoveRange(authors);

            // Assert
            context.Set<Author>().Should().BeEmpty();
        }



        [Fact]
        public async Task IsExitAsync_WhenExists_ShouldReturnTrue()
        {
            // Arrange
            using var context = new InMemoryDbContext();
            var author = new Author { Name = "Exists" };
            context.Add(author);
            await context.SaveChangesAsync();

            var repository = new BaseRepository<Author>(context);
            var token = CancellationToken.None;

            // Act
            var exists = await repository.IsExitAsync(a => a.Name == "Exists", token);

            // Assert
            exists.Should().BeTrue();
        }



        [Fact]
        public async Task IsExitAsync_WhenNotExists_ShouldReturnFalse()
        {
            // Arrange
            using var context = new InMemoryDbContext();
            var repository = new BaseRepository<Author>(context);
            var token = CancellationToken.None;

            // Act
            var exists = await repository.IsExitAsync(a => a.Name == "Not There", token);

            // Assert
            exists.Should().BeFalse();
        }
    }
}

