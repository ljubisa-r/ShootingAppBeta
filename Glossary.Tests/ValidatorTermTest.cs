using System;
using GlossaryAPI.DTOs;
using Xunit;

namespace GlossaryAPI.Tests
{
    public class GlossaryTermValidatorTests
    {
        private readonly GlossaryTermValidator _validator;

        public GlossaryTermValidatorTests()
        {
            _validator = new GlossaryTermValidator();
        }

        [Fact]
        public void ValidateTermForPublish_Throws_WhenTermIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _validator.ValidateTermForPublish(null));
        }

        [Fact]
        public void ValidateTermForPublish_Throws_WhenTermNameIsEmpty()
        {
            var dto = new GlossaryTermDTO
            {
                term = " ",
                definition = new string('a', 50)
            };

            var ex = Assert.Throws<ArgumentException>(() => _validator.ValidateTermForPublish(dto));
            Assert.Contains("Term name is required", ex.Message);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("definition less than 30")]
        public void ValidateTermForPublish_Throws_WhenDefinitionIsTooShort(string definition)
        {
            var dto = new GlossaryTermDTO
            {
                term = "ValidTerm",
                definition = definition
            };

            var ex = Assert.Throws<UnauthorizedAccessException>(() => _validator.ValidateTermForPublish(dto));
            Assert.Contains("Definition must be longer than 30 characters", ex.Message);
        }

        [Theory]
        [InlineData("Deffinition contains lorem ipsum test data.")]
        [InlineData("Deffinition with a sample sentence with forbidden content.")]
        [InlineData("Do not use TEST in the definition.")]
        public void ValidateTermForPublish_Throws_WhenDefinitionContainsForbiddenWords(string definition)
        {
            var dto = new GlossaryTermDTO
            {
                term = "ValidTerm",
                definition = definition + new string('a', 40)
            };

            var ex = Assert.Throws<UnauthorizedAccessException>(() => _validator.ValidateTermForPublish(dto));
            Assert.Contains("forbidden words", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ValidateTermForPublish_Passes_WhenValidInput()
        {
            var dto = new GlossaryTermDTO
            {
                term = "Physics",
                definition = "A valid scientific explanation with more than 30 characters."
            };

            // Act & Assert — ne baca izuzetak
            var exception = Record.Exception(() => _validator.ValidateTermForPublish(dto));

            Assert.Null(exception);
        }
    }
}
