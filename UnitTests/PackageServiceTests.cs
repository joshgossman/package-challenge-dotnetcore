using com.mobiquity.packer.Exceptions;
using com.mobiquity.packer.Models;
using com.mobiquity.packer.Services;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests
{
    [TestFixture]
    public class PackageServiceTests
    {
        private PackageService _packageService;

        [SetUp]
        public void Setup()
        {
            _packageService = new PackageService(new PackageFileParser());
        }

        [Test]
        public void Parse_Success()
        {
            var contents = new StringBuilder();
            contents.AppendLine("81 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)");
            contents.AppendLine("8 : (1,15.3,€34)");

            var result = _packageService.ParsePackageFile(contents.ToString());

            Assert.That(result.PackageModels.Count, Is.EqualTo(2));
            Assert.That(result.PackageModels[0].WeightLimit, Is.EqualTo(81));
            Assert.That(result.PackageModels[0].PackageItems.Count, Is.EqualTo(6));
            Assert.That(result.PackageModels[0].PackageItems[0].Index, Is.EqualTo(1));
            Assert.That(result.PackageModels[0].PackageItems[0].Weight, Is.EqualTo(53.38));
            Assert.That(result.PackageModels[0].PackageItems[0].Cost, Is.EqualTo(45));
        }

        [Test]
        public void Parse_IncorrectFormat()
        {
            var contents = new StringBuilder();
            contents.AppendLine("(81) (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)");
            contents.AppendLine("8 : (1,15.3,€34)");

            ActualValueDelegate<object> result = () => _packageService.ParsePackageFile(contents.ToString());

            Assert.That(result, Throws.TypeOf<Exception>());
        }

        [Test]
        public void Sort_Package_Items_Success_Highest_Cost()
        {
            var packageModels = new List<PackageModel>
            {
                new PackageModel{
                    WeightLimit = 100,
                    PackageItems = new List<PackageItemModel>
                    {
                        new PackageItemModel{ Index = 1, Weight = 50, Cost = 50 },
                        new PackageItemModel{ Index = 2, Weight = 50, Cost = 45 },
                        new PackageItemModel{ Index = 3, Weight = 50, Cost = 55 },
                    }
                }
            };

            var result = _packageService.SortPackages(packageModels);

            Assert.That(result[0].PackageItems.Count, Is.EqualTo(2));
            Assert.That(result[0].PackageItems[0].Index, Is.EqualTo(1));
            Assert.That(result[0].PackageItems[1].Index, Is.EqualTo(3));
        }

        [Test]
        public void Sort_Package_Items_Success_Lowest_Weight()
        {
            var packageModels = new List<PackageModel>
            {
                new PackageModel{
                    WeightLimit = 100,
                    PackageItems = new List<PackageItemModel>
                    {
                        new PackageItemModel{ Index = 1, Weight = 50, Cost = 50 },
                        new PackageItemModel{ Index = 2, Weight = 50, Cost = 50 },
                        new PackageItemModel{ Index = 3, Weight = 45, Cost = 50 },
                    }
                }
            };

            var result = _packageService.SortPackages(packageModels);



            Assert.That(result[0].PackageItems.Count, Is.EqualTo(2));
            Assert.That(result[0].PackageItems[0].Index, Is.EqualTo(1));
            Assert.That(result[0].PackageItems[1].Index, Is.EqualTo(3));
        }

        [Test]
        public void Parse_Sorted_Package_Items_Layout_Success_Multiple_Lines()
        {
            var packageModels = new List<PackageModel>
            {
                new PackageModel{
                    WeightLimit = 100,
                    PackageItems = new List<PackageItemModel>
                    {
                        new PackageItemModel{ Index = 1, Weight = 50, Cost = 50 },
                        new PackageItemModel{ Index = 2, Weight = 50, Cost = 50 }
                    }
                },
                new PackageModel{
                    WeightLimit = 100,
                    PackageItems = new List<PackageItemModel>
                    {
                        new PackageItemModel{ Index = 3, Weight = 50, Cost = 50 },
                        new PackageItemModel{ Index = 5, Weight = 50, Cost = 50 }
                    }
                }
            };

            var result = _packageService.ParseSortedPackagesToResult(packageModels);

            Assert.That(result, Is.EqualTo("1,2\r\n3,5"));
        }

        [Test]
        public void Parse_Sorted_Package_Items_Layout_Success_No_Results()
        {
            var packageModels = new List<PackageModel>
            {
                new PackageModel{
                    WeightLimit = 100,
                    PackageItems = new List<PackageItemModel>
                    {
                        new PackageItemModel{ Index = 1, Weight = 50, Cost = 50 },
                        new PackageItemModel{ Index = 2, Weight = 50, Cost = 50 }
                    }
                },
                new PackageModel{
                    WeightLimit = 100,
                    PackageItems = new List<PackageItemModel>()
                }
            };

            var result = _packageService.ParseSortedPackagesToResult(packageModels);

            Assert.That(result, Is.EqualTo("1,2\r\n-"));
        }
    }
}
