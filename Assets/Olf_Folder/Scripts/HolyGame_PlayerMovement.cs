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


    private InputAction G_MoveAction;
    private InputAction G_JumpAction;
    private InputAction G_AttackAction;
    private InputAction G_DashAction;

    private Vector2 G_MoveAmt;

    public float G_MovementSpeed;
    public float G_JumpSpeed;
    public float G_DashSpeed;

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
    }
    private void FixedUpdate()
    {
        float moveVelocityX = (G_MoveAmt.x * G_MovementSpeed);
        float moveVelocityY = GodRB.linearVelocity.y;
        GodRB.linearVelocity = new Vector2(moveVelocityX, moveVelocityY);
    }
}
