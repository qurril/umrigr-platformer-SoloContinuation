using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalMovement : MonoBehaviour
{
    public float speed = 5f;

    private float screenHeight;

    void Start()
    {
        screenHeight = Camera.main.orthographicSize * 1.1f;

        Vector3 startPosition = new Vector3(transform.position.x, screenHeight, transform.position.z);
        transform.position = startPosition;
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y <= -1 * screenHeight)
        {
            Destroy(gameObject);
        }
    }
}
