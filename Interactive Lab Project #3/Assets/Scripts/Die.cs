using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    private GameManager gameManager;

    //Color
    private int colorValue;
    private SpriteRenderer srDice;
    public Color[] diceColor;

    //Value
    private int value;
    private SpriteRenderer srValue;
    [Header("Value Sprite")]
    public GameObject valueObject;
    public Sprite[] valueSpriteDots;
    public Sprite[] valueSpriteNum;
    // Start is called before the first frame update
    private void Awake() 
    {
        srValue = valueObject.GetComponent<SpriteRenderer>();
        srDice = GetComponent<SpriteRenderer>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    void Start()
    {
        value = Random.Range(1, 7);
        if(gameManager.isDots)
            srValue.sprite = valueSpriteDots[value];
        else
            srValue.sprite =  valueSpriteNum[value];
        
        colorValue = Random.Range(1, 7);
        srDice.color = diceColor[colorValue];
    }

    private void OnMouseDown() 
    {
        gameManager.CheckValue(value, colorValue);
    }
}
