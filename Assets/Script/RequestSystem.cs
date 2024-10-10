using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RequestSystem : MonoBehaviour
{

    //퀘스트DB 가져오기
    public List<Dictionary<string, object>> RequestDB;

    public List<object> available_Requst_List = new List<object>();
    //선택된 퀘스트들 저장해두는 리스트
    public List<string> request_List = new List<string>();


    public int request_Count;

    // Start is called before the first frame update
    void Start()
    {
        RequestDB = CSVReader.Read("RequestDB");
        Setting_Available_Request();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //세팅 가능한 퀘스트들 추가로 집어넣기. 초기에 딱 한번만 돌아가게 함
    public void Setting_Available_Request()
    {
        for(int i = 0; i < RequestDB.Count; i++)
        {
            //해방 조건이 없는 퀘스트들은 바로 추가.
            if (RequestDB[i]["Condition"].ToString() == "X" )
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
        request_Count = Random.Range(3, 4);

        Debug.Log("request_Count : " + request_Count);
        Debug.Log("request_List.Count : " + request_List.Count);

        //중복 없이 받을 수 있는 퀘스트 종류가 5개가 될때까지 무한 루프
        for (int i = 0; true; i ++)
        {
            //코드 선별
            select_number = Random.Range(1, (available_Requst_List.Count));
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

        for (int i = 0; i < request_List.Count; i++)
        {
            Debug.Log("request_List[" + i + "] : " + request_List[i]);
            for (int n = 0; n < RequestDB.Count; n++)
            {
                if (request_List[i] == RequestDB[n]["Code"].ToString())
                {
                    Debug.Log("Content : " + RequestDB[n]["Content"]);
                    break;
                }
            }
        }
    }
}
