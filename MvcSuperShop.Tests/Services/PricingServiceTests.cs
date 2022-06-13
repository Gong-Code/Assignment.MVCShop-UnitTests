using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Services;
using System.Collections.Generic;
using System.Linq;

namespace MvcSuperShop.Tests.Services
{
    [TestClass]
    public class PricingServiceTests
    {
        private PricingService sut;

        [TestInitialize]
        public void Initialize()
        {
            sut = new PricingService();
        }

        [TestMethod]
        public void When_no_agreement_exists_product_baseprice_is_used()
        {
            // ARRANGE
            var productList = new List<ProductServiceModel>
            {
                new ProductServiceModel { BasePrice = 987654 }
            };

            var customerContext = new CurrentCustomerContext
            {
                Agreements = new List<Agreement>()
            };

            // ACT
            var products = sut.CalculatePrices(productList, customerContext);

            // ASSERT
            Assert.AreEqual(987654, products.First().Price);
        }

        [TestMethod]
        public void When_agreement_is_found_product_discount_is_used()
        {
            // ARRANGE
            var productList = new List<ProductServiceModel>
            {
                new ProductServiceModel { BasePrice = 530000}
            };

            var customerContext = new CurrentCustomerContext
            {
                Agreements = new List<Agreement>
                {
                    new Agreement()
                    {
                        AgreementRows = new List<AgreementRow>
                        {
                            new AgreementRow()
                            {
                                PercentageDiscount = 6
                            }
                        }
                    }

                }
            };

            // ACT
            var result = sut.CalculatePrices(productList, customerContext);

            // ASSERT
            Assert.AreEqual(498200, result.First().Price);
        }

        [TestMethod]
        public void When_several_agreements_use_biggest_discount()
        {
            // ARRANGE
            var productList = new List<ProductServiceModel>
            {
                new ProductServiceModel
                {
                    BasePrice = 1000,
                    Name = "Impala Hybrid",
                    CategoryName = "Volvo"

                }

            };

            var customerContext = new CurrentCustomerContext
            {
                Agreements = new List<Agreement>
                {
                     new Agreement()
                     {
                        AgreementRows = new List<AgreementRow>
                        {
                            new AgreementRow()
                            {
                                CategoryMatch = "volvo",
                                PercentageDiscount = 10
                            },
                            new AgreementRow()
                            {
                                ProductMatch = "hybrid",
                                PercentageDiscount = 4
                            },

                        }
                     },

                     new Agreement()
                     {
                        AgreementRows = new List<AgreementRow>
                        {
                            new AgreementRow()
                            {
                                PercentageDiscount = 6,
                                CategoryMatch = "van"
                            },
                            new AgreementRow()
                            {
                                PercentageDiscount = 5,
                                ProductMatch = "hybrid"
                            }

                        }
                     }
                }
            };

            // ACT
            var result = sut.CalculatePrices(productList, customerContext);

            // ASSERT
            Assert.AreEqual(900, result.First().Price);
        }
    }

}

