using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private const string CoinsKey = "Coins";
    private const string GemsKey = "Gems";

    public int Coins { get; private set; }
    public int Gems { get; private set; }

    public event Action OnCurrencyChanged;

    private void Start()
    {
        LoadCurrency();
    }
    public void AddCoins(int amount)
    {
        Coins += amount;
        OnCurrencyChanged?.Invoke();
        SaveCurrency();
    }

    public void AddGems(int amount)
    {
        Gems += amount;
        OnCurrencyChanged?.Invoke();
        SaveCurrency();
    }

    public bool TrySpendGems(int amount)
    {
        if (Gems >= amount)
        {
            Gems -= amount;
            OnCurrencyChanged?.Invoke();
            return true;
        }
        return false;
    }
    public void SubtractCoins(int amount)
    {
        if (Coins >= amount)
        {
            Coins -= amount;
            OnCurrencyChanged?.Invoke();
            SaveCurrency();
        }
    }

    public void SubtractGems(int amount)
    {
        if (Gems >= amount)
        {
            Gems -= amount;
            OnCurrencyChanged?.Invoke();
            SaveCurrency();
        }
    }
    public void SetCoins(int newAmount)
    {
        Coins = newAmount;
        OnCurrencyChanged?.Invoke();
        SaveCurrency();
    }

    public void SetGems(int newAmount)
    {
        Gems = newAmount;
        OnCurrencyChanged?.Invoke();
        SaveCurrency();
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(CoinsKey, Coins);
        PlayerPrefs.SetInt(GemsKey, Gems);
        PlayerPrefs.Save(); 
    }


    private void LoadCurrency()
    {
        Coins = PlayerPrefs.GetInt(CoinsKey, 0); 
        Gems = PlayerPrefs.GetInt(GemsKey, 0); 
        OnCurrencyChanged?.Invoke(); 
    }
}
