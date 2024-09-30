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
    //private GameObject ChooseItem;
    //public Text Content;
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
    private bool storageside;

    private int storagearray;

    // Start is called before the first frame update
    void Start()
    {
        Slot_Select.SetActive(false);
        Positioncount = 0;
        //ChooseItem = GameObject.Find("StorageChooseItem");
        storageUI.SetActive(false);
        InventorySystem = this.GetComponent<InventorySystem>();

        maxcount = 99;

        storagearray = 4;

        storageside = false;

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
        if (InventorySystem.InventoryList.Any() == false && StorageList.Any() == false)
        {
            Slot_Select.SetActive(false);
            storageside = false;
            //Reset_Information();
        }

        else
        {
            Slot_Select.SetActive(true);

            if (InventorySystem.InventoryList.Any() == true)
            {
                Slot_Select.transform.position = InventorySlot[0].transform.position;
                Positioncount = 0;
            }

            else if (InventorySystem.InventoryList.Any() == false && StorageList.Any() == true)
            {
                Slot_Select.transform.position = StorageSlot[0].transform.position;
                Positioncount = 0;
            }

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

    public void ResetPosition()
    {
        Positioncount = 0;
        SetPosition();
    }

    public void SetPosition()
    {
        if (storageside == false)
        {
            Slot_Select.transform.position = InventorySlot[Positioncount].transform.position;
        }

        else
        {
            Slot_Select.transform.position = StorageSlot[Positioncount].transform.position;
            
        }
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
        for (int i = 0; i < StorageSlot.Count; i++)
        {
            if (Slot_Select.transform.position == InventorySlot[i].transform.position)
            {
                storageside = false;
                break;
            }

            else if (Slot_Select.transform.position == StorageSlot[i].transform.position)
            {
                storageside = true;
                break;
            }
        }

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
                    SetPosition();
                }
            }

            else if (Input.GetButtonDown("Right"))
            {
                if (Positioncount < InventorySystem.SetSize && InventoryImageSlot[Positioncount + 1].GetComponent<Image>().enabled == true && storageside == false)
                {
                    Positioncount++;
                    Slot_Select.transform.position = InventorySlot[Positioncount].transform.position;
                }

                else if (StorageImageSlot[Positioncount + 1].GetComponent<Image>().enabled == true && storageside == true)
                {
                    Positioncount++;
                    Slot_Select.transform.position = StorageSlot[Positioncount].transform.position;
                }
            }

            else if (Input.GetButtonDown("Up"))
            {
                if ((Positioncount - storagearray) >= 0)
                {
                    Positioncount -= storagearray;
                    SetPosition();
                }
            }

            else if (Input.GetButtonDown("Down"))
            {
                if ((Positioncount + storagearray) < InventorySystem.SetSize && InventoryImageSlot[Positioncount + storagearray].GetComponent<Image>().enabled == true && storageside == false)
                {
                    Positioncount += storagearray;
                    Slot_Select.transform.position = InventorySlot[Positioncount].transform.position;
                }

                else if ((Positioncount + storagearray) < StorageSlot.Count && StorageImageSlot[Positioncount + storagearray].GetComponent<Image>().enabled == true & storageside == true)
                {
                    Positioncount += storagearray;
                    Slot_Select.transform.position = StorageSlot[Positioncount].transform.position;
                }
            }

            Load_Information();
        }
    }

    public void Load_Information()
    {
        //Image ChooseImage = ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>();
        

        for (int i = 0; i < InventorySlot.Count; i++)
        {

            if (Slot_Select.transform.position == InventorySlot[i].transform.position || Slot_Select.transform.position == StorageSlot[i].transform.position)
            {
                Positioncount = i;

                //if (ChooseImage.enabled == false)
                //{
                //    ChooseImage.enabled = true;
                //}

                //if (Slot_Select.transform.position == InventorySlot[i].transform.position)
                //{
                //    ChooseImage.sprite = InventoryImageSlot[i].GetComponent<Image>().sprite;
                //}
                //else if (Slot_Select.transform.position == StorageSlot[i].transform.position)
                //{
                //    ChooseImage.sprite = StorageImageSlot[i].GetComponent<Image>().sprite;
                //}

                //for (int j = 0; j < InventorySystem.ItemDB.Count; j++)
                //{
                //    if (ChooseImage.sprite.name.ToString() == InventorySystem.ItemDB[j]["ImgName"].ToString())
                //    {
                //        Content.text = InventorySystem.ItemDB[j]["Content"].ToString();

                //        break;
                //    }
                //}

                break;
            }
        }
    }

    //public void Reset_Information()
    //{
    //    Image ChooseImage = ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>();
    //    ChooseImage.enabled = false;
    //    ChooseImage.sprite = null;
    //    Content.text = null;
    //}

    public void Back_Home()
    {
        int InventoryCount = InventorySystem.InventoryList.Count;

        for (int i = 0 ; i < InventoryCount ; i++)
        {
            AddStorage(InventorySystem.InventoryList[0], InventorySystem.CountList[0]);
            InventorySystem.DeleteItem(InventorySystem.InventoryList[0]);
        }

        Debug.Log("모든 아이템이 창고로 옮겨졌습니다.");

        storageside = true;
        //ResetPosition();
    }

    //아이템 추가
    public void AddStorage(string name, int count)
    {
        //인벤토리에 같은 종류의 아이템이 없을때
        if (!StorageList.Contains(name))
        {
            StorageList.Add(name);
            StorageCountList.Add(count);
        }

        //인벤토리에 같은 종류의 아이템이 있을때
        else if (StorageList.Contains(name))
        {
            Debug.Log("아이템 발견");
            for (int n = 0; n < StorageList.Count; n++)
            {
                if (StorageList[n] == name)
                {
                    // 아이템 습득 후 갯수가 99이하일때
                    if ((StorageCountList[n] + count) < maxcount)
                    {
                        StorageCountList[n] += count;
                        Debug.Log(StorageCountList[n]);
                        Debug.Log("같은 아이템을 가지고 있습니다.");
                    }

                    //// 아이템 습득 후 갯수가 99이상일 때
                    else if ((StorageCountList[n] + count) >= maxcount)
                    {
                        Debug.Log("99개가 넘는다!!");
                        StorageCountList[n] += count;
                        int nextcount = StorageCountList[n] - maxcount;
                        StorageCountList[n] = maxcount;
                        
                        StorageList.Add(name);
                        StorageCountList.Add(nextcount);
                    }
                    StorageNumberList[n].text = StorageCountList[n].ToString();

                    break;
                }
            }

        }

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
    }

    public void TransItem()
    {
        int transnumber = 1;

        for (int i = 0; i < InventorySlot.Count; i++)
        {
            //인벤토리 to 창고
            if (storageside == false)
            {
                if (Slot_Select.transform.position == InventorySlot[i].transform.position)
                {
                    AddStorage(InventorySystem.InventoryList[i], transnumber);
                    InventoryUse(i, transnumber);

                    break;
                }
            }

            //창고 to 인벤토리
            else
            {
                if (Slot_Select.transform.position == StorageSlot[i].transform.position)
                {
                    for (int j  = 0; j < InventorySystem.ItemDB.Count; j++)
                    {
                        if (StorageList[i] == InventorySystem.ItemDB[j]["ImgName"].ToString())
                        {
                            InventorySystem.AddInventory(InventorySystem.ItemDB[j]["ImgName"], transnumber);
                            break;
                        }
                    }
                    
                    if(InventorySystem.FullInventory == false)
                    {
                        StorageUse(i, transnumber);
                    }

                    break;
                }
            }
        }


        //토큰 설정밎 포지션 리셋
        if (StorageList.Any() == false)
        {
            storageside = false;
            ResetPosition();
        }

        else if (InventorySystem.InventoryList.Any() == false)
        {
            storageside = true;
            ResetPosition();
        }
    }

    public void InventoryUse(int number, int transnumber)
    {
        InventorySystem.CountList[number] -= transnumber;
        InventorySystem.NumberList[number].text = InventorySystem.CountList[number].ToString();
        if (InventorySystem.CountList[number] == 0)
        {
            InventorySystem.DeleteItem(InventorySystem.InventoryList[number]);
        }

        LinkInventory();

        if (InventoryImageSlot[number].GetComponent<Image>().enabled == false && number != 0)
        {
            Slot_Select.transform.position = InventorySlot[number - 1].transform.position;
        }
    }

    public void StorageUse(int number, int transnumber)
    {
        StorageCountList[number] -= transnumber;
        StorageNumberList[number].text = StorageCountList[number].ToString();
        if(StorageCountList[number] == 0)
        {
            Delete_Storage_Item(StorageList[number]);
        }

        LinkInventory();
    }

    public void Delete_Storage_Item(string name)
    {
        for (int i = 0; i < StorageList.Count; i++)
        {
            if (StorageList[i] == name)
            {
                //아이템 사용으로 인벤토리에 아무것도 안남을때
                if (StorageList.Count == 1)
                {
                    StorageNumberList[i].text = null;
                    StorageNumberList[i].gameObject.SetActive(false);
                    StorageImageSlot[i].GetComponent<Image>().sprite = null;
                    StorageImageSlot[i].GetComponent<Image>().enabled = false;

                    ResetPosition();
                    //Reset_Information();
                }

                else
                {
                    for (int j = i; j < StorageList.Count; j++)
                    {
                        if ((j + 1) < StorageList.Count)
                        {
                            StorageNumberList[j].text = StorageNumberList[j + 1].text;
                            StorageImageSlot[j].GetComponent<Image>().sprite = StorageImageSlot[j + 1].GetComponent<Image>().sprite;
                        }

                        else if ((j + 1) == StorageList.Count)
                        {
                            StorageNumberList[j].gameObject.SetActive(false);
                            StorageNumberList[j].text = null;
                            StorageImageSlot[j].GetComponent<Image>().enabled = false;
                        }


                    }
                }

                StorageList.RemoveAt(i);
                StorageCountList.RemoveAt(i);

                if (StorageImageSlot[i].GetComponent<Image>().enabled == false && i != 0)
                {
                    Slot_Select.transform.position = StorageSlot[i - 1].transform.position;
                }
            }
        }
    }
}
