using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Level1 : MonoBehaviour
{

    [SerializeField] Animator anim;

    private bool isShopOpen = true;

    public static Level1 main;

    public Transform startPoint;
    public Transform[] path;

    public int currency = 500;
    public int health = 100;

    // Reference to the TextMeshProUGUI component that displays the currency
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI healthText;

    [Header("UI References")]
    public GameObject lossScreen;

    private bool gameStarted = false;
    private void Start()
    {
        UpdateCurrencyText();
        UpdateHealthText();
    }

    private void Awake()
    {
        main = this;
    }

    public void ToggleShop()
    {
        isShopOpen = !isShopOpen;
        anim.SetBool("ShopOpen", isShopOpen);
    }


    // Method to increase the player's currency
    public void increaseCurrency(int amount)
    {
        currency += amount;
        UpdateCurrencyText();
    }

    // Method to spend currency and check if the player has enough
    public bool spendCurrency(int amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            UpdateCurrencyText();
            return true;
        }
        else
        {
            Debug.Log("Not enough currency");
            return false;
        }
    }

    // Method to update the displayed currency
    private void UpdateCurrencyText()
    {
        if (currencyText != null)
        {
            currencyText.text = "<sprite name=guap>" + currency.ToString();
        }
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "<sprite name=HealthNew>" + health.ToString();

        }
    }

    public void DeductHealth(int amount)
    {
        health -= amount;
        UpdateHealthText();

        if (health <= 0)
        {
            Debug.Log("Game Over!");
            GameOver();
        }
    }

    public void StartGame()
    {
        gameStarted = true;
        Spawner.main.StartWaves();
    }

    public bool IsGameStarted()
    {
        return gameStarted;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        if (lossScreen != null)
        {
            lossScreen.SetActive(true);  // Show the Loss Screen UI
        }
        Time.timeScale = 0;  // Pause the game
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}


