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
    private GameObject overObject;

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
            if(InventorySystem.Inventory_Select.activeSelf == true && InventorySystem.Inventory.activeSelf == true)
            {
                InventorySystem.Inventory_Select.transform.position = clickedObject.transform.position;
                Debug.Log(clickedObject.name);
                ItemInformation.slot_Select.SetActive(false);
            }

            else if (StorageSystem.Slot_Select.activeSelf == true && StorageSystem.storageUI.activeSelf == true)
            {
                StorageSystem.Slot_Select.transform.position = clickedObject.transform.position;
                StorageSystem.Load_Information();
            }
            
        }

        if (clickedObject.name == "Bag")
        {
            if(InventorySystem.Inventory.activeSelf == false && InteractionSystem.UItoken == false)
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
            if(ItemInformation.InformationWindow.activeSelf == false)
            {
                ItemInformation.InformationWindow.SetActive(true);
            }

            //인벤토리가 켜졌을 때
            if(InventorySystem.Inventory.activeSelf == true)
            {
                ItemInformation.InformationWindow.transform.position = InventorySystem.Inventory.transform.position;
                ItemInformation.InformationWindow.transform.position += new Vector3(380, 0, 0);
                ItemInformation.Load_Information(overObject);
            }

            //아이템 슬롯 바로 옆에 생성
            else
            {
                ItemInformation.InformationWindow.transform.position = overObject.transform.position;
                ItemInformation.InformationWindow.transform.position += new Vector3(220, 0, 0);
                ItemInformation.Load_Information(overObject);
            }        
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        Debug.Log(overObject.tag);
        if (overObject.tag == "ItemImage")
        {
            if (ItemInformation.InformationWindow.activeSelf == true)
            {
                Debug.Log("이건 작동중?");
                ItemInformation.InformationWindow.SetActive(false);
            }
        }
    }

}