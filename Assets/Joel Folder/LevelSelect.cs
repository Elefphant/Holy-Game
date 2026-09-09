using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelSelect : MonoBehaviour
{
    [Header("Levels")]
    public List<Transform> levels = new List<Transform>();
    public List<Vector2Int> levelCoordinates = new List<Vector2Int>();
    private int currentLevel = 0;
    public float moveSpeed;

    [Serializable]
    public struct LevelConnection
    {
        public int levelAIndex;
        public int levelBIndex;
    }

    // Explicitly define which levels connect to each other in the Inspector
    public List<LevelConnection> connections = new List<LevelConnection>();


    [Header("Inputs")]
    public InputActionAsset InputActions;
    private InputAction NavigateAction;
    private InputAction SelectAction;

    private Vector2Int bufferedDirection = Vector2Int.zero;
    private bool hasBufferedInput = false;
    private void OnEnable()
    {
        InputActions.FindActionMap("UI").Enable();
    }
    private void OnDisable()
    {
        InputActions.FindActionMap("UI").Disable();
    }
    private void Awake()
    {
        var uiAction = InputActions.FindActionMap("UI");
        NavigateAction = uiAction.FindAction("Navigate");
        SelectAction = uiAction.FindAction("Submit");
    }

    private void Input()
    {
        Vector2 input = NavigateAction.ReadValue<Vector2>();

        if (NavigateAction.WasPressedThisFrame())
        {
            Vector2Int newDirection = Vector2Int.zero;

            if (input.x > 0) newDirection = Vector2Int.right;
            else if (input.x < 0) newDirection = Vector2Int.left;
            else if (input.y > 0) newDirection = Vector2Int.down;
            else if (input.y < 0) newDirection = Vector2Int.up;

            if (newDirection != Vector2Int.zero)
            {
                // If standing still, perform the move immediately
                if (transform.position == levels[currentLevel].position)
                {
                    Move(newDirection);
                }
                // If mid-movement, buffer the input for when arrival completes
                else
                {
                    bufferedDirection = newDirection;
                    hasBufferedInput = true;
                }
            }
        }
    }
    void Move(Vector2Int direction)
    {
        Vector2Int currentCoordinate = levelCoordinates[currentLevel];
        Vector2Int targetCoordinate = currentCoordinate + direction;

        for (int i = 0; i < levelCoordinates.Count; i++)
        {
            if (levelCoordinates[i] == targetCoordinate && AreLevelsConnected(currentLevel, i))
            {
                currentLevel = i;
                return;
            }
        }
    }
    public bool AreLevelsConnected(int indexA, int indexB)
    {
        foreach (var conn in connections)
        {
            if ((conn.levelAIndex == indexA && conn.levelBIndex == indexB) ||
                (conn.levelAIndex == indexB && conn.levelBIndex == indexA))
            {
                return true;
            }
        }
        return false;
    }
    private void Update()
    {
        Input();

        if (transform.position != levels[currentLevel].position)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                levels[currentLevel].position,
                moveSpeed * Time.deltaTime
            );
        }
        
        else if (hasBufferedInput)
        {
            hasBufferedInput = false;
            Vector2Int dirToExecute = bufferedDirection;
            bufferedDirection = Vector2Int.zero;

            Move(dirToExecute);
        }
    }
}
