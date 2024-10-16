using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private ElementManager elementManager;
    private PyroESkill pyroESkill;
    private TestSkill testSkill;

    public ObjectPool objectPool;

    public List<float> QSkillDamage = new List<float> { 2, 3, 2, 3 };
    public List<float> ESkillDamage = new List<float> { 5, 5, 5, 5 };

    Camera cam;

    void Start() 
    { 
        elementManager = GetComponent<ElementManager>();
        pyroESkill = GetComponent<PyroESkill>();
        testSkill = GetComponent<TestSkill>();
        cam = Camera.main;
    }

    public void QSkill(int Element)
    {
        ShootBullet();
        Debug.Log(elementManager.Element[Element] + " Q Skill Atcivity");
    }

    public void ESkill(int Element)
    {
        switch (Element)
        {
            case 0:
                PyroE();
                break;
            case 1:
                TestE();
                break;
            case 2:
                break;
            case 3:
                break;
        }
        
        Debug.Log(elementManager.Element[Element] + " E Skill Atcivity");
    }

    void ShootBullet()
    {
        GameObject bullet = objectPool.GetBullet();
        bullet.transform.position = transform.position;
        //bullet.transform.rotation = transform.rotation;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector2 dir = (mousePos - transform.position).normalized;

        bullet.GetComponent<Bullet>().Initialize(objectPool, dir);
    }

    void PyroE()
    {
        pyroESkill.Beam();
    }

    void TestE()
    {
        testSkill.Arc();
    }
}
