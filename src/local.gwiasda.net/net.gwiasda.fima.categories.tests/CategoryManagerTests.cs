using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Net.Gwiasda.FiMa.Tests
{
    public class CategoryManagerTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<ICategoryValidator> _categoryValidatorMock;
        private readonly CategoryManager _categoryManager;

        public CategoryManagerTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _categoryValidatorMock = new Mock<ICategoryValidator>();
            _categoryManager = new CategoryManager(_categoryRepositoryMock.Object, _categoryValidatorMock.Object);
        }

        [Fact]
        public async Task GetCategoriesAsync_ReturnsSortedCategories()
        {
            var root0 = Guid.NewGuid();
            var root1 = Guid.NewGuid();
            var element0_1 = Guid.NewGuid();
            // Arrange
            var categories = new List<CostCategory>
            {
                new CostCategory { Id = Guid.NewGuid(), ParentId = root0, Position = 1 },
                new CostCategory { Id = root1, ParentId = null, Position = 1 },
                new CostCategory { Id = root0, ParentId = null, Position = 0 },
                new CostCategory { Id = element0_1, ParentId = root0, Position = 0 },
                new CostCategory { Id = Guid.NewGuid(), ParentId = root1, Position = 1 },
                new CostCategory { Id = Guid.NewGuid(), ParentId = element0_1, Position = 0 },
                new CostCategory { Id = Guid.NewGuid(), ParentId = root1, Position = 0 }
            };

            _categoryRepositoryMock.Setup(x => x.GetCategoriesAsync<FinanceCategory>())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryManager.GetCategoriesAsync<FinanceCategory>();

            // Assert
            Assert.Equal(7, result.Count());
            Assert.Equal(categories[2], result.ElementAt(0));
            Assert.Equal(categories[3], result.ElementAt(1));
            Assert.Equal(categories[5], result.ElementAt(2));
            Assert.Equal(categories[0], result.ElementAt(3));
            Assert.Equal(categories[1], result.ElementAt(4));
            Assert.Equal(categories[6], result.ElementAt(5));
            Assert.Equal(categories[4], result.ElementAt(6));

            _categoryRepositoryMock.Verify(x => x.GetCategoriesAsync<FinanceCategory>(), Times.Once);
        }
    }
}
