using Xunit;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Application.DTOs.Books;
using Books.Hub.Api.Controllers;

public class SubscriberControllerTests
{
    private SubscribersController GetController(ISubscriberService service) => new SubscribersController(service);


    [Fact]
    public async Task AddAuthorSubscription_ValidRequest_ReturnsNoContent()
    {
        // Arrange
        var fakeService = A.Fake<ISubscriberService>();
        var controller = GetController(fakeService);

        string userId = "user123";
        int authorId = 1;
        var token = CancellationToken.None;

        // Act
        A.CallTo(() => fakeService.AddAuthorSubscribion(userId, authorId, token)).DoesNothing();
        var result = await controller.AddAuthorSubscription(userId, authorId, token) as NoContentResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(204);
    }



    [Fact]
    public async Task RemoveAuthorSubscription_ValidRequest_ReturnsNoContent()
    {        
        // Arrange
        var fakeService = A.Fake<ISubscriberService>();
        var controller = GetController(fakeService);

        string userId = "user123";
        int authorId = 1;
        var token = CancellationToken.None;

        // Act
        A.CallTo(() => fakeService.RemoveAuthorSubscribtion(userId, authorId, token)).DoesNothing();

        var result = await controller.RemoveAuthorSubscription(userId, authorId, token) as NoContentResult;
        
        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(204);
    }



    [Fact]
    public async Task GetSubscribedAuthors_ValidUserId_ReturnsOkWithAuthors()
    {
        // Arrange
        var fakeService = A.Fake<ISubscriberService>();
        var controller = GetController(fakeService);

        string userId = "user123";
        var token = CancellationToken.None;

        var users = new List<AuthorDTO>() { new AuthorDTO() };

        // Act
        A.CallTo(() => fakeService.GetSubscribedAuthors(userId, token)).Returns(Task.FromResult(users));
        var result = await controller.GetSubscribedAuthors(userId, token) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(users);
    }



    [Fact]
    public async Task BuyBook_ValidRequest_ReturnsNoContent()
    {        
        // Arrange
        var fakeService = A.Fake<ISubscriberService>();
        var controller = GetController(fakeService);

        string userId = "user123";
        int bookId = 1;
        var token = CancellationToken.None;

        // Act
        A.CallTo(() => fakeService.BuyBook(userId, bookId, token)).DoesNothing();

        var result = await controller.BuyBook(userId, bookId, token) as NoContentResult;
        // Assert

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(204);
    }



    [Fact]
    public async Task GetBoughtBooks_ValidUserId_ReturnsOkWithBooks()
    {        
        // Arrange
        var fakeService = A.Fake<ISubscriberService>();
        var controller = GetController(fakeService);

        string userId = "user123";
        var token = CancellationToken.None;
        var books = new List<BookDTO>() { new BookDTO() };

        // Act
        A.CallTo(() => fakeService.GetBoughtBooks(userId, token)).Returns(Task.FromResult(books));
        var result = await controller.GetBoughtBooks(userId, token) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(books);
    }



    [Fact]
    public async Task AddBookToFavourites_ValidRequest_ReturnsNoContent()
    {        
        // Arrange
        var fakeService = A.Fake<ISubscriberService>();
        var controller = GetController(fakeService);

        string userId = "user123";
        int bookId = 1;
        var token = CancellationToken.None;
        
        // Act
        A.CallTo(() => fakeService.AddBookToFavourites(userId, bookId, token)).DoesNothing();
        var result = await controller.AddBookToFavourites(userId, bookId, token) as NoContentResult;
        
        // Assert

        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(204);
    }



    [Fact]
    public async Task RemoveBookFromFavourites_ValidRequest_ReturnsNoContent()
    {       
        // Arrange
        var fakeService = A.Fake<ISubscriberService>();
        var controller = GetController(fakeService);

        string userId = "user123";
        int bookId = 1;
        var token = CancellationToken.None;
        
        // Act
        A.CallTo(() => fakeService.RemoveBookFromFavourites(userId, bookId, token)).DoesNothing();
        var result = await controller.RemoveBookFromFavourites(userId, bookId, token) as NoContentResult;
        
        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(204);
    }



    [Fact]
    public async Task GetFavouriteBooks_ValidUserId_ReturnsOkWithBooks()
    {        
        // Arrange
        var fakeService = A.Fake<ISubscriberService>();
        var controller = GetController(fakeService);

        string userId = "user123";
        var token = CancellationToken.None;

        var books = new List<BookDTO>() { new BookDTO()};
        
        // Act
        A.CallTo(() => fakeService.GetFavouriteBooks(userId, token)).Returns(Task.FromResult(books));
        var result = await controller.GetFavouriteBooks(userId, token) as OkObjectResult;
        
        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(books);
    }
}
