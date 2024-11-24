using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject pyroBulletPrefab;
    public GameObject HydroBulletPrefab;
    public GameObject stonePrefab;
    public int bulletPoolSize = 20;
    public int stonePoolSize = 30;

    private Queue<GameObject> pyroBulletPool = new Queue<GameObject>();
    private Queue<GameObject> hydroBulletPool = new Queue<GameObject>();
    private Queue<GameObject> stonePool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < bulletPoolSize / 2; i++)
        {
            GameObject bullet = Instantiate(pyroBulletPrefab);
            bullet.SetActive(false);
            pyroBulletPool.Enqueue(bullet);
        }

        for (int i = 0; i < bulletPoolSize / 2; i++)
        {
            GameObject bullet = Instantiate(pyroBulletPrefab);
            bullet.SetActive(false);
            hydroBulletPool.Enqueue(bullet);
        }

        for (int i = 0; i < stonePoolSize; i++)
        {
            GameObject stone = Instantiate(stonePrefab);
            stone.SetActive(false);
            stonePool.Enqueue(stone);
        }



    }

    public GameObject GetPyroBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject pyroBullet = bulletPool.Dequeue();
            pyroBullet.SetActive(true);
            return pyroBullet;
        }
        else
        {
            GameObject pyroBullet = Instantiate(pyroBulletPrefab);
            return pyroBullet;
        }
    }

    public GameObject GetHydroBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject hydroBullet = bulletPool.Dequeue();
            hydroBullet.SetActive(true);
            return hydroBullet;
        }

        else
        {
            GameObject hydroBullet = Instantiate(pyroBulletPrefab);
            return hydroBullet;
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
