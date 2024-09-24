using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GatheringSystem : MonoBehaviour
{
    public GameObject Manager;
    InventorySystem InventorySystem;
    private bool ObjectToken;

    public List<Dictionary<string, object>> ItemDB;
    private List<string> TriggerList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        ObjectToken = false;
        InventorySystem = Manager.GetComponent<InventorySystem>();
        ItemDB = CSVReader.Read("ItemDB");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interaction") && InventorySystem.Inventory.activeSelf == false)
        {
            AddDropItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Forage")
        {
            Debug.Log("채집물");
            TriggerList.Add(col.gameObject.name);
            ObjectToken = true;

        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Forage")
        {
            ObjectToken = true ;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Forage")
        {
            Debug.Log("리스트 클리어 작동 했음");
            TriggerList.Clear();
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
                    InventorySystem.AddInventory(ItemDB[IDB]["ImgName"]);
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
            ObjectToken = false;
        }

    }
}
