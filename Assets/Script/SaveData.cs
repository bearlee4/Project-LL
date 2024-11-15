using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    //인벤토리 정보
    public List<string> Inventory = new List<string>();
    public List<int> Inventory_CountList = new List<int>();

    //창고 정보
    public List<string> Storage = new List<string>();
    public List<int> Storage_CountList = new List<int>();

    //돈
    public int gold;

    //의뢰
    //public List<object> available_Requst = new List<object>();
    public List<string> today_request = new List<string>();
    public List<int> position_Number = new List<int>();
    public List<int> image_Number = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
