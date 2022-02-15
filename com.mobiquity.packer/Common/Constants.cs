namespace com.mobiquity.packer.Common
{
	public static class Constants
	{
		public static readonly char FILE_PARSE_PACKAGE_WEIGHT_DELIMITER = ':';
		public static readonly char[] FILE_PARSE_PACKAGE_ITEM_DELIMITER = new char[] { '(', ')' };
		public static readonly char FILE_PARSE_PACKAGE_ITEM_DETAIL_DELIMITER = ',';

		public static readonly int PACKAGE_WEIGHT_MAX = 100;
		public static readonly int PACKAGE_ITEM_WEIGHT_MAX = 100;
		public static readonly int PACKAGE_ITEM_COST_MAX = 100;
	}
}
