using CommonCode.Networking.Packets;
using MapHandler;
using ServerCore.Assets;
using ServerCore.Game.Entities;
using ServerCore.Networking;
using ServerCore.Networking.PacketListeners;
using Storage.Players;

namespace ServerCore.GameServer.Players
{
    public class OnlinePlayer : LivingEntity, IClientLayeredRenderable
    {
        public ConnectedClientTcpHandler Tcp;

        // Becomes true when all assets have been loaded so its a "valid" online player
        public bool AssetsReady = false;

        // When this player can perform a movement again (in MS)
        public long CanMoveAgainTime = 0;


        private SpriteAsset _bodySprite = new SpriteAsset()
        {
            ImageName = DefaultAssets.SPR_BODIES,
            SpriteRowIndex = 2,
        };

        private SpriteAsset _legSprite = new SpriteAsset()
        {
            ImageName = DefaultAssets.SPR_LEGS,
            SpriteRowIndex = 2,
        };

        private SpriteAsset _headSprite = new SpriteAsset()
        {
            ImageName = DefaultAssets.SPR_HEADS,
            SpriteRowIndex = 2,
        };

        private SpriteAsset _chestSprite = new SpriteAsset()
        {
            ImageName = DefaultAssets.SPR_CHESTS,
            SpriteRowIndex = 2,
        };

        public SpriteAsset[] GetSpriteAsset()
        {
            return new SpriteAsset[] {
                _bodySprite,
                _legSprite,
                _headSprite,
                _chestSprite
            };
        }

        public void FromStored(StoredPlayer player)
        {
            this.Name = player.Login;
            this.MoveSpeed = player.MoveSpeed;
            this.Position = new Position(player.X, player.Y);
            this.UID = player.UserId;
            // TODO
            // this.Sprites =
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

                BodySpriteIndex = this.GetSpriteAsset()[0].SpriteRowIndex,
                HeadSpriteIndex = this.GetSpriteAsset()[1].SpriteRowIndex,
                LegSpriteIndex = this.GetSpriteAsset()[2].SpriteRowIndex,
                ChestSpriteIndex = this.GetSpriteAsset()[3].SpriteRowIndex
            };
            return packet;
        }
    }
}
