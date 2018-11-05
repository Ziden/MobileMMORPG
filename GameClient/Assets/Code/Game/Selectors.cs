using Assets.Code.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Game
{
    public class Selectors
    {
        private static GameObject _moveSelector = null;
        private static SpriteRenderer _renderer = null;

        public static void HideSelector()
        {
            _renderer.sortingOrder = -1;
        }

        public static void MoveSelector(Vector2 vec)
        {
            if(_moveSelector == null)
            {
                _moveSelector = new GameObject("Move Selector");
                _renderer = _moveSelector.AddComponent<SpriteRenderer>();
                // Selector Image
                var tileset = AssetHandler.LoadedAssets[AssetHandler.TILESET_FILE];
                _renderer.sprite = tileset[3,1];
                _renderer.color = Color.green;
                _renderer.sortingOrder = 1;
                _moveSelector.transform.localScale = new Vector2(100, 100);
                _moveSelector.transform.position = new Vector2(vec.x * 16, vec.y * 16);
            } else
            {
                _renderer.sortingOrder = 1;
                _moveSelector.transform.position = new Vector2(vec.x * 16, vec.y * 16);
            }
        }

    }
}
