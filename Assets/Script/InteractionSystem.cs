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

    private GameObject canvas;
    UISystem UISystem;

    public bool UItoken;
    private bool forageincounter;
    private bool storageincounter;
    private bool alchemyincounter;

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
        InventorySystem = Manager.GetComponent<InventorySystem>();
        StorageSystem = Manager.GetComponent<StorageSystem>();
        ItemInformation = Manager.GetComponent<ItemInformation>();
        AlchemySystem = Manager.GetComponent<AlchemySystem>();
        ItemDB = CSVReader.Read("ItemDB");

        backhomeincounter = false;
    }


    // Update is called once per frame
    void Update()
    {
        CheckButton();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
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
    }

    private void OnTriggerStay2D(Collider2D col)
    {

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Forage")
        {
            Debug.Log("리스트 클리어 작동 했음");
            forageincounter = true;
            TriggerList.Clear();
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
    }

    public void CheckButton()
    {
        //F버튼
        if (Input.GetButtonDown("Interaction"))
        {
            //채집물 획득
            if (InventorySystem.Inventory.activeSelf == false && forageincounter == true)
            {
                AddDropItem();
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
                    AlchemySystem.alchemyUI.SetActive(false);
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
    }

    //아이템 추가
    public void AddDropItem()
    {
        int listcount;

        listcount = TriggerList.Count;

        if (TriggerList.Any() == true)
        {

            for (int IDB = 0; IDB < ItemDB.Count; IDB++)
            {
                if (TriggerList[0] == ItemDB[IDB]["ImgName"].ToString())
                {
                    InventorySystem.AddInventory(ItemDB[IDB]["ImgName"], InventorySystem.GetCount);
                    break;
                }

            }

            if (InventorySystem.FullInventory == false)
            {
                GameObject.Find(TriggerList[0]).SetActive(false);
                //TriggerList.Remove(TriggerList[0]);
            }
        }

        else if (!TriggerList.Any() == false)
        {
            forageincounter = false;
        }

    }
}
