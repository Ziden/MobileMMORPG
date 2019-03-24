using Common.Entity;
using CommonCode.EntityShared;
using CommonCode.Networking.Packets;
using MapHandler;
using ServerCore.Assets;
using ServerCore.Game.Entities;
using ServerCore.Networking;
using Storage.Players;

namespace ServerCore.GameServer.Players
{
    public class OnlinePlayer : LivingEntity, IClientLayeredRenderable
    {
        public OnlinePlayer()
        {
            EntityType = EntityType.PLAYER;
        }

        public ConnectedClientTcpHandler Tcp;

        // Becomes true when all assets have been loaded so its a "valid" online player
        public bool AssetsReady = false;

        // When this player can perform a movement again (in MS)
        public long CanMoveAgainTime = 0;


        private SpriteAsset BodySprite = new SpriteAsset()
        {
            ImageName = DefaultAssets.SPR_BODIES,
            SpriteRowIndex = 2,
        };

        private SpriteAsset LegSprite = new SpriteAsset()
        {
            ImageName = DefaultAssets.SPR_LEGS,
            SpriteRowIndex = 2,
        };

        private SpriteAsset HeadSprite = new SpriteAsset()
        {
            ImageName = DefaultAssets.SPR_HEADS,
            SpriteRowIndex = 2,
        };

        private SpriteAsset ChestSprite = new SpriteAsset()
        {
            ImageName = DefaultAssets.SPR_CHESTS,
            SpriteRowIndex = 2,
        };

        public SpriteAsset[] GetSpriteAsset()
        {
            return new SpriteAsset[] {
                BodySprite,
                LegSprite,
                HeadSprite,
                ChestSprite
            };
        }

        public void FromStored(StoredPlayer player)
        {
            this.Name = player.Login;
            this.MoveSpeed = player.MoveSpeed;
            this.Position = new Position(player.X, player.Y);
            this.UID = player.UserId;
            this.BodySprite.SpriteRowIndex = player.BodySpriteIndex;
            this.LegSprite.SpriteRowIndex = player.LegsSpriteIndex;
            this.ChestSprite.SpriteRowIndex = player.ChestSpriteIndex;
            this.HeadSprite.SpriteRowIndex = player.ChestSpriteIndex;

            this.Atk = player.Atk;
            this.Def = player.Def;
            this.AtkSpeed = player.AtkSpeed;
            this.HP = player.HP;
            this.MAXHP = player.MaxHp;
        }

        public PlayerPacket ToPacket()
        {
            var packet = new PlayerPacket()
            {
                Name = this.Name,
                X = this.Position.X,
                Y = this.Position.Y,
                UserId = this.UID,
                Speed = this.MoveSpeed,

                BodySpriteIndex = BodySprite.SpriteRowIndex,
                HeadSpriteIndex = HeadSprite.SpriteRowIndex,
                LegSpriteIndex = LegSprite.SpriteRowIndex,
                ChestSpriteIndex = ChestSprite.SpriteRowIndex,

                Atk = this.Atk,
                Def = this.Def,
                AtkSpeed = this.AtkSpeed,
                HP = this.HP,
                MAXHP = this.MAXHP
            };
            return packet;
        }

        
        public override void Die()
        {
            // TODO
        }
    }
}
