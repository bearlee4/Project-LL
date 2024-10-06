using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private ElementManager elementManager;

    public ObjectPool objectPool;
    public GameObject beamPrefab;
    public GameObject beamShadowPrefab;

    public List<float> QSkillDamage = new List<float> { 2, 3, 2, 3 };
    public List<float> ESkillDamage = new List<float> { 5, 5, 5, 5 };

    Camera cam;

    public bool beamShadowActive = false;
    public bool beamActive = false;
    Vector3 beamPos;
    Quaternion beamRot;

    void Start() 
    { 
        elementManager = GetComponent<ElementManager>(); 
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            PyroE();
        }

        else
        {
            beamShadowActive = false;
            beamShadowPrefab.SetActive(false);
            beamActive = false;
            beamPrefab.SetActive(false);
        }
    }

    public void QSkill(int Element)
    {
        ShootBullet();
        Debug.Log(elementManager.Element[Element] + " Q Skill Atcivity");
    }

    public void ESkill(int Element)
    {

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
        beamShadowActive = true;
        beamShadowPrefab.SetActive(true);
        StartCoroutine(BeamStopDelay(0.5f));
    }

    private IEnumerator BeamStopDelay(float v)
    {
        yield return new WaitForSeconds(v);
        beamShadowActive = false;
        beamShadowPrefab.SetActive(false);
        beamPos = beamShadowPrefab.transform.position;
        beamRot = beamShadowPrefab.transform.rotation;
        beamActive = true;
        beamPrefab.SetActive(true);

        //beamPrefab.transform.position = beamPos;
        //beamPrefab.transform.rotation = beamRot;

    }
}
