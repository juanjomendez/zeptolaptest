using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.scripts
{

    public class playerScript : MonoBehaviour
    {

        Animator anim;
        bool readyToStart, windowToClimb;
        int timeStartingWindow;
        enum movement { LEFT, RIGHT, JUMP_LEFT, JUMP_RIGHT, NONE};
        movement currentDirection;
        float INC = 0.02f;
        int THRESHOLD_Y = 20;
        Vector3 startPosition;
        string stateAnimation;
        int count = 0;

        public RawImage kkTestImage;
        public Text kkshowText;

        public void setStartPosition(int[,] screen, int w, int h)
        {
            int x, y;
            x = 0;
            y = 0;
            bool found = false;
            for (int i = 0; i < w; i++)
            {
                for (int j = h-1; j >= 0; j--)
                {
                    if (screen[i, j] == 1)
                    {
                        x = i;
                        y = j;
                        found = true;
                        break;
                    }
                }
                if (found == true)
                    break;
            }

            transform.localPosition = new Vector3(x * 2, (-y * 2) - 1, 0);
            transform.Rotate(new Vector3(0, 1, 0), 90);
        }


        void Start()
        {

            anim = GetComponent<Animator>();

            readyToStart = false;
            currentDirection = movement.RIGHT;

            transform.localScale = new Vector3(0.7f, 0.7f, 0.79f);

            

            stateAnimation = "idle";

            anim.Play("idle");




        }

        public void startTheGame()
        {
            readyToStart = true;
            anim.Play("walking");
        }



        void Update()
        {
            kkshowText.text = currentDirection.ToString();
            if (windowToClimb == true)
            {
                kkTestImage.color = Color.red;
                int currentTime = (System.DateTime.Now.Hour * 3600000) + (System.DateTime.Now.Minute * 60000) + (System.DateTime.Now.Second * 1000) + (System.DateTime.Now.Millisecond);
                int deltaTime = currentTime - timeStartingWindow;
                if (deltaTime > 2000)
                    windowToClimb = false;
            }
            else
                kkTestImage.color = Color.white;
            if (readyToStart == true)
            {

                switch (currentDirection)
                {
                    case movement.RIGHT:
                        transform.localPosition = new Vector3(transform.localPosition.x + INC, transform.localPosition.y , transform.localPosition.z);
                    break;

                    case movement.LEFT:
                        transform.localPosition = new Vector3(transform.localPosition.x - INC, transform.localPosition.y, transform.localPosition.z);
                    break;
                        
                    case movement.JUMP_RIGHT:
                        if (windowToClimb == false)
                            transform.localPosition = new Vector3(transform.localPosition.x + INC, transform.localPosition.y, transform.localPosition.z);
                        else
                            transform.localPosition = new Vector3(transform.localPosition.x + INC, transform.localPosition.y + 0.05f, transform.localPosition.z);
                        //if (AnimatorIsPlaying("jumping") == true)
                        //    currentDirection = movement.RIGHT;
                        if (AnimatorIsPlaying("walking"))
                            currentDirection = movement.RIGHT;
                        //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsName("jumping").ToString());

                        break;

                    case movement.JUMP_LEFT:
                        if (windowToClimb == false)
                            transform.localPosition = new Vector3(transform.localPosition.x - INC, transform.localPosition.y, transform.localPosition.z);
                        else
                            transform.localPosition = new Vector3(transform.localPosition.x - INC, transform.localPosition.y + 0.05f, transform.localPosition.z);
                        //if (AnimatorIsPlaying("jumping") == true)
                        //    currentDirection = movement.LEFT;
                        if (AnimatorIsPlaying("walking"))
                            currentDirection = movement.LEFT;


                        break;
                        
                    case movement.NONE:
                        break;

                }

                
                if (Input.GetMouseButtonDown(0))
                    startPosition = Input.mousePosition;

                if (Input.GetMouseButtonUp(0))
                {

                    float swipeY = startPosition.y - Input.mousePosition.y;
                    float swipeX = startPosition.x - Input.mousePosition.x;

                    stateAnimation = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;//////////////////////////////////////////////

                    if (Mathf.Abs(swipeY) < THRESHOLD_Y)//move in x
                    {
                        if ((stateAnimation.CompareTo("walking") == 0) || (stateAnimation.CompareTo("idle") == 0))
                        {
                            if (swipeX < 0)
                            {
                                //if (currentDirection == movement.LEFT)
                                //    transform.Rotate(new Vector3(0, 1, 0), 180);

                                transform.LookAt(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z));

                                if (stateAnimation.CompareTo("walking") != 0)
                                    anim.Play("walking");

                                currentDirection = movement.RIGHT;
                            }
                            else
                            {
                                //if (currentDirection == movement.RIGHT)
                                //    transform.Rotate(new Vector3(0, 1, 0), 180);
                                transform.LookAt(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z));

                                if (stateAnimation.CompareTo("walking") != 0)
                                    anim.Play("walking");

                                currentDirection = movement.LEFT;
                            }
                        }
                    }
                    else//jump
                    {
                        //if (stateAnimation.CompareTo("walking") == 0)
                        {
                            if (swipeX < 0)
                            {
                                //if ((currentDirection == movement.LEFT) || (currentDirection == movement.JUMP_LEFT))
                                //    transform.Rotate(new Vector3(0, 1, 0), 180);
                                transform.LookAt(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z));

                                count = 0;
                                anim.Play("jumping");
                                currentDirection = movement.JUMP_RIGHT;
                            }
                            else
                            {
                                //if ((currentDirection == movement.RIGHT) || (currentDirection == movement.JUMP_RIGHT))
                                //    transform.Rotate(new Vector3(0, 1, 0), 180);
                                transform.LookAt(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z));

                                count = 0;
                                anim.Play("jumping");
                                currentDirection = movement.JUMP_LEFT;
                            }
                        }
                        /*
                        if (windowToClimb == true)
                        {
                            //anim.StopPlayback();
                            
                            //Debug.Log("climbing !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                            if (swipeX < 0)
                            {
                                //  Debug.Log("RIGHT !!!!!!!!!!!!!!!");
                                //if (currentDirection == movement.JUMP_LEFT)
                                //    transform.Rotate(new Vector3(0, 1, 0), 180);
                                transform.LookAt(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z));

                                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y+0.5f, transform.localPosition.z);

                                //anim.StopPlayback();
                                count = 0;
                                anim.Play("jumping");
                                currentDirection = movement.JUMP_RIGHT;
                            }
                            else
                            {
                                //Debug.Log("LEFT !!!!!!!!!!!!!!!");
                                //if (currentDirection == movement.JUMP_RIGHT)
                                //    transform.Rotate(new Vector3(0, 1, 0), 180);
                                transform.LookAt(new Vector3(transform.position.x - 1, transform.position.y + 0.5f, transform.position.z));

                                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);

                                //anim.StopPlayback();
                                count = 0;
                                anim.Play("jumping");
                                currentDirection = movement.JUMP_LEFT;
                            }
                            
                        }
                        */

                    }

                }
                


            }
        }

        bool AnimatorIsPlaying()
        {
            return anim.GetCurrentAnimatorStateInfo(0).length >
                   anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        bool AnimatorIsPlaying(string stateName)
        {
            //bool animatorPlaying = AnimatorIsPlaying();
            //bool animatorPlayingName = anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
            count++;

            return AnimatorIsPlaying() && anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) && count>1;
        }

        public void pressbuttonfake1()//left
        {
            anim.StopPlayback();

            if ((currentDirection == movement.JUMP_RIGHT) || (currentDirection == movement.RIGHT))
                transform.Rotate(new Vector3(0, 1, 0), 180);

            //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z);

            count = 0;
            anim.Play("jumping");
            currentDirection = movement.JUMP_LEFT;
        }
        public void pressbuttonfake2()//right
        {
            anim.StopPlayback();

            if ((currentDirection == movement.JUMP_LEFT) || (currentDirection == movement.LEFT))
                transform.Rotate(new Vector3(0, 1, 0), 180);

            //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 1, transform.localPosition.z);

            count = 0;
            anim.Play("jumping");
            currentDirection = movement.JUMP_RIGHT;

        }



        void OnCollisionEnter(Collision collision)
        {
            windowToClimb = true;

            Assets.scripts.cubeScript scr = collision.gameObject.GetComponent<Assets.scripts.cubeScript>();
            Assets.scripts.cubeScript.modes m = scr.getMode();
            if (anim != null)
                anim.Play("idle");

            timeStartingWindow = (System.DateTime.Now.Hour * 3600000) + (System.DateTime.Now.Minute * 60000) + (System.DateTime.Now.Second * 1000) + (System.DateTime.Now.Millisecond);
        }


    }
}