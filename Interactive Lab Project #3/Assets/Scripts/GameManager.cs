using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //Player things
    [HideInInspector]
    public int health = 6;
    private bool valueNull = true;
    public int score = 0;
    public int chain = 0;
    private ScoreTracker scoreTracker;
    private bool gameLost = false;
    public float exitSpeed;
    private bool inGame = false;


    //Die things
    public GameObject diePrefab;
    private GameObject currentDie;
    private GameObject newDie;
    [HideInInspector]
    public int currentColor;
    [HideInInspector]
    public bool isDots = true;
    [HideInInspector]
    public int currentValue;
    private float timer = 0;

    //Columns
    public List<GameObject>[] columnArray = new List<GameObject>[5];
    public List<GameObject> column1 = new List<GameObject>();
    public List<GameObject> column2 = new List<GameObject>();
    public List<GameObject> column3 = new List<GameObject>();
    public List<GameObject> column4 = new List<GameObject>();
    public List<GameObject> column5 = new List<GameObject>();

    //UI
    [Header("UI Elements")]
    public GameObject healthBar;
    public Image[] healthPoints;
    public Text chainText;
    public Text scoreText;
    public Slider progressBar;
    private Image sliderFill;
    public Animator statsScreen;
    public Animator mainMenu;
    public Animator gameMenu;
    public Animator gameLogo;

    private void Awake() 
    {
        columnArray[0] = column1;
        columnArray[1] = column2;
        columnArray[2] = column3;
        columnArray[3] = column4;
        columnArray[4] = column5;
        scoreTracker = GameObject.Find("Score Tracker").GetComponent<ScoreTracker>();
        healthPoints = healthBar.GetComponentsInChildren<Image>();
        sliderFill = progressBar.transform.Find("Fill Area").GetComponentInChildren<Image>();
    }

    private void Update() 
    {
        scoreText.text = score.ToString();
        chainText.text = chain.ToString();
        progressBar.value = score;

        if(currentDie != null)
        {
            timer += Time.deltaTime;
            currentDie.transform.parent.transform.position = Vector3.Lerp(newDie.transform.parent.transform.position, new Vector3(0, -3.3f, 0),timer);
            timer = Mathf.Clamp(timer, 0, 1);
        }

        if(gameLost)
        {
            foreach(List<GameObject> columnList in columnArray)
            {
                if(columnList[0] != null)
                {
                    columnList[0].transform.Translate((Vector3.left*exitSpeed)*Time.deltaTime);
                    columnList[0].GetComponentInChildren<Animator>().Play("idle_death");
                }
                if(columnList[1] != null)
                    columnList[1].transform.Translate((Vector3.right*exitSpeed)*Time.deltaTime);
            }
        }

        if(scoreTracker.canClick)
        {
            if(Input.GetMouseButtonDown(0))
            {
                statsScreen.Play("stats_slide_out");
                scoreTracker.canClick = false;
                mainMenu.Play("main_menu_intro");

                if(inGame)
                {
                    gameLogo.Play("logo_right");
                    gameMenu.Play("game_menu_outro");
                    score = 0;
                    scoreTracker.BoolFalse();
                    score = 0;
                    PaletteManager.GetInstance().UpdateColors();
                    inGame = false;
                }
            }
        }

        if(score >= 50)
            sliderFill.color = Color.Lerp(PaletteManager.GetInstance().currentPalette[8], Color.white, Mathf.PingPong(Time.time * 5, 1));
    }

    public void CheckValue(int valueToCheck, int colorToCheck, GameObject newestDie)
    {
        if(valueToCheck == currentValue + 1 || currentColor == colorToCheck||valueNull)
        {
            newDie = newestDie;
            score ++;
            chain ++;
            currentValue = valueToCheck;
            if(currentValue == 6)
            {
                currentValue = 0;
            }
            currentColor = colorToCheck;
            NextDie();
            valueNull = false;
        }
        else
        newestDie.GetComponent<Animator>().Play("shake");
    }

    private void NextDie()
    {
        timer = 0;
        if(currentDie != null)
            Destroy(currentDie.transform.parent.gameObject);

        //Change value of currentDie
        currentDie = newDie;
        currentDie.GetComponentInChildren<Die>().isCurrentDie = true;
        foreach(List<GameObject> columnList in columnArray)
        {
            if(columnList.Contains(currentDie.transform.parent.gameObject))
            {
                columnList.RemoveAt(0);
                columnList.TrimExcess();
                if(columnList.Count == 0)
                {
                    columnList.Add(Instantiate(diePrefab, new Vector3(currentDie.transform.position.x, 0, 0), transform.rotation));
                }
                columnList[0].GetComponentInChildren<Animator>().SetBool("2ndRow", true);
                columnList.Add(Instantiate(diePrefab, new Vector3(currentDie.transform.position.x, 0, 0), transform.rotation));
            }
        }
    }

    private void GameOver()
    {
        //Won
        if(score >= 50)
        {
            scoreTracker.won ++;
            if(scoreTracker.winStreak < 0)
                scoreTracker.winStreak = 0;
            scoreTracker.winStreak ++;
            foreach(List<GameObject> columnList in columnArray)
                columnList[0].GetComponentInChildren<Animator>().Play("idle_death");
            StartCoroutine(PlayParticles());
            StartCoroutine(scoreTracker.UpdateStats(5));
        }
        //Lost
        else
        {
            scoreTracker.lost ++;
            if(scoreTracker.winStreak > 0)
                scoreTracker.winStreak = 0;
            scoreTracker.winStreak --;
            gameLost = true;
            StartCoroutine(scoreTracker.UpdateStats(1.5f));
        }
    }

    public void TakeDamage()
    {
        if(healthPoints[health] != null)
            healthPoints[health].color = PaletteManager.GetInstance().currentPalette[7];
        health --;
        chain = 0;
        valueNull = true;

        if(health < 0)    
        {
            valueNull = false;
            currentValue = 10;
            currentColor = 10;
            GameOver();
        }
    }

    private IEnumerator PlayParticles()
    {
        yield return new WaitForSeconds(.3f);
        currentDie.GetComponent<ParticleSystem>().Play();
        currentDie.GetComponent<SpriteRenderer>().enabled = false;
        foreach(SpriteRenderer dieSprite in currentDie.GetComponentsInChildren<SpriteRenderer>())
                    dieSprite.enabled = false;
        yield return new WaitForSeconds(1);
        foreach(List<GameObject> columnList in columnArray)
            {
                columnList[0].GetComponentInChildren<ParticleSystem>().Play();
                columnList[1].GetComponentInChildren<ParticleSystem>().Play();
                foreach(SpriteRenderer dieSprite in columnList[0].GetComponentsInChildren<SpriteRenderer>())
                    dieSprite.enabled = false;
                foreach(SpriteRenderer dieSprite in columnList[1].GetComponentsInChildren<SpriteRenderer>())
                    dieSprite.enabled = false;
            }
        yield return new WaitForSeconds(3);
        Destroy(currentDie.transform.parent.gameObject);
        foreach(List<GameObject> columnList in columnArray)
        {
            Destroy(columnList[0]);
            Destroy(columnList[1]);
        }
    }

    public IEnumerator StartGame()
    {
        inGame = true;

        //Reset values
        health = 6;
        score = 0;
        chain = 0;
        currentDie = null;
        newDie = null;
        valueNull = true;
        gameLost = false;

        foreach(Image healthSprite in healthPoints)
        {
            healthSprite.color = PaletteManager.GetInstance().currentPalette[8];
        }


        column1.Clear();
        column2.Clear();
        column3.Clear();
        column4.Clear();
        column5.Clear();

        scoreTracker.played ++;

        //1
        column1.Add(Instantiate(diePrefab, new Vector3(-7, 0, 0), transform.rotation));
        column1[0].GetComponentInChildren<Animator>().SetBool("2ndRow", true);

        //2
        yield return new WaitForSeconds(.1f);
        column2.Add(Instantiate(diePrefab, new Vector3(-3.5f, 0, 0), transform.rotation));
        column2[0].GetComponentInChildren<Animator>().SetBool("2ndRow", true);

        //3
        yield return new WaitForSeconds(.1f);
        column3.Add(Instantiate(diePrefab, new Vector3(0, 0, 0), transform.rotation));
        column3[0].GetComponentInChildren<Animator>().SetBool("2ndRow", true);

        //4
        yield return new WaitForSeconds(.1f);
        column4.Add(Instantiate(diePrefab, new Vector3(3.5f, 0, 0), transform.rotation));
        column4[0].GetComponentInChildren<Animator>().SetBool("2ndRow", true);

        //5
        yield return new WaitForSeconds(.1f);
        column5.Add(Instantiate(diePrefab, new Vector3(7, 0, 0), transform.rotation));
        column5[0].GetComponentInChildren<Animator>().SetBool("2ndRow", true);

        //6
        if(column5.Count == 1)
        {
            yield return new WaitForSeconds(.1f);
            column5.Add(Instantiate(diePrefab, new Vector3(7, 0, 0), transform.rotation));
        }

        //7
        if(column4.Count == 1)
        {
            yield return new WaitForSeconds(.1f);
            column4.Add(Instantiate(diePrefab, new Vector3(3.5f, 0, 0), transform.rotation));   
        }

        //8
        if(column3.Count == 1)
        {
            yield return new WaitForSeconds(.1f);
            column3.Add(Instantiate(diePrefab, new Vector3(0, 0, 0), transform.rotation));   
        }

        //9
        if(column2.Count == 1)
        {
            yield return new WaitForSeconds(.1f);
            column2.Add(Instantiate(diePrefab, new Vector3(-3.5f, 0, 0), transform.rotation));   
        }

        //10
        if(column1.Count == 1)
        {
            yield return new WaitForSeconds(.1f);
            column1.Add(Instantiate(diePrefab, new Vector3(-7, 0, 0), transform.rotation));             
        }

    }
}
