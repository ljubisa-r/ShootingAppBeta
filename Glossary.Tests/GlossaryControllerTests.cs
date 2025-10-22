using GlossaryAPI.Controllers;
using GlossaryAPI.DTOs;
using GlossaryAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Reflection;
using System.Security.Claims;


namespace GlossaryAPI.Tests.Controllers
{
    public class GlossaryControllerTests
    {
        private readonly Mock<IGlossaryService> _mockService;
        private readonly GlossaryController _controller;

        public GlossaryControllerTests()
        {
            _mockService = new Mock<IGlossaryService>();
            _controller = new GlossaryController(_mockService.Object);

            // Mock korisnika sa userId = 1
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "mock"));

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Fact]
        public void GetAllTerms_ReturnsOk_WithListOfTerms()
        {
            var terms = new List<GlossaryTermDTO>
            {
                new GlossaryTermDTO { id = 1, term = "API", definition = "Interface" }
            };
            _mockService.Setup(s => s.GetAllTerms()).Returns(terms);

            var result = _controller.GetAllTerms();

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTerms = Assert.IsType<List<GlossaryTermDTO>>(ok.Value);
            Assert.Single(returnedTerms);
        }

        [Fact]
        public void GetTermById_ReturnsOk_WhenTermExists()
        {
            var term = new GlossaryTermDTO { id = 1, term = "API" };
            _mockService.Setup(s => s.GetTermById(1)).Returns(term);

            var result = _controller.GetTermById(1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var returnedTerm = Assert.IsType<GlossaryTermDTO>(ok.Value);
            Assert.Equal(1, returnedTerm.id);
        }

        [Fact]
        public void GetTermById_ReturnsNotFound_WhenMissing()
        {
            _mockService.Setup(s => s.GetTermById(1)).Returns((GlossaryTermDTO)null);

            var result = _controller.GetTermById(1);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void CreateTerm_ReturnsCreatedAtAction_WhenValid()
        {
            var newTerm = new GlossaryTermDTO { term = "REST" };
            var createdTerm = new GlossaryTermDTO { id = 1, term = "REST" };
            _mockService.Setup(s => s.CreateTerm(newTerm, 1)).Returns(createdTerm);

            var result = _controller.CreateTerm(newTerm);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedTerm = Assert.IsType<GlossaryTermDTO>(created.Value);
            Assert.Equal(1, returnedTerm.id);
        }

        [Fact]
        public void CreateTerm_ReturnsBadRequest_WhenNull()
        {
            var result = _controller.CreateTerm(null);
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Term data is required", badRequest.Value);
        }

        [Fact]
        public void UpdateTerm_ReturnsNoContent_WhenSuccessful()
        {
            var updated = new GlossaryTermDTO { id = 1, term = "Updated" };
            _mockService.Setup(s => s.UpdateTerm(updated, 1)).Returns(updated);

            var result = _controller.UpdateTerm(updated);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateTerm_ReturnsNotFound_WhenKeyNotFound()
        {
            var updated = new GlossaryTermDTO { id = 1 };
            _mockService.Setup(s => s.UpdateTerm(updated, 1)).Throws<KeyNotFoundException>();

            var result = _controller.UpdateTerm(updated);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ArchiveTerm_ReturnsNoContent_WhenSuccessful()
        {
            var archived = new GlossaryTermDTO { id = 1, status = Models.ItemStatus.Archived };
            _mockService.Setup(s => s.ArchiveTerm(1, 1)).Returns(archived);

            var result = _controller.ArchiveTerm(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void ArchiveTerm_ReturnsNotFound_WhenKeyNotFound()
        {
            _mockService.Setup(s => s.ArchiveTerm(1, 1)).Throws<KeyNotFoundException>();

            var result = _controller.ArchiveTerm(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void ArchiveTerm_ReturnsForbid_WhenUnauthorized()
        {
            _mockService.Setup(s => s.ArchiveTerm(1, 1)).Throws<UnauthorizedAccessException>();

            var result = _controller.ArchiveTerm(1);

            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public void PublishTerm_ReturnsNoContent_WhenSuccessful()
        {
            var updated = new GlossaryTermDTO { id = 1, term = "Publish" };
            _mockService.Setup(s => s.PublishTerm(updated, 1)).Returns(updated);

            var result = _controller.PublishTerm(updated);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void PublishTerm_ReturnsNotFound_WhenMissing()
        {
            var updated = new GlossaryTermDTO { id = 1 };
            _mockService.Setup(s => s.PublishTerm(updated, 1)).Throws<KeyNotFoundException>();

            var result = _controller.PublishTerm(updated);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteTerm_ReturnsNoContent_WhenSuccessful()
        {
            var result = _controller.DeleteTerm(1);

            Assert.IsType<NoContentResult>(result);
            _mockService.Verify(s => s.DeleteTerm(1, 1), Times.Once);
        }

        [Fact]
        public void DeleteTerm_ReturnsNotFound_WhenKeyNotFound()
        {
            _mockService.Setup(s => s.DeleteTerm(1, 1)).Throws<KeyNotFoundException>();

            var result = _controller.DeleteTerm(1);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public void DeleteTerm_ReturnsBadRequest_WhenInvalidOperation()
        {
            _mockService.Setup(s => s.DeleteTerm(1, 1)).Throws<InvalidOperationException>();

            var result = _controller.DeleteTerm(1);

            Assert.IsType<BadRequestObjectResult>(result);
        }


        // Testovi za GetUserId()


        [Fact]
        public void GetUserId_ReturnsInt_WhenClaimExists()
        {
            var method = typeof(GlossaryController)
                .GetMethod("GetUserId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var result = (int)method.Invoke(_controller, null);

            Assert.Equal(1, result);
        }

        [Fact]
        public void GetUserId_ThrowsUnauthorizedAccess_WhenClaimMissing()
        {
            var controllerNoUser = new GlossaryController(_mockService.Object);
            controllerNoUser.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
            };

            var method = typeof(GlossaryController)
                .GetMethod("GetUserId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var exception = Assert.Throws<TargetInvocationException>(() => method.Invoke(controllerNoUser, null));
            Assert.IsType<UnauthorizedAccessException>(exception.InnerException);
        }
    }
}
