using Assets.Code.AssetHandling;
using Assets.Code.Game.Entities;
using Assets.Code.Net;
using Client.Net;
using Common.Entity;
using CommonCode.EntityShared;
using MapHandler;
using System.Linq;
using UnityEngine;

namespace Assets.Code.Game.Factories
{
    public class MonsterFactory
    {
        public static void BuildAndInstantiate(MonsterFactoryOpts opts)
        {
            var monsterObj = GameObject.Find(opts.MonsterUid);
            if (monsterObj == null)
            {
                monsterObj = new GameObject(opts.MonsterUid);
                monsterObj.transform.localScale = new Vector3(100, 100);

                FactoryMethods.AddCollider(monsterObj);

                var spriteRenderer = monsterObj.AddComponent<SpriteRenderer>();
                var spriteSheet = monsterObj.AddComponent<SpriteSheet>();
                spriteRenderer.sortingOrder = 2;
                var bodySprite = AssetHandler.LoadedAssets[DefaultAssets.SPR_MONTERS_1];
                var spriteRow = bodySprite.SliceRow(opts.SpriteIndex).ToArray();
                spriteSheet.SetSheet(spriteRow, rowSize:2);
                spriteRenderer.sprite = spriteSheet.WalkSouth[1];

                // Moving Entity
                var movingBehaviour = monsterObj.AddComponent<MovingEntityBehaviour>();
                movingBehaviour.SpriteSheets.Add(spriteSheet);
                var monsterEntityWrapper = new MonsterWrapper()
                {
                    MonsterObj = monsterObj,
                    EntityType = EntityType.MONSTER,
                    MoveSpeed = opts.MoveSpeed,
                    UID = opts.MonsterUid,
                    Position = opts.Position
                };

                movingBehaviour.Entity = monsterEntityWrapper;
                monsterObj.transform.position = opts.Position.ToUnityPosition();
                UnityClient.Map.UpdateEntityPosition(monsterEntityWrapper, opts.Position, opts.Position);

            }
        }
    }
}

public class MonsterFactoryOpts
{
    public string MonsterUid;
    public Position Position;
    public string MonsterName;
    public int SpriteIndex;
    public int MoveSpeed;
}
