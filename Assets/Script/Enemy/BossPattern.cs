using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class BossPattern : MonoBehaviour
{

    public ObjectPool objectPool;
    Transform targetTransform = null;

    CircleCollider2D CircleCollider2D;

    public float shootCount = 3f;

    // Start is called before the first frame update
    void Start()
    {
        CircleCollider2D = GetComponent<CircleCollider2D>();
        StartCoroutine(Ativity1());
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
            yield return new WaitForSeconds(3f);
            ShootStone();
            for (int i = 0; i < shootCount - 1; i++)
            {
                int delay = Random.Range(1, 3);
                yield return new WaitForSeconds(delay);
                ShootStone();
            }
        }
    }
    private void OnDrawGizmos()     // 인식범위표시
    {
        if (CircleCollider2D != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, CircleCollider2D.radius * 2f);
        }
    }
}
