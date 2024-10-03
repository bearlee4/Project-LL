using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private ElementManager elementManager;
    private SkillManager skillManager;

    public float speed = 10f;
    public float time = 2f;
    public ObjectPool objectPool;
    public float damage;

    void Start() 
    { 
        elementManager = GetComponent<ElementManager>();
        skillManager = GetComponent<SkillManager>(); 
    }

    public void Initialize(ObjectPool Pool)
    {
        objectPool = Pool;
        Invoke("ReturnToPool", time);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
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
}
