using System;
using System.Collections.Generic;

namespace com.mobiquity.packer.Common
{
    public static class Helpers
    {
        public static int FetchLargestDecimalPlace(List<decimal> numbers)
        {
            var largestDecimalPlace = 0;

            foreach (var number in numbers)
            {
                var decimalPlace = GetDecimalPlaces(number);

                if (decimalPlace > largestDecimalPlace)
                {
                    largestDecimalPlace = decimalPlace;
                }
            }

            return largestDecimalPlace;
        }

        public static int GetDecimalPlaces(decimal number)
        {
            if ((int)number == number)
            {
                return 0;
            }
            number = Math.Abs(number);
            var decimalPlaces = 0;
            while (number > 0)
            {
                decimalPlaces++;
                number *= 10;
                number -= (int)number;
            }
            return decimalPlaces;
        }
    }
}
