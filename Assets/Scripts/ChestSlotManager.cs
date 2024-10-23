using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSlotManager : MonoBehaviour
{
    public int maxSlots = 4;
    public List<Chest> chestSlots = new List<Chest>();
    public ChestType[] availableChestConfigs;
    public GameObject chestPrefab;
    public Transform chestParentContainer;
    public CurrencyUIHandler currencyUIHandler;

    private bool isChestUnlocking = false;
    private CurrencyManager currencyManager;
    private void Awake()
    {
        currencyManager = FindAnyObjectByType<CurrencyManager>();
        currencyUIHandler = FindAnyObjectByType<CurrencyUIHandler>();
    }
    public void AddChestToSlot(ChestType chestType)
    {
        if (chestSlots.Count < maxSlots)
        {
            var newChest = new Chest(chestType);
            chestSlots.Add(newChest);
        }
        else
        {
            Debug.Log("Slots are full.");
        }
    }
    public void GenerateRandomChest()
    {
        if (chestSlots.Count < maxSlots)
        {
            ChestType randomChestConfig = availableChestConfigs[UnityEngine.Random.Range(0, availableChestConfigs.Length)];
            Chest newChest = new Chest(randomChestConfig);
            chestSlots.Add(newChest);

            GameObject chestSlotInstance = Instantiate(chestPrefab, chestParentContainer);
            ChestSlotUI chestSlotUI = chestSlotInstance.GetComponent<ChestSlotUI>();
            chestSlotUI.Initialize(newChest, this);
        }
        else
        {
            Debug.Log("No available slots. All slots are full.");
        }
    }
    private void Update()
    {
        if (isChestUnlocking)
        {
            foreach (var chest in chestSlots)
            {
                chest.UpdateTimer(Time.deltaTime);
                if (chest.State == Chest.ChestState.Unlocked)
                {
                    isChestUnlocking = false;
                }
            }
        }
    }

    public void StartUnlockingChest(Chest chest)
    {
        if (!isChestUnlocking && chest.State == Chest.ChestState.Locked)
        {
            chest.StartUnlocking();
            isChestUnlocking = true;
        }
        else
        {
            Debug.Log("A chest is already unlocking.");
        }
    }

    public void UnlockWithGems(Chest chest, int playerGems)
    {
        if (chest.State == Chest.ChestState.Unlocking)
        {
            int timeLeftInMinutes = Mathf.CeilToInt(chest.TimeRemaining / 60f);
            int gemsRequired = Mathf.CeilToInt(timeLeftInMinutes / 10f);
            if (playerGems >= gemsRequired)
            {
                chest.UnlockWithGems(gemsRequired, currencyManager);
                isChestUnlocking = false;
                currencyUIHandler.currencyManager.SubtractGems(gemsRequired);
                currencyUIHandler.UpdateCurrencyUI();
            }
            else
            {
                Debug.Log("Not enough gems.");
            }
        }
    }

    public void CollectChest(Chest chest, CurrencyManager currencyManager)
    {
        if (chest.State == Chest.ChestState.Unlocked)
        {
            chest.CollectRewards(currencyManager);
            chestSlots.Remove(chest);
        }
    }
}
