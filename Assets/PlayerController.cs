using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip jumpSound;
    public AudioClip hitSound;

    public float laneDistance = 3f;
    public float laneChangeSpeed = 10f;

    public float jumpSpeed = 10f;
    public float gravity = -25f;
    public float slideDuration = 0.5f;

    private int targetLane = 1;
    private bool isGameOver = false;
    private bool isHitProcessing = false;

    private bool isJumping = false;
    private bool isSliding = false;

    private float verticalVelocity = 0f;
    private float groundY = 0.5f;

    private int hitCount = 0;
    private float lastHitTime = -10f;
    private float hitCooldown = 10f;

    private Rigidbody rb;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            return;
        }

        if (isHitProcessing)
            return;

        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Lane
        if (Input.GetKeyDown(KeyCode.A))
            targetLane = Mathf.Max(0, targetLane - 1);

        if (Input.GetKeyDown(KeyCode.D))
            targetLane = Mathf.Min(2, targetLane + 1);

        float targetX = (targetLane - 1) * laneDistance;

        Vector3 pos = rb.position;
        pos.x = Mathf.MoveTowards(pos.x, targetX, laneChangeSpeed * Time.deltaTime);

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !isSliding)
        {
            isJumping = true;
            verticalVelocity = jumpSpeed;
            audioSource.PlayOneShot(jumpSound);
        }

        // Slide
        if (Input.GetKeyDown(KeyCode.S) && !isSliding && !isJumping)
        {
            StartCoroutine(Slide());
        }

        // Gravity
        if (isJumping)
        {
            verticalVelocity += gravity * Time.deltaTime;
            pos.y += verticalVelocity * Time.deltaTime;

            if (pos.y <= groundY)
            {
                pos.y = groundY;
                isJumping = false;
            }
        }

        rb.MovePosition(pos);
    }

    IEnumerator Slide()
    {
        isSliding = true;

        transform.localScale = new Vector3(1, 0.5f, 1);

        yield return new WaitForSeconds(slideDuration);

        transform.localScale = new Vector3(1, 1, 1);

        isSliding = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Obstacle"))
            return;

        if (isGameOver)
            return;

        Obstacle obstacle = collision.gameObject.GetComponent<Obstacle>();

        // 🧠 CORE LOGIC
        if (obstacle != null)
        {
            if (obstacle.type == ObstacleType.Tall && isJumping)
                return;

            if (obstacle.type == ObstacleType.Low && isSliding)
                return;
        }

        Vector3 toObstacle = (collision.transform.position - transform.position).normalized;

        if (Vector3.Dot(toObstacle, Vector3.forward) > 0.7f)
        {
            Time.timeScale = 0f;
            isGameOver = true;
            GameManager.instance.GameOver();
            return;
        }

        float currentTime = Time.time;

        if (currentTime - lastHitTime > hitCooldown)
            hitCount = 0;

        hitCount++;
        lastHitTime = currentTime;

        if (isHitProcessing)
            return;

        StartCoroutine(HandleHit());
    }

    IEnumerator HandleHit()
    {
        isHitProcessing = true;
        audioSource.PlayOneShot(hitSound);

        rb.linearVelocity = Vector3.zero;
        transform.position += Vector3.back * 0.8f;

        HitFlash.instance.Flash();
        CameraShake.instance.Shake(0.2f, 0.2f);

        yield return new WaitForSeconds(0.2f);

        if (hitCount >= 2)
        {
            Time.timeScale = 0f;
            isGameOver = true;
            GameManager.instance.GameOver();
        }

        isHitProcessing = false;
    }
}