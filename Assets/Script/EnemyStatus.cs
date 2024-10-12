using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public float hp;
    public float atk;
    public float def;

    public void Damaged(float damage)
    {
        hp =- damage;
    }

    void Die()
    {
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
