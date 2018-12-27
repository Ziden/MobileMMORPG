using System;

namespace ServerCore.Utils
{
    public static class ArrayExtensions
    {
        public static void Loop<Type>(this Type[,] array, Action<Type> DoThing)
        {
            for (int k = 0; k < array.GetLength(0); k++)
                for (int l = 0; l < array.GetLength(1); l++)
                    DoThing(array[k,l]);
        }
    }
}
