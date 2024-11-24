using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private ElementManager elementManager;
    private SkillManager skillManager;

    public float speed;
    public float time;
    public ObjectPool objectPool;
    private float damage;
    public float length;

    private Vector2 dir;

    GameObject player;

    void Start() 
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerStatus playerStatus = player.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                damage = playerStatus.atk;
            }
        }

        elementManager = GetComponent<ElementManager>();
        skillManager = GetComponent<SkillManager>();
        
    }

    public void Initialize(ObjectPool Pool, Vector2 direction)
    {
        objectPool = Pool;
        dir = direction.normalized;
        Invoke("ReturnToPool", time);
    }

    void Update()
    {
        transform.Translate(dir * speed * Time.deltaTime);
    }

    void ReturnToPool()
    {
        Debug.Log("풀로 반환");
        objectPool.ReturnBullet(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("부딪힘");
        ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && collision is CapsuleCollider2D)
        {
            EnemyStatus enemy = collision.GetComponent<EnemyStatus>();
            EnemyStatus boss = collision.GetComponent<EnemyStatus>();

            if (enemy != null)
            {
                enemy.Damaged(damage);
            }

            ReturnToPool();
        }

        if (collision.gameObject.CompareTag("Boss") && collision is CapsuleCollider2D)
        {

            BossStatus boss = collision.gameObject.GetComponent<BossStatus>();
            if (boss != null)
                boss.Damaged(damage);

            ReturnToPool();
        }
    }
}
