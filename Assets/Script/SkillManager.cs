using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public void QSkill(int Element)
    {
        switch (Element)
        {
            case 0:
                PyroQSkill(); break;
            case 1:
                HydroQSkill(); break;
            case 2:
                AnemoQSkill(); break;
            case 3:
                GeoQSkill(); break;
        }
    }

    public void ESkill(int Element)
    {
        switch (Element)
        {
            case 0:
                PyroESkill(); break;
            case 1:
                HydroESkill(); break;
            case 2:
                AnemoESkill(); break;
            case 3:
                GeoESkill(); break;
        }

    }

    public void PyroQSkill()
    {
        Debug.Log("Pyro Q Skill Atcivity");
    }

    public void PyroESkill()
    {
        Debug.Log("Pyro E Skill Atcivity");
    }

    public void HydroQSkill()
    {
        Debug.Log("Hydro Q Skill Atcivity");
    }

    public void HydroESkill()
    {
        Debug.Log("Hydro E Skill Atcivity");
    }

    public void AnemoQSkill()
    {
        Debug.Log("Anemo Q Skill Atcivity");
    }

    public void AnemoESkill()
    {
        Debug.Log("Anemo E Skill Atcivity");
    }

    public void GeoQSkill()
    {
        Debug.Log("Geo Q Skill Atcivity");
    }

    public void GeoESkill()
    {
        Debug.Log("Geo E Skill Atcivity");
    }
}
