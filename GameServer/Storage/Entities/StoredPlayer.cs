namespace Storage.Players
{
    public class StoredPlayer : RedisEntity
    {
        [RedisField("uid")]
        [RedisKey("u")]
        public string UserId { get; set; }

        [RedisField("x")]
        public int X { get; set; }

        [RedisField("y")]
        public int Y { get; set; }

        [RedisField("x")]
        public int SpawnX { get; set; }

        [RedisField("y")]
        public int SpawnY { get; set; }

        [RedisField("s")]
        public int MoveSpeed { get; set; }

        [RedisField("cspr")]
        public int ChestSpriteIndex { get; set; }

        [RedisField("lspr")]
        public int LegsSpriteIndex { get; set; }

        [RedisField("hspr")]
        public int HeadSpriteIndex { get; set; }

        [RedisField("bspr")]
        public int BodySpriteIndex { get; set; }

        [RedisField("l")]
        public string Login { get; set; }

        [RedisField("p")]
        public string Password { get; set; }

        [RedisField("e")]
        public string Email { get; set; }

        [RedisField("sid")]
        public string SessionId { get; set; }

        [RedisField("hp")]
        public int HP { get; set; }

        [RedisField("mhp")]
        public int MaxHp { get; set; }

        [RedisField("atk")]
        public int Atk { get; set; }

        [RedisField("def")]
        public int Def { get; set; }

        [RedisField("atkspeed")]
        public int AtkSpeed { get; set; }

    }
}
