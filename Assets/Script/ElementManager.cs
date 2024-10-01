using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
    private SkillManager skillManager;
    public Boolean skill_Q = true;
    public Boolean skill_E = true;

    public string skillText_1;
    public string skillText_2;

    // Pyro, Hydro, Anemo, Geo
    public List<String> Element = new List<String> { "Pyro", "Hydro", "Anemo", "Geo" };
    public List<float> QSkillDelay = new List<float> { 2f, 3f, 2f, 3f };
    public List<float> ESkillDelay = new List<float> { 7f, 8f, 8f, 7f };
    private int currentElement = 0;

    void Start() { skillManager = GetComponent<SkillManager>(); }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))      // Q스킬사용
        {
            UseQSkill();
        }

        if (Input.GetKeyUp(KeyCode.E))      // E스킬사용
        {
            UseESkill();
        }

        if (Input.GetKeyUp(KeyCode.Tab))    // 원소바꾸기
        {
            ChangeElement();
        }

    }

    void ChangeElement()
    {
        currentElement = (currentElement + 1) % Element.Count;
        Debug.Log("현재 원소: " + Element[currentElement]);
    }

    private void UseQSkill()
    {
        float delay = QSkillDelay[currentElement];

        if (skill_Q)
        {
            Debug.Log("Q스킬 사용 중, currentElement: " + currentElement);
            skillManager.QSkill(currentElement);
            StartCoroutine(QSkillDelayCoroutine(delay));
        }
        else
        {
            Debug.Log("Q스킬 사용 불가, 쿨타임 중");
        }
    }
    private IEnumerator QSkillDelayCoroutine(float delay)
    {
        skill_Q = false;
        Debug.Log("Q스킬 쿨타임 중");
        yield return new WaitForSeconds(delay);
        Debug.Log("Q스킬 사용가능");
        skill_Q = true;
    }

    private void UseESkill()
    {
        float delay = ESkillDelay[currentElement];
        if (skill_E)
        {
            skillManager.ESkill(currentElement);
            StartCoroutine(SkillEDelayCoroutine(7f));
        }
        else
        {
            Debug.Log("E스킬 사용 불가, 쿨타임 중");
        }
    }
    private IEnumerator SkillEDelayCoroutine(float delay)
    {
        skill_E = false;
        Debug.Log("E스킬 쿨타임 중!!");
        yield return new WaitForSeconds(delay);
        Debug.Log("E스킬 사용가능");
        skill_E = true;
    }


}