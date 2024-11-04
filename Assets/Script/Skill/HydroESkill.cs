using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydroESkill : MonoBehaviour
{
    private Player player;
    private ElementManager elementManager;

    public GameObject wavePrefab;

    public float coolTime;

    private IEnumerator WaveCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        elementManager = GetComponent<ElementManager>();
        coolTime = elementManager.ESkillDelay[1];
    }

    public void Wave()
    {
        WaveCoroutine = WaveAct();
        StartCoroutine(WaveCoroutine);
    }

    private IEnumerator WaveAct()
    {
        player.MovePower = 0f;
        elementManager.skill_E = false;

        wavePrefab.SetActive(true);

        Tsunami tsunami = wavePrefab.GetComponent<Tsunami>();
        if (tsunami != null)
        {
            tsunami.isActive = true;
        }
        yield return new WaitForSeconds(1f);

        player.MovePower = 5f;
        elementManager.StartCoroutine(elementManager.SkillEDelayCoroutine(coolTime));
    }
}
