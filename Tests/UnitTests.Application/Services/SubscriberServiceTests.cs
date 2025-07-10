using Xunit;
using FakeItEasy;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Books.Hub.Application.Common.Exceptions;
using Books.Hub.Application.Identity;
using Books.Hub.Application.Interfaces.IRepositories;
using Books.Hub.Application.Services;
using Books.Hub.Domain.Entities;

public class SubscriberServiceTests
{
    [Fact]
    public async Task AddAuthorSubscribion_Should_Throw_NotFoundException_If_User_Not_Found()
    {
        // Arrange
        var fakeUow = A.Fake<IUnitOfWork>();
        var fakeUserManager = A.Fake<UserManager<ApplicationUser>>(
            options => options.WithArgumentsForConstructor(() =>
                new UserManager<ApplicationUser>(
                    A.Fake<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null))
        );

        // Return empty user list
        A.CallTo(() => fakeUserManager.Users).Returns(new List<ApplicationUser>().AsQueryable());

        var service = new SubscriberService(fakeUow, fakeUserManager);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.AddAuthorSubscribion("u1", 1, CancellationToken.None));
    }

    [Fact]
    public async Task AddAuthorSubscribion_Should_Throw_NotFoundException_If_Author_Not_Found()
    {
        // Arrange
        var user = new ApplicationUser { Id = "u1", AuthorSubscribers = new List<AuthorSubscriber>() };

        var fakeUow = A.Fake<IUnitOfWork>();
        var fakeAuthorsRepo = A.Fake<IAuthorsRepository>();
        A.CallTo(() => fakeAuthorsRepo.GetById(1, A<CancellationToken>._))
            .Returns(Task.FromResult<Author?>(null)); // Author not found
        A.CallTo(() => fakeUow.Authors).Returns(fakeAuthorsRepo);

        var fakeUserManager = A.Fake<UserManager<ApplicationUser>>(
            options => options.WithArgumentsForConstructor(() =>
                new UserManager<ApplicationUser>(
                    A.Fake<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null))
        );
        A.CallTo(() => fakeUserManager.Users).Returns(new List<ApplicationUser> { user }.AsQueryable());

        var service = new SubscriberService(fakeUow, fakeUserManager);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.AddAuthorSubscribion("u1", 1, CancellationToken.None));
    }

    [Fact]
    public async Task AddAuthorSubscribion_Should_Throw_ConflictException_If_Already_Subscribed()
    {
        // Arrange
        var user = new ApplicationUser
        {
            Id = "u1",
            AuthorSubscribers = new List<AuthorSubscriber> { new AuthorSubscriber { AuthorId = 1 } }
        };

        var fakeAuthor = new Author { Id = 1 };

        var fakeUow = A.Fake<IUnitOfWork>();
        var fakeAuthorsRepo = A.Fake<IAuthorsRepository>();
        A.CallTo(() => fakeAuthorsRepo.GetById(1, A<CancellationToken>._)).Returns(Task.FromResult<Author?>(fakeAuthor));
        A.CallTo(() => fakeUow.Authors).Returns(fakeAuthorsRepo);

        var fakeUserManager = A.Fake<UserManager<ApplicationUser>>(
            options => options.WithArgumentsForConstructor(() =>
                new UserManager<ApplicationUser>(
                    A.Fake<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null))
        );
        A.CallTo(() => fakeUserManager.Users).Returns(new List<ApplicationUser> { user }.AsQueryable());

        var service = new SubscriberService(fakeUow, fakeUserManager);

        // Act & Assert
        await Assert.ThrowsAsync<ConflictException>(() =>
            service.AddAuthorSubscribion("u1", 1, CancellationToken.None));
    }

    [Fact]
    public async Task AddAuthorSubscribion_Should_Add_If_Not_Already_Subscribed()
    {
        // Arrange
        var user = new ApplicationUser
        {
            Id = "u1",
            AuthorSubscribers = new List<AuthorSubscriber>()
        };

        var fakeAuthor = new Author { Id = 1 };

        var fakeUow = A.Fake<IUnitOfWork>();
        var fakeAuthorsRepo = A.Fake<IAuthorsRepository>();
        A.CallTo(() => fakeAuthorsRepo.GetById(1, A<CancellationToken>._)).Returns(Task.FromResult<Author?>(fakeAuthor));
        A.CallTo(() => fakeUow.Authors).Returns(fakeAuthorsRepo);

        var fakeUserManager = A.Fake<UserManager<ApplicationUser>>(
            options => options.WithArgumentsForConstructor(() =>
                new UserManager<ApplicationUser>(
                    A.Fake<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null))
        );
        A.CallTo(() => fakeUserManager.Users).Returns(new List<ApplicationUser> { user }.AsQueryable());

        var service = new SubscriberService(fakeUow, fakeUserManager);

        // Act
        await service.AddAuthorSubscribion("u1", 1, CancellationToken.None);

        // Assert
        Assert.Single(user.AuthorSubscribers);
        A.CallTo(() => fakeUow.SaveChangesAsync(A<CancellationToken>._)).MustHaveHappenedOnceExactly();
    }
}
