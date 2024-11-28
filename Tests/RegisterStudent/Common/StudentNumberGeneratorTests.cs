
namespace RegisterStudent.Common.Tests
{
    public class StudentNumberGeneratorTests
    {
        /// <summary>
        /// Tests the <see cref="StudentNumberGenerator.Generate"/> method to ensure that it returns a string with a length of 10.
        /// </summary>
        [Fact]
        public void Generate_ReturnsStringWithLength10()
        {
            // Act
            var result = StudentNumberGenerator.Generate();

            // Assert
            Assert.Equal(10, result.Length);
        }


        /// <summary>
        /// Tests the <see cref="StudentNumberGenerator.Generate"/> method to ensure that it returns a string
        /// composed entirely of alphanumeric characters.
        /// </summary>
        [Fact]
        public void Generate_ReturnsStringWithAlphanumericCharacters()
        {
            // Act
            var result = StudentNumberGenerator.Generate();

            // Assert
            Assert.All(result, c => Assert.True(char.IsLetterOrDigit(c)));
        }

        [Fact]
        public void Generate_ReturnsDifferentStringEachTime()
        {
            // Act
            var result1 = StudentNumberGenerator.Generate();
            var result2 = StudentNumberGenerator.Generate();

            // Assert
            Assert.NotEqual(result1, result2);
        }
    }
}