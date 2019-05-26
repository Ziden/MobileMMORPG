using Assets.Code.AssetHandling;
using Assets.Code.Net;
using MapHandler;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Game.Factories
{
    public class AnimationFactory
    {
        public static void BuildAndInstantiate(AnimationOpts opts)
        {
            var animationName = opts.AnimationImageName;
            if(!animationName.Contains(".png"))
            {
                animationName = animationName + ".png";
            }
            var animationObj = new GameObject("animation");
            animationObj.transform.localScale = new Vector3(GameCamera.GAME_OBJECTS_SCALE, GameCamera.GAME_OBJECTS_SCALE);

            var spriteRenderer = animationObj.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 10;
            var animationBehaviour = animationObj.AddComponent<AnimationBehaviour>();
            var animationSprite = AssetHandler.LoadedAssets[opts.AnimationImageName];
            var spriteRow = animationSprite.SliceRow(0).ToArray();
            animationBehaviour.FrameArray = spriteRow;
            animationObj.transform.position = opts.MapPosition.ToUnityPosition();
        }
    }
}

public class AnimationOpts
{
    public string AnimationImageName;
    public Position MapPosition;
}
