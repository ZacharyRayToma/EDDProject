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
    public int player;
    private List<string> player1controls = new List<string>();
    private List<string> player2controls = new List<string>();
    private List<int> animationlistright = new List<int>();
    private List<int> animationlistleft = new List<int>();


    // Use this for initialization
    void Start()
    {
        position = gameObject.transform.position;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Application.targetFrameRate = 60;
        islocked = false;
        keyPress = false;
        //controls for player 1 (names corespond to the names set in unity), the commented number is its number in the list counting from 0
        player1controls.Add("P1X"); //0
        player1controls.Add("DashP1");  //1
        player1controls.Add("JumpP1");  //2
        player1controls.Add("CrouchP1");    //3

        //controls for player 2 (names corespond to the names set in unity), the commented number is its number in the list counting from 0
        player2controls.Add("P2X"); //0
        player2controls.Add("DashP2");  //1
        player2controls.Add("JumpP2");  //2
        player2controls.Add("CrouchP2");    //3

        //list used for directional animations for facing right, added because incorrect animations when facing left
        animationlistright.Add(1); //walking forward (0)
        animationlistright.Add(2); //walking backward (1)
        animationlistright.Add(3); //dashing forward (2)
        animationlistright.Add(4); //dashing backward (3)

        //list used for directional animations for facing left
        animationlistleft.Add(2); //walking forward (0)
        animationlistleft.Add(1); //walking backward (1)
        animationlistleft.Add(4); //dashing forward (2)
        animationlistleft.Add(3); //dashing backward (3)

    }

    // Update is called once per frame
    void Update()
    {
        if (player == 1) PlayerMovement(player1controls, getDirectionalAnimationlist());
        if (player == 2) PlayerMovement(player2controls, getDirectionalAnimationlist());

    }

    List<int> getDirectionalAnimationlist()
    {
        if (facingright == true) return animationlistright;
        else return animationlistleft;
    }

    void ZachMovement(Vector3 zachdirection, float zachspeed, int animationstate) //determines which way the model should be facing, the current movement speed and direction, and what animation is being played. verifies the unlockkey matches if movement is locked
    {
        if (facingright && !islocked || facingright && animationstate == unlockkey)
        {
            keyPress = true;
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

    void Movementlock(double delaytime, int currentkey) //animations will not be able to play if they dont match the unlockkey after this method is called, if the method is called after unlocktime it will unlock the movement and allow any key to be pressed
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
    float getKeyPressed(string yes) //returns the axis number coresponding to the key being pressed, returns 0 if movement is locked
    {
        if (!islocked)
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

    bool verifyUnlockKey(int keyPressed) //Checks if the input key matches the current unlockkey
    {
        if (keyPressed == unlockkey) return true;
        else return false;
    }

    void PlayerMovement(List<string> controls, List<int> directionalnumber) // determines what key the player is pressing or locked into, and matches it with its coresponding animation.
    {
        keyPress = false;
        if (getKeyPressed(controls[0]) > 0|| verifyUnlockKey(directionalnumber[2]))
        {
            if (getKeyPressed(controls[1]) > 0 || unlockkey == directionalnumber[2])
            {
                Movementlock(0.25, directionalnumber[2]);
                ZachMovement(Vector2.right, ((float)unlocktime - Time.time) * dashmultiplyer, directionalnumber[2]);
            }
            else
            {
                ZachMovement(Vector2.right, speed, directionalnumber[0]);
            }
        }
        if (getKeyPressed(controls[0]) < 0 || verifyUnlockKey(directionalnumber[3]))
        {
            if (getKeyPressed(controls[1]) > 0 || verifyUnlockKey(directionalnumber[3]))
            {
                Movementlock(0.25, directionalnumber[3]);
                ZachMovement(Vector2.left, ((float)unlocktime - Time.time) * dashmultiplyer, directionalnumber[3]);
            }
            else
            {
                ZachMovement(Vector2.left, speed, directionalnumber[1]);
            }
        }
        if (getKeyPressed(controls[2]) > 0 || unlockkey == 5)
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
        
        if (getKeyPressed(controls[3]) > 0)
        {
            ZachMovement(Vector3.down, 0, 7);
        }  
         
        //will play if nothing else is being done
        if (!keyPress && !islocked)
        {
            ZachMovement(Vector3.up, 0, 0);
            keyPress = false;
            
        }
    }


}
