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
    private int score = 0;
    private int chain = 0;

    //Die things
    public GameObject diePrefab;
    private GameObject currentDie;
    private GameObject newDie;
    [HideInInspector]
    public int currentColor = 10;
    [HideInInspector]
    public bool isDots = true;
    [HideInInspector]
    public int currentValue = 10;
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
    public Text healthText;
    public Text chainText;
    public Text scoreText;
    public Slider progressBar;

    private void Awake() 
    {
        columnArray[0] = column1;
        columnArray[1] = column2;
        columnArray[2] = column3;
        columnArray[3] = column4;
        columnArray[4] = column5;
    }

    private void Start() 
    {
        StartCoroutine(StartGame());
        valueNull = true;
    }

    private void Update() 
    {
        if(health < 0)    
        {
            GameOver();
        }

        healthText.text = "Health: " + health;
        scoreText.text = score.ToString();
        chainText.text = chain.ToString();
        progressBar.value = score;

        if(currentDie != null)
        {
            timer += Time.deltaTime;
            currentDie.transform.parent.transform.position = Vector3.Lerp(newDie.transform.parent.transform.position, new Vector3(0, -3, 0),timer);
            timer = Mathf.Clamp(timer, 0, 1);
        }
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
        Destroy(currentDie);

        //Change value of currentDie
        currentDie = newDie;
        currentDie.GetComponentInChildren<Die>().isCurrentDie = true;
        foreach(List<GameObject> columnList in columnArray)
        {
            if(columnList.Contains(currentDie.transform.parent.gameObject))
            {
                columnList.RemoveAt(0);
                columnList.TrimExcess();
                columnList[0].GetComponentInChildren<Animator>().SetBool("2ndRow", true);
                columnList.Add(Instantiate(diePrefab, new Vector3(currentDie.transform.position.x, 0, 0), transform.rotation));
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("GameOver");
    }

    public void ResetCurrentValue()
    {
        valueNull = true;
        chain = 0;
        health --;
    }

    private IEnumerator StartGame()
    {
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
        yield return new WaitForSeconds(.1f);
        column5.Add(Instantiate(diePrefab, new Vector3(7, 0, 0), transform.rotation));

        //7
        yield return new WaitForSeconds(.1f);
        column4.Add(Instantiate(diePrefab, new Vector3(3.5f, 0, 0), transform.rotation));

        //8
        yield return new WaitForSeconds(.1f);
        column3.Add(Instantiate(diePrefab, new Vector3(0, 0, 0), transform.rotation));

        //9
        yield return new WaitForSeconds(.1f);
        column2.Add(Instantiate(diePrefab, new Vector3(-3.5f, 0, 0), transform.rotation));

        //10
        yield return new WaitForSeconds(.1f);
        column1.Add(Instantiate(diePrefab, new Vector3(-7, 0, 0), transform.rotation));  
    }
}
