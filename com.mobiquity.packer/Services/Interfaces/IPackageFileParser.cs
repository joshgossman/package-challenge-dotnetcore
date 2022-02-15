using com.mobiquity.packer.Models;

namespace com.mobiquity.packer.Services.Interfaces
{
	public interface IPackageFileParser
	{
		PackageFileModel Parse(string fileContents);
	}
}
