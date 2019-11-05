using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    private Vector2 position;
    // Use this for initialization
    void Start()
    {
        position = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D) && position.x < 100) {
            transform.Translate(Vector2.right * speed * Time.deltaTime, transform);
        }
        if (Input.GetKey(KeyCode.A) && position.x < 100)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime, transform);
        }
    }
}