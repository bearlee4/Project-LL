using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager.UI;

public class ItemInformation : MonoBehaviour
{

    public GameObject InformationWindow;
    public GameObject ChooseItem;
    public GameObject slot_Select;
    public GameObject notice;
    public GameObject notice_Window;
    public GameObject notice_fullinventory;
    public Text Content;
    public Text title;

    InventorySystem InventorySystem;
    InteractionSystem InteractionSystem;

    private GameObject canvas;
    UISystem UISystem;

    public int positioncount;
    public bool sidetoken;

    private GameObject windowobject;
    private string contenttext;
    private string kor_name;

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
                    //Content.text = InventorySystem.ItemDB[j]["Content"].ToString();

                    contenttext = InventorySystem.ItemDB[j]["Content"].ToString();
                    Cut_Line();

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
            if (slot_Select.transform.position.ToString() == mainside[i].transform.position.ToString())
            {
                positioncount = i;
                sidetoken = true;

                Debug.Log("트루 작동중");
            }
        }

        for (int i = 0; i < subside.Count; i++)
        {
            if (slot_Select.transform.position.ToString() == subside[i].transform.position.ToString())
            {
                positioncount = i;
                sidetoken = false;

                Debug.Log("flase 작동중");
            }
        }
    }

    public void Cut_Line()
    {
        string[] text_line = contenttext.Split(". ");
        Debug.Log(text_line.Length);
        Content.text = null;

        for (int i = 0; i < text_line.Length; i++)
        {

            //마지막 줄이 아닐 경우
            if (i + 1 < text_line.Length)
            {
                text_line[i] += ".";

            }

            Debug.Log(text_line[i]);
            Content.text += text_line[i] + "\n";
        }
    }

    public void Create_notice_Window(string name, int number)
    {
        float destructionDelay = 3.0f;
        Instantiate(notice_Window, notice.transform);

        for (int i = 0; i < notice.transform.childCount; i++)
        {
            //획득 알림창이 아닐 경우 넘기기
            if (notice.transform.GetChild(i).name != "Notice Window(Clone)")
            {
                continue;
            }

            //이미지가 활성되지 않은, 즉 막 생성된 알림창 찾기
            if (notice.transform.GetChild(i).Find("Slot").Find("Image").GetComponent<Image>().enabled == false)
            {
                GameObject window = notice.transform.GetChild(i).gameObject;
                Image Image = notice.transform.GetChild(i).Find("Slot").Find("Image").GetComponent<Image>();
                Text Text = notice.transform.GetChild(i).Find("Text").GetComponent<Text>();


                for (int n = 0; n < InventorySystem.ItemDB.Count; n++)
                {
                    if (name == InventorySystem.ItemDB[n]["ImgName"].ToString())
                    {
                        kor_name = InventorySystem.ItemDB[n]["Name"].ToString();
                        Debug.Log("한글 이름 찾기 작동 체크");
                        break;
                    }
                }

                Image.enabled = true;
                Image.sprite = Resources.Load<Sprite>("Image/" + name);
                Debug.Log("kor_name :" + kor_name);
                Text.text = (kor_name + " x" + number + "개를 획득했습니다.");
                Destroy(window, destructionDelay);
            }
            
        }    
    }

    public void Create_Fullinventory_Notice()
    {
        float destructionDelay = 3.0f;
        windowobject = Instantiate(notice_fullinventory, notice.transform);

        Destroy(windowobject, destructionDelay);
    }
}
