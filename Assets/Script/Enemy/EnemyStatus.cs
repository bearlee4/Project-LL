using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public GameObject Drop_Item;
    CircleCollider2D CircleCollider2D;

    public float maxHp;
    public float currentHp;
    public float atk;
    public float def;
    public float speed;
    public float returnSpeed;

    public int EnemyType;

    public bool invincible = false; // 무적

    private void Start()
    {
        switch (EnemyType)
        {
            case 0:
                speed = 0.5f;
                break;
            case 1:
                speed = 0.25f;
                break;
            case 2:
                speed = 0f;
                break;
        }

        CircleCollider2D = GetComponent<CircleCollider2D>();
        currentHp = maxHp;
    }

    public void Damaged(float damage)
    {
        if (!invincible)
        {
            currentHp -= damage;
            if (currentHp <= 0)
            {
                Die();
            }
            if (EnemyType != 4)
                Hit();
        }
    }

    void Die()
    {
        Instantiate(Drop_Item, gameObject.transform.position, Quaternion.identity);
        currentHp = maxHp;
        gameObject.SetActive(false);
    }

    void Hit()
    {
        CircleCollider2D.radius = 10f;
    }
}
