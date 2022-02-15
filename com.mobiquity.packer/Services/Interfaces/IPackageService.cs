using com.mobiquity.packer.Models;
using System.Collections.Generic;

namespace com.mobiquity.packer.Services.Interfaces
{
	public interface IPackageService
	{
		PackageFileModel ParsePackageFile(string filePath);
		List<PackageModel> SortPackages(List<PackageModel> packages);
		string ParseSortedPackagesToResult(List<PackageModel> packages);
	}
}
