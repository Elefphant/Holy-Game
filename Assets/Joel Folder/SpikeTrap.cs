using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public float extendSpeed;
    public float retractSpeed;

    public float maxLength;

    public float extendTimer;
    private float eTimer;

    public bool isRetracting = false;

    private Vector3 originalPos;
    void Start()
    {
        eTimer = extendTimer;

        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        extendTimer -= Time.deltaTime;
        if (extendTimer <= 0 && !isRetracting)
        {
            Extend();
        }
        else if (isRetracting)
        {
            Retract();
        }
    }

    public void Extend()
    {
        GetComponent<Rigidbody2D>().linearVelocityY = extendSpeed;
        if (transform.position.y >= maxLength)
        {
            GetComponent<Rigidbody2D>().linearVelocityY = 0;
            isRetracting = true;
        }
    }
    public void Retract()
    {
        GetComponent<Rigidbody2D>().linearVelocityY = -retractSpeed;
        if (transform.position.y <= originalPos.y)
        {
            transform.position = originalPos;
            GetComponent<Rigidbody2D>().linearVelocityY = 0;
            isRetracting = false;
            extendTimer = eTimer;
        }
    }
}
