using UnityEngine;

public class Throwable : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Throw Settings")]
    public Vector2 throwDirection = new Vector2(1f, 1f); // Throw angle (up and right)
    public float throwForce = 10f;                       // Speed/Power of the throw

    public bool throwing = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (throwing)
        {
            Throw();
            throwing = false;
        }
    }

    public void Throw()
    {
        // Reset velocity first so previous movement doesn't interfere
        rb.linearVelocity = Vector2.zero;

        // Apply force directly using Impulse mode for a quick burst
        rb.AddForce(throwDirection.normalized * throwForce, ForceMode2D.Impulse);
    }
}
