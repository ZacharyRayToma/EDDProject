using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    private Vector2 position;
    public Animator animator;
    public string moveLock;
    public int maxlock;
    // Use this for initialization
    void Start()
    {
        position = gameObject.transform.position;
        animator = GetComponent<Animator>();
        moveLock = null;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (moveLock == "dashforward" || Input.GetKey(KeyCode.D)) {
            if (moveLock == "dashforward" || Input.GetKey(KeyCode.LeftShift))
            {
                maxlock = 30;
                transform.Translate(Vector2.right * speed*5 * Time.deltaTime, transform);
                animator.SetInteger("AnimationState", 3);
                for(int locktime = 0; locktime < maxlock; locktime++)
                {
                    if(locktime < maxlock - 1)
                    {
                        moveLock = "dashforward";
                    }
                    else
                    {
                        moveLock = null;
                    }
                }
            }
            else
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime, transform);
                animator.SetInteger("AnimationState", 1);
            }
        }
        else if (Input.GetKey(KeyCode.A) && position.x < 100)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime, transform);
            animator.SetInteger("AnimationState", 2);
        }
        else
        {
            animator.SetInteger("AnimationState", 0);
        }
    }
}