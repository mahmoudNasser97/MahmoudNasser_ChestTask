using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChestSlotUI : MonoBehaviour
{
    public TMP_Text chestTypeText;
    public TMP_Text timerText;
    public Button startTimerButton;
    public Button unlockWithGemsButton;
    public Button collectButton;
    public GameObject chestPopUpPanel;
    public CurrencyManager currencyManager;
    private Chest chest;
    private ChestSlotManager chestSlotManager;
    private bool isPopUpActive = false;

    private void Awake()
    {
        currencyManager = FindAnyObjectByType<CurrencyManager>();
    }
    public void Initialize(Chest chest, ChestSlotManager slotManager)
    {
        this.chest = chest;
        this.chestSlotManager = slotManager;
        chestTypeText.text = chest.Config.chestName;
        if (currencyManager == null)
        {
            currencyManager = FindObjectOfType<CurrencyManager>();
        }
        UpdateUI();

        startTimerButton.onClick.AddListener(OnStartTimerClicked);
        unlockWithGemsButton.onClick.AddListener(OnUnlockWithGemsClicked);
        collectButton.onClick.AddListener(OnCollectClicked);
    }

    private void Update()
    {
        if (chest.State == Chest.ChestState.Unlocking)
        {
            chest.UpdateTimer(Time.deltaTime);
            UpdateUI();
        }
    }

    public void OnChestClicked()
    {
        isPopUpActive = !isPopUpActive;
        chestPopUpPanel.SetActive(isPopUpActive);

        if (isPopUpActive)
        {
            chestPopUpPanel.GetComponentInChildren<Text>().text = GetChestDetails();
        }
    }

    private string GetChestDetails()
    {
        return $"Type: {chest.Config.chestName}\nCoins: {chest.CoinsReward}\nGems: {chest.GemsReward}\nState: {chest.State.ToString()}\nUnlock Cost: {chest.GetType()} Gems";
    }

    private void OnStartTimerClicked()
    {
        if (chest.State == Chest.ChestState.Locked)
        {
            chestSlotManager.StartUnlockingChest(chest);
            UpdateUI();
        }
    }
    private void OnUnlockWithGemsClicked()
    {
        if (chest.State == Chest.ChestState.Unlocking)
        {
            chestSlotManager.UnlockWithGems(chest, currencyManager.Gems);
            UpdateUI();
        }
    }

    private void OnCollectClicked()
    {
        if (chest.State == Chest.ChestState.Unlocked)
        {
            chestSlotManager.CollectChest(chest, currencyManager);
            Destroy(this.gameObject);
        }
    }
    private void UpdateUI()
    {
        if (chest.State == Chest.ChestState.Locked)
        {
            timerText.text = "Locked";
            startTimerButton.gameObject.SetActive(true);
            unlockWithGemsButton.gameObject.SetActive(false);
            collectButton.gameObject.SetActive(false);
        }
        else if (chest.State == Chest.ChestState.Unlocking)
        {
            TimeSpan time = TimeSpan.FromSeconds(chest.TimeRemaining);
            timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
            startTimerButton.gameObject.SetActive(false);
            unlockWithGemsButton.gameObject.SetActive(true);
            collectButton.gameObject.SetActive(true);
            chestPopUpPanel.gameObject.SetActive(false);

        }
        else if (chest.State == Chest.ChestState.Unlocked)
        {
            timerText.text = "Unlocked";
            startTimerButton.gameObject.SetActive(false);
            unlockWithGemsButton.gameObject.SetActive(false);
            collectButton.gameObject.SetActive(true);
            chestPopUpPanel.gameObject.SetActive(false);

        }
    }
}
