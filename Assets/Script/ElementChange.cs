using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementChange : MonoBehaviour
{
    public Sprite[] change_img;
    public SpriteRenderer SpriteRenderer;
    public Image currentImg;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        if (currentImg != null)
        {
            currentImg.sprite = change_img[0];
        }
    }

    public void ChangeEImage(int e)
    {
            currentImg.sprite = change_img[e];
    }

}

