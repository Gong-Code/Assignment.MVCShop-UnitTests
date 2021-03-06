using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSuperShop.Data;
using MvcSuperShop.Services;
using System.Collections.Generic;
using System.Linq;

namespace MvcSuperShop.Tests.Services
{
    [TestClass]
    public class CategoryServiceTests : BaseTest
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
        public void At_in_Index_get_trending_categories_should_be_displayed()
        {
            // ARRANGE 
            var categories = fixture.CreateMany<Category>(3).ToList();
           
            context.Categories.AddRange(categories);
            context.SaveChanges();

            // ACT
            var result = sut.GetTrendingCategories(3);

            // ASSERT
            Assert.AreEqual(3, result.Count());
           
        }
    }
}
