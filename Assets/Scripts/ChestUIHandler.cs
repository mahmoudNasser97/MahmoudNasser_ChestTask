using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestUIHandler : MonoBehaviour
{
    public ChestSlotManager chestSlotManager;
    public Button generateNewChestButton;

    private void Start()
    {      
        generateNewChestButton.onClick.AddListener(OnGenerateChestButtonClicked);
    }

    private void OnGenerateChestButtonClicked()
    {
        chestSlotManager.GenerateRandomChest();
    }
}
