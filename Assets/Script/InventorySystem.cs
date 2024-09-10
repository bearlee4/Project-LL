using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventorySystem : MonoBehaviour
{
    public GameObject Inventory;

    private int SetSize;
    private int GetCount;

    //인벤토리 슬롯
    public List<GameObject> Slot = new List<GameObject> ();

    //이미지 슬롯
    public List<GameObject> ImageSlot = new List<GameObject>();

    //인벤토리 아이템 이름 저장 리스트
    public List<string> InventoryList = new List<string>();
    
    //갯수 저장 리스트
    private List<int> CountList = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        //기본 사이즈 지정
        ChangeSize(1);

        //인벤토리 UI 숨김
        Inventory.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        List<Dictionary<string, object>> ItemDB = CSVReader.Read("ItemDB");

        //코드 0번 아이템 추가 테스트
        if (Input.GetButtonDown("Test1"))
        {
            Debug.Log(ItemDB[0]["ImgName"]);
            AddInventory(ItemDB[0]["ImgName"]);
        }

        //코드 1번 아이템 추가 테스트
        else if (Input.GetButtonDown("Test2"))
        {
            Debug.Log(ItemDB[1]["ImgName"]);
            AddInventory(ItemDB[1]["ImgName"]);
        }

        //코드 2번 아이템 추가 테스트
        else if (Input.GetButtonDown("Test3"))
        {
            Debug.Log(ItemDB[2]["ImgName"]);
            AddInventory(ItemDB[2]["ImgName"]);
        }

        //코드 3번 아이템 추가 테스트
        else if (Input.GetButtonDown("Test4"))
        {
            Debug.Log(ItemDB[3]["ImgName"]);
            AddInventory(ItemDB[3]["ImgName"]);
        }

        //코드 4번 아이템 추가 테스트
        else if (Input.GetButtonDown("Test5"))
        {
            Debug.Log(ItemDB[4]["ImgName"]);
            AddInventory(ItemDB[4]["ImgName"]);
        }

        CheckButton();
    }

    //인벤토리 사이즈 변화
    public void ChangeSize(int size)
    {
        if (size == 1)
            SetSize = 5;

        else if (size == 2)
            SetSize = 7;

        else if (size == 3)
            SetSize = 9;

    }

    //버튼 체크
    public void CheckButton()
    {
        //I버튼
        if (Input.GetButtonDown("Inventory"))
        {
            if (Inventory.activeSelf == false)
            {
                Debug.Log("open");
                Inventory.SetActive(true);
                for (int num = SetSize; Slot.Count > num; num++ )
                {
                    Slot[num].SetActive(false);
                }
            }

            else
            {
                Debug.Log("close");
                Inventory.SetActive(false);
            }

        }
    }

    //인벤토리에 아이템 추가
    public void AddInventory(object name)
    {
        string Strname = name.ToString();

        for (int i = 0; i < SetSize; i++)
        {
            //임의로 지정한 획득한 아이템갯수(나중에 수정예정)
            GetCount = Random.Range(1, 3);

            if (!InventoryList.Contains(Strname))
            {
                InventoryList.Add(Strname);
                CountList.Add(GetCount);
                Debug.Log("Add Item");
                break;
            }

            else if (InventoryList.Contains(Strname))
            {

                for (int n = 0; n < SetSize; n++)
                {
                    if (InventoryList[n] == Strname)
                    {
                        CountList[n] += GetCount;
                        Debug.Log(CountList[n]);
                        Debug.Log("같은 아이템을 가지고 있습니다.");
                        break;
                    }
                }
                break;
            }

            else if (i > SetSize)
            {
                Debug.Log("인벤토리 공간이 부족합니다.");
                break;
            }
        }

        // 이미지 생성
        for (int num = 0; num < (InventoryList.Count); num++)
        {
            Image Image = ImageSlot[num].GetComponent<Image>();
            if (Image.enabled == false)
            {
                Debug.Log("여기까진 작동중");
                Image.sprite = Resources.Load<Sprite>("Image/" + Strname);
                Image.enabled = true;
            }
        }

        //Debug.Log("인벤토리의 길이는" + (InventoryList.Count));



            //for (int num = 0; num < (InventoryList.Count); num++)
            //{
            //    Debug.Log("테스트" + ((int)InventoryList[num]));
            //    Image Image = ImageSlot[(((int)InventoryList[num]) -1)].GetComponent<Image>();
            //    Image.enabled = true;
            //    Image.sprite = ItemImage[(((int)InventoryList[num]) -1)];
            //}
            //Debug.Log("인벤토리의 길이는" + (InventoryList.Count));




    }
}
