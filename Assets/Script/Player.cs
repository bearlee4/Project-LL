using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float MovePower = 5f;
    public GameObject Manager;
    Rigidbody2D rigid;

    InventorySystem Inventory;

    public List<Dictionary<string, object>> ItemDB;

    // Start is called before the first frame update
    void Start()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
        Inventory = Manager.GetComponent<InventorySystem>();
        ItemDB = CSVReader.Read("ItemDB");

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        
    }

    //캐릭터 움직임
    void Move()
    { 
        float dirx = Input.GetAxisRaw("Horizontal");
        float diry = Input.GetAxisRaw("Vertical");

        Vector3 move = (Vector3.right * dirx) + (Vector3.up * diry);

        move.Normalize();

        transform.position += move * MovePower * Time.deltaTime;
    }
}
