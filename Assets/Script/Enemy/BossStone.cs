using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BossStone : MonoBehaviour
{
    private BossStatus bossStatus;

    public ObjectPool objectPool;
    private Vector2 dir;
    public float speed;
    public float time;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
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
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), boss.GetComponent<Collider2D>());
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
        Debug.Log("부딪힘");
        ReturnToPool();
    }
}
