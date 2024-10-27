using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMovement : MonoBehaviour
{
    public float speed = 5f;
    public float motionMagnitude = 3f;

    private Vector3 startPos;
    private float direction;

    void Start()
    {
        direction = Random.value < 0.5f ? -1f : 1f;

        float screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;

        float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
        startPos = new Vector3(randomX, transform.position.y, transform.position.z);
        transform.position = startPos;
    }

    void Update()
    {
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPosition.x <= 0.0f || screenPosition.x >= 1.0f)
        {
            direction = -direction;
        }
    }
}
