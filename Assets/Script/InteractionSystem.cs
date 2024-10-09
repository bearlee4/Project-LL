using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionSystem : MonoBehaviour
{
    public GameObject Manager;
    InventorySystem InventorySystem;
    StorageSystem StorageSystem;
    ItemInformation ItemInformation;
    AlchemySystem AlchemySystem;
    FieldSystem FieldSystem;

    private GameObject canvas;
    UISystem UISystem;

    public bool UItoken;
    private bool forageincounter;
    private bool storageincounter;
    private bool alchemyincounter;
    private bool spawnerincounter;

    public bool max_Trans_toggle;
    public GameObject enter_object;

    //테스트용
    private bool backhomeincounter;

    public List<Dictionary<string, object>> ItemDB;

    private List<string> TriggerList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        UISystem = canvas.GetComponent<UISystem>();

        UItoken = false;
        forageincounter = false;
        storageincounter = false;
        alchemyincounter = false;
        spawnerincounter = false;
        InventorySystem = Manager.GetComponent<InventorySystem>();
        StorageSystem = Manager.GetComponent<StorageSystem>();
        ItemInformation = Manager.GetComponent<ItemInformation>();
        AlchemySystem = Manager.GetComponent<AlchemySystem>();
        FieldSystem = Manager.GetComponent<FieldSystem>();
        ItemDB = CSVReader.Read("ItemDB");

        max_Trans_toggle = false;

        backhomeincounter = false;

        FieldSystem.Forage_Classification();
    }


    // Update is called once per frame
    void Update()
    {
        CheckButton();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        enter_object = col.gameObject;

        //채집물 접촉
        if (col.gameObject.tag == "Forage")
        {
            Debug.Log("채집물");
            forageincounter = true;
            TriggerList.Add(col.gameObject.name);

        }

        //상호작용 가능한 오브젝트 접촉
        if (col.gameObject.tag == "InteractionObject")
        {
            if(col.gameObject.name == "Storage")
            {
                Debug.Log("창고 접촉");
                storageincounter = true;
            }

            if (col.gameObject.name == "Alchemy")
            {
                Debug.Log("연금솥 접촉");
                alchemyincounter = true;
            }

        }

        //일과 끝내기 접촉
        if (col.gameObject.name == "Back_Home")
        {
            Debug.Log("일과 끝내기 접촉");
            backhomeincounter = true;
        }

        //스포너와 접촉
        if (col.gameObject.tag == "Spawner")
        {
            Debug.Log("스포너 접촉");
            spawnerincounter = true;
        }

        //스포너리스폰과 접촉시 발동
        if (col.gameObject.name == "Spawner_Respawn")
        {
            FieldSystem.Respawn_spawner();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Forage")
        {
            forageincounter = false;
        }

        if (col.gameObject.name == "Storage")
        {
            storageincounter = false;
        }

        if (col.gameObject.name == "Alchemy")
        {
            alchemyincounter = false;
        }

        if (col.gameObject.name == "Back_Home")
        {
            backhomeincounter = false;
        }

        if (col.gameObject.tag == "Spawner")
        {
            Debug.Log("스포너 접촉");
            spawnerincounter = false;
        }
    }

    public void CheckButton()
    {
        //UI가 꺼져 있을때만 작동
        if (UItoken == false)
        {
            //1 버튼
            //1번 퀵슬롯에 지정해둔게 있을때
            if (Input.GetButtonDown("QuickSlot1") && InventorySystem.QuickSlotList[0] != "null")
            {
                //그 아이템 사용
                InventorySystem.UseItem(InventorySystem.QuickSlotPosition[0]);
                //정보 리로드
                //InventorySystem.Quick_Load_Info();
            }

            //2 버튼
            //2번 퀵슬롯에 지정해둔게 있을때
            if (Input.GetButtonDown("QuickSlot2") && InventorySystem.QuickSlotList[1] != "null")
            {
                //그 아이템 사용
                InventorySystem.UseItem(InventorySystem.QuickSlotPosition[1]);
                //정보 리로드
                //InventorySystem.Quick_Load_Info();
            }

            //3 버튼
            //3번 퀵슬롯에 지정해둔게 있을때
            if (Input.GetButtonDown("QuickSlot1") && InventorySystem.QuickSlotList[2] != "null")
            {
                //그 아이템 사용
                InventorySystem.UseItem(InventorySystem.QuickSlotPosition[2]);
                //정보 리로드
                //InventorySystem.Quick_Load_Info();
            }
        }

        //F버튼
        if (Input.GetButtonDown("Interaction"))
        {
            //채집물 획득
            if (InventorySystem.Inventory.activeSelf == false && forageincounter == true)
            {
                AddDropItem(enter_object);
            }

            //창고 여닫기
            if (storageincounter == true)
            {
                if (StorageSystem.storageUI.activeSelf == false && UItoken == false)
                {
                    StorageSystem.OpenStorage();
                    UItoken = true;
                }

                else if (StorageSystem.storageUI.activeSelf == true && UItoken == true)
                {
                    StorageSystem.CloseStorage();
                    UISystem.clicktoggle = false;
                    if(ItemInformation.slot_Select.activeSelf == true)
                    {
                        ItemInformation.slot_Select.SetActive(false);
                    }
                    UItoken = false;
                }
            }

            //연금 여닫기
            if (alchemyincounter == true)
            {
                if (AlchemySystem.alchemyUI.activeSelf == false && UItoken == false)
                {
                    AlchemySystem.OpenAlchemy();
                    UItoken = true;
                }

                else if (AlchemySystem.alchemyUI.activeSelf == true && UItoken == true)
                {
                    AlchemySystem.CloseAlchemy();
                    UISystem.clicktoggle = false;
                    if (ItemInformation.slot_Select.activeSelf == true)
                    {
                        ItemInformation.slot_Select.SetActive(false);
                    }
                    UItoken = false;
                }
            }

            //하루 일과 끝내기
            if (backhomeincounter == true)
            {
                StorageSystem.Back_Home();
            }

            //스포너 상호작용
            if (spawnerincounter == true)
            {
                RandomItem(enter_object);
            }
        }

        //I버튼
        if (Input.GetButtonDown("Inventory"))
        {
            if (InventorySystem.Inventory.activeSelf == false && UItoken == false)
            {
                InventorySystem.OpenInventory();
                UItoken = true;
            }

            else if (InventorySystem.Inventory.activeSelf == true && UItoken == true)
            {
                InventorySystem.CloseInventory();
                UISystem.clicktoggle = false;
                if (ItemInformation.slot_Select.activeSelf == true)
                {
                    ItemInformation.slot_Select.SetActive(false);
                }
                UItoken = false;
            }

        }

        //Z버튼
        if (Input.GetButtonDown("Confirm"))
        {
            if (InventorySystem.Inventory.activeSelf == true && ItemInformation.slot_Select.activeSelf == true)
            {
                InventorySystem.UseItem(InventorySystem.Positioncount);
            }

            if (StorageSystem.storageUI.activeSelf == true && ItemInformation.slot_Select.activeSelf == true && UISystem.clicktoggle == true)
            {
                Debug.Log("전송 버튼 작동중");
                StorageSystem.TransItem();
            }

            if (AlchemySystem.alchemyUI.activeSelf == true && ItemInformation.slot_Select.activeSelf == true && UISystem.clicktoggle == true)
            {
                Debug.Log("연금UI 전송 버튼 작동중");
                AlchemySystem.TransItem();
            }
        }

        //esc버튼
        if (Input.GetButtonDown("Cancel"))
        {
            if (StorageSystem.storageUI.activeSelf == true && UItoken == true)
            {
                StorageSystem.CloseStorage();
                UISystem.clicktoggle = false;
                if (ItemInformation.slot_Select.activeSelf == true)
                {
                    ItemInformation.slot_Select.SetActive(false);
                }
                UItoken = false;
            }

            else if (InventorySystem.Inventory.activeSelf == true && UItoken == true)
            {
                InventorySystem.CloseInventory();
                UISystem.clicktoggle = false;
                if (ItemInformation.slot_Select.activeSelf == true)
                {
                    ItemInformation.slot_Select.SetActive(false);
                }
                UItoken = false;
            }

            else if (AlchemySystem.alchemyUI.activeSelf == true && UItoken == true)
            {
                AlchemySystem.alchemyUI.SetActive(false);
                UISystem.clicktoggle = false;
                if (ItemInformation.slot_Select.activeSelf == true)
                {
                    ItemInformation.slot_Select.SetActive(false);
                }
                UItoken = false;
            }
        }

        //Ctrl 버튼
        if (Input.GetButton("Ctrl"))
        {
            max_Trans_toggle = true;
        }

        else if (Input.GetButtonUp("Ctrl"))
        {
            max_Trans_toggle = false;
        }
    }


    //아이템 추가
    public void AddDropItem(GameObject col)
    {

        for (int IDB = 0; IDB < ItemDB.Count; IDB++)
        {
            if (col.name == ItemDB[IDB]["ImgName"].ToString())
            {
                InventorySystem.AddInventory(ItemDB[IDB]["ImgName"], InventorySystem.GetCount);
                break;
            }

        }

        if (InventorySystem.FullInventory == false)
        {
            col.SetActive(false);
            //TriggerList.Remove(TriggerList[0]);
        }

    }

    

    //커먼 8, 레어 2 비율로 확률 돌리기
    public void RandomItem(GameObject col)
    {
        int random_number = Random.Range(1, 10);
        int second_random_number;
        InventorySystem.GetCount = Random.Range(1, 2);

        if (random_number <= 8)
        {
            Debug.Log("일반 등급 당첨!");
            second_random_number = Random.Range(1, FieldSystem.common_ForageList.Count);
            InventorySystem.AddInventory(FieldSystem.common_ForageList[second_random_number], InventorySystem.GetCount);

        }

        else if(random_number > 8)
        {
            Debug.Log("희귀 등급 당첨!");
            second_random_number = Random.Range(1, FieldSystem.rare_ForageList.Count);
            InventorySystem.AddInventory(FieldSystem.rare_ForageList[second_random_number], InventorySystem.GetCount);
        }

        if(InventorySystem.FullInventory == false)
        {
            col.SetActive(false);
        }
    }

    ////아이템 추가
    //public void AddDropItem()
    //{
    //    int listcount;

    //    listcount = TriggerList.Count;

    //    if (TriggerList.Any() == true)
    //    {

    //        for (int IDB = 0; IDB < ItemDB.Count; IDB++)
    //        {
    //            if (TriggerList[0] == ItemDB[IDB]["ImgName"].ToString())
    //            {
    //                InventorySystem.AddInventory(ItemDB[IDB]["ImgName"], InventorySystem.GetCount);
    //                break;
    //            }

    //        }

    //        if (InventorySystem.FullInventory == false)
    //        {
    //            GameObject.Find(TriggerList[0]).SetActive(false);
    //            //TriggerList.Remove(TriggerList[0]);
    //        }
    //    }

    //    else if (!TriggerList.Any() == false)
    //    {
    //        forageincounter = false;
    //    }

    //}
}
