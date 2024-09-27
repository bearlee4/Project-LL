using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StorageSystem : MonoBehaviour
{

    public GameObject storageUI;
    public GameObject Slot_Select;
    private GameObject ChooseItem;
    public Text Content;
    private int Positioncount;

    InventorySystem InventorySystem;

    //인벤토리측 슬롯
    public List<GameObject> InventorySlot = new List<GameObject>();
    public List<GameObject> InventoryImageSlot = new List<GameObject>();
    public List<Text> InventoryNumberList = new List<Text>();

    //창고측 슬롯
    public List<GameObject> StorageSlot = new List<GameObject>();
    public List<GameObject> StorageImageSlot = new List<GameObject>();
    public List<Text> StorageNumberList = new List<Text>();

    //창고 아이템 관련
    public List<string> StorageList = new List<string>();
    private List<int> StorageCountList = new List<int>();

    private int maxcount;

    // Start is called before the first frame update
    void Start()
    {
        Slot_Select.SetActive(false);
        Positioncount = 0;
        ChooseItem = GameObject.Find("StorageChooseItem");
        storageUI.SetActive(false);
        InventorySystem = this.GetComponent<InventorySystem>();

        maxcount = 99;

        //창고가 비었을때 갯수 텍스트 감추기
        for (int i = 0; i < StorageNumberList.Count; i++)
        {
            if (StorageList.Any() == false)
            {
                StorageNumberList[i].gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Slot_Select.activeSelf == true)
        {
            Select_Move();
        }
    }

    //창고 UI 열기
    public void OpenStorage()
    {
        Debug.Log("Storage open");
        LinkInventory();
        storageUI.SetActive(true);
        Positioncount = 0;
        Slot_Select.transform.position = InventorySlot[Positioncount].transform.position;

        //슬롯 선택 이미지 표기
        if (InventorySystem.InventoryList.Any() == false)
        {
            Slot_Select.SetActive(false);
            Reset_Information();
        }

        else
        {
            Slot_Select.SetActive(true);
            Load_Information();
        }

        //인벤토리 사이즈에 맞게 인벤토리 슬롯 생성
        for (int num = InventorySystem.SetSize; InventorySlot.Count > num; num++)
        {
            InventorySlot[num].SetActive(false);
        }
    }

    public void CloseStorage()
    {
        Debug.Log("Storage close");
        storageUI.SetActive(false);
    }

    public void LinkInventory()
    {
        Slot_Reset();
        if (InventorySystem.InventoryList.Any() == true)
        {
            for (int i = 0; i < InventorySystem.InventoryList.Count; i++)
            {
                //이미지 연동
                Image Image = InventoryImageSlot[i].GetComponent<Image>();
                Image InventoryImage = InventorySystem.ImageSlot[i].GetComponent<Image>();
                if (Image.enabled == false)
                {
                    Image.sprite = InventoryImage.sprite;
                    Image.enabled = true;
                }

                //텍스트 연동
                InventoryNumberList[i].text = InventorySystem.NumberList[i].text;


            }
        }
    }

    public void Slot_Reset()
    {
        for (int i = 0; i < InventorySystem.SetSize; i++)
        {
            Image Image = InventoryImageSlot[i].GetComponent<Image>();
            Image.sprite = null;
            Image.enabled = false;
            InventoryNumberList[i].text = null;
        }
    }

    //슬롯 선택 오브젝트 움직이기
    public void Select_Move()
    {
        if (storageUI.activeSelf == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log(Positioncount);
            }

            if (Input.GetButtonDown("Left"))
            {
                if (Positioncount != 0)
                {
                    Positioncount--;
                    Slot_Select.transform.position = InventorySlot[Positioncount].transform.position;
                }
            }

            else if (Input.GetButtonDown("Right"))
            {
                if (Positioncount < InventorySystem.SetSize && InventoryImageSlot[Positioncount + 1].GetComponent<Image>().enabled == true)
                {
                    Positioncount++;
                    Slot_Select.transform.position = InventorySlot[Positioncount].transform.position;
                }
            }

            else if (Input.GetButtonDown("Up"))
            {
                if ((Positioncount - 3) >= 0)
                {
                    Positioncount -= 3;
                    Slot_Select.transform.position = InventorySlot[Positioncount].transform.position;
                }
            }

            else if (Input.GetButtonDown("Down"))
            {
                if ((Positioncount + 3) <= InventorySystem.SetSize && InventoryImageSlot[Positioncount + 3].GetComponent<Image>().enabled == true)
                {
                    Positioncount += 3;
                    Slot_Select.transform.position = InventorySlot[Positioncount].transform.position;
                }
            }

            Load_Information();
        }
    }

    public void Load_Information()
    {
        Image ChooseImage = ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>();

        

        for (int i = 0; i < InventorySlot.Count; i++)
        {

            if (Slot_Select.transform.position == InventorySlot[i].transform.position)
            {

                if (ChooseImage.enabled == false)
                {
                    ChooseImage.enabled = true;
                }

                ChooseImage.sprite = InventoryImageSlot[i].GetComponent<Image>().sprite;

                for (int j = 0; j < InventorySystem.ItemDB.Count; j++)
                {
                    if (ChooseImage.sprite.name.ToString() == InventorySystem.ItemDB[j]["ImgName"].ToString())
                    {
                        Content.text = InventorySystem.ItemDB[j]["Content"].ToString();

                        break;
                    }
                }

                break;
            }
        }
    }

    public void Reset_Information()
    {
        Image ChooseImage = ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>();
        ChooseImage.enabled = false;
        Content.text = null;
    }

    public void Back_Home()
    {

        for (int i = 0; i < InventorySystem.InventoryList.Count; i++)
        {
            //인벤토리에 같은 종류의 아이템이 없을때
            if (!StorageList.Contains(InventorySystem.InventoryList[i]))
            {
                AddStorage(InventorySystem.InventoryList[i], InventorySystem.CountList[i]);
            }

            //인벤토리에 같은 종류의 아이템이 있을때
            else if (StorageList.Contains(InventorySystem.InventoryList[i]))
            {
                Debug.Log("아이템 발견");
                for (int n = 0; n < StorageList.Count; n++)
                {
                    if (StorageList[n] == InventorySystem.InventoryList[i])
                    {
                        // 아이템 습득 후 갯수가 99이하일때
                        if ((StorageCountList[n] + InventorySystem.CountList[i]) < maxcount)
                        {
                            StorageCountList[n] += InventorySystem.CountList[i];
                            Debug.Log(StorageCountList[n]);
                            Debug.Log("같은 아이템을 가지고 있습니다.");
                        }

                        //// 아이템 습득 후 갯수가 99이상일 때
                        else if ((StorageCountList[n] + InventorySystem.CountList[i]) >= maxcount)
                        {
                            Debug.Log("99개가 넘는다!!");
                            StorageCountList[n] += InventorySystem.CountList[i];
                            int nextcount = StorageCountList[n] - maxcount;
                            StorageCountList[n] = maxcount;
                            AddStorage(StorageList[n], nextcount);
                        }
                        StorageNumberList[n].text = StorageCountList[n].ToString();

                        break;
                    }
                }

            }
        }

        // 이미지 생성
        for (int num = 0; num < StorageList.Count; num++)
        {
            //이미지 연동
            Image Image = StorageImageSlot[num].GetComponent<Image>();
            if (Image.enabled == false)
            {
                Image.sprite = Resources.Load<Sprite>("Image/" + StorageList[num]);
                Image.enabled = true;
            }

        }

        int InventoryCount = InventorySystem.InventoryList.Count;

        for (int i = 0 ; i < InventoryCount ; i++)
        {
            InventorySystem.DeleteItem(InventorySystem.InventoryList[0]);
        }

        Debug.Log("모든 아이템이 창고로 옮겨졌습니다.");
    }

    //아이템 추가
    public void AddStorage(string name, int count)
    {
        StorageList.Add(name);
        StorageCountList.Add(count);
        Debug.Log("Add Storage");

        //갯수 텍스트 키기
        for (int n = 0; n < StorageList.Count; n++)
        {
            if (StorageNumberList[n].gameObject.activeSelf == false)
            {
                StorageNumberList[n].gameObject.SetActive(true);
                StorageNumberList[n].text = StorageCountList[n].ToString();
                break;
            }
        }
    }
}
