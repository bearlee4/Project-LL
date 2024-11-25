using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElementManager : MonoBehaviour
{
    private SkillManager skillManager;
    private PlayerStatus playerStatus;

    public GameObject ElementSlot;
    private ElementChange change;
    public Image QCoolTime;
    public Image ECoolTime;



    public Boolean skill_Q = true;      //스킬사용가능
    public Boolean skill_E = true;

    public string skillText_1;
    public string skillText_2;



    // Pyro, Hydro, Anemo, Geo
    public List<String> Element = new List<String> { "Pyro", "Hydro"};
    public List<float> QSkillDelay = new List<float> { 2, 3};
    public List<float> ESkillDelay = new List<float> { 7, 5};


    public int currentElement = 0;

    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        skillManager = GetComponent<SkillManager>();
        change = ElementSlot.GetComponent<ElementChange>();

        QCoolTime.fillAmount = 0f;
        ECoolTime.fillAmount = 0f;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))      // Q스킬사용
        {
            UseQSkill();
        }

        if (Input.GetKeyDown(KeyCode.E))      // E스킬사용
        {
            UseESkill();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))    // 원소바꾸기
        {
            if (skill_Q && skill_E)       // 스킬 쿨타임 중 못 바꿈
                ChangeElement();
            else
                Debug.Log("스킬 쿨이 돌아가고 있어 바꾸지 못함");
        }

    }

    void ChangeElement()
    {
        currentElement = (currentElement + 1) % Element.Count;
        Debug.Log("현재 원소: " + Element[currentElement]);
        change.ChangeEImage(currentElement);
    }

    private void UseQSkill()
    {
        float delay = QSkillDelay[currentElement];

        if (playerStatus.currentMP >= 10 && skill_Q)
        {
            Debug.Log("Q스킬 사용 중, currentElement: " + currentElement);
            skillManager.QSkill(currentElement);
            StartCoroutine(QSkillDelayCoroutine(delay));

        }
        else
        {
            Debug.Log("Q스킬 사용 불가, 쿨타임 중, 혹은 마나부족");
        }
    }
    private IEnumerator QSkillDelayCoroutine(float delay)
    {
        skill_Q = false;
        Debug.Log("Q스킬 쿨타임 중");

        float elapsedTime = 0f;
        QCoolTime.fillAmount = 1f;

        while (elapsedTime < delay)     // 원 줄이기
        {
            elapsedTime += Time.deltaTime;
            QCoolTime.fillAmount = 1f - (elapsedTime / delay);
            yield return null;
        }
        Debug.Log("Q스킬 사용가능");
        skill_Q = true;
    }

    private void UseESkill()
    {
        float coolTime = ESkillDelay[currentElement];
        if (currentElement == 0 && playerStatus.currentMP >= 25 && skill_E)
        {
            skillManager.ESkill(currentElement);
        }

        else if (currentElement == 1 && playerStatus.currentMP >= 15 && skill_E)
        {
            skillManager.ESkill(currentElement);
        }

        else
        {
            Debug.Log("E스킬 사용 불가, 쿨타임 중");
        }
    }
    internal IEnumerator SkillEDelayCoroutine(float delay)
    {
        Debug.Log("E스킬 쿨타임 중!!");

        float elapsedTime = 0f;
        ECoolTime.fillAmount = 1f;

        while (elapsedTime < delay)     // 원 줄이기
        {
            elapsedTime += Time.deltaTime;
            ECoolTime.fillAmount = 1f - (elapsedTime / delay);
            yield return null;
        }

        Debug.Log("E스킬 사용가능");
        skill_E = true;
    }


}