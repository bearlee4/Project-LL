using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSystem : MonoBehaviour
{
    public GameObject Player;
    public GameObject spawner;
    public GameObject Monster;
    public GameObject Test_spawner;
    InteractionSystem InteractionSystem;

    public List<string> common_ForageList = new List<string>();
    public List<string> rare_ForageList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        InteractionSystem = Player.GetComponent<InteractionSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //채집물 등급별로 리스트 정리
    public void Forage_Classification()
    {
        for (int i = 0; i < InteractionSystem.ItemDB.Count; i++)
        {
            if (InteractionSystem.ItemDB[i]["Tag"].ToString() == "Forage")
            {
                if (InteractionSystem.ItemDB[i]["Tier"].ToString() == "Common")
                {
                    common_ForageList.Add(InteractionSystem.ItemDB[i]["ImgName"].ToString());
                }

                else if (InteractionSystem.ItemDB[i]["Tier"].ToString() == "Rare")
                {
                    rare_ForageList.Add(InteractionSystem.ItemDB[i]["ImgName"].ToString());
                }
            }
        }

        Debug.Log("common_ForageList.Count : " + common_ForageList.Count);
        Debug.Log("rare_ForageList.Count : " + rare_ForageList.Count);
    }

    public void Respawn_spawner()
    {
        for (int i = 0; i < spawner.transform.childCount; i++)
        {
            if(spawner.transform.GetChild(i).gameObject.activeSelf == false)
            {
                spawner.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        for (int i = 0; i < Monster.transform.childCount; i++)
        {
            if (Monster.transform.GetChild(i).gameObject.activeSelf == false)
            {
                Monster.transform.GetChild(i).gameObject.SetActive(true);
                Monster.transform.GetChild(i).gameObject.GetComponent<EnemyMovement>().StartCoroutine(Monster.transform.GetChild(i).gameObject.GetComponent<EnemyMovement>().NormalMove());
            }
        }
    }

    public void Respawn_Test_spawner()
    {
        for (int i = 0; i < Test_spawner.transform.childCount; i++)
        {
            if (Test_spawner.transform.GetChild(i).gameObject.activeSelf == false)
            {
                Test_spawner.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
