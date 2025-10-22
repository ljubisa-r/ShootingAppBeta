
using GlossaryAPI.DTOs;
using GlossaryAPI.Interfaces;
using GlossaryAPI.Models;
using GlossaryAPI.Services;
using Moq;


namespace GlossaryAPI.Tests
{
    public class GlossaryServiceTests
    {
        private readonly Mock<IGlossaryRepository> _mockGlossaryRepo;
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<GlossaryTermValidator> _mockValidator;
        private readonly GlossaryService _service;

        public GlossaryServiceTests()
        {
            _mockGlossaryRepo = new Mock<IGlossaryRepository>();
            _mockUserRepo = new Mock<IUserRepository>();
            _mockValidator = new Mock<GlossaryTermValidator>();
            _service = new GlossaryService(_mockGlossaryRepo.Object, _mockUserRepo.Object, _mockValidator.Object);
        }

        [Fact]
        public void GetAllTerms_ExcludesArchived_ReturnsOnlyNonArchived()
        {
            // Arrange
            var terms = new List<GlossaryTerm>
            {
                new GlossaryTerm { Id = 1, Term = "A", Status = ItemStatus.Draft },
                new GlossaryTerm { Id = 2, Term = "B", Status = ItemStatus.Published },
                new GlossaryTerm { Id = 3, Term = "C", Status = ItemStatus.Archived }
            };
            _mockGlossaryRepo.Setup(r => r.GetAll()).Returns(terms.AsQueryable());

            // Act
            var result = _service.GetAllTerms();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.DoesNotContain(result, t => t.status == ItemStatus.Archived);
        }

        [Fact]
        public void GetTermById_ReturnsTerm_WhenExists()
        {
            // Arrange
            var term = new GlossaryTerm { Id = 1, Term = "Test", Definition = "Def", Status = ItemStatus.Draft };
            _mockGlossaryRepo.Setup(r => r.GetById(1)).Returns(term);

            // Act
            var result = _service.GetTermById(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(term.Id, result.id);
        }

        [Fact]
        public void GetTermById_ReturnsNull_WhenNotExists()
        {
            _mockGlossaryRepo.Setup(r => r.GetById(1)).Returns((GlossaryTerm)null);

            var result = _service.GetTermById(1);

            Assert.Null(result);
        }

        [Fact]
        public void CreateTerm_SetsDraftAndCreatedBy_ReturnsDTO()
        {
            // Arrange
            var newTermDto = new GlossaryTermDTO { term = "New", definition = "Some definition here" };
            var user = new User { Id = 1 };
            _mockUserRepo.Setup(r => r.GetById(1)).Returns(user);
            _mockGlossaryRepo.Setup(r => r.Add(It.IsAny<GlossaryTerm>())).Callback<GlossaryTerm>(t => t.Id = 100);

            // Act
            var result = _service.CreateTerm(newTermDto, 1);

            // Assert
            Assert.Equal(100, result.id);
            Assert.Equal(ItemStatus.Draft, result.status);
            _mockGlossaryRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void UpdateTerm_Throws_WhenTermNotFound()
        {
            var updatedTerm = new GlossaryTermDTO { id = 1, term = "X", definition = "Def", status = ItemStatus.Draft };
            _mockGlossaryRepo.Setup(r => r.GetById(1)).Returns((GlossaryTerm)null);

            Assert.Throws<KeyNotFoundException>(() => _service.UpdateTerm(updatedTerm, 1));
        }

        [Fact]
        public void PublishTerm_UpdatesStatusToPublished()
        {
            var existingTerm = new GlossaryTerm { Id = 1, Term = "T", Definition = "Definition need to be longer than 30 chars", Status = ItemStatus.Draft };
            var updatedTerm = new GlossaryTermDTO { id = 1, term = "T", definition = "Definition need to be longer than 30 chars" };
            var user = new User { Id = 1 };

            _mockGlossaryRepo.Setup(r => r.GetById(1)).Returns(existingTerm);
            _mockUserRepo.Setup(r => r.GetById(1)).Returns(user);

            var result = _service.PublishTerm(updatedTerm, 1);

            Assert.Equal(ItemStatus.Published, result.status);
            _mockGlossaryRepo.Verify(r => r.SaveChanges(), Times.Once);
        }

        [Fact]
        public void ArchiveTerm_Throws_WhenTermNotPublished()
        {
            var existingTerm = new GlossaryTerm { Id = 1, Status = ItemStatus.Draft };
            _mockGlossaryRepo.Setup(r => r.GetById(1)).Returns(existingTerm);

            Assert.Throws<InvalidOperationException>(() => _service.ArchiveTerm(1, 1));
        }

        [Fact]
        public void DeleteTerm_Throws_WhenNotDraft()
        {
            var existingTerm = new GlossaryTerm { Id = 1, Status = ItemStatus.Published, CreatedBy = 1 };
            _mockGlossaryRepo.Setup(r => r.GetById(1)).Returns(existingTerm);
            _mockUserRepo.Setup(r => r.GetById(1)).Returns(new User { Id = 1 });

            Assert.Throws<InvalidOperationException>(() => _service.DeleteTerm(1, 1));
        }

        [Fact]
        public void DeleteTerm_Deletes_WhenDraftAndCreatedByUser()
        {
            var existingTerm = new GlossaryTerm { Id = 1, Status = ItemStatus.Draft, CreatedBy = 1 };
            _mockGlossaryRepo.Setup(r => r.GetById(1)).Returns(existingTerm);
            _mockUserRepo.Setup(r => r.GetById(1)).Returns(new User { Id = 1 });

            _service.DeleteTerm(1, 1);

            _mockGlossaryRepo.Verify(r => r.Delete(existingTerm), Times.Once);
            _mockGlossaryRepo.Verify(r => r.SaveChanges(), Times.Once);
        }
    }
}
