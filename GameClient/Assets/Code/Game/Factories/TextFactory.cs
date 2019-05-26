using UnityEngine;

namespace Assets.Code.Game.Factories
{
    public class TextFactory
    {
        public static void BuildAndInstantiate<BehaviourType>(TextOptions opts) where BehaviourType : MonoBehaviour
        {
            var textPrefab = ClientPrefabs.Get().TextPrefab;
            var position = new Vector2(opts.UnityX, opts.UnityY);        
            var textGameObject = MonoBehaviour.Instantiate(textPrefab, position, textPrefab.transform.rotation);
            var textMesh = textGameObject.GetComponent<TextMesh>();
            if(opts.TextColor != null)
                textMesh.color = opts.TextColor;
            textGameObject.transform.localScale = new Vector2(opts.Size, opts.Size);
            textGameObject.GetComponent<MeshRenderer>().sortingOrder = 100;
            textMesh.text = opts.Text;
            var textBehaviour = textGameObject.AddComponent<BehaviourType>();
        }
    }

    public class TextOptions
    {
        public float UnityX;
        public float UnityY;
        public string Text;
        public int Size;
        public Color TextColor;
    }

}
