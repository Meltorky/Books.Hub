using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Hub.Api.Controllers;
using Books.Hub.Api.Validators;
using Books.Hub.Application.DTOs.Authors;
using Books.Hub.Application.Interfaces.IServices;
using Books.Hub.Application.Options;
using Books.Hub.Domain.Common;
using Books.Hub.Domain.Entities;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace UnitTests.API.Controllers
{
    public class AuthorsControllerTests
    {
        //[Fact]
        //public async Task GetAllAsync_ValidSearch_ReturnsOkWithAuthors()
        //{
        //    // Arrange
        //    var fakeService = A.Fake<IAuthorService>();
        //    var options = A.Fake<IOptions<ImagesOptions>>();

        //    IEnumerable<AuthorDTO> expectedAuthors = new List<AuthorDTO> { new AuthorDTO { } };
        //    var controller = new AuthorsController(fakeService , options);
        //    var token = CancellationToken.None;


        //    // Act
        //    A.CallTo(() => fakeService.GetAllAsync(A<QuerySpecification<Author>>._, token)).Returns(Task.FromResult(expectedAuthors));
        //    var result = await controller.GetAllAsync(token, "Jo", 1, 10, "trending", true) as OkObjectResult;

        //    // Assert
        //    result.Should().NotBeNull();
        //    result!.StatusCode.Should().Be(200);
        //    result.Value.Should().BeEquivalentTo(expectedAuthors);
        //}



        //[Fact]
        //public async Task CreateAuthorProfile_InvalidModelState_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var fakeService = A.Fake<IAuthorService>();
        //    var fakeOptions = A.Fake<IOptions<ImagesOptions>>();
        //    var controller = new AuthorsController(fakeService, fakeOptions);
        //    var token = CancellationToken.None;

        //    var dto = new CreateAuthorDTO();
        //    controller.ModelState.AddModelError("Name", "Name is required");

        //    // Act
        //    var result = await controller.CreateAuthorProfile("user123", dto, token) as BadRequestObjectResult;

        //    // Assert
        //    result.Should().NotBeNull();
        //    result!.StatusCode.Should().Be(400);
        //}




    }
}
