using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour
{
    //Int to track
    [HideInInspector]
    public float played = 0;
    [HideInInspector]
    public float won = 0;
    [HideInInspector]
    public int lost = 0;
    float winRatio = 0;
    [HideInInspector]
    public int winStreak = 0;
    int bestStreak = 0;
    int bestChain = 0;
    int highScore = 0;

    //UI elements
    [SerializeField]
    private Text playedText;
    [SerializeField]
    private Text wonText;
    [SerializeField]
    private Text lostText;
    [SerializeField]
    private Text winRatioText;
    [SerializeField]
    private Text winStreakText;
    [SerializeField]
    private Text bestStreakText;
    [SerializeField]
    private Text bestChainText;
    private bool chainChange = false;
    [SerializeField]
    private Text highScoreText;
    [SerializeField]
    private Animator statsMenu;

    private GameManager gameManager;

    public bool canClick = false;
    private bool[] statChange = new bool[4]{false,false,false,false};
    private Text[] textToFlash;
    private Color transparent;

    private void Awake() 
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();    
        textToFlash = new Text[4]{wonText, lostText, bestChainText, highScoreText};
        transparent = PaletteManager.GetInstance().currentPalette[7];
        transparent.a = 0;
    }

    private void Update() 
    {
        if(bestChain < gameManager.chain)    
        {
            bestChain = gameManager.chain;
            chainChange = true;
        }

        if(statChange[0]||statChange[1]||statChange[2]||statChange[3])
        {
            for(int i = 0; i < 4; i ++)
            {
                if(statChange[i])
                {
                    textToFlash[i].transform.parent.GetComponent<Text>().color = Color.Lerp(PaletteManager.GetInstance().currentPalette[7], transparent, Mathf.PingPong(Time.time * 5, 1));
                }
            }
        }
    }

    public IEnumerator UpdateStats(float seconds)
    {
        int i = 0;
        yield return new WaitForSeconds(seconds);
        statsMenu.Play("game_over");

        yield return new WaitForSeconds(2.5f);
        playedText.text = played.ToString();
        playedText.GetComponent<Animator>().Play("stats_pop");

        yield return new WaitForSeconds(.5f);
        if(wonText.text != won.ToString())
        {
            wonText.text = won.ToString();
            wonText.GetComponent<Animator>().Play("stats_pop");
            statChange[i] = true;
        }
        i ++;

        yield return new WaitForSeconds(.5f);
        if(lostText.text !=lost.ToString())
        {
            lostText.text = lost.ToString();
            lostText.GetComponent<Animator>().Play("stats_pop");
            statChange[i] = true;
        }
        i ++;

        yield return new WaitForSeconds(.5f);
        
        winRatio = (won * 100)/played;
        int tempWinRatio  = (int) winRatio;
        if(winRatio == tempWinRatio)
        {   
            if(winRatioText.text != winRatio.ToString())
            {
                winRatioText.text = winRatio.ToString();
                winRatioText.GetComponent<Animator>().Play("stats_pop");
            }
        }
        else if (winRatioText.text != winRatio.ToString("0.0"))
        {
            winRatioText.text = winRatio.ToString("0.0");
            winRatioText.GetComponent<Animator>().Play("stats_pop");
        }

        yield return new WaitForSeconds(.5f);
        if(bestStreak < winStreak)
        {
            bestStreak = winStreak;
            winStreakText.text = winStreak.ToString();
            winStreakText.GetComponent<Animator>().Play("stats_pop");
        }

        yield return new WaitForSeconds(.5f);
        bestStreakText.text = bestStreak.ToString();
        bestStreakText.GetComponent<Animator>().Play("stats_pop");

        yield return new WaitForSeconds(.5f);
        if(chainChange)    
        {
            bestChainText.text = bestChain.ToString();
            bestChainText.GetComponent<Animator>().Play("stats_pop");
            statChange[i] = true;
            chainChange = false;
        }
        i ++;

        yield return new WaitForSeconds(.5f);
        if(highScore < gameManager.score)
        {
            highScore = gameManager.score;
            highScoreText.text = highScore.ToString();
            highScoreText.GetComponent<Animator>().Play("stats_pop");
            statChange[i] = true;
        }

        yield return new WaitForSeconds(.5f);
        canClick = true;
    }

    public void BoolFalse()
    {
        for(int i = 0; i < statChange.Length; i ++)
            statChange[i] = false;
    }
}
