using Client.Net;
using UnityEngine;
using UnityEngine.UI;

public class StyleDialog : MonoBehaviour
{
    public GameObject ButtonList;

    private int _spriteSheetIndex = 1;

    public void SetPlayerColor(Color color)
    {
        var playerBehaviour = UnityClient.Player.PlayerObject.GetComponent<PlayerBehaviour>();
        var spriteSheet = playerBehaviour.SpriteSheets[_spriteSheetIndex];
        spriteSheet.GetComponent<SpriteRenderer>().color = color;
    }

    public void OnEnable()
    {
        for (var i = 0; i < ButtonList.transform.childCount; i++)
        {
            var button = ButtonList.transform.GetChild(i).GetComponent<Button>();
            var img = ButtonList.transform.GetChild(i).GetComponent<Image>();
            button.onClick.AddListener(() =>
            {
                var color = img.color;
                SetPlayerColor(color);
            });
        }
    }
}
