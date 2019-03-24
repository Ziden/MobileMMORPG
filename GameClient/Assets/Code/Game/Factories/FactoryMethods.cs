using UnityEngine;

namespace Assets.Code.Game.Factories
{
    public class FactoryMethods
    {
        public static void AddCollider(GameObject obj)
        {
            var collider = obj.AddComponent<BoxCollider2D>();
            collider.offset = new Vector2(0.08f, 0.08f);
            collider.size = new Vector2(0.2f, 0.2f);
        }

        public static void AddHealthBar(GameObject obj)
        {
            var healthbar = ClientPrefabs.Get().HealthbarPrefab;
            var instance = MonoBehaviour.Instantiate(healthbar, obj.transform.position, Quaternion.identity, obj.transform);
            var lvingEnt = obj.GetComponent<LivingEntityBehaviour>();
            var healthbarBehav = instance.AddComponent<HealthBarBehaviour>();
            lvingEnt.HealthBar = healthbarBehav;
            instance.transform.localPosition = new Vector2(0.08f, 0.18f);
            healthbarBehav.SetLife(lvingEnt.Entity.HP, lvingEnt.Entity.MAXHP);
        }
    }
}
