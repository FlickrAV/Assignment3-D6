using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    private GameManager gameManager;
    public bool isCurrentDie = false;
    public bool shouldDestroy = false;
    private BoxCollider2D bCollider2D;
    private ParticleSystem particleSystem;

    //Color
    private int colorValue;
    private SpriteRenderer srDice;

    //Value
    private int value;
    private SpriteRenderer srValue;
    [Header("Value Sprite")]
    public GameObject valueObject;
    public Sprite[] valueSpriteDots;
    public Sprite[] valueSpriteNum;
    
    private void Awake() 
    {
        srValue = valueObject.GetComponent<SpriteRenderer>();
        srDice = GetComponent<SpriteRenderer>();
        bCollider2D = GetComponent<BoxCollider2D>();
        particleSystem = GetComponent<ParticleSystem>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Start()
    {
        value = Random.Range(1, 7);
        if(gameManager.isDots)
            srValue.sprite = valueSpriteDots[value];
        else
            srValue.sprite =  valueSpriteNum[value];
        
        colorValue = Random.Range(0, 6);
        srDice.color = PaletteManager.GetInstance().currentPalette[colorValue];
        particleSystem.startColor = PaletteManager.GetInstance().currentPalette[colorValue];
    }

    private void Update() 
    {
        if(transform.position.x > 12 || transform.position.x < -12)
            Destroy(transform.parent.gameObject);
        if(!isCurrentDie)    
            return;
        else
        {
            bCollider2D.size = new Vector2(2.17f, 2.17f);
            bCollider2D.offset = Vector2.zero;
            if(shouldDestroy)
            {
                Destroy(gameObject.transform.parent.gameObject);
            }
        }
    }

    private void OnMouseDown() 
    {
        if(!isCurrentDie)
            gameManager.CheckValue(value, colorValue, gameObject);
        else
        {
            gameManager.TakeDamage();
            if(gameManager.score >= 50 && gameManager.health < 0)
            {
                GetComponent<Animator>().SetBool("gameOver", true);
                GetComponent<Animator>().Play("shake");
            }
            else
                GetComponent<Animator>().Play("slide_down_destroy");
        }
    }
}
