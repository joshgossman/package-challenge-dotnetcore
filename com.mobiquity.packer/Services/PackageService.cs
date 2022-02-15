using com.mobiquity.packer.Common;
using com.mobiquity.packer.Models;
using com.mobiquity.packer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.mobiquity.packer.Services
{
	public class PackageService : IPackageService
	{
		private IPackageFileParser _packageFileParser;

		public PackageService(IPackageFileParser packageFileParser)
		{
			_packageFileParser = packageFileParser;
		}

		public PackageFileModel ParsePackageFile(string fileContents)
		{
			return _packageFileParser.Parse(fileContents);
		}

		public List<PackageModel> SortPackages(List<PackageModel> packages)
		{
			var result = new List<PackageModel>();
			foreach (var package in packages)
			{
				// Clean up the list so we only work with relevant items
				package.PackageItems.RemoveAll(x => x.Weight > package.WeightLimit);

				// Order list to ensure items that weigh less will take precedence if others cost the same
				package.PackageItems = package.PackageItems.OrderBy(x => x.Weight).ToList();

				var knapsackResult = KnapSackCalculatorOnPackageItems(package.PackageItems, package.WeightLimit);

				result.Add(new PackageModel
				{
					WeightLimit = package.WeightLimit,
					PackageItems = knapsackResult.OrderBy(x => x.Index).ToList()
				}); ;
			}

			return result;
		}

		public string ParseSortedPackagesToResult(List<PackageModel> packages)
		{
			var sb = new StringBuilder();
			foreach (var package in packages)
			{
				foreach (var item in package.PackageItems)
				{
					sb.Append(item.Index);
					if (item != package.PackageItems.Last())
					{
						sb.Append(",");
					}
				}

				if (!package.PackageItems.Any())
				{
					sb.Append("-");
				}

				if (package != packages.Last())
				{
					sb.AppendLine();
				}
			}

			return sb.ToString();
		}

		private List<PackageItemModel> KnapSackCalculatorOnPackageItems(List<PackageItemModel> packageItems, int weightLimit)
		{
			var result = new List<PackageItemModel>();

			// Get decimal weights as factor to turn decimal into integers for use as indexes
			var decimalWeightFactor = (int)Math.Pow(10, Helpers.FetchLargestDecimalPlace(packageItems.Select(x => x.Weight).ToList()));
			var factoredWeightLimit = weightLimit * decimalWeightFactor;

			// Following solution derived from https://dotnetcoretutorials.com/2020/04/22/knapsack-algorithm-in-c/
			var itemCount = packageItems.Count;

			int[,] matrix = new int[itemCount + 1, factoredWeightLimit + 1];

			for (int i = 0; i <= itemCount; i++)
			{
				for (int w = 0; w <= factoredWeightLimit; w++)
				{
					if (i == 0 || w == 0)
					{
						matrix[i, w] = 0;
						continue;
					}

					var currentItemIndex = i - 1;
					var currentItem = packageItems[currentItemIndex];
					if (currentItem.Weight * decimalWeightFactor <= w)
					{
						matrix[i, w] = Math.Max(currentItem.Cost + matrix[i - 1, w - (int)(currentItem.Weight * decimalWeightFactor)], matrix[i - 1, w]);
					}
					else
					{
						matrix[i, w] = matrix[i - 1, w];
					}
				}
			}

			// Determine which package items are in the results
			var res = matrix[itemCount, factoredWeightLimit];

			var weight = factoredWeightLimit;
			for (var i = itemCount; i > 0 && res > 0; i--)
			{
				if (res == matrix[i - 1, weight])
					continue;
				else
				{
					result.Add(packageItems[i - 1]);

					res = res - packageItems[i - 1].Cost;
					weight = weight - (int)(packageItems[i - 1].Weight * decimalWeightFactor);
				}
			}
			return result;
		}
	}
}
