using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI collectibleText;
    private int totalCollectibles;
    private void OnEnable()
    {
        EventManager.CollectedCoin += DiscountCollectibleCount;
    }
    private void OnDisable()
    {
        EventManager.CollectedCoin -= DiscountCollectibleCount;
    }
    void Start()
    {
        FindCollectibles();
        UpdateCollectibleCount();
    }
    void FindCollectibles()
    {
        totalCollectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;
    }
    void UpdateCollectibleCount()
    {
        collectibleText.text = "Collectibles: " + totalCollectibles;
    }
    void DiscountCollectibleCount()
    {
        totalCollectibles -= 1;
        UpdateCollectibleCount();
    }
}