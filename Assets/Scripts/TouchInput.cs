using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    float jumpForce;
    bool isJumping = false;

    bool isPerimeter = false;

    bool isMovingLeft = false;

    bool isFlipped = false;

    float numJumps = 0;

    public AudioClip jumpSound;

    Popup pausePopup;

    Timer currTime;

    float moveSpeed = 3;

    float waitTime;

    Animator playerAnimation;

    // Use this for initialization
    void Start()
    {
        jumpForce = 4;

        // Disables Built-in Gravity on player
        this.GetComponent<Rigidbody2D>().gravityScale = 0;
        // Disables Built-in Rotation on player
        this.GetComponent<Rigidbody2D>().freezeRotation = true;

        GameObject popupController = GameObject.Find("PopupController");
        pausePopup = popupController.GetComponent<Popup>();

        // Insantiate timer obj
        GameObject timer = GameObject.Find("timer");
        currTime = timer.GetComponent<Timer>();

        // Player animation
        playerAnimation = this.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {

        //Check currTime. If it is 150 then increase speed by 1. Increase speed by 1 at every 50
        if(((int)currTime.time >= 50.0f) && ((int)currTime.time % 50 == 0) && moveSpeed <= 5)
        {
            if (Time.time > waitTime)
            {
                moveSpeed++;
                Debug.Log(moveSpeed);
                waitTime = Time.time + 1.0f;
            }
        }

        if (isMovingLeft)
        {
            // Moves the player to the left.
            transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

            // Flip player
            if (isFlipped == false)
            {
                Vector3 newScale = transform.localScale;
                newScale.x *= -1;
                transform.localScale = newScale;

                isFlipped = true;
            }
        }
        else
        {
            // Moves the player to the right.
            transform.Translate(Vector2.right * Time.deltaTime * moveSpeed);

            // Flip player
            if (isFlipped)
            {
                Vector3 newScale = transform.localScale;
                newScale.x *= -1;
                transform.localScale = newScale;

                isFlipped = false;
            }
        }

        // If the player has touched and released the screen and the they are on the ground then the player jumps.
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && IsJumping == false)
        {
            if (!(pausePopup.Paused))
            {
                if (isPerimeter == false)
                {

                    numJumps++;

                    if (numJumps == 2)
                    {
                        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    }
                    this.GetComponent<Rigidbody2D>().AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

               
                    IsJumping = true;
                    
                    GetComponent<AudioSource>().PlayOneShot(jumpSound, 1);
                }
                else
                {
                    if (isMovingLeft == true)
                    {
                        isMovingLeft = false;
                    }
                    else
                    {
                        isMovingLeft = true;
                    }
                    GameObject.Find("Cloudus 456").GetComponent<obstacleGenerator>().ChangeDirection(isMovingLeft);
                    isPerimeter = false;
                }
            }
        }


        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            if (numJumps != 2)
            {
                IsJumping = false;
            }
        }


        // For testing through computer.
        if (Input.GetKeyDown(KeyCode.W) && IsJumping == false)
        {
            if (!(pausePopup.Paused))
            {
                if (isPerimeter == false)
                {

                    numJumps++;

                    if (numJumps == 2)
                    {
                        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    }
                    this.GetComponent<Rigidbody2D>().AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
               
                    IsJumping = true;
  
                    GetComponent<AudioSource>().PlayOneShot(jumpSound, 1);
                }
                else
                {
                    if (isMovingLeft == true)
                    {
                        isMovingLeft = false;
                    }
                    else
                    {
                        isMovingLeft = true;
                    }
                    GameObject.Find("Cloudus 456").GetComponent<obstacleGenerator>().ChangeDirection(isMovingLeft);
                    isPerimeter = false;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            if (numJumps != 2)
            {
                IsJumping = false;
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            if(!pausePopup.Dead)
                pausePopup.Pause();
        }

        if (isJumping)
        {
            playerAnimation.speed = 0;
        }
    }

    // When the player has touched the floor then they are able to jump again.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Cloudus 456")
        {
            IsJumping = false;
            numJumps = 0;
            playerAnimation.speed = 1.0f;
        }
    }

    public bool IsInPerimeter
    {
        get
        {
            return isPerimeter;
        }

        set
        {
            isPerimeter = value;
        }
    }

    public bool IsJumping
    {
        get
        {
            return isJumping;
        }

        set
        {
            isJumping = value;
        }
    }

    public float NumJumps
    {
        get
        {
            return numJumps;
        }

        set
        {
            numJumps = value;
        }
    }
}

 
