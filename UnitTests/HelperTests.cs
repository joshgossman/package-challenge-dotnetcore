using com.mobiquity.packer.Common;
using NUnit.Framework;
using System.Collections.Generic;

namespace UnitTests
{
	[TestFixture]
	public class HelperTests
	{
		[Test]
		public void FetchLargestDecimalPlace()
		{
			var input = new List<decimal> { 1.01m, 1.1m, 1.0001m };

			var result = Helpers.FetchLargestDecimalPlace(input);

			Assert.That(result, Is.EqualTo(4));
		}

		[TestCase(1, ExpectedResult = 0)]
		[TestCase(1.1, ExpectedResult = 1)]
		[TestCase(1.01, ExpectedResult = 2)]
		[TestCase(1.0, ExpectedResult = 0)]
		[TestCase(2.10, ExpectedResult = 1)]
		[TestCase(-0.1, ExpectedResult = 1)]
		public int GetDecimalPlaces(decimal input)
		{
			var result = Helpers.GetDecimalPlaces(input);

			return result;
		}
	}
}
