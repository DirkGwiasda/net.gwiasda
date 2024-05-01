using Moq;
using Net.Gwiasda.FiMa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace net.gwiasda.fima.reports.tests
{
    public class CreateMonthlyReportWorkflowTests
    {
        private Mock<ICategoryManager> _categoryManagerMock;
        private Mock<IGetBookingsFromMonthWorkflow> _getBookingsFromMonthWorkflowMock;
        private CreateMonthlyReportWorkflow _test;

        private void Setup()
        {
            _categoryManagerMock = new Mock<ICategoryManager>();
            _getBookingsFromMonthWorkflowMock = new Mock<IGetBookingsFromMonthWorkflow>();
            _test = new CreateMonthlyReportWorkflow(_categoryManagerMock.Object, _getBookingsFromMonthWorkflowMock.Object);
        }

        [Fact]
        public void ForceParentCategoriesExist()
        {
            // Arrange
            Setup();
            var rootId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var categories = new List<FinanceCategory>
            {
                new CostCategory { Id = rootId, Name = "Root" },
                new CostCategory { Id = parentId, Name = "Child", ParentId = rootId },
                new CostCategory { Id = Guid.NewGuid(), Name = "Child", ParentId = parentId }
            };
            var categoryReports = new List<CategoryReport>
            {
                new CategoryReport { Category = categories[2], IsCost = true }
            };

            // Act
            _test.ForceParentCategoriesExist(categoryReports, categories);

            // Assert
            Assert.Equal(3, categoryReports.Count);
            Assert.Contains(categoryReports, (cr => cr?.Category?.Id == rootId));
            Assert.Contains(categoryReports, (cr => cr?.Category?.Id == parentId));
        }
        [Fact]
        public void CreateCategoryTree()
        {
            Setup();
            var rootId = Guid.NewGuid();
            var parentId = Guid.NewGuid();
            var childId = Guid.NewGuid();
            var categories = new List<FinanceCategory>
            {
                new CostCategory { Id = rootId, Name = "Root" },
                new CostCategory { Id = parentId, Name = "Child", ParentId = rootId },
                new CostCategory { Id = childId, Name = "Child", ParentId = parentId }
            };
            var categoryReports = new List<CategoryReport>
            {
                new CategoryReport { Category = categories[2], IsCost = true },
                new CategoryReport { Category = categories[0], IsCost = true },
                new CategoryReport { Category = categories[1], IsCost = true }
            };

            var result = _test.CreateCategoryTrees(categoryReports);
            Assert.Single(result);
            var root = result.First();
            Assert.Equal(rootId, root.Category.Id);

            Assert.Single(root.ChildCategories);
            var parent = root.ChildCategories.First();
            Assert.Equal(parentId, parent.Category.Id);

            Assert.Single(parent.ChildCategories);
            var child = parent.ChildCategories.First();
            Assert.Equal(childId, child.Category.Id);
        }
        [Fact]
        public void AddSumToParentSum()
        {
            Setup();
            var rootId = Guid.NewGuid();
            var root2Id = Guid.NewGuid();
            var childId = Guid.NewGuid();
            var childChild1Id = Guid.NewGuid();
            var childChild2Id = Guid.NewGuid();
            var categories = new List<FinanceCategory>
            {
                new CostCategory { Id = childChild2Id, Hierarchy = 2, ParentId = childId, Name = "childChild2" },
                new CostCategory { Id = rootId, Name = "root" },
                new CostCategory { Id = childId, Hierarchy = 1, ParentId = rootId, Name = "child" },
                new CostCategory { Id = childChild1Id, Hierarchy = 2, ParentId = childId, Name = "childChild1" },
                new CostCategory { Id = root2Id, Name = "root2" }
            };
            var categoryReports = new List<CategoryReport>
            {
                new CategoryReport { Category = categories[4], Sum = 38.2m },
                new CategoryReport { Category = categories[2], Sum = 12.3m },
                new CategoryReport { Category = categories[0], Sum = 8.6m },
                new CategoryReport { Category = categories[1], Sum = 20 },
                new CategoryReport { Category = categories[3], Sum = 5 },
            };


            _test.AddSumToParentSum(categoryReports);


            var item = categoryReports.Where(cr => cr.Category.Id == root2Id).First();
            Assert.Equal(38.2m, item.Sum);

            item = categoryReports.Where(cr => cr.Category.Id == childChild1Id).First();
            Assert.Equal(5, item.Sum);

            item = categoryReports.Where(cr => cr.Category.Id == childChild2Id).First();
            Assert.Equal(8.6m, item.Sum);

            item = categoryReports.Where(cr => cr.Category.Id == childId).First();
            Assert.Equal(25.9m, item.Sum);

            item = categoryReports.Where(cr => cr.Category.Id == rootId).First();
            Assert.Equal(45.9m, item.Sum);
        }
    }
}