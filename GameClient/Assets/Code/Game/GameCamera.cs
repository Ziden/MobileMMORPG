using UnityEngine;
using System.Collections;
using Client.Net;

public class GameCamera : MonoBehaviour
{
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        if (UnityClient.Player.PlayerObject == null)
            return;
        var target = UnityClient.Player.PlayerObject.transform;
        if (target)
        {
            Vector3 point = Camera.main.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

    }
}