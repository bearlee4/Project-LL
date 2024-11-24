using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    PlayerStatus playerStatus;
    Player player;

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
        player = GetComponent<Player>();
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
        player.SpendMP(10);

        StartCoroutine(Casting());

        if (Element == 0)
        {
            Debug.Log("Firing Pyro Bullet");
            GameObject Pyrobullet = objectPool.GetBullet();
            Pyrobullet.transform.position = transform.position;

            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector2 dir = (mousePos - transform.position).normalized;

            Pyrobullet.GetComponent<Bullet>().Initialize(objectPool, dir);
        }

        else if (Element == 1)
        {
            Debug.Log("Firing Hydro Bullet");
            GameObject HydroBullet = objectPool.GetBullet2();
            HydroBullet.transform.position = transform.position;

            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            Vector2 dir = (mousePos - transform.position).normalized;

            HydroBullet.GetComponent<Bullet1>().Initialize(objectPool, dir);
        }
    }

    void PyroE()
    {
        player.SpendMP(20);
        pyroESkill.Arc();
    }

    void HydroE()
    {
        player.SpendMP(15);
        hydroESkill.Wave();
    }

    void SpecialSkill()
    {
        beamSkill.Beam();
    }

    IEnumerator Casting()
    {
        player.moveable = false;
        player.animator.SetBool("MoveAble", player.moveable);
        yield return new WaitForSeconds(0.5f);
        player.moveable = true;
        player.animator.SetBool("MoveAble", player.moveable);
    }

}
