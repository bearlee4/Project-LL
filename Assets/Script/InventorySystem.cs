using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventorySystem : MonoBehaviour
{
    public GameObject Inventory;
    //public GameObject Inventory_Select;
    //public Text Content;
    //private GameObject ChooseItem;

    private GameObject canvas;
    UISystem UISystem;

    ItemInformation ItemInformation;

    private GameObject Player;
    InteractionSystem InteractionSystem;


    

    //스크립트 내 사용되는 변수들
    public int Positioncount;
    public bool FullInventory;
    public int SetSize;
    public int GetCount;
    private int maxcount;
    
    //아이템DB 가져오기
    public List<Dictionary<string, object>> ItemDB;

    //퀵슬롯 관련
    public List<GameObject> QuickSlot = new List<GameObject>();
    public List<GameObject> QuickSlotImage = new List<GameObject>();
    public List<Text> QuickSlotNumberList = new List<Text>();
    public List<string> QuickSlotList = new List<string>();
    public List<int> QuickSlotPosition = new List<int>();

    //인벤토리 아이템 관련
    public List<string> InventoryList = new List<string>();
    public List<int> CountList = new List<int>();

    //인벤토리 UI 관련
    public List<GameObject> Slot = new List<GameObject>();
    public List<GameObject> ImageSlot = new List<GameObject>();
    public List<Text> NumberList = new List<Text>();
    private List<Vector3> Slot_Position = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        UISystem = canvas.GetComponent<UISystem>();
        ItemInformation = this.GetComponent<ItemInformation>();
        Player = GameObject.Find("Player");
        InteractionSystem = Player.GetComponent<InteractionSystem>();

        //ChooseItem = GameObject.Find("ChooseItem");
        // Inventory_Select.SetActive(false);
        //Content.text = null;
        FullInventory = false;
        maxcount = 99;
        Positioncount = 0;
        QuickSlotList = new List<string>() { "null", "null", "null" };
        QuickSlotPosition = new List<int>() { -1, -1, -1 };

        ItemDB = CSVReader.Read("ItemDB");

        //임의로 지정한 획득한 아이템갯수(나중에 수정예정)
        GetCount = 10;

        //기본 사이즈 지정
        ChangeSize(1);

        //선택 이미지 초기 위치 지정
        //Inventory_Select.transform.position = Slot[0].transform.position;

        for (int i = 0; i < Slot.Count; i++)
        {
            Slot_Position.Add(Slot[i].transform.position);
        }

        //인벤토리 UI 숨김
        //Inventory.SetActive(false);

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
        //if (Inventory_Select.activeSelf == true)
        //{
        //    Select_Move();
        //}

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

    //인벤토리 열기
    public void OpenInventory()
    {
        Debug.Log("Inventoryopen");
        Inventory.SetActive(true);
        Positioncount = 0;

        //인벤토리 사이즈에 맞게 인벤토리 슬롯 생성
        for (int num = SetSize; Slot.Count > num; num++)
        {
            Slot[num].SetActive(false);
        }

        ////슬롯 선택 이미지 표기
        //if (InventoryList.Any() == false)
        //{
        //    Inventory_Select.SetActive(false);
        //}

        //else
        //{
        //    Inventory_Select.SetActive(true);
        //    Load_Information();
        //}
    }

    //인벤토리 닫기
    public void CloseInventory()
    {
        Debug.Log("Inventoryclose");
        Inventory.SetActive(false);
    }

    //인벤토리에 아이템 추가
    public void AddInventory(object name, int number)
    {
        string Strname = name.ToString();

        //인벤토리가 비었을때 아이템 추가
        if (InventoryList.Any() == false)
        {
            AddItem(Strname, number);
        }

        else
        {
            for (int i = 0; i < InventoryList.Count; i++)
            {
                //인벤토리에 같은 종류의 아이템이 없을때
                if (!InventoryList.Contains(Strname) && CountList.Count < SetSize)
                {
                    AddItem(Strname, number);
                    break;
                }

                //인벤토리에 같은 종류의 아이템이 있을때
                else if (InventoryList.Contains(Strname))
                {
                    if (InventoryList[i] == Strname)
                    {
                        Debug.Log("이거 작동중임?");

                        // 아이템 습득 후 갯수가 99이하일때
                        if ((CountList[i] + number) < maxcount)
                        {
                            CountList[i] += number;
                            Debug.Log(CountList[i]);
                            Debug.Log("같은 아이템을 가지고 있습니다.");
                        }

                        //// 아이템 습득 후 갯수가 99이상일 때
                        else if ((CountList[i] + number) >= maxcount)
                        {
                            if ((InventoryList.Count + 1) > SetSize)
                            {
                                FullInventory = true;
                                Debug.Log("인벤토리 공간이 부족합니다.");
                                break;
                            }

                            CountList[i] += number;
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
        for (int num = 0; num < InventoryList.Count; num++)
        {
            Image Image = ImageSlot[num].GetComponent<Image>();
            if (ImageSlot[num].activeSelf == false)
            {
                ImageSlot[num].SetActive(true);
            }

            if (Image.enabled == false)
            {
                Debug.Log("여기까진 작동중");
                Image.sprite = Resources.Load<Sprite>("Image/" + Strname);
                Image.enabled = true;
                break;
            }
        }
    }

    public void Load_Information(GameObject selectObject)
    {
        //Image ChooseImage = ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>();

        for (int i = 0; i < Slot_Position.Count; i++)
        {

            if (selectObject.transform.position == Slot_Position[i])
            {
                Debug.Log(i+1 + "번째 칸에 있음");
                Positioncount = i;

                //if (ChooseImage.enabled == false)
                //{
                //    ChooseImage.enabled = true;
                //}

                //ChooseImage.sprite = ImageSlot[i].GetComponent<Image>().sprite;

                //if (ChooseImage.sprite == null)
                //{
                //    ChooseImage.enabled = false;
                //    Content.text = null;
                //    if (i <= 0)
                //    {
                //        Inventory_Select.transform.position = Slot_Position[i - 1];
                //    }

                //}

                //else
                //{
                //    for (int j = 0; j < ItemDB.Count; j++)
                //    {
                //        if (ChooseImage.sprite.name.ToString() == ItemDB[j]["ImgName"].ToString())
                //        {
                //            Content.text = ItemDB[j]["Content"].ToString();

                //            break;
                //        }
                //    }
                //}

                break;
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

                    //아이템을 다 사용했을 떄
                    if (CountList[number] == 0)
                    {
                        if(QuickSlotList.Contains(InventoryList[number]))
                        {
                            Quick_Reset(InventoryList[number]);
                        }

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
        for (int i = 0; i < InventoryList.Count; i++)
        {
            if (InventoryList[i] == name)
            {
                //아이템 사용으로 인벤토리에 아무것도 안남을때
                if (InventoryList.Count == 1)
                {
                    NumberList[i].text = null;
                    NumberList[i].gameObject.SetActive(false);
                    //Inventory_Select.SetActive(false);
                    ImageSlot[i].GetComponent<Image>().sprite = null;
                    ImageSlot[i].GetComponent<Image>().enabled = false;
                    if (ImageSlot[i].activeSelf == true)
                    {
                        ImageSlot[i].SetActive(false);
                    }
                    //Content.text = null;
                    //ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>().sprite = null;
                    //ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>().enabled = false;
                }

                else
                {

                    for (int j = i; j < InventoryList.Count; j++)
                    {
                        //인벤토리 마지막 칸이 아닐때 다음 슬롯 정보 당겨오기
                        if ((j + 1) < InventoryList.Count)
                        {
                            if (ImageSlot[j].activeSelf == false)
                            {
                                ImageSlot[j].SetActive(true);
                            }

                            NumberList[j].text = NumberList[j + 1].text;
                            ImageSlot[j].GetComponent<Image>().sprite = ImageSlot[j + 1].GetComponent<Image>().sprite;
                            if(InteractionSystem.UItoken == true)
                            {
                                ItemInformation.Load_Information(UISystem.overObject);
                            }
                        }

                        //인벤토리 마지막칸일때
                        else if ((j + 1) == InventoryList.Count)
                        {
                            NumberList[j].gameObject.SetActive(false);
                            NumberList[j].text = null;
                            ImageSlot[j].GetComponent<Image>().enabled = false;
                            ImageSlot[j].SetActive(false);
                        }


                    }
                }
                InventoryList.RemoveAt(i);
                CountList.RemoveAt(i);

                if (InventoryList.Count < SetSize)
                {
                    FullInventory = false;
                }

                if (ImageSlot[i].activeSelf == false)
                {
                    ItemInformation.slot_Select.SetActive(false);
                    UISystem.clicktoggle = false;
                }
            }
        }
    }

    public void Set_QuickSlot()
    {
        bool delete_toggle;
        delete_toggle = false;

        //누른 위치 포지션 순서 값 가져오기
        Load_Information(UISystem.overObject);

        for (int i = 0; i <= ItemDB.Count; i++)
        {

            //소모품인지 알아내기
            if (InventoryList[Positioncount].ToString() == ItemDB[i]["ImgName"].ToString())
            {

                //소모품이 맞을 경우
                if (ItemDB[i]["UseItem"].ToString() == "O")
                {
                    //포지션 카운트로 퀵슬롯에 지정되어 있는지 확인
                    for (int n = 0; n < QuickSlotPosition.Count; n++)
                    {
                        //지정되어 있을 경우 퀵슬롯 해제
                        if (QuickSlotPosition[n] == Positioncount)
                        {
                            QuickSlotList[n] = "null";
                            QuickSlotPosition[n] = -1;
                            Debug.Log("퀵슬롯 해제");
                            
                            //퀵슬롯 해제에 사용함
                            delete_toggle = true;
                            break;

                        }
                    }

                    //삭제 용도로 돌아간게 아닐 경우
                    if(delete_toggle == false)
                    {
                        //퀵슬롯 지정
                        for (int j = 0; j < QuickSlotList.Count; j++)
                        {
                            if (QuickSlotList[j] == "null")
                            {
                                //퀵슬롯 좌표 및 정보 저장
                                QuickSlotList[j] = InventoryList[Positioncount];
                                QuickSlotPosition[j] = Positioncount;

                                Debug.Log(j);
                                Debug.Log(QuickSlotList[j]);
                                Debug.Log(QuickSlotPosition[j]);

                                break;

                            }

                            else
                            {
                                Debug.Log("퀵슬롯이 꽉차있습니다.");
                            }
                        }
                    }


                    break;
                }

                //아닐 경우
                else
                {
                    Debug.Log("소모품이 아닙니다.");
                    break;
                }
            }

        }

        //퀵슬롯에 선택된 인벤토리 정보 연동
        Quick_Load_Info();
    }

    public void Quick_Load_Info()
    {

        //연동하기 전 퀵슬롯 정보 리셋
        for (int i = 0; i < QuickSlot.Count; i++)
        {
            if (QuickSlotImage[i].activeSelf == true)
            {
                QuickSlotImage[i].SetActive(false);
            }

            Image Image = QuickSlotImage[i].GetComponent<Image>();
            Image.sprite = null;
            if (Image.enabled == true)
            {
                Image.enabled = false;
            }

            QuickSlotNumberList[i].text = null;
        }

        
        for (int i = 0; i < QuickSlotList.Count; i ++)
        {

            //null 아닌 값이 있을 경우
            if (QuickSlotList[i] != "null")
            {

                //이미지 연동
                Image QuickImage = QuickSlotImage[i].GetComponent<Image>();
                Image InventoryImage = ImageSlot[i].GetComponent<Image>();



                //인벤토리 이동이 있을때
                for (int j = 0; j < InventoryList.Count; j++)
                {
                    if (QuickSlotList[i] == InventoryList[j])
                    {
                        InventoryImage = ImageSlot[j].GetComponent<Image>();
                        QuickSlotPosition[i] = j;

                        //텍스트 연동
                        QuickSlotNumberList[i].text = NumberList[j].text;

                        break;
                    }
                }
                //폐기 예정
                if (InventoryImage.sprite == null)
                {
                    Debug.Log("이거 지금 작동하면 안됨");
                    
                }

                else
                {
                    
                }


                //퀵슬롯 이미지가 꺼져있을 경우
                if (QuickSlotImage[i].activeSelf == false)
                {
                    QuickSlotImage[i].SetActive(true);
                }

                //이미지 컴포넌트가 꺼져있을 경우 키고 이미지 받아오기
                if (QuickImage.enabled == false)
                {
                    QuickImage.enabled = true;
                }

                QuickImage.sprite = InventoryImage.sprite;
                
            }
        }
    }

    public void Quick_Reset(string name)
    {

        for (int i = 0; i < QuickSlotList.Count; i ++)
        {
            if (QuickSlotList[i] == name)
            {

                //정보 리셋
                QuickSlotList[i] = "null";
                QuickSlotPosition[i] = -1;
                QuickSlotImage[i].SetActive(false);
                break;

            }
        }
    }

    //준 폐기
    ////슬롯 선택 오브젝트 움직이기
    //public void Select_Move()
    //{
    //    if (Inventory.activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("Jump"))
    //        {
    //            Debug.Log(Positioncount);
    //        }

    //        if (Input.GetButtonDown("Left"))
    //        {
    //            if (Positioncount != 0)
    //            {
    //                Positioncount--;
    //                Inventory_Select.transform.position = Slot_Position[Positioncount];
    //            }
    //        }

    //        else if (Input.GetButtonDown("Right"))
    //        {
    //            if (Positioncount < SetSize && ImageSlot[Positioncount + 1].GetComponent<Image>().enabled == true)
    //            {
    //                Positioncount++;
    //                Inventory_Select.transform.position = Slot_Position[Positioncount];
    //            }
    //        }

    //        else if (Input.GetButtonDown("Up"))
    //        {
    //            if ((Positioncount - 3) >= 0)
    //            {
    //                Positioncount -= 3;
    //                Inventory_Select.transform.position = Slot_Position[Positioncount];
    //            }
    //        }

    //        else if (Input.GetButtonDown("Down"))
    //        {
    //            if ((Positioncount + 3) <= SetSize && ImageSlot[Positioncount + 3].GetComponent<Image>().enabled == true)
    //            {
    //                Positioncount += 3;
    //                Inventory_Select.transform.position = Slot_Position[Positioncount];
    //            }
    //        }

    //        Load_Information();
    //    }
    //}

}
