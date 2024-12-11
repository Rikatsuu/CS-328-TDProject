using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Threading.Tasks;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;
    [SerializeField] RectTransform pausePanelRect;
    [SerializeField] float topPos, midPos;
    [SerializeField] float tweenDuration;
    [SerializeField] CanvasGroup canvasGroup;

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void Pause()
    {
        pausePanelIn();
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Home()
    {
        SceneManager.LoadScene("Main menu");
        Time.timeScale = 1;

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    
    public async void Resume()
    {
        await pausePanelOut();
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }


    void pausePanelIn()
    {
        canvasGroup.DOFade(1, tweenDuration).SetUpdate(true);
        pausePanelRect.DOAnchorPosY(midPos, tweenDuration).SetUpdate(true);
    }

    async Task pausePanelOut()
    {
        canvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
        await pausePanelRect.DOAnchorPosY(topPos, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();

    }
}
