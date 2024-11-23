using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BossStone : MonoBehaviour
{
    private EnemyStatus enemyStatus;

    public ObjectPool objectPool;
    private Vector2 dir;
    public float speed;
    public float time;
    public float damage;

    public GameObject Boss;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), Boss.GetComponent<Collider2D>());
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
        Invoke("ReturnToPool", time);
    }

    void ReturnToPool()
    {
        Debug.Log("풀로 반환");
        objectPool.ReturnStone(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("부딪힘");
        ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                player.Damaged(damage);
            }
            ReturnToPool();
        }
    }


}
