using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class HolyGame_PlayerMovement : MonoBehaviour
{
    Rigidbody2D GodRB;
    public InputActionAsset InputActions;
    SpriteRenderer God_Sprite;
    public int FacingDirection = 1;

    private InputAction G_MoveAction;
    private InputAction G_JumpAction;
    private InputAction G_AttackAction;
    private InputAction G_DashAction;

    private Vector2 G_MoveAmt;

    public float G_MovementSpeed;
    public float G_JumpSpeed;
    public float G_DashSpeed;

    private bool IsFacingRight;
    private bool GroundCheckFrame;
    public Transform GroundCheck;
    public Vector2 GroundCheckBox = new Vector2(0.4f, 0.1f);
    public LayerMask Groundmask;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }
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

        if (G_JumpAction.WasPressedThisFrame() && GroundCheckFrame)
        {
            Jump();
        }

        if (G_MoveAmt.x < 0f && !IsFacingRight)
        {
            Flip();
        }
        if (G_MoveAmt.x > 0f && IsFacingRight)
        {
            Flip();
        }
    }

    private bool IsGrounded(LayerMask groundLayer)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(GroundCheck.position, GroundCheckBox, 0f, groundLayer);

        foreach (var collider in colliders)
        {
            if (collider.gameObject == gameObject)
                continue;
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

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }
    private void Jump()
    {
        GodRB.linearVelocity = new Vector2(GodRB.linearVelocity.x, 0);
        GodRB.AddForce(Vector2.up * G_JumpSpeed, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        float moveVelocityX = (G_MoveAmt.x * G_MovementSpeed);
        float moveVelocityY = GodRB.linearVelocity.y;
        GodRB.linearVelocity = new Vector2(moveVelocityX, moveVelocityY);
    }
}
