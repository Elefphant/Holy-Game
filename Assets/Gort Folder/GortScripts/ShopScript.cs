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
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            if (i == CurrentSlot && Items[CurrentSlot] != null)
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
            if (Currency - cost >= 0 )
            {
                Currency -= cost;
                CurrencyText.text = "Currency: " + Currency;
                Destroy(Items[CurrentSlot]);

                soundCounter = 0;
                hasPlayedSFX1 = false;
                hasPlayedSFX2 = false;
                hasPlayedSFX3 = false;
            }   
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
        Indicator.transform.position = ItemSlots[CurrentSlot].position;
    }
    void Update()
    {
        PlayerInput();
        UpdateSounds();
        UpdateIndicator();
        UpdateCostTexts();
    }
}
