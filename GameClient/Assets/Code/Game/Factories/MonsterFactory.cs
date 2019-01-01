using Assets.Code.AssetHandling;
using Assets.Code.Game.Entities;
using Assets.Code.Net;
using Client.Net;
using Common.Entity;
using Common.Networking.Packets;
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
            var monsterObj = GameObject.Find(opts.Packet.MonsterUid);
            if (monsterObj == null)
            {
                monsterObj = new GameObject(opts.Packet.MonsterUid);
                monsterObj.transform.localScale = new Vector3(100, 100);


                var spriteRenderer = monsterObj.AddComponent<SpriteRenderer>();
                var spriteSheet = monsterObj.AddComponent<SpriteSheet>();
                spriteRenderer.sortingOrder = 2;
                var bodySprite = AssetHandler.LoadedAssets[DefaultAssets.SPR_MONTERS_1];
                var spriteRow = bodySprite.SliceRow(opts.Packet.SpriteIndex).ToArray();
                spriteSheet.SetSheet(spriteRow, rowSize:2);
                spriteRenderer.sprite = spriteSheet.WalkSouth[1];

                // Moving Entity
                var livingEntityBhv = monsterObj.AddComponent<LivingEntityBehaviour>();
                livingEntityBhv.SpriteSheets.Add(spriteSheet);
                var monsterEntityWrapper = new MonsterWrapper()
                {
                    MonsterObj = monsterObj,
                    EntityType = EntityType.MONSTER,
                    MoveSpeed = opts.Packet.MoveSpeed,
                    UID = opts.Packet.MonsterUid,
                    Position = opts.Position,
                    Atk = opts.Packet.Atk,
                    Def = opts.Packet.Def,
                    HP = opts.Packet.HP,
                    MAXHP = opts.Packet.MAXHP
                };

                monsterObj.transform.parent = UnityExtensions.GetEntitiesContainer().transform;

                livingEntityBhv.Entity = monsterEntityWrapper;
                monsterObj.transform.position = opts.Position.ToUnityPosition();
                UnityClient.Map.UpdateEntityPosition(monsterEntityWrapper, null, opts.Position);

                FactoryMethods.AddCollider(monsterObj);
                FactoryMethods.AddHealthBar(monsterObj);
            }
        }
    }
}

public class MonsterFactoryOpts
{
    public MonsterPacket Packet;
    public Position Position;
}
