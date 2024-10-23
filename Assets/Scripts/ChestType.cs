using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestType", menuName = "Chests/ChestType")]
public class ChestType : ScriptableObject
{
    public string chestName;
    public int minCoins;
    public int maxCoins;
    public int minGems;
    public int maxGems;
    public int unlockTimeMinutes;
}
public class Chest
{
    public enum ChestState { Locked, Unlocking, Unlocked, Collected }

    public ChestState State { get; private set; }
    public ChestType Config { get; private set; }
    public int CoinsReward { get; private set; }
    public int GemsReward { get; private set; }
    public float TimeRemaining { get; private set; }

    public CurrencyManager currencyManager;
    
    
    public Chest(ChestType config)
    {
        Config = config;
        CoinsReward = Random.Range(config.minCoins, config.maxCoins);
        GemsReward = Random.Range(config.minGems, config.maxGems);
        TimeRemaining = config.unlockTimeMinutes;
        State = ChestState.Locked;
    }

    public void StartUnlocking()
    {
        if (State == ChestState.Locked)
        {
            State = ChestState.Unlocking;
        }
    }

    public void UpdateTimer(float deltaTime)
    {
        if (State == ChestState.Unlocking)
        {
            TimeRemaining -= deltaTime;
            if (TimeRemaining <= 0)
            {
                TimeRemaining = 0;
                State = ChestState.Unlocked;
            }
        }
    }

    public void UnlockWithGems(int gemsToSpend,CurrencyManager currencyManager)
    {
        if (currencyManager.Gems >= gemsToSpend)
        {
            currencyManager.SubtractCoins(gemsToSpend);
            TimeRemaining = 0;
            State = ChestState.Unlocked;
        }
    }

    public void CollectRewards(CurrencyManager currencyManager)
    {
        if (State == ChestState.Unlocked)
        {
            currencyManager.AddCoins(CoinsReward);
            currencyManager.AddGems(GemsReward);
            State = ChestState.Collected;
        }
    }
}
