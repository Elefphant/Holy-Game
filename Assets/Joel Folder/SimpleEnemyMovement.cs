using UnityEngine;

public class SimpleEnemyMovement : MonoBehaviour
{
    public float speed;
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
        gameObject.GetComponent<Rigidbody2D>().linearVelocityX = speed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Flip();
    }

    public void Flip()
    {
        isGoingLeft = !isGoingLeft;
        speed *= -1;
    }
}
