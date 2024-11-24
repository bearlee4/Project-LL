using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletPrefab2;
    public GameObject stonePrefab;
    public int bulletPoolSize = 5;
    public int bullet2PoolSize = 5;
    public int stonePoolSize = 5;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();
    private Queue<GameObject> bullet2Pool = new Queue<GameObject>();
    private Queue<GameObject> stonePool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < bulletPoolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }

        for (int i = 0; i < stonePoolSize; i++)
        {
            GameObject stone = Instantiate(stonePrefab);
            stone.SetActive(false);
            stonePool.Enqueue(stone);
        }

        for (int i = 0; i < bullet2PoolSize; i++)
        {
            GameObject bullet2 = Instantiate(bulletPrefab2);
            bullet2.SetActive(false);
            bullet2Pool.Enqueue(bullet2);
        }
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            GameObject bullet = Instantiate(bulletPrefab);
            return bullet;
        }
    }
    public GameObject GetBullet2()
    {
        if (bullet2Pool.Count > 0)
        {
            GameObject bullet2 = bullet2Pool.Dequeue();
            bullet2.SetActive(true);
            return bullet2;
        }
        else
        {
            GameObject bullet2 = Instantiate(bulletPrefab2);
            return bullet2;
        }
    }

    public GameObject GetStone()
    {
        if (stonePool.Count > 0)
        {
            GameObject stone = stonePool.Dequeue();
            stone.SetActive(true);
            return stone;
        }
        else
        {
            GameObject stone = Instantiate(stonePrefab);
            return stone;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }

    public void ReturnBullet2(GameObject bullet2)
    {
        bullet2.SetActive(false);
        bullet2Pool.Enqueue(bullet2);
    }

    public void ReturnStone(GameObject stone)
    {
        stone.SetActive(false);
        stonePool.Enqueue(stone);
    }
}
