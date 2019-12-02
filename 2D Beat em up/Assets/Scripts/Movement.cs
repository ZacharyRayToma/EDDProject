using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    private Vector2 position;
    public Animator animator;
    private string moveLock;
    public int dashmultiplyer;
    // Use this for initialization
    void Start()
    {
        position = gameObject.transform.position;
        animator = GetComponent<Animator>();
        Application.targetFrameRate = 60;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(Vector2.right * speed * dashmultiplyer * Time.deltaTime, transform);
                animator.SetInteger("AnimationState", 3);

            }
            else
            {
                ZachMovement(Vector2.right, speed, 1);
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(Vector2.left * speed * dashmultiplyer * Time.deltaTime, transform);
                animator.SetInteger("AnimationState", 4);
            }
            else
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime, transform);
                animator.SetInteger("AnimationState", 2);
            }
        }
        else if (Input.GetKey(KeyCode.W))
        {

            transform.Translate(Vector2.left * speed * dashmultiplyer * Time.deltaTime, transform);
            animator.SetInteger("AnimationState", 4);
        }
        else
        {
            animator.SetInteger("AnimationState", 0);
        }

    }
     void ZachMovement(Vector3 zachdirection, float zachspeed, int animationstate)
    {
        transform.Translate(zachdirection * zachspeed * Time.deltaTime, transform);
        animator.SetInteger("AnimationState", animationstate);
    }
}