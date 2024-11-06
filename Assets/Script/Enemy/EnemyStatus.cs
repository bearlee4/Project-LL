using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    CircleCollider2D CircleCollider2D;

    public float maxHp;
    public float currentHp;
    public float atk;
    public float def;
    public float speed;
    public float returnSpeed;

    public bool invincible = false; // 무적

    private void Start()
    {
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
            Hit();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }

    void Hit()
    {
        CircleCollider2D.radius = 10f;
    }
}
