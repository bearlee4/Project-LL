using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PyroESkill : MonoBehaviour
{
    private Player player;
    private ElementManager elementManager;

    public GameObject beamPrefab;
    public GameObject beamShadowPrefab;

    public bool beamShadowActive = false;
    public bool beamActive = false;
    public bool isBeamCoroutineRunning = false;
    public bool isBeamStopCoroutine = false;
    public float coolTime;

    private IEnumerator BeamCoroutine;

    Vector3 beamPos;
    Quaternion beamRot;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();      
        elementManager = GetComponent<ElementManager>();
    }

    // Update is called once per frame
    void Update()
    {
        coolTime = elementManager.ESkillDelay[elementManager.currentElement];

        if (Input.GetKeyUp(KeyCode.E) && !isBeamStopCoroutine)
        {
            StopCoroutine(BeamCoroutine);
            StartCoroutine(BeamStop(coolTime));
            elementManager.StartCoroutine(elementManager.SkillEDelayCoroutine(coolTime));
        }
    }

    public void Beam()
    {
        BeamCoroutine = BeamAiming(0.75f);
        StartCoroutine(BeamCoroutine);
    }

    private IEnumerator BeamAiming(float delay)
    {
        player.MovePower = 1.5f;

        elementManager.skill_E = false;

        isBeamCoroutineRunning = true;
        beamShadowActive = true;
        beamShadowPrefab.SetActive(true);

        ShadowBeam shadowBeam = beamShadowPrefab.GetComponent<ShadowBeam>();
        if (shadowBeam != null)
        {
            shadowBeam.isActive = true;
        }

        yield return new WaitForSeconds(delay);

        player.MovePower = 0f;

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
        Debug.Log("빔 사용 끝");

        elementManager.StartCoroutine(elementManager.SkillEDelayCoroutine(coolTime));
        yield return BeamStop(coolTime);
    }

    private IEnumerator BeamStop(float coolTime)
    {
        StartCoroutine(SkillMoveDelay(0.5f));

        isBeamStopCoroutine = true;

        beamShadowActive = false;
        beamShadowPrefab.SetActive(false);
        beamActive = false;
        beamPrefab.SetActive(false);
        yield return new WaitForSeconds(coolTime);

        isBeamCoroutineRunning = false;
        isBeamStopCoroutine = false;
    }

    private IEnumerator SkillMoveDelay(float delay)
    {
        player.MovePower = 1.5f;
        yield return new WaitForSeconds(delay);
        player.MovePower = 5f;
    }

}
