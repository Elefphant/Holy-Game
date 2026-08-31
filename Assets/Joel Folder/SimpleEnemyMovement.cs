using UnityEngine;

public class SimpleEnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public bool isWalking = true;
    public bool isGoingLeft = true;

    [Header("Homing")]
    public bool homing;
    public bool isAggro;
    public float aggroDistance;
    public float aggroSpeed;
    public GameObject player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!isGoingLeft)
        {
            Flip();
        }
    }

    void Update()
    {
        // Speed magnitude based on aggro
        float currentSpeed = isAggro ? aggroSpeed : speed;

        // Movement (Direction is determined by isGoingLeft)
        if (isWalking)
        {
            float direction = isGoingLeft ? -1f : 1f;
            rb.linearVelocityX = currentSpeed * direction;
        }
        else
        {
            rb.linearVelocityX = 0f;
        }

        // Homing Logic
        if (!homing || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer < aggroDistance)
        {
            isAggro = true;

            // Player is to the right, but enemy is facing left
            if (player.transform.position.x > transform.position.x && isGoingLeft)
            {
                Flip();
            }
            // Player is to the left, but enemy is facing right
            else if (player.transform.position.x < transform.position.x && !isGoingLeft)
            {
                Flip();
            }
        }
        else isAggro = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Flip();
    }

    public void Flip()
    {
        isGoingLeft = !isGoingLeft;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
