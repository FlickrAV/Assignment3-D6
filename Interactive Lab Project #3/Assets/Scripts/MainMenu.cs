using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Text displayText;
    public GameManager gameManager;
    public Animator gameLogo;
    public Animator gameMenu;
    public Animator statsMenu;
    public GameObject tutorialMenu;
    public GameObject tutorial;
    [SerializeField]
    private ScoreTracker scoreTracker;

    public void GameStart()
    {
        StartCoroutine(gameManager.StartGame());
        gameLogo.Play("logo_left");
        GetComponent<Animator>().Play("main_menu_outro");
        gameMenu.Play("game_menu_intro");
    }
    
    public void ShowStats()
    {
        GetComponent<Animator>().Play("main_menu_outro");
        statsMenu.Play("stats_slide_in");
        scoreTracker.canClick = true;
    }

    public void ChangeDisplay()
    {
        gameManager.isDots = !gameManager.isDots;
        if(gameManager.isDots)
            displayText.text = "DISPLAY:DOTS";
        else
            displayText.text = "DISPLAY:NUMBERS";
    }

    public void HowToPlay()
    {
        Instantiate(tutorialMenu, Vector3.zero, transform.rotation).transform.SetParent(tutorial.transform);
        gameLogo.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
