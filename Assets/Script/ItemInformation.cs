using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemInformation : MonoBehaviour
{
    public GameObject InformationWindow;
    public GameObject ChooseItem;
    public GameObject slot_Select;
    public Text Content;

    InventorySystem InventorySystem;

    // Start is called before the first frame update
    void Start()
    {
        InformationWindow = GameObject.Find("ItemInformation");
        InventorySystem = this.GetComponent<InventorySystem>();
        slot_Select = InformationWindow.transform.Find("Slot_Select").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load_Information(GameObject overObject)
    {
        if(slot_Select.activeSelf == false)
        {
            slot_Select.SetActive(true);
        }

        slot_Select.transform.position = overObject.transform.position;

        Image ChooseImage = ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>();

        if (ChooseImage.enabled == false)
        {
            ChooseImage.enabled = true;
        }

        ChooseImage.sprite = overObject.GetComponent<Image>().sprite;
        for (int j = 0; j < InventorySystem.ItemDB.Count; j++)
        {
            if (ChooseImage.sprite.name.ToString() == InventorySystem.ItemDB[j]["ImgName"].ToString())
            {
                Content.text = InventorySystem.ItemDB[j]["Content"].ToString();

                break;
            }
        }

    }
}
