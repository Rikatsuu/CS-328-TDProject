//MainMenu.cs - class to handle overall game behavior from the main menu

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadSceneAsync("Level 1"); //loads level 1 to start game
    }

    public void quitGame()
    {
        Application.Quit(); //quits game when button is clicked
    }
}
