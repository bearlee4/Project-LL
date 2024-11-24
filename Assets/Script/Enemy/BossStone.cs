using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class BossStone : MonoBehaviour
{
    private BossStatus bossStatus;
    GameObject boss;

    public ObjectPool objectPool;
    private Vector2 dir;
    public float speed;
    public float time;
    public float damage;

    public int BossLayer = 10;
    public int StoneLayer = 7;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null)
        {
            bossStatus = boss.GetComponent<BossStatus>();
            damage = bossStatus.atk;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    public void Initialize(ObjectPool Pool, Vector2 direction)
    {
        objectPool = Pool;
        dir = direction.normalized;

        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null)
        {
            Physics2D.IgnoreLayerCollision(BossLayer, StoneLayer, true);
        }

        Invoke("ReturnToPool", time);
    }

    void ReturnToPool()
    {
        Debug.Log("풀로 반환");
        objectPool.ReturnStone(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("WaterWave"))
        {
            Debug.Log("asdasdsad");
            CancelInvoke("ReturnToPool");
            Physics2D.IgnoreLayerCollision(BossLayer, StoneLayer, false);
            Reflection();
        }

        if (col.gameObject.CompareTag("Boss"))
        {
            bossStatus.Damaged(damage);
            ReturnToPool();
        }

        //else
        //{
        //    Debug.Log("부딪힘");
        //    ReturnToPool();
        //}
    }

    private void Reflection()
    {
        Vector2 bossPos = boss.transform.position;
        dir = (bossPos - (Vector2)transform.position).normalized;
    }
}
