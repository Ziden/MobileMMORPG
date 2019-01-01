using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour {

    TextMesh _textMesh;

    private float _alpha = 1f;
    private float _velX = 3;
    private float _velY = 25;
    private float _initialScale = 5;

    // Use this for initialization
    void Start () {
        _textMesh = GetComponent<TextMesh>();
        _initialScale = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector2(transform.position.x + (_velX * Time.deltaTime), transform.position.y + (_velY * Time.deltaTime));
        _alpha -= 0.008f;
        _textMesh.color = new Color(_textMesh.color.r, _textMesh.color.g, _textMesh.color.b, _alpha);

        if(_velY > 24)
        {
            transform.localScale = new Vector2(_initialScale, _initialScale);
        }
        else if(_velY > 20)
        {
            transform.localScale = new Vector2(_initialScale+((45 - _velY) / 10), _initialScale + ((45 - _velY) / 10));
        } else
        {
            transform.localScale = new Vector2(_initialScale, _initialScale);
        }

        _velY -= 55 * Time.deltaTime;

        if(_alpha <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
