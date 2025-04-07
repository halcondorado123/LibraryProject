using LibraryProject.Application.Services.Location;
using LibraryProject.Domain.Entities.Location;
using LibraryProject.Infraestructure.Interface.Location;
using Moq;
using Xunit;

namespace LibraryProject.Tests.Application.Services.Location
{
    public class CountryServiceTests
    {
        private readonly Mock<ICountryRepository> _mockRepo;
        private readonly CountryApplication _service;

        public CountryServiceTests()
        {
            _mockRepo = new Mock<ICountryRepository>();
            _service = new CountryService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllCountriesAsync_ShouldReturnAllCountries()
        {
            // Arrange
            var testCountries = new List<CountryME>
            {
                new CountryME { CountryId = 1, IsoCode = "US", Country = "United States" },
                new CountryME { CountryId = 2, IsoCode = "CA", Country = "Canada" },
                new CountryME { CountryId = 3, IsoCode = "MX", Country = "Mexico" }
            };

            _mockRepo.Setup(repo => repo.GetAllCountriesAsync())
                    .ReturnsAsync(testCountries);

            // Act
            var result = await _service.GetAllCountriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            Assert.Contains(result, c => c.IsoCode == "CA");
            _mockRepo.Verify(repo => repo.GetAllCountriesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllCountriesAsync_WhenRepositoryFails_ShouldThrowException()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllCountriesAsync())
                    .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.GetAllCountriesAsync());
        }

        [Fact]
        public async Task GetAllCountriesAsync_WhenNoCountriesExist_ShouldReturnEmptyList()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAllCountriesAsync())
                    .ReturnsAsync(new List<CountryME>());

            // Act
            var result = await _service.GetAllCountriesAsync();

            // Assert
            Assert.Empty(result);
        }
    }
}