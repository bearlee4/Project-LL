using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MovePower = 5f;
    public GameObject Manager;
    Rigidbody2D rigid;
    InventorySystem Inventory;


    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        Inventory = Manager.GetComponent<InventorySystem>();
        Debug.Log("테스트");

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    //캐릭터 움직임
    void Move()
    { 
        Vector3 move = Vector3.zero;

        if(Input.GetAxisRaw("Horizontal") < 0)
        {
            move = Vector3.left;
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            move = Vector3.right;
        }

        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            move = Vector3.down;
        }

        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            move = Vector3.up;
        }

        transform.position += move * MovePower * Time.deltaTime;
    }

    //상호작용
    private void OnTriggerEnter2D(Collider2D col)
    {
        List<Dictionary<string, object>> ItemDB = CSVReader.Read("ItemDB");

        //채집물 태그와 상호작용
        if (col.gameObject.tag == "Forage")
        {
            
            if (col.name == "Twincleglass")
            {
                Debug.Log("반짝이풀");
                Inventory.AddInventory(ItemDB[0]["ImgName"]);
            }

            else if (col.name == "Sakura")
            {
                Debug.Log("벚꽃");
                Inventory.AddInventory(ItemDB[1]["ImgName"]);
            }
        }
    }
}
