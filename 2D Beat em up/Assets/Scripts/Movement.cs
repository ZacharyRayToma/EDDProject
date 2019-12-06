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
    private bool islocked;
    private double unlocktime;
    private int unlockkey;
    public int jumpmultiplyer;
    public bool keyPress;
    // Use this for initialization
    void Start()
    {
        position = gameObject.transform.position;
        animator = GetComponent<Animator>();
        Application.targetFrameRate = 60;
        islocked = false;
        

    }

    // Update is called once per frame
    void Update()
    {
        keyPress = false;
        if (getKeyPressed(KeyCode.D) || unlockkey == 3)
        {
            if (getKeyDown(KeyCode.LeftShift) || unlockkey == 3)
            {
                Movementlock(0.25, 3);
                ZachMovement(Vector2.right, ((float)unlocktime - Time.time) * dashmultiplyer, 3);
            }
            else
            {
                ZachMovement(Vector2.right, speed, 1);
            }
        }
        if (getKeyPressed(KeyCode.A) || unlockkey == 4)
        {
            if (getKeyDown(KeyCode.LeftShift) || unlockkey == 4)
            {
                Movementlock(0.25, 4);
                ZachMovement(Vector2.left, ((float)unlocktime - Time.time) * dashmultiplyer, 4);
            }
            else
            {
                ZachMovement(Vector2.left, speed, 2);
            }
        }
        if (getKeyDown(KeyCode.W) || unlockkey == 5)
        {
            Movementlock(0.5, 5);
            if (Time.time >= unlocktime - .25 && islocked)
            {
                ZachMovement(Vector3.down, speed * jumpmultiplyer, 5);
            }
            else if(Time.time <= unlocktime - .25 && islocked)
            {
                ZachMovement(Vector3.up, speed * jumpmultiplyer, 5);
            }
        }

        //will play if nothing else is being done
        if (!keyPress)
        {
            animator.SetInteger("AnimationState", 0);
        }

    }
     void ZachMovement(Vector3 zachdirection, float zachspeed, int animationstate)
    {
        if (!islocked || animationstate == unlockkey)
        {
            keyPress = true;
            transform.Translate(zachdirection * zachspeed * Time.deltaTime, transform);
            animator.SetInteger("AnimationState", animationstate);
        }
    }

    void Movementlock(double delaytime, int currentkey)
    {
        if(!islocked)
        {
            unlockkey = currentkey;
            islocked = true;
            unlocktime = Time.time + delaytime;
        }
        else if(unlocktime <= Time.time)
        {
            islocked = false;
            unlockkey = 9999;
        }
        
    }
    bool getKeyPressed(KeyCode yes)
    {
        if (!islocked)
        {
            return Input.GetKey(yes);
        }
        else return false;
    }
    bool getKeyDown(KeyCode yes)
    {
        if (!islocked)
        {
            return Input.GetKeyDown(yes);
        }
        else return false;
    }
    bool getKeyUp(KeyCode yes)
    {
        if (!islocked)
        {
            return Input.GetKeyUp(yes);
        }
        else return false;
    }

}