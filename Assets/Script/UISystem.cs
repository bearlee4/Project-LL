using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UISystem : MonoBehaviour
{

    public GameObject Inventory;

    // Start is called before the first frame update
    void Start()
    {
        Inventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckButton();
    }

    public void CheckButton()
    {
        //I��ư
        if (Input.GetButtonDown("Inventory"))
        {
            if (Inventory.activeSelf == false)
            {
                Debug.Log("����");
                Inventory.SetActive(true);
            }

            else
            {
                Debug.Log("����");
                Inventory.SetActive(false);
            }

        }
    }
}