using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts
{

    public class playerScript : MonoBehaviour
    {

        Animator anim;
        bool readyToStart, windowToClimb;
        int timeStartingWindow;
        enum movement { LEFT, RIGHT, JUMP_LEFT, JUMP_RIGHT, NONE};
        movement currentDirection;
        float INC = 0.01f;
        float ANGLE_INCREASE = 0.05f;
        int THRESHOLD_Y = 20;
        float angle, previousY;
        Vector3 startPosition;

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

            readyToStart = false;
            currentDirection = movement.RIGHT;

            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            anim = GetComponent<Animator>();

            anim.Play("idle");

        }

        public void startTheGame()
        {
            readyToStart = true;
            anim.Play("walking");
        }


        void Update()
        {
            //Debug.Log(windowToClimb.ToString());
            if (windowToClimb == true)
            {
                int currentTime = (System.DateTime.Now.Hour * 3600000) + (System.DateTime.Now.Minute * 60000) + (System.DateTime.Now.Second * 1000) + (System.DateTime.Now.Millisecond);
                int deltaTime = currentTime - timeStartingWindow;
                if (deltaTime > 1000)
                    windowToClimb = false;
            }
            if (readyToStart == true)
            {
                switch (currentDirection)
                {
                    case movement.RIGHT:
                        transform.localPosition = new Vector3(transform.localPosition.x + INC, transform.localPosition.y, transform.localPosition.z);
                    break;

                    case movement.LEFT:
                        transform.localPosition = new Vector3(transform.localPosition.x - INC, transform.localPosition.y, transform.localPosition.z);
                    break;

                    case movement.JUMP_RIGHT:
                        transform.localPosition = new Vector3(transform.localPosition.x + INC, transform.localPosition.y + Mathf.Sin(angle) / 50f, transform.localPosition.z);
                        angle += ANGLE_INCREASE;

                        if (angle > Mathf.PI * 2f)
                        {
                            transform.localPosition = new Vector3(transform.localPosition.x, previousY, transform.localPosition.z);
                            angle = 0;
                            currentDirection = movement.RIGHT;
                            anim.Play("walking");
                        }
                        break;

                    case movement.JUMP_LEFT:
                        transform.localPosition = new Vector3(transform.localPosition.x - INC, transform.localPosition.y + Mathf.Sin(angle) / 50f, transform.localPosition.z);
                        angle += ANGLE_INCREASE;
                        
                        if (angle > Mathf.PI * 2f)
                        {
                            transform.localPosition = new Vector3(transform.localPosition.x, previousY, transform.localPosition.z);
                            angle = 0;
                            currentDirection = movement.LEFT;
                            anim.Play("walking");
                        }

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

                    string stateAnimation = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;//////////////////////////////////////////////

                    if (Mathf.Abs(swipeY) < THRESHOLD_Y)//move in x
                    {
                        if ((stateAnimation.CompareTo("walking") == 0) || (stateAnimation.CompareTo("idle") == 0))
                        {
                            if (swipeX < 0)
                            {
                                if (currentDirection == movement.LEFT)
                                    transform.Rotate(new Vector3(0, 1, 0), 180);

                                //string stateAnimation = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                                if (stateAnimation.CompareTo("walking") != 0)
                                    anim.Play("walking");

                                currentDirection = movement.RIGHT;
                            }
                            else
                            {
                                if (currentDirection == movement.RIGHT)
                                    transform.Rotate(new Vector3(0, 1, 0), 180);

                                //string stateAnimation = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                                if (stateAnimation.CompareTo("walking") != 0)
                                    anim.Play("walking");

                                currentDirection = movement.LEFT;
                            }
                        }
                    }
                    else//jump
                    {
                        if (stateAnimation.CompareTo("walking") == 0)
                        {
                            if (swipeX < 0)
                            {
                                if ((currentDirection == movement.LEFT) || (currentDirection == movement.JUMP_LEFT))
                                    transform.Rotate(new Vector3(0, 1, 0), 180);

                                previousY = transform.localPosition.y;

                                anim.Play("jumping");
                                Debug.Log("jumping right");
                                currentDirection = movement.JUMP_RIGHT;
                            }
                            else
                            {
                                if ((currentDirection == movement.RIGHT) || (currentDirection == movement.JUMP_RIGHT))
                                    transform.Rotate(new Vector3(0, 1, 0), 180);

                                anim.Play("jumping");
                                Debug.Log("jumping left");
                                currentDirection = movement.JUMP_LEFT;
                            }
                        }
                        if (windowToClimb == true)
                        {
                            if (swipeX < 0)
                            {
                                if (currentDirection == movement.JUMP_LEFT)
                                    transform.Rotate(new Vector3(0, 1, 0), 180);

                                previousY = transform.localPosition.y;

                                anim.StopPlayback();
                                anim.Play("jumping");
                                Debug.Log("jumping right (climbing)");
                                currentDirection = movement.JUMP_RIGHT;
                            }
                            else
                            {
                                if (currentDirection == movement.JUMP_RIGHT)
                                    transform.Rotate(new Vector3(0, 1, 0), 180);

                                anim.StopPlayback();
                                anim.Play("jumping");
                                Debug.Log("jumping left (climbing)");
                                currentDirection = movement.JUMP_LEFT;
                            }
                        }
                    }

                }


            }
        }

        void OnCollisionEnter(Collision collision)
        {
            windowToClimb = true;
            anim.Play("idle");
            timeStartingWindow = (System.DateTime.Now.Hour * 3600000) + (System.DateTime.Now.Minute * 60000) + (System.DateTime.Now.Second * 1000) + (System.DateTime.Now.Millisecond);
            //currentDirection = movement.NONE;
            //foreach (ContactPoint contact in collision.contacts)
            //{
            //    Debug.DrawRay(contact.point, contact.normal, Color.white);
            //}
        }


    }
}