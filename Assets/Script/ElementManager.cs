using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementManager : MonoBehaviour
{
    public Boolean skill_Q = true;
    public Boolean skill_E = true;

    public string skillText_1;
    public string skillText_2;

    // Pyro, Hydro, Anemo, Geo
    public List<String> Element = new List<String> { "Pyro", "Hydro", "Anemo", "Geo" };
    private int currentElement = 0;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))      // Q스킬사용
        {
            Use1Skill();
        }

        if (Input.GetKeyUp(KeyCode.E))      // E스킬사용
        {
            Use2Skill();
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

    private void Use1Skill()
    {
        if (skill_Q)
        {
            StartCoroutine(SkillQDelayCoroutine(2f));
        }
    }
    private IEnumerator SkillQDelayCoroutine(float delay)
    {
        skill_Q = false;
        Debug.Log("스킬1 사용!!");
        yield return new WaitForSeconds(delay);
        Debug.Log("스킬1 사용가능");
        skill_Q = true;
    }

    private void Use2Skill()
    {
        if (skill_E)
        {
            StartCoroutine(SkillEDelayCoroutine(7f));
        }
    }
    private IEnumerator SkillEDelayCoroutine(float delay)
    {
        skill_E = false;
        Debug.Log("스킬2 사용!!");
        yield return new WaitForSeconds(delay);
        Debug.Log("스킬2 사용가능");
        skill_E = true;
    }

    

}
