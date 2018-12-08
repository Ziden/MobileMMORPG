namespace Storage.Players
{
    public class PlayerService
    {
        public static void UpdatePlayerPosition(string uid, int x, int y)
        {
            var fields = new string[2]
            {
                "X",
                "Y"
            };
            var values = new object[2]
            {
                x,
                y
            };
            RedisHash<StoredPlayer>.Update(uid, fields, values);
        }
    }
}
