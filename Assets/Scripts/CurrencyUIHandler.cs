using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyUIHandler : MonoBehaviour
{
    public CurrencyManager currencyManager;

    public TMP_Text coinsText;
    public TMP_Text gemsText;


    private void Start()
    {
        currencyManager.OnCurrencyChanged += UpdateCurrencyUI;

        UpdateCurrencyUI();
    }

    public void AddCoinsFromUI(int amount)
    {
        currencyManager.AddCoins(amount);
    }

    public void AddGemsFromUI(int amount)
    {
        currencyManager.AddGems(amount);
    }

    public void SubtractCoinsFromUI(int amount)
    {
        currencyManager.SubtractCoins(amount);
    }

    public void SubtractGemsFromUI(int amount)
    {
        currencyManager.SubtractGems(amount);
    }


    public void UpdateCurrencyUI()
    {
        coinsText.text = currencyManager.Coins.ToString();
        gemsText.text = currencyManager.Gems.ToString();
    }
    private void OnDestroy()
    {
        currencyManager.OnCurrencyChanged -= UpdateCurrencyUI;
    }
}
