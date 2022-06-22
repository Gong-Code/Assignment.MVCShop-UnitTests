using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MvcSuperShop.Controllers;
using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Services;
using MvcSuperShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MvcSuperShop.Tests.Controllers
{

    [TestClass]
    public class HomeControllerTests : BaseControllerTests
    {
        private HomeController sut;
        private Mock<ICategoryService> categoryServiceMock;
        private Mock<IProductService> productServiceMock;
        private Mock<IMapper> mapperMock;
        private ApplicationDbContext context;

        [TestInitialize]
        public void Initialize()
        {
            categoryServiceMock = new Mock<ICategoryService>();
            productServiceMock = new Mock<IProductService>();
            mapperMock = new Mock<IMapper>();

            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            context = new ApplicationDbContext(contextOptions);
            context.Database.EnsureCreated();

            sut = new HomeController(categoryServiceMock.Object,
                productServiceMock.Object,
                mapperMock.Object,
                context);
        }
        
        [TestMethod]
        public void Index_should_show_3_categories()
        {
            // ARRANGE
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "gunnar@somecompany.com")
                //other required and custom claims
            }, "TestAuthentication"));

            sut.ControllerContext = new ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext()
            {
                User = user
            };

            categoryServiceMock.Setup(e => e.GetTrendingCategories(3))
            .Returns(fixture.CreateMany<Category>(3).ToList());          

            mapperMock.Setup(m => m.Map<List<CategoryViewModel>>(It.IsAny<List<Category>>()))
            .Returns(fixture.CreateMany<CategoryViewModel>(3).ToList());           

            // ACT
            var result = sut.Index() as ViewResult;
            var model = result.Model as HomeIndexViewModel;

            // ASSERT
            Assert.AreEqual(3, model.TrendingCategories.Count);
        }

        [TestMethod]
        public void Index_should_return_correct_view()
        {
            //ARRANGE
            sut.ControllerContext = SetupControllerContext();

            // ACT
            var result = sut.Index() as ViewResult;

            // ASSERT
            Assert.IsNull(result.ViewName);
        }

        [TestMethod]
        public void Index_should_set_new_10_products()
        {
            // ARRANGE
            sut.ControllerContext = SetupControllerContext();

            productServiceMock.Setup(p => p.GetNewProducts(10,It.IsAny<CurrentCustomerContext>()))
                .Returns(fixture.CreateMany<ProductServiceModel>(10).ToList());

            mapperMock.Setup(m => m.Map<List<ProductBoxViewModel>>(It.IsAny<IEnumerable<ProductServiceModel>>()))
            .Returns(fixture.CreateMany<ProductBoxViewModel>(10).ToList());
           
            // ACT
            var result = sut.Index() as ViewResult;

            var model = result.Model as HomeIndexViewModel;

            // ASSERT
            Assert.AreEqual(10, model.NewProducts.Count);
        }

    }
}
