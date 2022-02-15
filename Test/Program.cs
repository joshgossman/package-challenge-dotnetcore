using com.mobiquity.packer;
using System;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			var result = Packer.Pack("C:\\temp\\packageInput");
			Console.WriteLine(result);

			Console.ReadLine();
		}
	}
}
