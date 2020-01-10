﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts
{
    public class Movement : MonoBehaviour
    {
        private int directionlist;
        public float speed;
        private Vector2 position;
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


        // Use this for initialization
        void Start()
        {
            isfalling = false;
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
            elevation = transform.position.y; // gets current y value of player
            fallCheck();
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
            if (!islocked)
            {
                unlockkey = currentkey;
                islocked = true;
                unlocktime = Time.time + delaytime;
            }
            else if (unlocktime <= Time.time)
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
            if (getKeyPressed(controls[0]) > 0 || verifyUnlockKey(directionalnumber[2])) // walking and dashing right
            {
                if (getKeyPressed(controls[1]) > 0 || unlockkey == directionalnumber[2]) //dashing right
                {
                    Movementlock(0.25, directionalnumber[2]);
                    ZachMovement(Vector2.right, ((float)unlocktime - Time.time) * dashmultiplyer, directionalnumber[2]);
                }
                else //walking right
                {
                    ZachMovement(Vector2.right, speed, directionalnumber[0]);
                }
            }

            if (getKeyPressed(controls[0]) < 0 || verifyUnlockKey(directionalnumber[3])) //walking and dashing left
            {
                if (getKeyPressed(controls[1]) > 0 || verifyUnlockKey(directionalnumber[3])) //dashing left
                {
                    Movementlock(0.25, directionalnumber[3]);
                    ZachMovement(Vector2.left, ((float)unlocktime - Time.time) * dashmultiplyer, directionalnumber[3]);
                }
                else //walking left
                {
                    ZachMovement(Vector2.left, speed, directionalnumber[1]);
                }
            }

            if (getKeyPressed(controls[2]) > 0 || unlockkey == 5) //jumping
            {
                Movementlock(.5, 5);
                if (Time.time >= unlocktime - .25 && islocked)
                {
                    ZachMovement(Vector3.down, speed * jumpmultiplyer, 5);
                }
                else if (Time.time <= unlocktime - .25 && islocked)
                {
                    ZachMovement(Vector3.up, speed * jumpmultiplyer, 5);
                }
            }

            if (getKeyPressed(controls[3]) > 0) //crouching (WIP)
            {
                ZachMovement(Vector3.down, 0, 7);
            }

            if (!keyPress && !islocked || unlockkey == 0) //idle
            {
                Movementlock(0, 0);
                ZachMovement(Vector3.up, 0, 0);
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
                ZachMovement(Vector2.down, speed, 0);
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

        private void OnTriggerEnter(Collider other)
        {
            speed = 0;
        }
        void setdirection()
        {
            if(directionint)
        }


    }
}