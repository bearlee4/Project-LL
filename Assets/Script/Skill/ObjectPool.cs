using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject stonePrefab;
    public int bulletPoolSize = 2;
    public int stonePoolSize = 3;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();
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
    public void ReturnStone(GameObject stone)
    {
        stone.SetActive(false);
        stonePool.Enqueue(stone);
    }
}
