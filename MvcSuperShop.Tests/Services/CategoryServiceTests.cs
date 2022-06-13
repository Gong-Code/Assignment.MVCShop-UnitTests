using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSuperShop.Data;
using MvcSuperShop.Services;
using System.Collections.Generic;
using System.Linq;

namespace MvcSuperShop.Tests.Services
{
    [TestClass]
    public class CategoryServiceTests
    {      
        private ApplicationDbContext context;
        private CategoryService sut;

        [TestInitialize]
        public void Initialize()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Hej")
                .Options;

            context = new ApplicationDbContext(contextOptions);
            context.Database.EnsureCreated();

            sut = new CategoryService(context);
        }

        [TestMethod]
        public void When_in_Index_get_trending_categories_should_be_displayed()
        {
            // ARRANGE dasdasdsadsadas
            var categories = new List<Category>
            {
                new Category
                {
                    Name = "sdasdas",
                    Icon = "dasd",
                },
                new Category
                {
                    Name = "gfsdg",
                    Icon = "gdfg",
                },
                new Category
                {
                    Name = "uyutio",
                    Icon = "ghjhk",
                }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            // ACT
            var result = sut.GetTrendingCategories(3);

            // ASSERT
            Assert.AreEqual(3, result.Count());
           
        }
    }
}
