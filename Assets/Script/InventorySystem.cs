using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventorySystem : MonoBehaviour
{
    public GameObject Inventory;
    public GameObject Inventory_Select;
    public Text Content;
    private GameObject ChooseItem;


    public bool FullInventory;
    private int maxcount;

    //인벤토리 선택 위치
    private int Positioncount;

    private int SetSize;
    private int GetCount;

    public List<Dictionary<string, object>> ItemDB;

    //인벤토리 아이템 관련
    public List<string> InventoryList = new List<string>();
    private List<int> CountList = new List<int>();

    //인벤토리 UI 관련
    public List<GameObject> Slot = new List<GameObject> ();
    public List<GameObject> ImageSlot = new List<GameObject>();
    public List<Text> NumberList = new List<Text>();
    private List<Vector3> Slot_Position = new List<Vector3> ();

    // Start is called before the first frame update
    void Start()
    {
        ChooseItem = GameObject.Find("ChooseItem");
        Inventory_Select.SetActive(false);
        Content.text = null;
        FullInventory = false;
        maxcount = 99;
        Positioncount = 0;

        ItemDB = CSVReader.Read("ItemDB");

        //기본 사이즈 지정
        ChangeSize(1);

        //선택 이미지 초기 위치 지정
        Inventory_Select.transform.position = Slot[0].transform.position;

        for (int i = 0; i < Slot.Count; i++)
        {
            Slot_Position.Add(Slot[i].transform.position);
        }

        //인벤토리 UI 숨김
        Inventory.SetActive(false);

        //인벤토리가 비었을때 갯수 텍스트 감추기
        for (int i = 0; i < SetSize; i++)
        {
            if (InventoryList.Any() == false)
            {
                NumberList[i].gameObject.SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        CheckButton();

        //슬롯 선택 이미지 표기
        if (InventoryList.Any() == false)
        {
            Inventory_Select.SetActive(false);
        }

        else
        {
            Inventory_Select.SetActive(true);
        }

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

                //인벤토리 사이즈에 맞게 인벤토리 슬롯 생성
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

        if (Inventory_Select.activeSelf == true)
        {
            Select_Inventory();
        }

        //z버튼
        if (Input.GetButtonDown("Confirm"))
        {
            if (Inventory.activeSelf == true && Inventory_Select.activeSelf == true)
            {
                UseItem(Positioncount);
            }
        }


    }

    //인벤토리에 아이템 추가
    public void AddInventory(object name)
    {
        string Strname = name.ToString();
        //임의로 지정한 획득한 아이템갯수(나중에 수정예정)
        GetCount = 99;

        //인벤토리가 비었을때 아이템 추가
        if (InventoryList.Any() == false)
        {
            AddItem(Strname, GetCount);
        }

        else
        {
            for (int i = 0; i < InventoryList.Count; i++)
            {
                //인벤토리에 같은 종류의 아이템이 없을때
                if (!InventoryList.Contains(Strname) && CountList.Count < SetSize)
                {
                    AddItem(Strname, GetCount);
                    break;
                }

                //인벤토리에 같은 종류의 아이템이 있을때
                else if (InventoryList.Contains(Strname))
                {
                    if (InventoryList[i] == Strname)
                    {
                        Debug.Log("이거 작동중임?");

                        // 아이템 습득 후 갯수가 99이하일때
                        if ((CountList[i] + GetCount) < maxcount)
                        {
                            CountList[i] += GetCount;
                            Debug.Log(CountList[i]);
                            Debug.Log("같은 아이템을 가지고 있습니다.");
                        }

                        //// 아이템 습득 후 갯수가 99이상일 때
                        else if ((CountList[i] + GetCount) >= maxcount)
                        {
                            if ((InventoryList.Count + 1) > SetSize)
                            {
                                FullInventory = true;
                                Debug.Log("인벤토리 공간이 부족합니다.");
                                break;
                            }

                            CountList[i] += GetCount;
                            int nextcount = CountList[i] - maxcount;
                            CountList[i] = maxcount;
                            AddItem(Strname, nextcount);
                        }
                        NumberList[i].text = CountList[i].ToString();

                        break;

                    }

                }

                //인벤토리 공간이 부족할때
                else if (CountList.Count == SetSize)
                {
                    FullInventory = true;
                    Debug.Log("인벤토리 공간이 부족합니다.");
                    break;
                }

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
                break;
            }
        }
    }

    //인벤토리 창 선택 했을 때의 상호작용들
    public void Select_Inventory()
    {
        Image ChooseImage = ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>();

        for (int i = 0; i < Slot_Position.Count; i++)
        {

            if (Inventory_Select.transform.position == Slot_Position[i])
            {
                Positioncount = i;

                if (ChooseImage.enabled == false)
                {
                    ChooseImage.enabled = true;
                }

                ChooseImage.sprite = ImageSlot[i].GetComponent<Image>().sprite;

                if (ChooseImage.sprite == null)
                {
                    ChooseImage.enabled = false;
                    Content.text = null;
                    if (i <= 0)
                    {
                        Inventory_Select.transform.position = Slot_Position[i - 1];
                    }
                    
                }
                
                else
                {
                    for (int j = 0; j < ItemDB.Count; j++)
                    {
                        if (ChooseImage.sprite.name.ToString() == ItemDB[j]["ImgName"].ToString())
                        {
                            Content.text = ItemDB[j]["Content"].ToString();

                            break;
                        }
                    }
                }

                break;
            }
        }

        if(Inventory.activeSelf == true)
        {
            if(Input.GetButtonDown("Jump"))
            {
                Debug.Log(Positioncount);
            }

            if(Input.GetButtonDown("Left"))
            {
                if(Positioncount != 0)
                {
                    Positioncount--;
                    Inventory_Select.transform.position = Slot_Position[Positioncount];
                }
            }

            else if (Input.GetButtonDown("Right"))
            {
                if(Positioncount < SetSize && ImageSlot[Positioncount + 1].GetComponent<Image>().enabled == true)
                {
                    Positioncount++;
                    Inventory_Select.transform.position = Slot_Position[Positioncount];
                }            
            }

            else if (Input.GetButtonDown("Up"))
            {
                if ((Positioncount - 3) >= 0)
                {
                    Positioncount -= 3;
                    Inventory_Select.transform.position = Slot_Position[Positioncount];
                }
            }

            else if (Input.GetButtonDown("Down"))
            {
                if ((Positioncount + 3) <= SetSize && ImageSlot[Positioncount + 3].GetComponent<Image>().enabled == true)
                {
                    Positioncount += 3;
                    Inventory_Select.transform.position = Slot_Position[Positioncount];
                }
            }

        }

        

    }

    //아이템 추가
    public void AddItem(string name, int count)
    {
        InventoryList.Add(name);
        CountList.Add(count);
        Debug.Log("Add Item");

        //갯수 텍스트 키기
        for (int n = 0; n < InventoryList.Count; n++)
        {
            if (NumberList[n].gameObject.activeSelf == false)
            {
                NumberList[n].gameObject.SetActive(true);
                Debug.Log(CountList[n].ToString() + "갯수 표시");
                NumberList[n].text = CountList[n].ToString();
                break;
            }
        }
    }

    //아이템 사용
    public void UseItem(int number)
    {
        for (int i = 0; i <= ItemDB.Count; i++)
        {
            if (InventoryList[number] == ItemDB[i]["ImgName"].ToString())
            {
                if (ItemDB[i]["UseItem"].ToString() == "O")
                {
                    CountList[number] -= 1;
                    NumberList[number].text = CountList[number].ToString();
                    Debug.Log("아이템을 사용하였습니다.");

                    if (CountList[number] == 0)
                    {
                        DeleteItem(InventoryList[number]);
                    }
                    break;
                }

                else
                {
                    Debug.Log("사용 가능한 아이템이 아닙니다.");
                    break;
                }
            }
            
        }
    }

    //아이템 삭제
    public void DeleteItem(string name)
    {
        for(int i = 0; i < InventoryList.Count; i++)
        {
            if (InventoryList[i] == name)
            {
                //인벤토리 마지막 아이템까지 다 사용했을 떄
                if (InventoryList.Count == 1)
                {
                    NumberList[i].text = null;
                    NumberList[i].gameObject.SetActive(false);
                    ImageSlot[i].GetComponent<Image>().sprite = null;
                    ImageSlot[i].GetComponent<Image>().enabled = false;
                    Content.text = null;
                    ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>().sprite = null;
                    ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>().enabled = false;
                }

                else
                {
                    for (int j = i; j < InventoryList.Count; j++)
                    {
                        if ((j + 1) < InventoryList.Count)
                        {
                            NumberList[j].text = NumberList[j + 1].text;
                            ImageSlot[j].GetComponent<Image>().sprite = ImageSlot[j + 1].GetComponent<Image>().sprite;
                        }

                        else if ((j + 1) == InventoryList.Count)
                        {
                            NumberList[j].gameObject.SetActive(false);
                            NumberList[j].text = null;
                            ImageSlot[j].GetComponent<Image>().enabled = false;
                        }

                        
                    }
                }
                InventoryList.RemoveAt(i);
                CountList.RemoveAt(i);

                if (InventoryList.Count < SetSize)
                {
                    FullInventory = false;
                }
            }
        }
    }
}
