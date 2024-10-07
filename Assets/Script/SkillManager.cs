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
    public bool isBeamCoroutineRunning = false;
    public bool isBeamStopCoroutine = false;

    private IEnumerator BeamCoroutine;

    Vector3 beamPos;
    Quaternion beamRot;

    void Start() 
    { 
        elementManager = GetComponent<ElementManager>(); 
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V) && !isBeamCoroutineRunning)
        {
            PyroE();
        }

        else if (Input.GetKeyUp(KeyCode.V) && !isBeamStopCoroutine)
        {
            StopCoroutine(BeamCoroutine);
            StartCoroutine(BeamStop(6f));
        }

        else if (Input.GetKeyDown(KeyCode.V) && isBeamCoroutineRunning)
        {
            Debug.Log("빔 쿨타임 중");
        }
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
        BeamCoroutine = BeamAiming(0.75f);
        StartCoroutine(BeamCoroutine);
    }

    private IEnumerator BeamAiming(float delay)
    {
        isBeamCoroutineRunning = true;
        beamShadowActive = true;
        beamShadowPrefab.SetActive(true);

        ShadowBeam shadowBeam = beamShadowPrefab.GetComponent<ShadowBeam>();
        if (shadowBeam != null)
        {
            shadowBeam.isActive = true;
        }

        yield return new WaitForSeconds(delay);

        if (shadowBeam != null)
        {
            shadowBeam.isActive = false;
        }
        beamShadowActive = false;
        beamShadowPrefab.SetActive(false);

        beamPos = beamShadowPrefab.transform.position;
        beamRot = beamShadowPrefab.transform.rotation;
        beamActive = true;
        beamPrefab.SetActive(true);

        beamPrefab.transform.position = beamPos;
        beamPrefab.transform.rotation = beamRot;

        yield return new WaitForSeconds(1.5f);

        yield return BeamStop(6f);
    }

    private IEnumerator BeamStop(float coolTime)
    {
        isBeamStopCoroutine = true;

        beamShadowActive = false;
        beamShadowPrefab.SetActive(false);
        beamActive = false;
        beamPrefab.SetActive(false);
        yield return new WaitForSeconds(coolTime);
        Debug.Log("빔 사용가능");

        isBeamCoroutineRunning = false;
        isBeamStopCoroutine = false;
    }
}
