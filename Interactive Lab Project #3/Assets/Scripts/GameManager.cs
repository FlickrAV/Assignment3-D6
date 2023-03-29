using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool isDots = true;
    public int currentValue;
    public int currentColor;
    public int health = 6;

    private void Update() 
    {
        if(health < 0)    
        {
            GameOver();
        }
    }

    public void CheckValue(int valueToCheck, int colorToCheck)
    {
        if(valueToCheck == currentValue + 1 || currentColor == colorToCheck)
        {
            NextDie();
        }
        else
        Debug.Log("nah uh");
    }

    private void NextDie()
    {
        Debug.Log("you did a good!");
    }

    private void GameOver()
    {
        Debug.Log("GameOver");
    }
}
