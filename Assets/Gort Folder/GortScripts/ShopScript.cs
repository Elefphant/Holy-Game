using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopScript : MonoBehaviour
{
    public List<Transform> ItemSlots = new List<Transform>();
    public List<Vector2Int> SlotCoordinates = new List<Vector2Int>();

    public GameObject Potion;
    public GameObject Potion2;
    public GameObject Helmet;
    public GameObject Sword;
    public GameObject Book;

    public GameObject Indicator;

    public int columns = 5;

    private int currentSlot = 0;
    void Start()
    {
        RefreshShop();
        UpdateIndicator();
    }
    void RefreshShop()
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            int decider = Random.Range(1, 6);
            
            if (decider == 1)
            {
                Instantiate(Potion, ItemSlots[i].position, ItemSlots[i].rotation);
            }
            else if (decider == 2)
            {
                Instantiate(Potion2, ItemSlots[i].position, ItemSlots[i].rotation);
            }
            else if (decider == 3)
            {
                Instantiate(Helmet, ItemSlots[i].position, ItemSlots[i].rotation);
            }
            else if (decider == 4)
            {
                Instantiate(Book, ItemSlots[i].position, ItemSlots[i].rotation);
            }
            else if (decider == 5)
            {
                Instantiate(Sword, ItemSlots[i].position, ItemSlots[i].rotation);
            }
        }
    }
    void Move(Vector2Int direction)
    {
        Vector2Int currentCoordinate = SlotCoordinates[currentSlot];
        Vector2Int targetCoordinate = currentCoordinate + direction;

        for (int i = 0; i < SlotCoordinates.Count; i++)
        {
            if (SlotCoordinates[i] == targetCoordinate)
            {
                currentSlot = i;
                return;
            }
        }
    }
    void UpdateIndicator()
    {
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            Move(Vector2Int.right);
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            Move(Vector2Int.left);
        }

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            Move(Vector2Int.up);
        }

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            Move(Vector2Int.down);
        }

        Indicator.transform.position = ItemSlots[currentSlot].position;
    }
    void Update()
    {
        UpdateIndicator();
    }
}
