using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPlayer : MonoBehaviour
{
    //added by me
    public float slidingTime = 1; // 1 second duration to activate reverse again

    private float startDistance; // check distance from center
    public Vector3 basePosition; // check position of base while player is jumping
    public GameManager gameManager;
    public int itemScore = 100;
    public int itemGold = 100;
    private Animator animator;
    private bool isSlidingButtonDown;
    private bool isJumpingButtonDown;
    private bool isGameStarted;

    //added by gpt
    public Transform planet; // Reference to the planet
    public float orbitSpeed = 50f; // Speed of the player's movement around the planet
    public float jumpHeight = 2f; // Distance the player jumps away from the planet
    public float jumpSpeed = 5f; // Speed of the jump
    private float orbitSpeedOriginal;
    private float jumpSpeedOriginal;
    public bool isJumping = false; // Tracks if the player is currently jumping
    private float currentDistance; // Current distance from the planet's center
    private float targetDistance; // Target distance after jumping
    public int direction = 1; // 1 for clockwise, -1 for counterclockwise

    void Start()
    {
        // Calculate initial distance from the planet
        currentDistance = startDistance = Vector3.Distance(transform.position, planet.position);

        targetDistance = currentDistance;

        animator = GetComponent<Animator>();
        gameManager.playerAnimator = animator;
        gameManager.playerRunAnimName = "isRunning";

        isSlidingButtonDown = false;
        isJumpingButtonDown = false;
        isGameStarted = false;
        orbitSpeedOriginal = orbitSpeed;
        jumpSpeedOriginal = jumpSpeed;
    }

    void Update()
    {
        if (gameManager.isPlaying)
        {
            HandleInput();
            MoveAroundPlanet();
            AlignToSurface();
        }

        // Handle jumping
        if (isJumpingButtonDown)
        {
            if (!isJumping)
            {
                StartCoroutine(JumpLoop());
            }
            StartCoroutine(Jump());
        }
        
        // Handle sliding
        if (isSlidingButtonDown)
        {
            if (!isJumping)
            {
                animator.SetBool("isSliding", true);
            }
        }
        else
        {
            animator.SetBool("isSliding", false);
        }

    }
    void HandleInput()
    {
        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpingButtonDown = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumpingButtonDown = false;
        }
        // Toggle direction when pressing the "Switch Direction" button (e.g., Left Arrow)
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isSlidingButtonDown = true;
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            isSlidingButtonDown = false;
        }
    }
    void MoveAroundPlanet()
    {
        // Orbit around the planet based on the current direction
        transform.RotateAround(planet.position, Vector3.back, direction * orbitSpeed * Time.deltaTime);

        // Maintain the correct distance from the planet
        Vector3 directionToPlanet = (transform.position - planet.position).normalized;
        transform.position = planet.position + directionToPlanet * currentDistance;
        basePosition = planet.position + directionToPlanet * startDistance;
    }

    void AlignToSurface()
    {
        // Rotate the player to face the planet's center
        Vector3 directionToPlanet = (planet.position - transform.position).normalized;
        float angle = Mathf.Atan2(directionToPlanet.y, directionToPlanet.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90);
    }
    IEnumerator JumpLoop()
    {
        while (isJumpingButtonDown) // Keep jumping while the key is held
        {
            yield return StartCoroutine(Jump()); // Perform one jump
        }
    }

    IEnumerator Jump()
    {
        isJumping = true;
        animator.SetBool("isJumping", true);
        animator.SetBool("isSliding", false);

        float jumpProgress = 0f; // Progress from 0 to 1 during the jump
        float duration = jumpHeight / jumpSpeed; // Total duration of the jump

        while (jumpProgress < 1f)
        {
            jumpProgress += Time.deltaTime / duration; // Normalize progress over the total duration
            float height = Mathf.SmoothStep(0, jumpHeight, Mathf.Sin(jumpProgress * Mathf.PI));
            // Single parabolic arc
            currentDistance = startDistance + height;
            yield return null;
        }

        // Ensure the player is at the correct original distance
        currentDistance = startDistance;
        isJumping = false;
        animator.SetBool("isJumping", false);
        if (isSlidingButtonDown && !isJumpingButtonDown)
        {
            animator.SetBool("isSliding", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //check point earned
        if (collision.gameObject.CompareTag("Score"))
        {
            GameObject target = collision.gameObject;
            float scoreAmount = target.GetComponent<ObjectScore>().scoreAmount;
            StartCoroutine(gameManager.IncreaseScoreItem(scoreAmount));
        }

        //check gold earned
        if (collision.gameObject.tag == "Gold")
        {
            GameObject target = collision.gameObject;
            float goldAmount = target.GetComponent<ObjectGold>().goldAmount;
            StartCoroutine(gameManager.IncreaseGold(goldAmount));
        }

        //check game over
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameManager.isPlaying = false;
            gameManager.GameOver();
        }
    }

    public void JumpUp(bool _jump)
    {
        isJumpingButtonDown = _jump;
    }
    public void SlideDown(bool _slide)
    {
        isSlidingButtonDown = _slide;
    }
}
