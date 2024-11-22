//Level1.cs - class to handle level 1 attributes

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

    //initialize sstarting currency and health
    public int currency = 500;
    public int health = 100;

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

    //sets an animation for the shop when toggle button is pressed
    public void ToggleShop()
    {
        isShopOpen = !isShopOpen;
        anim.SetBool("ShopOpen", isShopOpen);
    }


    //function to increase currency based on how much enemy is worth
    public void increaseCurrency(int amount)
    {
        currency += amount;
        UpdateCurrencyText();
    }

    //function to allow player to spend currency 
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

    //updates player currency
    private void UpdateCurrencyText()
    {
        if (currencyText != null)
        {
            currencyText.text = "<sprite name=guap>" + currency.ToString();
        }
    }

    //updates player health
    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "<sprite name=HealthNew>" + health.ToString();

        }
    }

    //takes health away from player
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
            lossScreen.SetActive(true);  //displays loss screen
        }
        Time.timeScale = 0;  //pause the game
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


