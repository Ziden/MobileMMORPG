using Assets.Code.Net;
using CommonCode.EntityShared;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Game
{
    public class Selectors
    {
        private static Dictionary<string, GameObject> _selectors = new Dictionary<string, GameObject>();
        private static GameObject _moveSelector = null;
        private static SpriteRenderer _renderer = null;

        public static void HideSelector()
        {
            _renderer.sortingOrder = -1;
        }

        public static void AddSelector(GameObject obj, string selectorName, Color color)
        {
            if(obj.transform.Find(selectorName) !=null)
            {
                return;
            }

            if(_selectors.ContainsKey(selectorName))
            {
                GameObject.Destroy(_selectors[selectorName]);
                _selectors.Remove(selectorName);
            }

            var selector = new GameObject(selectorName);
            var renderer = selector.AddComponent<SpriteRenderer>();
            // Selector Image
            var tileset = AssetHandler.LoadedAssets[DefaultAssets.TLE_SET1];
            renderer.sprite = tileset[3, 1];
            renderer.color = color;
            renderer.sortingOrder = 1;
            selector.transform.localScale = obj.transform.localScale;
            selector.transform.position = obj.transform.position;
            selector.transform.parent = obj.transform;
            _selectors.Add(selectorName, selector);
        }

        public static void RemoveSelector(string selectorName)
        {
            if (_selectors.ContainsKey(selectorName))
            {
                GameObject.Destroy(_selectors[selectorName]);
                _selectors.Remove(selectorName);
            }
        }

        public static void MoveSelector(Vector2 vec)
        {
            if(_moveSelector == null)
            {
                _moveSelector = new GameObject("Move Selector");
                _renderer = _moveSelector.AddComponent<SpriteRenderer>();
                // Selector Image
                var tileset = AssetHandler.LoadedAssets[DefaultAssets.TLE_SET1];
                _renderer.sprite = tileset[3,1];
                _renderer.color = Color.green;
                _renderer.sortingOrder = 1;
                _moveSelector.transform.localScale = new Vector2(100, 100);
                _moveSelector.transform.position = new Vector2(vec.x * 16, -vec.y * 16);
            } else
            {
                _renderer.sortingOrder = 1;
                _moveSelector.transform.position = new Vector2(vec.x * 16, -vec.y * 16);
            }
        }

    }
}
