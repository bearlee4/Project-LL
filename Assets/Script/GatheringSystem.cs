using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GatheringSystem : MonoBehaviour
{
    public GameObject Manager;
    InventorySystem InventorySystem;

    public List<Dictionary<string, object>> ItemDB;

    private List<GameObject> TriggerList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        InventorySystem = Manager.GetComponent<InventorySystem>();
        ItemDB = CSVReader.Read("ItemDB");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Interaction") && InventorySystem.Inventory.activeSelf == false)
        {
            AddItem();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Forage")
        {
            Debug.Log("채집물");
            TriggerList.Add(col.gameObject);           

        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Forage")
        {
            TriggerList.Clear();
        }
    }

    //미완성 한꺼번에 먹게할지 따로따로 먹게할지에 따라서 다르게 할 예정
    public void AddItem()
    {
        if (TriggerList.Any() == true)
        {
            Debug.Log(TriggerList.Count + "2@작동중");

            for (int TL = 0; TL < TriggerList.Count; TL++)
            {
                Debug.Log("브레이크 후 또 뜸?");
                for (int IDB = 0; IDB < ItemDB.Count; IDB++)
                {
                    Debug.Log("d여기까지 작동중");
                    if (TriggerList[TL].name.ToString() == ItemDB[IDB]["ImgName"].ToString())
                    {
                        InventorySystem.AddInventory(ItemDB[IDB]["ImgName"]);
                        TriggerList[TL].SetActive(false);
                        break;
                    }
                }
            }
        }

    }
}
