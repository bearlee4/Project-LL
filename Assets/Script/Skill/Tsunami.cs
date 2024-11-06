using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsunami : MonoBehaviour
{
    public Transform player;

    public float increase = 0.1f;
    public bool isActive = false;
    public float damage;

    private void Start()
    {

    }

    private void Update()
    {
        Wave();
    }

    // Update is called once per frame
    void Wave()
    {
        if (isActive)
        {
            
            StartCoroutine("Expand");
        }
    }

    IEnumerator Expand()
    {
        transform.position = player.position;
        transform.localScale = new Vector3(2, 2, 2);
        float increaseSpeed = (increase * Time.deltaTime * 100);


        while (transform.localScale.x < 10f)
        {
            transform.localScale += new Vector3(increaseSpeed, increaseSpeed, 0);
            yield return null;
        }

        isActive = false;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision.collider is CapsuleCollider2D)
        {
            EnemyStatus enemy = collision.gameObject.GetComponent<EnemyStatus>();

            if (enemy != null)
            {
                enemy.Damaged(damage);
            }
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Enemy") && collision is CapsuleCollider2D)
    //    {
    //        EnemyStatus enemy = collision.GetComponent<EnemyStatus>();

    //        if (enemy != null)
    //        {
    //            enemy.Damaged(damage);
    //        }
    //    }
    //}

}
