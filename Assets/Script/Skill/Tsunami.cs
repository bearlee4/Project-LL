using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsunami : MonoBehaviour
{
    public Transform player;

    public float increase = 0.1f;
    public bool isActive = false;
    public float damage;

    private void Start()
    {

    }

    private void Update()
    {
        Wave();
    }

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
        transform.position = player.position;
        transform.localScale = new Vector3(2, 2, 2);

        while (transform.localScale.x < 10f)
        {
            transform.localScale += new Vector3(increase * Time.deltaTime * 50, increase * Time.deltaTime * 50, 0);
            yield return null;
        }

        isActive = false;
        gameObject.SetActive(false);
    }

}
