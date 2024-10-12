using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private ElementManager elementManager;
    private SkillManager skillManager;

    public float speed = 6.5f;
    public float time = 1f;
    public ObjectPool objectPool;
    public float damage;

    private Vector2 dir;

    void Start() 
    { 
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
        if (collision.gameObject.CompareTag("Enemy")&&collision is CapsuleCollider2D)
        {
            EnemyStatus enemy = collision.GetComponent<EnemyStatus>();

            if (enemy != null)
            {
                enemy.Damaged(damage);
            }
            ReturnToPool();
        }
    }
}
