using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public int maxHP;
    public int CurrentHP;
    public int maxMP;
    public int CurrentMP;
    public float atk;
    public float speed;
    public float def;
    public int crt_R;
    public int crt_H;

    void Awake()
    {
        maxHP = 20;
        CurrentHP = maxHP;
        maxMP = 100;
        CurrentMP = maxMP;
    }

    void Start()
    {
        
    }

    void Die()
    {
        // 애니메이션 재생, 집으로 복귀, 체력 풀로 채우기 등등
    }

    // 포션에 들어갈 단순 회복효과
    public void HealHP(int num)                     // 수치(정수)
    {
        if ((CurrentHP + num) > maxHP) CurrentHP = maxHP;
        else CurrentHP = CurrentHP + num;
    }

    public void HealMP(int num)
    {
        if ((CurrentMP + num) > maxMP) CurrentMP = maxMP;
        else CurrentMP = CurrentMP + num;
    }

    public void Buff(int type, float duration, float num)    // 종류, 지속시간, 수치(퍼센트)
    {
        StartCoroutine(BuffCoroutine(type, duration, num));
    }

    IEnumerator BuffCoroutine(int type, float duration, float num)
    {
        switch (type)
        {
            case 0:
                atk *= num;
                Debug.Log("공격력 증가, 현재 공격력 : " + atk);

                yield return new WaitForSeconds(duration);

                atk /= num;
                Debug.Log("버프 끝, 현재 공격력 : " + atk);
                break;
            case 1:
                def *= num;
                Debug.Log("방어력 증가, 현재 방어력 : " + def);

                yield return new WaitForSeconds(duration);

                def /= num;
                Debug.Log("버프 끝, 현재 방어력 : " + def);
                break;
            case 2:
                speed *= num;
                Debug.Log("속도 증가, 현재 속도 : " + speed);

                yield return new WaitForSeconds(duration);

                speed /= num;
                Debug.Log("버프 끝, 현재 속도 : " + speed);
                break;
        }
    }
}
