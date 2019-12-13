using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public float speed;
    private Vector2 position;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private string moveLock;
    public int dashmultiplyer;
    private bool islocked;
    private double unlocktime;
    private int unlockkey;
    public int jumpmultiplyer;
    private bool keyPress;
    public bool facingright;
    public float dashdelay;

    // Use this for initialization
    void Start()
    {
        position = gameObject.transform.position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Application.targetFrameRate = 60;
        islocked = false;
        keyPress = false;
        

    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }
    void ZachMovement(Vector3 zachdirection, float zachspeed, int animationstate)
    {
        if (facingright && !islocked || facingright && animationstate == unlockkey)
        {
            //keyPress = true;
            transform.Translate(zachdirection * zachspeed * Time.deltaTime, transform);
            animator.SetInteger("AnimationState", animationstate);
            spriteRenderer.flipX = false;
        }
        if (!facingright && !islocked || !facingright && animationstate == unlockkey)
        {
            keyPress = true;
            transform.Translate(zachdirection * zachspeed * Time.deltaTime, transform);
            animator.SetInteger("AnimationState", animationstate);
            spriteRenderer.flipX = true;
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
    float getKeyPressed(string yes)
    {
        if (!islocked || Input.GetAxis(yes) != 0)
        {
            return Input.GetAxis(yes);
        }
        else return 0;
    }
    bool getKeyUp(KeyCode yes)
    {
        if (!islocked)
        {
            return Input.GetKeyUp(yes);
        }
        else return false;
    }
    void delaydash(string controller)
    {
        dashdelay = Input.GetAxis(controller);
    }
    void PlayerMovement()
    {
        keyPress = false;
        if (getKeyPressed("P1X") > 0|| unlockkey == 3)
        {
            if (getKeyPressed("DashP1") > 0 || unlockkey == 3)
            {
                Movementlock(0.25, 3);
                ZachMovement(Vector2.right, ((float)unlocktime - Time.time) * dashmultiplyer, 3);
            }
            else
            {
                ZachMovement(Vector2.right, speed, 1);
            }
        }
        if (getKeyPressed("P1X") < 0 || unlockkey == 4)
        {
            if (getKeyPressed("DashP1") > 0 || unlockkey == 4)
            {
                Movementlock(0.25, 4);
                ZachMovement(Vector2.left, ((float)unlocktime - Time.time) * dashmultiplyer, 4);
            }
            else
            {
                ZachMovement(Vector2.left, speed, 2);
            }
        }
        if (getKeyPressed("JumpP1") > 0 || unlockkey == 5)
        {
            Movementlock(0.5, 5);
            if (Time.time >= unlocktime - .25 && islocked)
            {
                ZachMovement(Vector3.down, speed * jumpmultiplyer, 5);
            }
            else if (Time.time <= unlocktime - .25 && islocked)
            {
                ZachMovement(Vector3.up, speed * jumpmultiplyer, 5);
            }
        }
        
        /*if (getKeyPressed("CrouchP1") > 0 || unlockkey == 7 || unlockkey == 6)
        {
           if (keyPress)
           {
               Movementlock(0.1, 7);
               ZachMovement(Vector3.down, 0, 7);
           }
           if (!keyPress)
           {
               Movementlock(0.1, 6);
               ZachMovement(Vector3.down, 0, 6);
           }
           else
           {
               ZachMovement(Vector3.down, 0, 8);
           }
         }  
         */
        //will play if nothing else is being done
        if (!keyPress && !islocked)
        {
            ZachMovement(Vector3.up, 0, 0);
            keyPress = false;
            
        }

    }


}
