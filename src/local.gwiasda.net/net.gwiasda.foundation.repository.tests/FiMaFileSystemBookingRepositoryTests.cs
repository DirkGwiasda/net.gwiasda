using Moq;
using Net.Gwiasda;
using Net.Gwiasda.Local.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.gwiasda.foundation.repository.tests
{
    public class FiMaFileSystemBookingRepositoryTests
    {
        [Fact]
        public void GetFQBookingFileName_ReturnsCorrectFileName()
        {
            // Arrange
            var test = new FiMaFileSystemBookingRepository();

            var expectedFileName = Path.Combine(
                test.GetBaseDirectory(FiMaFileSystemBookingRepository.BookingDirectory), 
                "2024_03", 
                $"2024_03_20.{FiMaFileSystemBookingRepository.BookingFileExtension}");

            // Act
            var actualFileName = test.GetFQBookingFileName(new DateTime(2024, 3, 20));

            // Assert
            Assert.Equal(expectedFileName, actualFileName);
        }

        [Fact]
        public void GetBookingMonthDirectory_ReturnsCorrectDirectory()
        {
            // Arrange
            var test = new FiMaFileSystemBookingRepository();

            var expectedDirectory = Path.Combine(
                test.GetBaseDirectory(FiMaFileSystemBookingRepository.BookingDirectory),"2024_03");

            // Act
            var actualDirectory = test.GetBookingMonthDirectory(new DateTime(2024, 3, 20));

            // Assert
            Assert.Equal(expectedDirectory, actualDirectory);
            Assert.True(Directory.Exists(actualDirectory));
        }
    }
}
