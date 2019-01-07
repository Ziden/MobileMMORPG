using UnityEngine;

public class HealthBarBehaviour : MonoBehaviour {

    private float _maxSize = 0;
    private float _initialX = 0;
    private Transform _greenBar;

    public int _cachedHp = 0;
    public int _cachedMax = 0;

	// Use this for initialization
	void Start () {
        _greenBar = transform.Find("Greenbar").transform;
        _maxSize = _greenBar.localScale.x;
        _initialX = _greenBar.position.x;
	}

    public void SetLife(int hp, int maxHp)
    {
        // Because we can set before loading it.
        if (_greenBar == null)
        {
            _cachedHp = hp;
            _cachedMax = maxHp;
            return;
        }

        float pct = (hp * 100) / maxHp;

        float newScale = _maxSize * pct/100;
        float scaleDiff = _maxSize - newScale;
        var newX = _initialX - (scaleDiff * 3.6f);
        if (newScale < 0)
            newScale = 0;
        _greenBar.localScale = new Vector2(newScale, _greenBar.localScale.y);
        _greenBar.position = new Vector2(newX, _greenBar.position.y);
    }
	
	// Update is called once per frame
	void Update () {
		if(_cachedMax != 0 && _cachedHp != 0)
        {
            SetLife(_cachedHp, _cachedMax);
            _cachedMax = 0;
            _cachedHp = 0;
        }
	}
}
