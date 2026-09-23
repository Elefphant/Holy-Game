using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public List<AudioClip> coinSounds;

    private List<GameObject> Items = new List<GameObject>();
    private List<List<GameObject>> ItemGroups = new List<List<GameObject>>();
    public List<Transform> ItemSlots = new List<Transform>();
    public List<Vector2Int> GroupCoordinates = new List<Vector2Int>();
    private List<int> GroupItemIndexes = new List<int>();

    public GameObject Potion;
    public GameObject Potion2;
    public GameObject Helmet;
    public GameObject Sword;
    public GameObject Book;

    public GameObject Indicator;
    public int Columns = 5;
    private int CurrentGroup = 0;

    public int Currency;
    public Text CurrencyText;

    public InputActionAsset InputActions;
    private InputAction ShopNavigateAction;
    private InputAction ShopBuyAction;

    public float soundCounter = 5;
    public bool hasPlayedSFX1 = true;
    public bool hasPlayedSFX2 = true;
    public bool hasPlayedSFX3 = true;
    public float sfxTimer;

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
        ShopNavigateAction = uiAction.FindAction("Navigate");
        ShopBuyAction = uiAction.FindAction("Submit");
    }
    void Start()
    {
        Currency = 100;
        RefreshShop();
        UpdateIndicator();
        UpdateCostTexts();
    }
    void RefreshShop()
    {
        Items.Clear();
        ItemGroups.Clear();
        GroupItemIndexes.Clear();

            
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            GameObject newItem = new GameObject();
            int decider = Random.Range(1, 5);
            
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
                newItem = Instantiate(Sword, ItemSlots[i].position, ItemSlots[i].rotation);
            }

            Items.Add(newItem);
        }
        
        List<GameObject> tempList = new List<GameObject>();
        List<GameObject> tempList2 = new List<GameObject>();
        List<GameObject> tempList3 = new List<GameObject>();
        List<GameObject> tempList4 = new List<GameObject>();
        List<GameObject> tempList5 = new List<GameObject>();
        List<GameObject> tempList6 = new List<GameObject>();

        tempList.Add(Items[0]);
        tempList.Add(Items[1]);
        tempList.Add(Items[2]);
        tempList2.Add(Items[3]);
        tempList2.Add(Items[4]);
        tempList2.Add(Items[5]);
        tempList3.Add(Items[6]);
        tempList4.Add(Items[7]);
        tempList5.Add(Items[8]);
        tempList5.Add(Items[9]);
        tempList5.Add(Items[10]);
        tempList6.Add(Items[11]);
        tempList6.Add(Items[12]);
        tempList6.Add(Items[13]);

        ItemGroups.Add(tempList);
        ItemGroups.Add(tempList2);
        ItemGroups.Add(tempList3);
        ItemGroups.Add(tempList4);
        ItemGroups.Add(tempList5);
        ItemGroups.Add(tempList6);

        for (int i = 0; i < ItemGroups.Count; i++)
        {
            GroupItemIndexes.Add(0);
        }

        CurrencyText.text = "Currency: " + Currency;
    }
    void PlayRandomCoinSound()
    {
        int randomSound = Random.Range(0, coinSounds.Count);
        TestSoundScript.Instance.PlaySoundEffect(coinSounds[randomSound], 1f);
    }
    void UpdateSounds()
    {
        soundCounter += Time.deltaTime;
        if (hasPlayedSFX1 == false)
        {
            PlayRandomCoinSound();
            sfxTimer = Random.Range(0.1f, 0.2f);

            hasPlayedSFX1 = true;
        }
        else if (hasPlayedSFX2 == false)
        {
            if (soundCounter >= sfxTimer)
            {
                PlayRandomCoinSound();
                soundCounter = 0;
                sfxTimer = Random.Range(0.1f, 0.2f);

                hasPlayedSFX2 = true;
            }
        }
        else if (hasPlayedSFX3 == false)
        {
            if (soundCounter >= sfxTimer)
            {
                PlayRandomCoinSound();

                hasPlayedSFX3 = true;
            }
        }
    }
    void UpdateCostTexts()
    {
        for (int groupIndex = 0; groupIndex < ItemGroups.Count; groupIndex++) 
        { 
            List<GameObject> group = ItemGroups[groupIndex]; 
            int activeIndex = GroupItemIndexes[groupIndex]; 

            for (int itemIndex = 0; itemIndex < group.Count; itemIndex++) 
            { 
                GameObject item = group[itemIndex]; 
                if (item == null)
                {
                    continue;
                }

                int itemSlotIndex = Items.IndexOf(item); 
                if (itemSlotIndex < 0 || itemSlotIndex >= ItemSlots.Count)
                { 
                    continue; 
                }

                Text costText = ItemSlots[itemSlotIndex].GetComponentInChildren<Text>();

                if (costText == null)
                {
                    continue;
                }
                    
                if (itemIndex == activeIndex && groupIndex == CurrentGroup) 
                { 
                    ItemStatsScript stats = item.GetComponent<ItemStatsScript>(); 
                    if (stats != null) 
                    { 
                        costText.text = "Cost: " + stats.Price; 
                    } 
                } 
                else 
                { 
                    costText.text = ""; 
                } 
            } 
        }
    }
    void BuyCurrentItem()
    {
        if (CurrentGroup < 0 || CurrentGroup >= ItemGroups.Count)
        {
            return;
        }

        List<GameObject> currentGroup = ItemGroups[CurrentGroup];
        int currentItemIndex = GroupItemIndexes[CurrentGroup];

        if (currentItemIndex >= currentGroup.Count)
        {
            return;
        }

        GameObject currentItem = currentGroup[currentItemIndex];
        if (currentItem == null)
        {
            return;
        }

        int cost = currentItem.GetComponent<ItemStatsScript>().Price;

        if (Currency - cost < 0)
        {
            return;
        }
        
        Currency -= cost;
        CurrencyText.text = "Currency: " + Currency;

        Destroy(currentItem);
        GroupItemIndexes[CurrentGroup]++;

        soundCounter = 0;
        hasPlayedSFX1 = false;
        hasPlayedSFX2 = false;
        hasPlayedSFX3 = false;
        
    }
    void PlayerInput()
    {
        if (ShopBuyAction.WasPerformedThisFrame())
        {
            BuyCurrentItem();
        }

        Vector2 input = ShopNavigateAction.ReadValue<Vector2>();

        if (ShopNavigateAction.WasPressedThisFrame())
        {
            if (input.x > 0)
            {
                Move(Vector2Int.right);
            }
            else if (input.x < 0)
            {
                Move(Vector2Int.left);
            }
            else if (input.y > 0)
            {
                Move(Vector2Int.up);
            }
            else if (input.y < 0)
            {
                Move(Vector2Int.down);
            }
        }
    }
    void Move(Vector2Int direction)
    {
        Vector2Int currentCoordinate = GroupCoordinates[CurrentGroup];
        Vector2Int targetCoordinate = currentCoordinate + direction;

        for (int i = 0; i < GroupCoordinates.Count; i++)
        {
            if (GroupCoordinates[i] == targetCoordinate)
            {
                CurrentGroup = i;
                
                return;
            }
        }
    }
    void UpdateIndicator()
    {
        List<GameObject> currentGroup = ItemGroups[CurrentGroup];
        int currentItemIndex = GroupItemIndexes[CurrentGroup];

        if (currentItemIndex >= currentGroup.Count) 
        { 
            Indicator.SetActive(false); 
            return; 
        }

        GameObject currentItem = currentGroup[currentItemIndex]; 
        if (currentItem == null) 
        { 
            Indicator.SetActive(false); 
            return; 
        }

        Indicator.SetActive(true); 
        Indicator.transform.position = currentItem.transform.position;
    }
    void Update()
    {
        PlayerInput();
        UpdateSounds();
        UpdateIndicator();
        UpdateCostTexts();
    }
}
