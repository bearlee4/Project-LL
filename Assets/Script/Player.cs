using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject HP_bar;
    public GameObject MP_bar;

    private Slider HP_slider;
    private Slider MP_slider;

    private string currentSceneName;
    public int gold;
    public Text gold_Text;
    public float MovePower = 5f;
    public GameObject Manager;
    Rigidbody2D rigid;

    InventorySystem InventorySystem;
    InteractionSystem InteractionSystem;
    PlayerStatus PlayerStatus;

    public List<Dictionary<string, object>> ItemDB;

    // Start is called before the first frame update
    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        rigid = gameObject.GetComponent<Rigidbody2D>();
        InventorySystem = Manager.GetComponent<InventorySystem>();
        InteractionSystem = this.GetComponent<InteractionSystem>();
        PlayerStatus = this.GetComponent<PlayerStatus>();
        ItemDB = CSVReader.Read("ItemDB");
        gold = 0;
        gold_Text.text = gold.ToString();
        gold_Text.color = Color.yellow;


        HP_slider = HP_bar.GetComponent<Slider>();
        MP_slider = MP_bar.GetComponent<Slider>();
        HP_slider.maxValue = PlayerStatus.maxHP;
        MP_slider.maxValue = PlayerStatus.maxMP;
        HP_slider.value = PlayerStatus.CurrentHP;
        MP_slider.value = PlayerStatus.CurrentMP;

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
