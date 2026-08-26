using UnityEngine;

public class SimpleEnemyMovement : MonoBehaviour
{
    public float speed;
    public bool isWalking = true;
    public bool isGoingLeft = true;

    void Start()
    {
        if (!isGoingLeft)
        {
            Flip();
        }
    }

    void Update()
    {
        if (isWalking)
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = speed;
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().linearVelocityX = 0f;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Flip();
    }

    public void Flip()
    {
        isGoingLeft = !isGoingLeft;
        speed *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
