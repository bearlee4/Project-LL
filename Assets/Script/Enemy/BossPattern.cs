using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BossPattern : MonoBehaviour
{
    public ObjectPool objectPool;
    Transform targetTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Ativity1());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)     // 범위에 플레이어 들어감
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            targetTransform = collision.gameObject.transform;

        }
    }

    void ShootStone()
    {
        if (targetTransform != null)
        {
            GameObject stone = objectPool.GetStone();
            stone.transform.position = transform.position;

            Vector2 dir = (targetTransform.position - transform.position).normalized;

            stone.GetComponent<BossStone>().Initialize(objectPool, dir);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private IEnumerator Ativity1()
    {
        while (true)
        {
            ShootStone();
            yield return new WaitForSeconds(5f);
        }
    }
}
