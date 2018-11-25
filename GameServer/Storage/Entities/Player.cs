namespace Storage.Players
{
    public class Player : RedisEntity
    {
        [RedisField("uid")]
        [RedisKey("u")]
        public string UserId { get; set; }

        [RedisField("x")]
        public int X { get; set; }

        [RedisField("y")]
        public int Y { get; set; }

        [RedisField("s")]
        public int MoveSpeed { get; set; }

        [RedisField("si")]
        public int SpriteIndex { get; set; }

        [RedisField("l")]
        public string Login { get; set; }

        [RedisField("p")]
        public string Password { get; set; }

        [RedisField("e")]
        public string Email { get; set; }

        [RedisField("sid")]
        public string SessionId { get; set; }
    }
}
