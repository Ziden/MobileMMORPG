using ServerCore.Assets;

namespace ServerCore.Game.Entities
{
    public interface IClientLayeredRenderable
    {
        SpriteAsset [] GetSpriteAsset();
    }
}
