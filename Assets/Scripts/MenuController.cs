﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject storyMenu;
    [SerializeField] private GameObject controlsMenu;

    [Space]
    [SerializeField] private Text GOCoinText;
    [SerializeField] private GameObject GOPanel;
    [SerializeField] private GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void menuStartClicked()
    {
        mainMenu.SetActive(false);
    }

    public void storyStartClicked()
    {
        storyMenu.SetActive(false);
    }

    public void controlsStartClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void resumeClicked()
    {
        Time.timeScale = 1;
        FindObjectOfType<Character>().UIClicked = true;
        pausePanel.SetActive(false);
    }

    public void restartClicked()
    {
        StartCoroutine("restartFadeOut");
    }

    IEnumerator restartFadeOut()
    {
        MusicController theMusicController = FindObjectOfType<MusicController>();
        StartCoroutine(theMusicController.fadeInOutAudio(theMusicController.mainLoop, 0.25f));
        yield return null;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
    }

    public void mainMenuClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void quitClicked()
    {
        Application.Quit();
    }

    public void playerDied()
    {
        Time.timeScale = 0;
        GOCoinText.text = FindObjectOfType<Character>().coinsCollected.ToString();
        GOPanel.SetActive(true);
    }
}
