using UnityEngine;

public class SimpleFlyingEnemy : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public float damage;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocityX = -speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //deal damage
        }
    }
}
