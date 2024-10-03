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

    public GameObject Manager;
    private GameObject Player;
    public GameObject overObject;

    public bool clicktoggle;
    private RectTransform slotSize;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        InventorySystem = Manager.GetComponent<InventorySystem>();
        StorageSystem = Manager.GetComponent<StorageSystem>();
        ItemInformation = Manager.GetComponent<ItemInformation>();
        AlchemySystem = Manager.GetComponent<AlchemySystem>();
        InteractionSystem = Player.GetComponent<InteractionSystem>();

        slotSize = ItemInformation.slot_Select.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;

        //좌클릭시
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            Debug.Log("좌클릭");

            //아이템 창 클릭
            if (clickedObject.tag == "ItemImage")
            {
                RectTransform objectSize = clickedObject.GetComponent<RectTransform>();
                slotSize.sizeDelta = new Vector2(objectSize.sizeDelta.x + 10, objectSize.sizeDelta.y + 10);

                if (InventorySystem.Inventory.activeSelf == true)
                {
                    ItemInformation.slot_Select.transform.position = clickedObject.transform.position;
                    Debug.Log(clickedObject.name);
                    clicktoggle = true;
                    InventorySystem.Load_Information();
                }

                else if (StorageSystem.storageUI.activeSelf == true)
                {
                    ItemInformation.slot_Select.transform.position = clickedObject.transform.position;
                    ItemInformation.Set_Position(StorageSystem.StorageSlot, StorageSystem.InventorySlot);
                    clicktoggle = true;
                }

                else if (AlchemySystem.alchemyUI.activeSelf == true)
                {
                    ItemInformation.slot_Select.transform.position = clickedObject.transform.position;
                    ItemInformation.Set_Position(AlchemySystem.AlchemySlot, AlchemySystem.StorageSlot);
                    //AlchemySystem.Load_Information();
                    clicktoggle = true;
                }

            }

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
        }

        //우클릭시
        if (eventData.button.Equals(PointerEventData.InputButton.Right))
        {
            if (StorageSystem.storageUI.activeSelf == true && ItemInformation.slot_Select.activeSelf == true)
            {
                ItemInformation.slot_Select.transform.position = clickedObject.transform.position;
                ItemInformation.Set_Position(StorageSystem.StorageSlot, StorageSystem.InventorySlot);
                clicktoggle = false;
                Debug.Log("전송 버튼 작동중");
                StorageSystem.TransItem();  
            }

            if (AlchemySystem.alchemyUI.activeSelf == true && ItemInformation.slot_Select.activeSelf == true && clickedObject != AlchemySystem.resultImageSlot)
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
            ItemInformation.InformationWindow.transform.position += new Vector3(150, -200, 0);
            ItemInformation.Load_Information(overObject);

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
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        Debug.Log(overObject.tag + "나감");
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
    }

    public void ToggleStorageUI(GameObject UI)
    {
        if (UI != null)
        {
            UI.SetActive(!UI.activeSelf); // UI 요소의 활성 상태를 토글
        }
    }

}