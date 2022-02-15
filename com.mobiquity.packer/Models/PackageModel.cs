using com.mobiquity.packer.Common;
using System.Collections.Generic;

namespace com.mobiquity.packer.Models
{
	public class PackageModel
	{
		public int WeightLimit { get; set; }
		public List<PackageItemModel> PackageItems { get; set; }

		public PackageModel()
		{
			PackageItems = new List<PackageItemModel>();
		}
	}
}
