using Assets.Code.Net;
using CommonCode.EntityShared;
using MapHandler;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code.Game
{

    public enum SelectorType
    {
        MOVEMENT = 1,
        TARGET = 2
    }

    public class Selectors
    {
        private static Dictionary<string, GameObject> _selectors = new Dictionary<string, GameObject>();
        private static GameObject _moveSelector = null;
        private static SpriteRenderer _movementSelectorRenderer = null;

        public static void HideSelector()
        {
            _movementSelectorRenderer.sortingOrder = -1;
        }

        public static void AddSelector(GameObject obj, SelectorType selectorType, Color color)
        {
            var selectorName = "selector_" + selectorType;
            if (obj.transform.Find(selectorName) !=null)
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

        public static void RemoveSelector(SelectorType selector)
        {
            var selectorName = "selector_" + selector;
            if (_selectors.ContainsKey(selectorName))
            {
                GameObject.Destroy(_selectors[selectorName]);
                _selectors.Remove(selectorName);
            }
        }

        public static void MoveMovementSelectorTo(Position pos)
        {
            if(_moveSelector == null)
            {
                _moveSelector = new GameObject("Move Selector");
                _movementSelectorRenderer = _moveSelector.AddComponent<SpriteRenderer>();
                // Selector Image
                var tileset = AssetHandler.LoadedAssets[DefaultAssets.TLE_SET1];
                _movementSelectorRenderer.sprite = tileset[3,1];
                _movementSelectorRenderer.color = Color.green;
                _movementSelectorRenderer.sortingOrder = 1;
                _moveSelector.transform.localScale = new Vector2(GameCamera.GAME_OBJECTS_SCALE, GameCamera.GAME_OBJECTS_SCALE);
                _moveSelector.transform.position = pos.ToUnityPosition();
            } else
            {
                _movementSelectorRenderer.sortingOrder = 1;
                _moveSelector.transform.position = pos.ToUnityPosition();
            }
        }

    }
}
