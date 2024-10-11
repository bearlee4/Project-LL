using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RequestSystem : MonoBehaviour
{
    InventorySystem InventorySystem;

    private GameObject canvas;
    UISystem UISystem;

    //퀘스트DB 가져오기
    public List<Dictionary<string, object>> RequestDB;

    public List<object> available_Requst_List = new List<object>();
    //선택된 퀘스트들 저장해두는 리스트
    public List<string> request_List = new List<string>();
    public List<GameObject> scroll_Position = new List<GameObject>();
    public List<int> position_Number = new List<int>();

    public GameObject request_Scroll;
    public GameObject request_UI;
    public GameObject request_information;
    public GameObject request_Item;
    public int request_Count;
    public Text title;
    public Text content;

    private string contenttext;
    private int get_Itemcount;
    private int need_Itemcount;

    // Start is called before the first frame update
    void Start()
    {
        RequestDB = CSVReader.Read("RequestDB");
        canvas = GameObject.Find("Canvas");
        UISystem = canvas.GetComponent<UISystem>();
        InventorySystem = this.GetComponent<InventorySystem>();
        Debug.Log(request_Scroll.transform.childCount);

        if (request_UI.activeSelf == true)
        {
            request_UI.SetActive(false);
        }
        

        Setting_Available_Request();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open_RequestBoard()
    {
        request_UI.SetActive (true);

        if (request_information.activeSelf == true)
        {
            request_information.SetActive(false);
        }

        for (int i = 0; i < request_Scroll.transform.childCount; i++)
        {
            if (request_Scroll.transform.GetChild(i).gameObject.activeSelf == true)
            {
                request_Scroll.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < request_Scroll.transform.childCount; i++)
        {
            if (position_Number.Contains(i))
            {
                request_Scroll.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }


    //세팅 가능한 퀘스트들 추가로 집어넣기.
    public void Setting_Available_Request()
    {
        for(int i = 0; i < RequestDB.Count; i++)
        {
            //해방 조건이 없는 퀘스트들은 바로 추가. 이미 들어가 있는건 패스
            if (RequestDB[i]["Condition"].ToString() == "X" && !available_Requst_List.Contains(RequestDB[i]["Condition"].ToString()))
            {
                available_Requst_List.Add(RequestDB[i]["Code"]);
            }
        }

        Debug.Log("available_Requst_List.Count : " + available_Requst_List.Count);
        //테스트 용으로 임시 추가
        Set_Request();
    }

    public void Set_Request()
    {
        int select_number;
        //하루 의뢰 갯수. 조건 없는 의뢰가 당장은 4개까지라 3~4개로 설정. 3~5개로 할 예정
        request_Count = Random.Range(3, 5);

        Debug.Log("request_Count : " + request_Count);
        Debug.Log("request_List.Count : " + request_List.Count);

        //중복 없이 받을 수 있는 퀘스트 종류가 하루 의뢰 갯수가 될때까지 무한 루프
        for (int i = 0; true; i ++)
        {
            //코드 선별
            select_number = Random.Range(1, (available_Requst_List.Count + 1));
            if (request_List.Contains(select_number.ToString()))
            {
                continue;
            }

            request_List.Add(select_number.ToString());

            if(request_List.Count == request_Count)
            {
                break;
            }
        }

        //스크롤 위치 중복 없이 정해질떄까지 무한 루프
        for (int i = 0; true; i++)
        {
            //코드 선별
            select_number = Random.Range(0, request_Scroll.transform.childCount);
            if (position_Number.Contains(select_number))
            {
                continue;
            }

            position_Number.Add(select_number);

            if (position_Number.Count == request_Count)
            {
                break;
            }
        }
    }

    //아이템 선택 정보 로드
    public void Load_Information(GameObject overObject)
    {
        if (request_information.activeSelf == false)
        {
            request_information.SetActive(true);
        }

        Image RequestImage = request_Item.transform.Find("ItemImage").GetComponent<Image>();

        if (RequestImage.enabled == false)
        {
            RequestImage.enabled = true;
        }

        if (overObject.activeSelf == true)
        {
            Debug.Log("overObject.transform.position.ToString() : " + overObject.transform.position.ToString());
            for (int i = 0; i < position_Number.Count; i ++)
            {
                Debug.Log("request_Scroll.transform.GetChild(position_Number[" + i + "]).position.ToString() : " + request_Scroll.transform.GetChild(position_Number[i]).position.ToString());

                //선택한 위치와 같은 번호의 오브젝트 찾기
                if (overObject.transform.position.ToString() == request_Scroll.transform.GetChild(position_Number[i]).position.ToString())
                {
                    Debug.Log("들어온거 체크");
                    for (int n = 0; n < RequestDB.Count; n ++)
                    {

                        if (request_List[i] == RequestDB[n]["Code"].ToString())
                        {
                            Debug.Log("버그 체크");
                            RequestImage.sprite = Resources.Load<Sprite>("Image/" + RequestDB[n]["Need"].ToString());
                            title.text = RequestDB[n]["Title"].ToString();
                            contenttext = RequestDB[n]["Content"].ToString();
                            Cut_Line();
                            need_Itemcount = (int)RequestDB[n]["Count"];
                            get_Itemcount = 0;
                            if (!InventorySystem.InventoryList.Contains(RequestDB[n]["Need"].ToString()))
                            {
                                get_Itemcount = 0;
                            }

                            else if (InventorySystem.InventoryList.Contains(RequestDB[n]["Need"].ToString()))
                            {
                                for (int num = 0; num < InventorySystem.InventoryList.Count; num ++)
                                {
                                    if (InventorySystem.InventoryList[num] == RequestDB[n]["Need"].ToString())
                                    {
                                        get_Itemcount += InventorySystem.CountList[num];
                                    }
                                }
                            }

                            Text Requesttext = request_Item.transform.Find("Text").GetComponent<Text>();
                            
                            Requesttext.text = get_Itemcount.ToString() + " / " + need_Itemcount.ToString();

                            break;
                        }
                    }

                    break;
                }
            }

        }
    }

    public void Cut_Line()
    {
        string[] text_line = contenttext.Split("#");
        Debug.Log(text_line.Length);
        content.text = null;

        for (int i = 0; i < text_line.Length; i++)
        {

            Debug.Log(text_line[i]);
            content.text += text_line[i] + "\n";
        }
    }
}
