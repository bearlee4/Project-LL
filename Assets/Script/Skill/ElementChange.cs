using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementChange : MonoBehaviour
{
    public Sprite[] change_img;
    public Sprite[] Pyro;
    public Sprite[] Hydro;
    public SpriteRenderer SpriteRenderer;
    public GameObject Element_UI;
    public GameObject[] skill_Img;

    // Start is called before the first frame update
    void Start()
    {
        if (Element_UI.GetComponent<Image>().enabled == false)
        {
            Element_UI.GetComponent<Image>().enabled = true;
        }

        skill_Img[0].GetComponent<Image>().enabled = true;
        skill_Img[1].GetComponent<Image>().enabled = true;
        skill_Img[0].GetComponent<Image>().sprite = Pyro[0];
        skill_Img[1].GetComponent<Image>().sprite = Pyro[1];

        Element_UI.GetComponent<Image>().sprite = change_img[0];
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeEImage(int e)
    {
        Element_UI.GetComponent<Image>().sprite = change_img[e];

        switch (e)
        {
            //pyro
            case 0:
                {
                    skill_Img[0].GetComponent<Image>().sprite = Pyro[0];
                    skill_Img[1].GetComponent<Image>().sprite = Pyro[1];
                }

                break;

            //Hydro
            case 1:
                {
                    skill_Img[0].GetComponent<Image>().sprite = Hydro[0];
                    skill_Img[1].GetComponent<Image>().sprite = Hydro[1];
                }
                break;
        }
    }

}