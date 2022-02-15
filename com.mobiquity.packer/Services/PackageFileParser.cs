using com.mobiquity.packer.Common;
using com.mobiquity.packer.Models;
using com.mobiquity.packer.Services.Interfaces;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace com.mobiquity.packer.Services
{
    public class PackageFileParser : IPackageFileParser
    {
        public PackageFileModel Parse(string fileContents)
        {
            try
            {
                var result = new PackageFileModel();

                var file = new StringReader(fileContents);
                var line = "";

                while ((line = file.ReadLine()) != null)
                {
                    var packageResult = new PackageModel();

                    var lineClean = line.Replace(" ", "");

                    var lineItems = lineClean.Split(Constants.FILE_PARSE_PACKAGE_WEIGHT_DELIMITER);
                    var packageItems = lineItems[1].Split(Constants.FILE_PARSE_PACKAGE_ITEM_DELIMITER, StringSplitOptions.RemoveEmptyEntries);

                    packageResult.WeightLimit = int.Parse(lineItems[0]);
                    foreach (var packageItem in packageItems)
                    {
                        var packageItemDetails = packageItem.Split(Constants.FILE_PARSE_PACKAGE_ITEM_DETAIL_DELIMITER);

                        packageResult.PackageItems.Add(new PackageItemModel
                        {
                            Index = int.Parse(packageItemDetails[0]),
                            Weight = decimal.Parse(packageItemDetails[1], CultureInfo.InvariantCulture),
                            Cost = int.Parse(Regex.Match(packageItemDetails[2], @"\d+").Value) //Remove non-numeric data and parse to int
                        });
                    }

                    result.PackageModels.Add(packageResult);
                }

                file.Close();

                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Unable to parse file", e);
            }
        }
    }
}
