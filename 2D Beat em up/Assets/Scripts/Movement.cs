using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Assets.Scripts
{
    public class Movement : MonoBehaviour
    {
        private int directionlist;
        public float speed;
        public float position;
        public Animator animator;
        private SpriteRenderer spriteRenderer;
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
        private float elevation;
        private bool isfalling;
        private Movement2D mvnt = null;
        public float HEALTH;
        private GameObject otherPlayer;
        private Movement otherScript;
        private float distance;
        private bool stunned;
        private double stuntimePlaceholder;
        private float stunspeedplaceholder;
        private int stuntypeplaceholder;
        private bool stunupwardplaceholder;
        private int stundirectionplaceholder;
        public TextMesh tmP1;
        public TextMesh tmP2;
        public string player1losescreen;
        public string player2losescreen;



        // Use this for initialization
        void Start()
        {
            player1losescreen = "WinScreen2";
            player1losescreen = "WinScreen1";
            tmP1 = (TextMesh)GameObject.Find("HealthP1").GetComponent<TextMesh>();
            tmP2 = (TextMesh)GameObject.Find("HealthP2").GetComponent<TextMesh>();
            stunned = false; //whether or not the player is stunned, if the player is stunned they shouldnt be able to take damage
            HEALTH = 10000; //Health of the player, the player should lose if this number reaches zero. In all caps in honor of Keith's favorite band
            isfalling = false; // whether or not the player is involentarily falling, they should not be able to move if this is true
            otherPlayer = GameObject.FindGameObjectWithTag(findOtherPlayer()); // The referece to the other player's object
            otherScript = otherPlayer.GetComponent<Movement>(); // The other player's movement script, used to change its public variables
            animator = GetComponent<Animator>(); // this object's animator, used to change animations manually
            spriteRenderer = GetComponent<SpriteRenderer>(); // this object's sprite renderer, but be useful later
            Application.targetFrameRate = 60; // What the framerate should be, in order to keep the game from running too fast since certain mechanics are frame-based.
            islocked = false; //If this is true, the player will not be able to do a move unless the action matches their unlock key
            keyPress = false; //this should become true if a key (which is bound to an action) is being pressed




            //controls for player 1 (names corespond to the names set in unity), the commented number is its number in the list counting from 0
            player1controls.Add("P1X"); //0
            player1controls.Add("DashP1");  //1
            player1controls.Add("JumpP1");  //2
            player1controls.Add("CrouchP1");    //3
            player1controls.Add("HeavyPunchP1");    //4
            player1controls.Add("LightPunchP1");     //5
            player1controls.Add("HeavyKickP1");     //6
            player1controls.Add("LightKickP1");     //7
            player1controls.Add("SpecialButtonP1");     //8
            player1controls.Add("BlockP1");     //9
            player1controls.Add("DodgeP1");     //10


            //controls for player 2 (names corespond to the names set in unity), the commented number is its number in the list counting from 0
            player2controls.Add("P2X"); //0
            player2controls.Add("DashP2");  //1
            player2controls.Add("JumpP2");  //2
            player2controls.Add("CrouchP2");    //3
            player2controls.Add("HeavyPunchP2");    //4
            player2controls.Add("LightPunchP2");     //5
            player2controls.Add("HeavyKickP2");     //6
            player2controls.Add("LightKickP2");     //7
            player2controls.Add("SpecialButtonP2");     //8
            player2controls.Add("BlockP2");     //9
            player2controls.Add("DodgeP2");     //10


            //list used for directional animations for facing right
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
            
            position = gameObject.transform.position.x; // X position of this object
            mvnt = GetComponent<Movement2D>();
            elevation = transform.position.y; // gets current y value of player
            fallCheck();
            distance = getDistance();
            if (player == 1) PlayerMovement(player1controls, getDirectionalAnimationlist());
            if (player == 2) PlayerMovement(player2controls, getDirectionalAnimationlist());
            HealthDisplayUpdater();
            EndGameCheck();
        

        }


        void stunPlayer(int stuntype, float speed, bool isupward, double stuntime, int direction )
        {
            otherScript.Movementlock(stuntime, stuntype);
            otherScript.stunned = true;
            otherScript.stunspeedplaceholder = speed;
            otherScript.stuntimePlaceholder = stuntime;
            otherScript.stuntypeplaceholder = stuntype;
            otherScript.stunupwardplaceholder = isupward;
            otherScript.stundirectionplaceholder = direction;

        }

        int getDirection()
        {
            if (facingright == true) return 1; else return -1;
        }


        List<int> getDirectionalAnimationlist()
        {
            if (facingright == true) return animationlistright;
            else return animationlistleft;
        }

        void ZachMovement(float zachspeed, int animationstate, bool isjump, int direction) //determines which way the model should be facing, the current movement speed and direction, and what animation is being played. verifies the unlockkey matches if movement is locked
        {                                                                                                         //extra note: direction should either be 1 or -1; 1 if going a positive direction, -1 if going a negative direction
            if (!isjump && !islocked || !isjump && animationstate == unlockkey)
            {
                keyPress = true;
                mvnt.MoveAlongX(zachspeed * Time.deltaTime * direction);
                animator.SetInteger("AnimationState", animationstate);
                if (facingright) spriteRenderer.flipX = false; else spriteRenderer.flipX = true;
            }
            if (isjump && !islocked || isjump && animationstate == unlockkey)
            {
                keyPress = true;
                mvnt.MoveAlongY(zachspeed * Time.deltaTime * direction);
                animator.SetInteger("AnimationState", animationstate);
                if (facingright) spriteRenderer.flipX = false; else spriteRenderer.flipX = true;
            }
        }


        void Movementlock(double delaytime, int currentkey) //animations will not be able to play if they dont match the unlockkey after this method is called, if the method is called after unlocktime it will unlock the movement and allow any key to be pressed
        {
            if (!islocked)
            {
                unlockkey = currentkey;
                islocked = true;
                unlocktime = Time.time + delaytime;
            }
            else if (unlocktime <= Time.time)
            {
                islocked = false;
                stunned = false;
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
            if (stunned == true)
            {
                Movementlock(stuntimePlaceholder, unlockkey);
                ZachMovement(stunspeedplaceholder, stuntypeplaceholder, stunupwardplaceholder, stundirectionplaceholder);

            }
            if (getKeyPressed(controls[0]) > 0 || verifyUnlockKey(directionalnumber[2])) // walking and dashing right
            {
                if (getKeyPressed(controls[1]) > 0 || unlockkey == directionalnumber[2]) //dashing right
                {
                    Movementlock(0.25, directionalnumber[2]);
                    ZachMovement(((float)unlocktime - Time.time) * dashmultiplyer, directionalnumber[2], false, 1);
                }
                else //walking right
                {
                    ZachMovement(speed, directionalnumber[0], false, 1);
                }
            }

            if (getKeyPressed(controls[0]) < 0 || verifyUnlockKey(directionalnumber[3])) //walking and dashing left
            {
                if (getKeyPressed(controls[1]) > 0 || verifyUnlockKey(directionalnumber[3])) //dashing left
                {
                    Movementlock(0.25, directionalnumber[3]);
                    ZachMovement(((float)unlocktime - Time.time) * dashmultiplyer, directionalnumber[3], false, -1);
                }
                else //walking left
                {
                    ZachMovement(speed, directionalnumber[1], false, -1);
                }
            }

            if (getKeyPressed(controls[2]) > 0 || unlockkey == 5) //jumping
            {
                Movementlock(.5, 5);
                if (Time.time >= unlocktime - .25 && islocked)
                {
                    ZachMovement(speed * jumpmultiplyer, 5, true, -1);
                }
                else if (Time.time <= unlocktime - .25 && islocked)
                {
                    ZachMovement(speed * jumpmultiplyer, 5, true, 1);
                }
            }

            if (getKeyPressed(controls[3]) > 0) //crouching (WIP)
            {
                ZachMovement(0, 7, true, 1);
            }

            if (getKeyPressed(controls[4]) > 0 || unlockkey == 9) // heavy Punch
            {
                Movementlock(.65, 9);
                ZachMovement(1, 9, false, getDirection());
                hurtOtherPlayer(10, 100, 1, getDirection(), false, .25, 20);
            }
            if (getKeyPressed(controls[5]) > 0 || unlockkey == 10) // light Punch
            {
                Movementlock(.65, 10);
                ZachMovement(1, 10, false, getDirection());
                hurtOtherPlayer(10, 100, 1, getDirection(), false, .25, 20);
            }
            if (getKeyPressed(controls[6]) > 0 || unlockkey == 11) // heavy Kick
            {
                Movementlock(.65, 11);
                ZachMovement(1, 11, false, getDirection());
                hurtOtherPlayer(10, 100, 1, getDirection(), false, .25, 20);
            }
            if (getKeyPressed(controls[7]) > 0 || unlockkey == 12) // light kick
            {
                Movementlock(.65, 12);
                ZachMovement(1, 12, false, getDirection());
                hurtOtherPlayer(10, 100, 1, getDirection(), false, .25, 20);
            }
            if (getKeyPressed(controls[8]) > 0 && getKeyPressed(controls[4]) > 0 || unlockkey == 13) // special 1
            {
                Movementlock(.65, 13);
                ZachMovement(1, 13, false, getDirection());
                hurtOtherPlayer(10, 100, 1, getDirection(), false, .25, 20);
            }
            if (getKeyPressed(controls[8]) > 0 && getKeyPressed(controls[5]) > 0 || unlockkey == 14) // special 2
            {
                Movementlock(.65, 9);
                ZachMovement(1, 9, false, getDirection());
                hurtOtherPlayer(10, 100, 1, getDirection(), false, .25, 20);
            }
            if (getKeyPressed(controls[8]) > 0 && getKeyPressed(controls[6]) > 0 || unlockkey == 15) // special 3
            {
                Movementlock(.65, 9);
                ZachMovement(1, 9, false, getDirection());
                hurtOtherPlayer(10, 100, 1, getDirection(), false, .25, 20);
            }
            if (getKeyPressed(controls[8]) > 0 && getKeyPressed(controls[7]) > 0) // special 4
            {
                Movementlock(.65, 9);
                ZachMovement(1, 9, false, getDirection());
                hurtOtherPlayer(10, 100, 1, getDirection(), false, .25, 20);
            }
            if (getKeyPressed(controls[9]) > 0 || unlockkey == 18) // block
            {
                Movementlock(.65, 9);
                ZachMovement(1, 9, false, getDirection());
                hurtOtherPlayer(10, 100, 1, getDirection(), false, .25, 20);
            }
            if (getKeyPressed(controls[10]) > 0 || unlockkey == 15) // dodge
            {
                Movementlock(.65, 9);
                ZachMovement(1, 9, false, getDirection());
                hurtOtherPlayer(10, 100, 1, getDirection(), false, .25, 20);
            }

            if (!keyPress && !islocked || unlockkey == 0) //idle
            {
                Movementlock(0, 0);
                ZachMovement(0, 0, false, 1);
                keyPress = false;

            }
        }

        void fallCheck()
        {
            if (!islocked && elevation > 0 || elevation > 0 && isfalling)
            {
                islocked = true;
                isfalling = true;
                unlockkey = 0;
                ZachMovement(speed, 0, true, -1);
            }
            else if (isfalling && elevation <= 0)
            {
                islocked = false;
                isfalling = false;
                unlockkey = 9999;
            }
        } // checks if the player's base elevation is too high


        void endlag(float delay) // forces the player into an idlepose for a given amount of time
        {
            Movementlock(delay, 0);
        }

        
        void hurtOtherPlayer(float damage, int stuntype, float speed, int direction, bool isupward, double stuntime, float range )
        {
            if (getDistance() < range ) {
                otherScript.setHealth(damage);
                stunPlayer(stuntype, speed, isupward, stuntime, direction);
            }
        }


        public void getHurt(float damage, int stuntype, float speed, int direction, bool isupward, double stuntime, float range)
        {
            if (getDistance() < range)
            {
                HEALTH = HEALTH - damage;
                Movementlock(stuntime, stuntype);
                ZachMovement(speed, stuntype, isupward, direction);
            }
        }


        string findOtherPlayer()
        {
            if (player == 1) return "P2"; else return "P1";
        } // returns P2 if you are player 1, vice versa


        float getDistance()
        {
            float distance;
            float otherPosition = otherScript.position;
            distance = Mathf.Abs(position - otherPosition);
            return distance;
        }


        float getPosition(GameObject positionObject)
        {
            return positionObject.transform.position.x;
        }

        public float getHealth()
        {
            return HEALTH;
        }

        void setHealth(float damage)
        {
            HEALTH = HEALTH - damage;
        }

        void HealthDisplayUpdater()
        {
            if (player == 1) tmP1.text = "Health:" + string.Format("{0:N0}",HEALTH);
            else tmP2.text = "Health:" + string.Format("{0:N0}", HEALTH); ;
        }

        void EndGameCheck()
        {
            if (player == 1 && HEALTH <= 0) SceneManager.LoadScene("WinScreen2");
            else if (player == 2 && HEALTH <= 0) SceneManager.LoadScene("WinScreen1");
        }



    }
}