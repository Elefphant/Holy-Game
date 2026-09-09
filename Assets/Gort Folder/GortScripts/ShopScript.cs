using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public List<GameObject> Items = new List<GameObject>();
    public List<Transform> ItemSlots = new List<Transform>();
    public List<Vector2Int> SlotCoordinates = new List<Vector2Int>();

    public GameObject Potion;
    public GameObject Potion2;
    public GameObject Helmet;
    public GameObject Sword;
    public GameObject Book;

    public GameObject Indicator;
    public int Columns = 5;
    private int CurrentSlot = 0;

    public int Currency;
    public Text CurrencyText;
    void Start()
    {
        Currency = 100;
        RefreshShop();
        UpdateIndicator();
        UpdateCostTexts();
    }
    void RefreshShop()
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            GameObject newItem = new GameObject();
            int decider = Random.Range(1, 6);
            
            if (decider == 1)
            {
                newItem = Instantiate(Potion, ItemSlots[i].position, ItemSlots[i].rotation);
            }
            else if (decider == 2)
            {
                newItem = Instantiate(Potion2, ItemSlots[i].position, ItemSlots[i].rotation);
            }
            else if (decider == 3)
            {
                newItem = Instantiate(Helmet, ItemSlots[i].position, ItemSlots[i].rotation);
            }
            else if (decider == 4)
            {
                newItem = Instantiate(Book, ItemSlots[i].position, ItemSlots[i].rotation);
            }
            else if (decider == 5)
            {
                newItem = Instantiate(Sword, ItemSlots[i].position, ItemSlots[i].rotation);
            }

            Items.Add(newItem);
        }
        CurrencyText.text = "Currency: " + Currency;
    }
    void UpdateCostTexts()
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            if (i == CurrentSlot)
            {
                ItemSlots[i].GetChild(0).GetChild(0).GetComponent<Text>().text = "Cost: " + Items[CurrentSlot].GetComponent<ItemStatsScript>().Price;
            }
            else
            {
                ItemSlots[i].GetChild(0).GetChild(0).GetComponent<Text>().text = "";
            }
        }
    }
    void PlayerInput()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && Items[CurrentSlot] != null)
        {
            int cost = Items[CurrentSlot].GetComponent<ItemStatsScript>().Price;
            if (Currency - cost > 0 )
            {
                Currency -= cost;
                CurrencyText.text = "Currency: " + Currency;
                Destroy(Items[CurrentSlot]);
            }   
        }
    }
    void Move(Vector2Int direction)
    {
        Vector2Int currentCoordinate = SlotCoordinates[CurrentSlot];
        Vector2Int targetCoordinate = currentCoordinate + direction;

        for (int i = 0; i < SlotCoordinates.Count; i++)
        {
            if (SlotCoordinates[i] == targetCoordinate)
            {
                CurrentSlot = i;
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

        Indicator.transform.position = ItemSlots[CurrentSlot].position;
    }
    void Update()
    {
        PlayerInput();
        UpdateIndicator();
        UpdateCostTexts();
    }
}
