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
    public class HomeControllerTests
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
            .Returns(new List<Category>
            {
                new Category(),
                new Category(),
                new Category()
            });

            //När Map anropas med en Lista av CategoryViewModel och vill ha ut en lista med Categories ska du returnera en lista med CategoryViewModel
            mapperMock.Setup(m => m.Map<List<CategoryViewModel>>(It.IsAny<List<Category>>()))
            .Returns(new List<CategoryViewModel>
            {
                new CategoryViewModel(),
                new CategoryViewModel(),
                new CategoryViewModel()
            });
           
            // ACT
            var result = sut.Index() as ViewResult;
            var model = result.Model as HomeIndexViewModel;

            // ASSERT
            Assert.AreEqual(3, model.TrendingCategories.Count);
        }

        [TestMethod]
        public void Index_should_return_correct_view()
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

            // ACT
            var result = sut.Index() as ViewResult;

            // ASSERT
            Assert.IsNull(result.ViewName);
        }

        [TestMethod]
        public void Index_should_set_new_10_products()
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

            var customer = new CurrentCustomerContext
            {
                UserId = Guid.NewGuid(),
                Email = "blablabla@gmail.com"
            };

            productServiceMock.Setup(p => p.GetNewProducts(10, customer)).Returns(new List<ProductServiceModel>
            {
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel(),
                new ProductServiceModel()
            });

            mapperMock.Setup(m => m.Map<List<ProductBoxViewModel>>(It.IsAny<IEnumerable<ProductServiceModel>>()))
            .Returns(new List<ProductBoxViewModel>
            {
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel(),
               new ProductBoxViewModel()
            });

            // ACT
            var result = sut.Index() as ViewResult;

            var model = result.Model as HomeIndexViewModel;

            // ASSERT
            Assert.AreEqual(10, model.NewProducts.Count);
        }

    }
}
