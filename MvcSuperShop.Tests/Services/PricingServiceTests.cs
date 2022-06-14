using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcSuperShop.Data;
using MvcSuperShop.Infrastructure.Context;
using MvcSuperShop.Services;
using System;
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
        public void When_agreement_is_not_valid_use_BasePrice()
        {
            // ARRANGE
            var productList = new List<ProductServiceModel>
            {
                new ProductServiceModel
                {
                    BasePrice = 1000,
                    Name = "Challenger",
                    CategoryName = "Mini van"

                }

            };

            var customerContext = new CurrentCustomerContext()
            {
                Agreements = new List<Agreement>
                {
                    new Agreement
                    {
                        ValidFrom = new DateTime(2019, 01, 02),
                        ValidTo = new DateTime(2020, 04, 06),

                        AgreementRows = new List<AgreementRow>
                        {
                            
                            new AgreementRow
                            {
                                PercentageDiscount = 5,
                                ProductMatch = "hybrid"
                            },
                            new AgreementRow
                            {
                                PercentageDiscount = 6,
                                CategoryMatch = "van"
                            }
                        }
                    }
                }
            };

            // ACT
            var result = sut.CalculatePrices(productList, customerContext);

            // ASSERT
            Assert.AreEqual(1000, result.First().Price);
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
            var result = sut.CalculatePrices(productList, customerContext);

            // ASSERT
            Assert.AreEqual(987654, result.First().Price);
        }

        [TestMethod]
        public void When_single_category_are_matched_use_only_biggest_discount()
        {
            // ARRANGE
            var productList = new List<ProductServiceModel>
            {
                new ProductServiceModel
                {
                    BasePrice = 1000,
                    Name = "Challenger",
                    CategoryName = "Mini van"

                }

            };

            var customerContext = new CurrentCustomerContext()
            {
                Agreements = new List<Agreement>
                {
                    new Agreement
                    {
                        ValidTo = new DateTime(2032, 06, 01),

                        AgreementRows = new List<AgreementRow>
                        {
                            new AgreementRow
                            {
                                PercentageDiscount = 5,
                                ProductMatch = "hybrid"
                            },
                            new AgreementRow
                            {
                                PercentageDiscount = 6,
                                CategoryMatch = "van"
                            }
                        }
                    }
                }
            };

            // ACT
            var result = sut.CalculatePrices(productList, customerContext);

            // ASSERT
            Assert.AreEqual(940, result.First().Price);
        }

        [TestMethod]
        public void When_two_discount_are_triggered_use_biggest_discount()
        {
            // ARRANGE
            var productList = new List<ProductServiceModel>
            {
                new ProductServiceModel
                {
                    BasePrice = 1000,
                    Name = "Coupe",
                    CategoryName = "Mini Van"

                }

            };

            var customerContext = new CurrentCustomerContext()
            {
                Agreements = new List<Agreement>
                {
                    new Agreement
                    {
                        ValidTo = new DateTime(2032, 06, 01),

                        AgreementRows = new List<AgreementRow>
                        {
                            new AgreementRow
                            {
                                PercentageDiscount = 5,
                                ProductMatch = "hybrid"
                            },
                            new AgreementRow
                            {
                                PercentageDiscount = 6,
                                CategoryMatch = "van"
                            }
                        }
                    }
                }
            };

            // ACT
            var result = sut.CalculatePrices(productList, customerContext);

            // ASSERT
            Assert.AreEqual(940, result.First().Price);
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
                    Name = "XC60 Hybrid",
                    CategoryName = "Volvo"

                }

            };

            var customerContext = new CurrentCustomerContext
            {
                Agreements = new List<Agreement>
                {
                     new Agreement()
                     {
                        ValidTo = new DateTime(2032, 06, 01),

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
                        ValidTo = new DateTime(2032, 06, 01),

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

