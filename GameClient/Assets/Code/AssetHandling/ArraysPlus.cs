using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Code.AssetHandling
{
    public static class ArraysPlus
    {
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            T[] result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }

        public static IEnumerable<T> SliceRow<T>(this T[,] array, int row)
        {
            for (var i = array.GetLowerBound(0); i <= array.GetUpperBound(0); i++)
            {
                yield return array[i, row];
            }
        }

    }
}
