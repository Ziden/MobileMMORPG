namespace Storage.Players
{
    public class PlayerService
    {
        public static void UpdatePlayerPosition(Player player, int x, int y)
        {
            var fields = new string[2]
            {
                nameof(player.X),
                nameof(player.Y)
            };
            var values = new object[2]
            {
                x,
                y
            };
            RedisHash<Player>.Update(player, fields, values);
        }
    }
}
