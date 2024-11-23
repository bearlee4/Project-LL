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

    public int playerLayer = 8;
    public int tsunamiLayer = 9;

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
            Physics2D.IgnoreLayerCollision(playerLayer, tsunamiLayer, true);
            StartCoroutine("Expand");
        }
    }

    IEnumerator Expand()
    {
        
        transform.position = player.position;
        transform.localScale = new Vector3((float)0.5, (float)0.5, 1);
        float increaseSpeed = (increase * Time.deltaTime * 25);

        while (transform.localScale.x < 1.5f)
        {
            transform.localScale += new Vector3(increaseSpeed, increaseSpeed, 0);
            yield return null;
        }

        Physics2D.IgnoreLayerCollision(playerLayer, tsunamiLayer, false);

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
}
