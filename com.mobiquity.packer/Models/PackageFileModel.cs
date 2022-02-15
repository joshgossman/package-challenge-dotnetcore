using System.Collections.Generic;

namespace com.mobiquity.packer.Models
{
	public class PackageFileModel
	{
		public List<PackageModel> PackageModels { get; set; }

		public PackageFileModel()
		{
			PackageModels = new List<PackageModel>();
		}
	}
}
