using com.mobiquity.packer.Common;
using com.mobiquity.packer.Exceptions;
using com.mobiquity.packer.Services;
using System;
using System.IO;
using System.Linq;

namespace com.mobiquity.packer
{
	public static class Packer
	{
		public static string Pack(string filePath)
		{
			try
			{
				if (!File.Exists(filePath))
				{
					throw new APIException("File cannot be found.");
				}

				var packageService = new PackageService(new PackageFileParser());

				var fileContents = File.ReadAllText(filePath);
				var packageFile = packageService.ParsePackageFile(fileContents);

				if (packageFile.PackageModels.Any(x => x.WeightLimit > Constants.PACKAGE_WEIGHT_MAX))
				{
					throw new APIException($"Package weight limit is above the constraint of {Constants.PACKAGE_WEIGHT_MAX}.");
				}
				if (packageFile.PackageModels.SelectMany(s => s.PackageItems).Any(x => x.Cost > Constants.PACKAGE_ITEM_COST_MAX))
				{
					throw new APIException($"Package item exceeds maximum cost of {Constants.PACKAGE_ITEM_COST_MAX}.");
				}
				if (packageFile.PackageModels.SelectMany(s => s.PackageItems).Any(x => x.Weight > Constants.PACKAGE_ITEM_WEIGHT_MAX))
				{
					throw new APIException($"Package item exceeds maximum weight of {Constants.PACKAGE_ITEM_WEIGHT_MAX}.");
				}

				var sortedPackages = packageService.SortPackages(packageFile.PackageModels);
				var result = packageService.ParseSortedPackagesToResult(sortedPackages);

				return result;
			}
			catch (Exception e)
			{
				throw new APIException(e.Message, e);
			}
		}
	}
}
