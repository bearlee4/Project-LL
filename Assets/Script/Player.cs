using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private string currentSceneName;
    public int gold;
    public Text gold_Text;
    public float MovePower = 5f;
    public GameObject Manager;
    Rigidbody2D rigid;

    InventorySystem InventorySystem;
    InteractionSystem InteractionSystem;

    public List<Dictionary<string, object>> ItemDB;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        rigid = gameObject.GetComponent<Rigidbody2D>();
        InventorySystem = Manager.GetComponent<InventorySystem>();
        InteractionSystem = this.GetComponent<InteractionSystem>();
        ItemDB = CSVReader.Read("ItemDB");
        gold = 0;
        gold_Text.text = gold.ToString();
        gold_Text.color = Color.yellow;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if(currentSceneName != "Forest")
        {
            if (InteractionSystem.UItoken == false)
            {
                Move();
            }
        }

        else if(currentSceneName == "Forest")
        {
            Move();
        }
    }

    //캐릭터 움직임
    void Move()
    { 
        float dirx = Input.GetAxisRaw("Horizontal");
        float diry = Input.GetAxisRaw("Vertical");

        Vector3 move = (Vector3.right * dirx) + (Vector3.up * diry);

        move.Normalize();

        rigid.MovePosition(transform.position + move * MovePower * Time.deltaTime);

        //transform.position = transform.position + move * MovePower * Time.deltaTime;
    }

    public void Get_Gold(int number)
    {
        gold += number;
        gold_Text.text = gold.ToString();
    }

    public void Use_Gold(int number)
    {
        gold -= number;
        gold_Text.text = gold.ToString();
    }
}
