using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatheringSystem : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
        {
            Debug.Log("트리거 영역 진입!");
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
        {
            Debug.Log("트리거 영역 나감!");
        }
    }
}
