using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace MvcSuperShop.Tests.Controllers
{
    public class BaseControllerTests : BaseTest
    {
        protected ControllerContext SetupControllerContext()
        {
            // ARRANGE
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, "gunnar@somecompany.com")
                //other required and custom claims
            }, "TestAuthentication"));

            var controllerContext =  new ControllerContext();
            controllerContext.HttpContext = new DefaultHttpContext()
            {
                User = user
            };

            return controllerContext;
        }
        
    }
}
