using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public float maxHp;
    public float currentHp;
    public float atk;
    public float def;
    public float speed;
    public float returnSpeed;

    public bool invincible = false;

    private void Start()
    {
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
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
