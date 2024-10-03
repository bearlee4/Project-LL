using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemInformation : MonoBehaviour
{

    public GameObject InformationWindow;
    public GameObject ChooseItem;
    public GameObject slot_Select;
    public Text Content;
    public Text title;

    InventorySystem InventorySystem;
    InteractionSystem InteractionSystem;

    private GameObject canvas;
    UISystem UISystem;

    public int positioncount;
    public bool sidetoken;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        UISystem = canvas.GetComponent<UISystem>();
        InformationWindow = GameObject.Find("ItemInformation");
        InventorySystem = this.GetComponent<InventorySystem>();
        InteractionSystem = this.GetComponent<InteractionSystem>();

        positioncount = 0;

        //slot_Select = InformationWindow.transform.Find("Slot_Select").gameObject;

    }

    // Update is called once per frame
    void Update()
    {

    }

    //아이템 선택 정보 로드
    public void Load_Information(GameObject overObject)
    {
        if (slot_Select.activeSelf == false)
        {
            slot_Select.SetActive(true);
        }

        //선택 창 크기 조절
        if (UISystem.clicktoggle == false)
        {
            slot_Select.transform.position = overObject.transform.position;
            RectTransform objectSize = overObject.GetComponent<RectTransform>();
            slot_Select.GetComponent<RectTransform>().sizeDelta = new Vector2(objectSize.sizeDelta.x + 10, objectSize.sizeDelta.y + 10);
        }

        Image ChooseImage = ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>();

        if (ChooseImage.enabled == false)
        {
            ChooseImage.enabled = true;
        }

        if(overObject.activeSelf == true && slot_Select.activeSelf == true && overObject.GetComponent<Image>().sprite != null)
        {
            ChooseImage.sprite = overObject.GetComponent<Image>().sprite;

            for (int j = 0; j < InventorySystem.ItemDB.Count; j++)
            {
                if (ChooseImage.sprite.name.ToString() == InventorySystem.ItemDB[j]["ImgName"].ToString())
                {
                    title.text = InventorySystem.ItemDB[j]["Name"].ToString();
                    Content.text = InventorySystem.ItemDB[j]["Content"].ToString();

                    break;
                }
            }
        }
    }

    //mainside = sidecheck 명이랑 같은곳
    public void Set_Position(List<GameObject> mainside, List<GameObject> subside)
    {

        for (int i = 0; i < mainside.Count; i++)
        {
            if (slot_Select.transform.position == mainside[i].transform.position)
            {
                positioncount = i;
                sidetoken = true;

                Debug.Log("트루 작동중");
            }
        }

        for (int i = 0; i < subside.Count; i++)
        {
            if (slot_Select.transform.position == subside[i].transform.position)
            {
                positioncount = i;
                sidetoken = false;

                Debug.Log("flase 작동중");
            }
        }
    }
}
