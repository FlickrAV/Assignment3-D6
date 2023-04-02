using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour
{
    public List<GameObject> dieList;
    public Text[] textArray;
    private int[] colorIndex = new int[6]{1,3,2,3,5,7};
    private Animator menuAnim;
    private int animIndex = 0;
    private GameObject mainMenu;
    private GameObject gameLogo;
    public Sprite[] dotsArray;
    public Sprite[] numArray;
    private GameManager gameManager;
    public Text touchToContinue;
    private Color transparent;

    private void Awake() 
    {
        mainMenu = GameObject.Find("Main Menu");
        gameLogo = GameObject.Find("Game Logo");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        foreach(Transform child in transform)    
        {
            if(child.name.Contains("Die"))
            {
                dieList.Add(child.gameObject);
            }
        }
        textArray = GetComponentsInChildren<Text>();
        menuAnim = GetComponent<Animator>();
    }

    private void Start() 
    {
        for(int i = 0; i < colorIndex.Length; i++)    
        {
            dieList[i].GetComponent<Image>().color = PaletteManager.GetInstance().currentPalette[colorIndex[i]];
            if(i < 5)
            {
                Image[] valueArray = dieList[i].GetComponentsInChildren<Image>();
                if(gameManager.isDots)
                    valueArray[1].sprite = dotsArray[i];
                else
                    valueArray[1].sprite = numArray[i];
            }
        }
        foreach(Text t in textArray)
        {
            if(t.gameObject.tag == "Regular")
                t.color = PaletteManager.GetInstance().currentPalette[7];
            if(t.gameObject.tag == "Highlight")
                t.color = PaletteManager.GetInstance().currentPalette[8];
        }
        transparent = PaletteManager.GetInstance().currentPalette[8];
        transparent.a = 0;
    }

    private void Update() 
    {
        touchToContinue.color = Color.Lerp(PaletteManager.GetInstance().currentPalette[7], transparent, Mathf.PingPong(Time.time * 5, 1));
        if(Input.GetMouseButtonDown(0))    
        {   
            animIndex ++;
            menuAnim.SetInteger("animIndex", animIndex);
        }
    }

    public void DestroyMenu()
    {
        mainMenu.SetActive(true);
        gameLogo.SetActive(true);
        Destroy(gameObject.transform.parent.gameObject);
    }
}
