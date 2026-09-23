using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class HolyGame_PlayerMovement : MonoBehaviour
{
    Rigidbody2D GodRB;
    public InputActionAsset InputActions;
    SpriteRenderer God_Sprite;
    public int FacingDirection = 1;

    #region Input Actions
    private InputAction G_MoveAction;
    private InputAction G_JumpAction;
    private InputAction G_AttackAction;
    private InputAction G_DashAction;
    #endregion

    private Vector2 G_MoveAmt;
    private int G_DashAmt;

    #region Floats
    public float G_MovementSpeed;
    public float G_JumpSpeed;
    [Range(0f,1f)]public float G_JumpMultiplier = 0.5f;
    public float G_DashSpeed;
    public float G_DashLength = 0.2f; // in seconds
    public float G_MeleeDamage;
    public float G_MeleeAttackCooldown = 0.5f; // in seconds
    #endregion

    public Transform GroundCheck;
    public Vector2 GroundCheckBox = new Vector2(0.4f, 0.1f);
    public LayerMask Groundmask;
    public Transform MeleeAttack;
    public Vector2 MeleeAttackRange;
    public Vector2 ConstantMeleeBoxSize = new Vector2(1f, 1f);
    public Vector2 DashHurtBox = new Vector2(0.5f, 1f);
    public LayerMask EnemyLayerMask;

    #region Unlockables
    public bool HealthBuff = false;
    public bool StrengthBuff = false;
    public bool ManaBuff = false;
    public bool DashWeapon = false;
    #endregion

    #region Bools
    private bool IsAttacking = false;
    private bool IsDashing = false;
    private bool IsFacingRight;
    private bool GroundCheckFrame;
    #endregion

    #region Enbl Disbl
    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }
    #endregion

    private void Awake()
    {
        GodRB = GetComponent<Rigidbody2D>();
        God_Sprite = GetComponent<SpriteRenderer>();

        var playerAction = InputActions.FindActionMap("Player");
        G_MoveAction = playerAction.FindAction("Move");
        G_JumpAction = playerAction.FindAction("Jump");
        G_AttackAction = playerAction.FindAction("Attack");
        G_DashAction = playerAction.FindAction("Dash");
    }
    private void Update()
    {
        G_MoveAmt = G_MoveAction.ReadValue<Vector2>();
        GroundCheckFrame = IsGrounded(Groundmask);

        #region KeyPress
        if (G_DashAction.WasPressedThisFrame() && G_DashAmt > 0)
        {
            Dash();
        }

        if (G_JumpAction.WasPressedThisFrame() && GroundCheckFrame)
        {
            Jump();
        }

        if (G_JumpAction.WasReleasedThisFrame() && GodRB.linearVelocity.y > 0)
        {
            GodRB.linearVelocity = new Vector2(GodRB.linearVelocity.x, GodRB.linearVelocity.y * G_JumpMultiplier);
        }

        if (G_AttackAction.WasPressedThisFrame())
        {
            MeleeAttackAction();
        }

        if (G_MoveAmt.x < 0f && !IsFacingRight)
        {
            Flip();
        }
        if (G_MoveAmt.x > 0f && IsFacingRight)
        {
            Flip();
        }
        #endregion
    }
    
    #region Walk
    private void Walk()
    {
        float moveVelocityX = (G_MoveAmt.x * G_MovementSpeed);
        float moveVelocityY = GodRB.linearVelocity.y;
        GodRB.linearVelocity = new Vector2(moveVelocityX, moveVelocityY);
    }
    #endregion

    #region Flip Sprite
    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }
    #endregion

    #region Dash
    private void Dash()
    {
        if (!IsDashing)
        {
            StartCoroutine(DashRoutine());
        }
    }

    private IEnumerator DashRoutine()
    {
        IsDashing = true;
        float horizontalMovement = G_MoveAmt.x;
        Vector2 dashDirection;
        if (G_MoveAmt.x != 0)
        {
            dashDirection = new Vector2(Mathf.Sign(horizontalMovement), 0);
        }
        else
        {
            float facingDirection = IsFacingRight ? -1f : 1f;
            dashDirection = new Vector2(facingDirection, 0);
        }

        if (DashWeapon)
        {
            MeleeAttackRange = DashHurtBox;
            MeleeAttackAction();
            Debug.Log("Dash Attacked");
        }

        GodRB.linearVelocity = dashDirection * G_DashSpeed;

        float originalGravity = GodRB.gravityScale;
        GodRB.gravityScale = 0;
        G_DashAmt -= 1;

        yield return new WaitForSeconds(G_DashLength);

        GodRB.gravityScale = originalGravity;
        IsDashing = false;
    }
    #endregion

    #region GroundCheck
    private bool IsGrounded(LayerMask groundLayer)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(GroundCheck.position, GroundCheckBox, 0f, groundLayer);

        foreach (var collider in colliders)
        {
            if (collider.gameObject == gameObject)
                continue;
            G_DashAmt = 1;
            return true; 
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        if (GroundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(GroundCheck.position, GroundCheckBox);
        }
    }
    #endregion

    #region Jump
    private void Jump()
    {
        if (!IsDashing)
        {
            GodRB.linearVelocity = new Vector2(GodRB.linearVelocity.x, 0);
            GodRB.AddForce(Vector2.up * G_JumpSpeed, ForceMode2D.Impulse);
        }
    }
    #endregion

    #region Attack
    private void MeleeAttackAction()
    {
        if (!IsAttacking)
        {
            StartCoroutine(MeleeAttackRoutine(EnemyLayerMask));
        }
    }
    private IEnumerator MeleeAttackRoutine(LayerMask enemyLayer)
    {
        IsAttacking = true;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(MeleeAttack.position, MeleeAttackRange, 0f, enemyLayer);
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<SimpleEnemyMovement>(out SimpleEnemyMovement enemy))
            {
                enemy.TakeDamage(G_MeleeDamage);
            }
        }
        Debug.Log("Attacked");
        yield return new WaitForSeconds(G_MeleeAttackCooldown);
        MeleeAttackRange = ConstantMeleeBoxSize;
        IsAttacking = false;
    }
    private void OnDrawGizmos()
    {
        if (MeleeAttack != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(MeleeAttack.position, MeleeAttackRange);
        }
    }
    #endregion

    private void FixedUpdate()
    {
        if (!IsDashing)
        {
            Walk();
        }
    }
}
