using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PyroESkill : MonoBehaviour
{
    private Player player;
    private ElementManager elementManager;

    public GameObject arcPrefab;

    public float coolTime;

    private IEnumerator ArcCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
        elementManager = GetComponent<ElementManager>();
        coolTime = elementManager.ESkillDelay[0];
    }
    public void Arc()
    {
        ArcCoroutine = BeamAiming(0.4f);
        StartCoroutine(ArcCoroutine);
    }

    private IEnumerator BeamAiming(float delay)
    {
        player.currentSpeed = 0f;

        elementManager.skill_E = false;

        arcPrefab.SetActive(true);

        ArcManager arcManager = arcPrefab.GetComponent<ArcManager>();
        if (arcManager != null)
        {
            arcManager.isActive = true;
            yield return new WaitForSeconds(0.01f);
            arcManager.isActive = false;
        }

        yield return new WaitForSeconds(delay);
        arcPrefab.SetActive(false);

        
        player.currentSpeed = player.movePower;
        elementManager.StartCoroutine(elementManager.SkillEDelayCoroutine(coolTime));
    }
}
