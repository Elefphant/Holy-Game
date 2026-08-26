using UnityEngine;

public class FirstBoss : MonoBehaviour
{
    public float attackTimer;
    private float aTime;

    public float cooldownTimer;
    private float cdTime;

    public float attackActive;
    

    public bool hasAttacked;
    public BoxCollider2D attackHitbox;
    void Start()
    {
        aTime = attackTimer;
        cdTime = cooldownTimer;
        
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            GetComponent<SimpleEnemyMovement>().isWalking = false;

            if (!hasAttacked)
            {
                Attack();
            }

            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                GetComponent<SimpleEnemyMovement>().isWalking = true;
                attackTimer = aTime;
                cooldownTimer = cdTime;
                attackHitbox.enabled = false;
            }
        }
    }

    public void Attack()
    {
        attackHitbox.enabled = true;
    }
}
