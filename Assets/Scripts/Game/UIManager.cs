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
        EventManager.CollectedCoin += UpdateCollectibleCount;
    }
    private void OnDisable()
    {
        EventManager.CollectedCoin -= UpdateCollectibleCount;
    }
    void Start()
    {
        UpdateCollectibleCount();
    }

    void UpdateCollectibleCount()
    {
        totalCollectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;
        collectibleText.text = "Collectibles: " + totalCollectibles;
        if (totalCollectibles <= 0) { SceneManager.LoadScene("GameOver"); }
    }
}
