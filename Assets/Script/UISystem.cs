using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UISystem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    InventorySystem InventorySystem;
    StorageSystem StorageSystem;
    InteractionSystem InteractionSystem;
    ItemInformation ItemInformation;
    AlchemySystem AlchemySystem;
    RequestSystem RequestSystem;

    public GameObject Manager;
    private GameObject Player;
    private GameObject check_Object;
    public GameObject overObject;

    public bool clicktoggle;
    private RectTransform slotSize;
    private bool double_Click_trigger;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        InventorySystem = Manager.GetComponent<InventorySystem>();
        StorageSystem = Manager.GetComponent<StorageSystem>();
        ItemInformation = Manager.GetComponent<ItemInformation>();
        AlchemySystem = Manager.GetComponent<AlchemySystem>();
        RequestSystem = Manager.GetComponent<RequestSystem>();
        InteractionSystem = Player.GetComponent<InteractionSystem>();


        slotSize = ItemInformation.slot_Select.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (check_Object == eventData.pointerCurrentRaycast.gameObject)
        {
            double_Click_trigger = true;
        }

        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        check_Object = clickedObject;
        Debug.Log("clickedObject.name : " + clickedObject.name);
        //좌클릭시
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            Debug.Log("좌클릭");

            //아이템 창 클릭
            if (clickedObject.tag == "ItemImage" && clickedObject.name != "QuickSlotImage" && clickedObject.name != "ResultSlotImage")
            {
                RectTransform objectSize = clickedObject.GetComponent<RectTransform>();
                slotSize.sizeDelta = new Vector2(objectSize.sizeDelta.x + 10, objectSize.sizeDelta.y + 10);

                //인벤토리 슬롯 클릭
                if (InventorySystem.Inventory.activeSelf == true)
                {
                    ItemInformation.slot_Select.transform.position = clickedObject.transform.position;
                    Debug.Log(clickedObject.name);
                    clicktoggle = true;
                    InventorySystem.Load_Information(ItemInformation.slot_Select);
                }

                //창고 슬롯 클릭
                else if (StorageSystem.storageUI.activeSelf == true)
                {
                    ItemInformation.slot_Select.transform.position = clickedObject.transform.position;
                    ItemInformation.Set_Position(StorageSystem.StorageSlot, StorageSystem.InventorySlot);
                    clicktoggle = true;
                }

                //연금 슬롯 클릭
                else if (AlchemySystem.alchemyUI.activeSelf == true)
                {
                    ItemInformation.slot_Select.transform.position = clickedObject.transform.position;
                    ItemInformation.Set_Position(AlchemySystem.AlchemySlot, AlchemySystem.StorageSlot);
                    //AlchemySystem.Load_Information();
                    clicktoggle = true;
                }

            }

            if (clickedObject.tag == "RecipeImage")
            {

                if (double_Click_trigger == true)
                {
                    clicktoggle = false;
                    double_Click_trigger = false;
                }
            }

            //가방UI 클릭시
            if (clickedObject.name == "Bag")
            {
                if (InventorySystem.Inventory.activeSelf == false && InteractionSystem.UItoken == false)
                {
                    InventorySystem.OpenInventory();
                    InteractionSystem.UItoken = true;
                }

                else if (InventorySystem.Inventory.activeSelf == true && InteractionSystem.UItoken == true)
                {
                    InventorySystem.CloseInventory();
                    InteractionSystem.UItoken = false;
                }
            }

            if (clickedObject.name == "RequestScroll")
            {
                RequestSystem.information_toggle = true;
                RequestSystem.Load_Information(clickedObject);
            }
        }

        //우클릭시
        if (eventData.button.Equals(PointerEventData.InputButton.Right))
        {
            //퀵슬롯 지정
            if (InventorySystem.Inventory.activeSelf == true)
            {
                InventorySystem.Set_QuickSlot();
            }

            //창고 UI 전송시스템
            if (StorageSystem.storageUI.activeSelf == true && ItemInformation.slot_Select.activeSelf == true)
            {
                ItemInformation.slot_Select.transform.position = clickedObject.transform.position;
                ItemInformation.Set_Position(StorageSystem.StorageSlot, StorageSystem.InventorySlot);
                clicktoggle = false;
                Debug.Log("전송 버튼 작동중");
                StorageSystem.TransItem();  
            }

            //연금 UI 전송시스템
            if (AlchemySystem.alchemyUI.activeSelf == true && ItemInformation.slot_Select.activeSelf == true && clickedObject != AlchemySystem.resultImageSlot && AlchemySystem.ban_trans == false)
            {
                ItemInformation.slot_Select.transform.position = clickedObject.transform.position;
                ItemInformation.Set_Position(AlchemySystem.AlchemySlot, AlchemySystem.StorageSlot);
                clicktoggle = false;
                Debug.Log("연금UI 전송 버튼 작동중");
                AlchemySystem.TransItem();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        overObject = eventData.pointerCurrentRaycast.gameObject;
        if (overObject.tag == "ItemImage")
        {
            if (ItemInformation.InformationWindow.activeSelf == false)
            {
                ItemInformation.InformationWindow.SetActive(true);
            }

            ItemInformation.InformationWindow.transform.position = overObject.transform.position;
            ItemInformation.InformationWindow.transform.position += new Vector3(180, 0, 0);
            if (ItemInformation.InformationWindow.transform.position.x >= 1000)
            {
                ItemInformation.InformationWindow.transform.position += new Vector3(-360, 0, 0);
            }
            //ItemInformation.InformationWindow.transform.position += new Vector3(115, -165, 0);
            ItemInformation.Load_Information(overObject);

            //인벤토리가 켜졌을 떄
            if (InventorySystem.Inventory.activeSelf == true)
            {
                InventorySystem.Load_Information(ItemInformation.slot_Select);
            }

            //창고가 켜졌을 때
            if (StorageSystem.storageUI.activeSelf == true)
            {
                ItemInformation.Set_Position(StorageSystem.StorageImageSlot, StorageSystem.InventoryImageSlot);
                StorageSystem.storageside = ItemInformation.sidetoken;
            }

            //연금UI가 켜졌을 때
            else if (AlchemySystem.alchemyUI.activeSelf == true && overObject != AlchemySystem.resultImageSlot)
            {
                ItemInformation.Set_Position(AlchemySystem.AlchemyImageSlot, AlchemySystem.StorageImageSlot);
                AlchemySystem.alchemyside = ItemInformation.sidetoken;
            }

            //if (InventorySystem.Inventory.activeSelf == true)
            //{
            //    ItemInformation.InformationWindow.transform.position = overObject.transform.position;
            //    ItemInformation.InformationWindow.transform.position += new Vector3(150, -200, 0);
            //    ItemInformation.Load_Information(overObject);
            //}

            ////아이템 슬롯 바로 옆에 생성
            //else
            //{
            //    ItemInformation.InformationWindow.transform.position = overObject.transform.position;
            //    ItemInformation.InformationWindow.transform.position += new Vector3(150, -200, 0);
            //    ItemInformation.Load_Information(overObject);
            //}
        }

        else if (overObject.name == "RequestScroll" && RequestSystem.information_toggle == false)
        {
            Debug.Log("스크롤 접촉");
            RequestSystem.Load_Information(overObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (overObject.tag == "ItemImage")
        {

            ToggleStorageUI(ItemInformation.InformationWindow);

            if (clicktoggle == false && ItemInformation.slot_Select.activeSelf == true)
            {
                ToggleStorageUI(ItemInformation.slot_Select);
            }
            //if (ItemInformation.InformationWindow.activeSelf == true)
            //{
            //    Debug.Log("이건 작동중?");
            //    ItemInformation.InformationWindow.SetActive(false);

            //}
        }

        else if (overObject.name == "RequestScroll")
        {
            Debug.Log("스크롤 나감");
            if (RequestSystem.information_toggle == false)
            {
                RequestSystem.request_information.SetActive(false);
            }
        }
    }

    public void ToggleStorageUI(GameObject UI)
    {
        if (UI != null)
        {
            UI.SetActive(!UI.activeSelf); // UI 요소의 활성 상태를 토글
        }
    }

}