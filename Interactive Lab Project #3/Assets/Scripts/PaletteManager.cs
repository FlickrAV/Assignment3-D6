using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteManager : MonoBehaviour
{
    private List<Color[]> colorPalette = new List<Color[]>{};
    [SerializeField]
    private Color[]palette1,palette2,palette3,palette4,palette5,palette6,palette7,palette8,palette9,palette10,palette11,palette12;
    [SerializeField]
    private Image[] buttonImages;

    [Header("Background Color Elements")]
    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private GameObject[] highlightElements;
    [SerializeField]
    private GameObject[] regularElements;

    private int paletteIndex = 0;

    [HideInInspector]
    public Color[] currentPalette;
    private static PaletteManager instance;

    private void Awake() 
    {
        if(instance != null)
        {
            Debug.LogWarning("More than one instance");
        }
        instance = this;       
        GetAllPalettes();
        currentPalette = colorPalette[paletteIndex];

        highlightElements = GameObject.FindGameObjectsWithTag("Highlight");
        regularElements = GameObject.FindGameObjectsWithTag("Regular");
    }

    private void Start() 
    {
        paletteIndex = Random.Range(0, 9);
        currentPalette = colorPalette[paletteIndex];
        UpdateColors();
    }

    public void ChangePalette()
    {
        paletteIndex ++;
        if(paletteIndex > 11)
        {
            paletteIndex = 0;
        }
        currentPalette = colorPalette[paletteIndex];
        UpdateColors();
    }

    public void UpdateColors()
    {
        //Background color elements index 6
        mainCam.backgroundColor = currentPalette[6];

        //Regular color elements index 7
        foreach(GameObject regularObjects in regularElements)
        {
            if(regularObjects.GetComponent<Image>() != null)
                regularObjects.GetComponent<Image>().color = currentPalette[7];
            if(regularObjects.GetComponent<SpriteRenderer>() != null)
                regularObjects.GetComponent<SpriteRenderer>().color = currentPalette[7];
            if(regularObjects.GetComponent<Text>() != null)
                regularObjects.GetComponent<Text>().color = currentPalette[7];
            for(int i = 0; i < buttonImages.Length; i++)
            {
                buttonImages[i].color = currentPalette[i];
            }
        }

        //Highlight color elements index 8
        foreach(GameObject highlightObject in highlightElements)
        {
            if(highlightObject.GetComponent<Image>() != null)
                highlightObject.GetComponent<Image>().color = currentPalette[8];
            if(highlightObject.GetComponent<SpriteRenderer>() != null)
                highlightObject.GetComponent<SpriteRenderer>().color = currentPalette[8];
            if(highlightObject.GetComponent<Text>() != null)
                highlightObject.GetComponent<Text>().color = currentPalette[8];
        }
    }

    public static PaletteManager GetInstance()
    {
        return instance;
    }

    private void GetAllPalettes()
    {
        colorPalette.Add(palette1);
        colorPalette.Add(palette2);
        colorPalette.Add(palette3);
        colorPalette.Add(palette4);
        colorPalette.Add(palette5);
        colorPalette.Add(palette6);
        colorPalette.Add(palette7);
        colorPalette.Add(palette8);
        colorPalette.Add(palette9);
        colorPalette.Add(palette10);
        colorPalette.Add(palette11);
        colorPalette.Add(palette12);
    }
}
