namespace CommonCode.EntityShared
{
    public class Formulas
    {
        // Gets time in MS
        public static long GetTimeToMoveBetweenTwoTiles(int speed)
        {
            return 6000 / speed;
        }
    }
}
