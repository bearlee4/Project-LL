using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UISystem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject Manager;
    private GameObject Player;
    public GameObject overObject;

    public bool clicktoggle;

    InventorySystem InventorySystem;
    StorageSystem StorageSystem;
    InteractionSystem InteractionSystem;
    ItemInformation ItemInformation;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        InventorySystem = Manager.GetComponent<InventorySystem>();
        StorageSystem = Manager.GetComponent<StorageSystem>();
        ItemInformation = Manager.GetComponent<ItemInformation>();
        InteractionSystem = Player.GetComponent<InteractionSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;

        //아이템 창 클릭
        if (clickedObject.tag == "ItemImage")
        {
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
                StorageSystem.Load_Information();
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        overObject = eventData.pointerCurrentRaycast.gameObject;
        if (overObject.tag == "ItemImage")
        {
            Debug.Log(overObject.tag);
            if (ItemInformation.InformationWindow.activeSelf == false)
            {
                ItemInformation.InformationWindow.SetActive(true);
            }

            //인벤토리가 켜졌을 때
            if (InventorySystem.Inventory.activeSelf == true)
            {
                //ItemInformation.InformationWindow.transform.position = InventorySystem.Inventory.transform.position;
                ItemInformation.InformationWindow.transform.position = overObject.transform.position;
                ItemInformation.InformationWindow.transform.position += new Vector3(150, -200, 0);
                ItemInformation.Load_Information(overObject);
            }

            //아이템 슬롯 바로 옆에 생성
            else
            {
                ItemInformation.InformationWindow.transform.position = overObject.transform.position;
                ItemInformation.InformationWindow.transform.position += new Vector3(150, -200, 0);
                ItemInformation.Load_Information(overObject);
            }
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