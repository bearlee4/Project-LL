using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Player : MonoBehaviour
{
    public GameObject HP_bar;
    public GameObject MP_bar;

    private Slider HP_slider;
    private Slider MP_slider;

    private string currentSceneName;
    public int gold;
    public Text gold_Text;
    public float currentSpeed;
    public float movePower = 1.5f;
    public GameObject Manager;
    Rigidbody2D rigid;

    InventorySystem InventorySystem;
    InteractionSystem InteractionSystem;
    PlayerStatus PlayerStatus;

    public bool moveable = true;
    private bool invincible = false;

    public List<Dictionary<string, object>> ItemDB;

    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = movePower;

        currentSceneName = SceneManager.GetActiveScene().name;
        rigid = gameObject.GetComponent<Rigidbody2D>();
        InventorySystem = Manager.GetComponent<InventorySystem>();
        InteractionSystem = this.GetComponent<InteractionSystem>();
        PlayerStatus = this.GetComponent<PlayerStatus>();
        ItemDB = CSVReader.Read("ItemDB");
        gold = 1000;
        gold_Text.text = gold.ToString();
        gold_Text.color = Color.yellow;


        HP_slider = HP_bar.GetComponent<Slider>();
        MP_slider = MP_bar.GetComponent<Slider>();
        HP_slider.maxValue = PlayerStatus.maxHP;
        MP_slider.maxValue = PlayerStatus.maxMP;
        HP_slider.value = PlayerStatus.currentHP;
        MP_slider.value = PlayerStatus.currentMP;

        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentSceneName != "Forest")
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
        if (moveable)
        {
            float dirx = Input.GetAxisRaw("Horizontal");
            float diry = Input.GetAxisRaw("Vertical");

            
            //float xSpeed = dirx * currentSpeed;
            //float ySpeed = diry * currentSpeed;

            //Vector2 newVelocity = new Vector2(xSpeed, ySpeed);
            //rigid.velocity = newVelocity;

            Vector3 move = (Vector3.right * dirx) + (Vector3.up * diry);

            move.Normalize();

            rigid.MovePosition(transform.position + move * currentSpeed * Time.deltaTime);

            if (dirx > 0)
                spriteRenderer.flipX = true;
            if (dirx < 0)
                spriteRenderer.flipX = false;

            //transform.position = transform.position + move * MovePower * Time.deltaTime;
        }
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

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy") && col.collider is CapsuleCollider2D)
        {
            EnemyStatus enemy = col.gameObject.GetComponent<EnemyStatus>();

            if (enemy != null)
            {
                Damaged(enemy.atk);
                StartCoroutine(DamagedEffect());
                StartCoroutine(KnockBack(col));
            }

        }

        if (col.gameObject.CompareTag("Boss") && col.collider is CapsuleCollider2D)
        {
            BossStatus boss = col.gameObject.GetComponent<BossStatus>();

            if (boss != null)
            {
                Damaged(boss.atk);
                StartCoroutine(DamagedEffect());
                StartCoroutine(KnockBack(col));
            }

        }

        if (col.gameObject.CompareTag("Stone") && col.collider is CapsuleCollider2D)
        {
            BossStone stone = col.gameObject.GetComponent<BossStone>();

            if (stone != null)
            {
                Damaged(stone.damage);
                StartCoroutine(DamagedEffect());
                StartCoroutine(KnockBack(col));
            }

        }


    }

    private IEnumerator KnockBack(Collision2D col)
    {
        moveable = false;

        Vector2 KnockBack = col.contacts[0].normal;
        float knockStr = 0.5f;
        rigid.velocity = KnockBack * knockStr;
        StartCoroutine(DamagedEffect());
        yield return new WaitForSeconds(0.5f);

        rigid.velocity = Vector2.zero;
        moveable = true;
        yield break;
    }

    private IEnumerator DamagedEffect()
    {
        invincible = true;
        int time = 0;
        while (time < 3)
        {
            if (time % 2 == 0)
                spriteRenderer.color = new Color32(255, 255, 255, 90);
            else
                spriteRenderer.color = new Color32(255, 255, 255, 180);
            yield return new WaitForSeconds(0.4f);
            time++;
        }
        spriteRenderer.color = new Color32(255, 255, 255, 255);
        invincible = false;
    }

    public void Damaged(float damage)
    {
        if (!invincible)
        {
            PlayerStatus.currentHP -= damage;
            HP_slider.value = PlayerStatus.currentHP;

            if (PlayerStatus.currentHP <= 0)
            {
                PlayerStatus.Die();
            }
        }
    }
}
