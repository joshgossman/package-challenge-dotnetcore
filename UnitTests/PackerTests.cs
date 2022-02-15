using com.mobiquity.packer;
using com.mobiquity.packer.Common;
using com.mobiquity.packer.Exceptions;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using System.IO;

namespace UnitTests
{
	[TestFixture]
	public class PackerTests
	{
		private string _testFilePath = AppDomain.CurrentDomain.BaseDirectory + "testInput";

		[SetUp]
		public void Setup()
		{
			string path = _testFilePath;
			using (StreamWriter sw = File.CreateText(path))
			{
				sw.WriteLine("81 : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)");
				sw.WriteLine("8 : (1,15.3,€34)");
				sw.WriteLine("75 : (1,85.31,€29) (2,14.55,€74) (3,3.98,€16) (4,26.24,€55) (5,63.69,€52) (6,76.25,€75) (7,60.02,€74) (8,93.18,€35) (9,89.95,€78)");
				sw.WriteLine("56 : (1,90.72001,€13) (2,33.80,€40) (3,43.15,€10) (4,37.97,€16) (5,46.81,€36) (6,48.77,€79) (7,81.80,€45) (8,19.36,€79) (9,6.76,€64)");
			}
		}

		[Test]
		public void Packer_Success()
		{
			var result = Packer.Pack(_testFilePath);

			Assert.That(result, Is.EqualTo("4\r\n-\r\n2,7\r\n8,9"));
		}

		[Test]
		public void Packer_Fail_File_Not_Found_Throws_APIException()
		{
			ActualValueDelegate<object> testDelegate = () => Packer.Pack(_testFilePath + "nonsense");

			Assert.That(testDelegate, Throws.TypeOf<APIException>().With.Message.EqualTo($"File cannot be found."));
		}

		[Test]
		public void Packer_Fail_Package_Weight_Limit_Throws_APIException()
		{
			using (StreamWriter sw = File.CreateText(_testFilePath))
			{
				sw.WriteLine($"{Constants.PACKAGE_WEIGHT_MAX + 1} : (1,53.38,€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)");
			}

			ActualValueDelegate<object> testDelegate = () => Packer.Pack(_testFilePath);

			Assert.That(testDelegate, Throws.TypeOf<APIException>().With.Message.EqualTo($"Package weight limit is above the constraint of {Constants.PACKAGE_WEIGHT_MAX}."));
		}

		[Test]
		public void Packer_Fail_Package_Item_Weight_Limit_Throws_APIException()
		{
			using (StreamWriter sw = File.CreateText(_testFilePath))
			{
				sw.WriteLine($"81 : (1,{Constants.PACKAGE_ITEM_WEIGHT_MAX + 1},€45) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)");
			}

			ActualValueDelegate<object> testDelegate = () => Packer.Pack(_testFilePath);

			Assert.That(testDelegate, Throws.TypeOf<APIException>().With.Message.EqualTo($"Package item exceeds maximum weight of {Constants.PACKAGE_ITEM_COST_MAX}."));
		}

		[Test]
		public void Packer_Fail_Package_Item_Cost_Limit_Throws_APIException()
		{
			using (StreamWriter sw = File.CreateText(_testFilePath))
			{
				sw.WriteLine($"81 : (1,53.38,€{Constants.PACKAGE_ITEM_COST_MAX + 1}) (2,88.62,€98) (3,78.48,€3) (4,72.30,€76) (5,30.18,€9) (6,46.34,€48)");
			}

			ActualValueDelegate<object> testDelegate = () => Packer.Pack(_testFilePath);

			Assert.That(testDelegate, Throws.TypeOf<APIException>().With.Message.EqualTo($"Package item exceeds maximum cost of {Constants.PACKAGE_ITEM_WEIGHT_MAX}."));
		}

		private bool IsFileReady(string filename)
		{
			try
			{
				using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
					return inputStream.Length > 0;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
