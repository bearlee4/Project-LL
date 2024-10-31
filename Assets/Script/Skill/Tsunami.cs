using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsunami : MonoBehaviour
{
    public Transform player;

    public float maxRadius;
    public float currentRadius;
    public float duration;
    public bool isActive = false;

    // Update is called once per frame
    void Wave()
    {
        if (isActive)
        {
            StartCoroutine("Expand");
        }
    }

    IEnumerator Expand()
    {
        yield return new WaitForSeconds(duration);
    }

}
