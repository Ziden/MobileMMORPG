using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonCode.Pathfinder
{
    public class ArrayPrinter
    {
        public static void PrintArray(byte[,] arr)
        {
            int rowLength = arr.GetLength(0);
            int colLength = arr.GetLength(1);

            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", arr[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.ReadLine();
        }
    }
}
