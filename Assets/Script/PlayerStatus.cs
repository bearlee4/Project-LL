using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    public float maxMP;
    public float currentMP;
    public float atk = 3f;
    public float def;
    public float crt_R;
    public float crt_H;

    Player player;
    InteractionSystem InteractionSystem;

    void Awake()
    {
        maxHP = 20;
        currentHP = maxHP;
        maxMP = 100;
        currentMP = maxMP;
    }

    void Start()
    {
        player = GetComponent<Player>();
        InteractionSystem = this.GetComponent<InteractionSystem>();
    }

    public void Die()
    {
        // 애니메이션 재생, 집으로 복귀, 체력 풀로 채우기 등등
        player.animator.SetTrigger("Dead");
        StartCoroutine(Pause());
        Debug.Log("님 뒤졋으니까 리겜하셈");
        Debug.Log("대체 이걸 어캐 죽음?");

    }

    // 포션에 들어갈 단순 회복효과
    public void HealHP(int num)                     // 수치(정수)
    {
        if ((currentHP + num) > maxHP) currentHP = maxHP;
        else currentHP = currentHP + num;

        player.HP_bar.GetComponent<Slider>().value = currentHP;
    }

    public void HealMP(int num)
    {
        if ((currentMP + num) > maxMP) currentMP = maxMP;
        else currentMP = currentMP + num;

        player.MP_bar.GetComponent<Slider>().value = currentMP;
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

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(2.5f);
        InteractionSystem.Open_Death_UI();
    }
}
