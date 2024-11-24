using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    PlayerStatus playerStatus;

    private ElementManager elementManager;
    private BeamSkill beamSkill;
    private PyroESkill pyroESkill;
    private HydroESkill hydroESkill;

    public ObjectPool objectPool;

    public List<float> QSkillDamage = new List<float> { 2, 3, 2, 3 };
    public List<float> ESkillDamage = new List<float> { 5, 5, 5, 5 };

    Camera cam;

    void Start() 
    { 
        playerStatus = GetComponent<PlayerStatus>();
        elementManager = GetComponent<ElementManager>();
        beamSkill = GetComponent<BeamSkill>();
        pyroESkill = GetComponent<PyroESkill>();
        hydroESkill = GetComponent<HydroESkill>();
        cam = Camera.main;
    }

    public void QSkill(int Element)
    {
        ShootBullet(Element);
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
                HydroE();
                break;
            case 2:
                break;
            case 3:
                SpecialSkill();
                break;
        }
        
        Debug.Log(elementManager.Element[Element] + " E Skill Atcivity");
    }

    void ShootBullet(int Element)
    {
        if (Element == 0)
        {
            GameObject Pyrobullet = objectPool.GetBullet();
            Pyrobullet.transform.position = transform.position;
            //bullet.transform.rotation = transform.rotation;

            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector2 dir = (mousePos - transform.position).normalized;

            Pyrobullet.GetComponent<Bullet>().Initialize(objectPool, dir);
        }
        GameObject Hydrobullet = objectPool.GetBullet();

        if (Element == 1)
        {
            GameObject HydroBullet = objectPool.GetBullet();
            HydroBullet.transform.position = transform.position;
            //bullet.transform.rotation = transform.rotation;

            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector2 dir = (mousePos - transform.position).normalized;

            HydroBullet.GetComponent<Bullet>().Initialize(objectPool, dir);
        }
    }

    void PyroE()
    {
        playerStatus.currentMP =- 20;
        pyroESkill.Arc();
    }

    void HydroE()
    {
        playerStatus.currentMP = -15;
        hydroESkill.Wave();
    }

    void SpecialSkill()
    {
        beamSkill.Beam();
    }

}
