using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    public Sprite[] FrameArray;

    private SpriteRenderer _renderer;

    private int _frame = 0;
    private float _deltaTime = 0;
    private float _frameSeconds = 0.1f;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (FrameArray == null)
            return;

        _deltaTime += Time.deltaTime;

        while (_deltaTime >= _frameSeconds)
        {
            _deltaTime -= _frameSeconds;
            _frame++;
        }

        if (_frame == FrameArray.Length)
        {
            Destroy(this.gameObject);
            return;
        }

        _renderer.sprite = FrameArray[_frame];
    }
}
