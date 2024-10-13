using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class StorageSystem : MonoBehaviour
{

    public GameObject storageUI;
    //public GameObject Slot_Select;
    //private GameObject ChooseItem;
    //public Text Content;
    //private int Positioncount;

    InventorySystem InventorySystem;
    ItemInformation ItemInformation;

    private GameObject canvas;
    UISystem UISystem;

    private GameObject Player;
    InteractionSystem InteractionSystem;

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
    public List<int> StorageCountList = new List<int>();

    private int maxcount;
    private int nextcount;
    public bool storageside;

    private int pageNumber;
    private int pagecalcul;
    public Text pageText;

    private bool quick_toggle;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("Player");
        InteractionSystem = Player.GetComponent<InteractionSystem>();

        canvas = GameObject.Find("Canvas");
        UISystem = canvas.GetComponent<UISystem>();

        //Slot_Select.SetActive(false);
        //Positioncount = 0;
        //ChooseItem = GameObject.Find("StorageChooseItem");
        InventorySystem = this.GetComponent<InventorySystem>();
        ItemInformation = this.GetComponent<ItemInformation>();

        maxcount = 99;
        pageNumber = 1;
        pagecalcul = 12 * (pageNumber - 1);

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
        //if (Slot_Select.activeSelf == true)
        //{
        //    Select_Move();
        //}
    }

    //창고 UI 열기
    public void OpenStorage()
    {
        Debug.Log("Storage open");
        LinkInventory();
        storageUI.SetActive(true);
        //Positioncount = 0;
        //Slot_Select.transform.position = InventorySlot[Positioncount].transform.position;

        //슬롯 선택 이미지 표기
        //if (InventorySystem.InventoryList.Any() == false && StorageList.Any() == false)
        //{
        //    //Slot_Select.SetActive(false);
        //    storageside = false;
        //    //Reset_Information();
        //}

        //else
        //{
        //    //Slot_Select.SetActive(true);

        //    //if (InventorySystem.InventoryList.Any() == true)
        //    //{
        //    //    Slot_Select.transform.position = InventorySlot[0].transform.position;
        //    //    Positioncount = 0;
        //    //}

        //    //else if (InventorySystem.InventoryList.Any() == false && StorageList.Any() == true)
        //    //{
        //    //    Slot_Select.transform.position = StorageSlot[0].transform.position;
        //    //    Positioncount = 0;
        //    //}

        //}

        //인벤토리 사이즈에 맞게 인벤토리 슬롯 생성
        for (int num = InventorySystem.SetSize; InventorySlot.Count > num; num++)
        {
            InventorySlot[num].SetActive(false);
        }

        Reload_Info();
    }

    public void CloseStorage()
    {
        Debug.Log("Storage close");
        storageUI.SetActive(false);
    }

    //public void ResetPosition()
    //{
    //    Positioncount = 0;
    //    //SetPosition();
    //}

    //public void SetPosition()
    //{
    //    if (storageside == false)
    //    {
    //        Slot_Select.transform.position = InventorySlot[Positioncount].transform.position;
    //    }

    //    else
    //    {
    //        Slot_Select.transform.position = StorageSlot[Positioncount].transform.position;

    //    }
    //}


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
                if (InventoryImageSlot[i].activeSelf == false)
                {
                    InventoryImageSlot[i].SetActive(true);
                }

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

    public void Back_Home()
    {
        int InventoryCount = InventorySystem.InventoryList.Count;

        for (int i = 0; i < InventoryCount; i++)
        {
            AddStorage(InventorySystem.InventoryList[0], InventorySystem.CountList[0]);
            InventorySystem.DeleteItem(InventorySystem.InventoryList[0], 0);
        }

        Debug.Log("모든 아이템이 창고로 옮겨졌습니다.");

        storageside = true;
        //ResetPosition();
    }

    //아이템 추가
    public void AddStorage(string name, int count)
    {
        bool continue_toggle = false;

        //창고에 같은 종류의 아이템이 없을때
        if (!StorageList.Contains(name))
        {
            StorageList.Add(name);
            StorageCountList.Add(count);
        }

        //창고에 같은 종류의 아이템이 있을때
        else if (StorageList.Contains(name))
        {
            Debug.Log("아이템 발견");
            for (int n = 0; n < StorageList.Count; n++)
            {
                if (StorageList[n] == name)
                {
                    // 아이템 습득 후 갯수가 99이하일때
                    if ((StorageCountList[n] + count) <= maxcount && StorageCountList[n] != maxcount)
                    {
                        StorageCountList[n] += count;
                        Debug.Log(StorageCountList[n]);
                        Debug.Log("같은 아이템을 가지고 있습니다.");

                        continue_toggle = false;

                        break;
                    }

                    //// 아이템 습득 후 갯수가 99초과일 때
                    else if ((StorageCountList[n] + count) > maxcount && StorageCountList[n] != maxcount)
                    {
                        Debug.Log("99개가 넘는다!!");
                        StorageCountList[n] += count;
                        nextcount = StorageCountList[n] - maxcount;
                        StorageCountList[n] = maxcount;
                        continue_toggle = true;
                        count = nextcount;

                        continue;
                    }

                    //창고에 있는 아이템이 99개일때 넘겨버리기
                    else if (StorageCountList[n] == maxcount)
                    {
                        Debug.Log("넘겨버리기");
                        continue_toggle = true;
                        continue;
                    }

                }
            }


            //컨티뉴로 넘어가서 같은걸 못찾았을때
            if (continue_toggle == true)
            {
                //새로 하나 추가해버리기
                Debug.Log("안에 같은 종류는 있으나 99개로 꽉차서 새로 하나 만듬");
                StorageList.Add(name);
                StorageCountList.Add(count);
            }

        }

        Debug.Log("Add Storage");

        Reload_Info();

        ////갯수 텍스트 키기
        //for (int n = 0; n < StorageList.Count; n++)
        //{
        //    if (StorageNumberList[n].gameObject.activeSelf == false)
        //    {
        //        StorageNumberList[n].gameObject.SetActive(true);
        //        StorageNumberList[n].text = StorageCountList[n].ToString();
        //        break;
        //    }
        //}

        //// 이미지 생성
        //for (int num = 0; num < StorageList.Count; num++)
        //{
        //    //이미지 연동
        //    Image Image = StorageImageSlot[num].GetComponent<Image>();

        //    if (StorageImageSlot[num].activeSelf == false)
        //    {
        //        StorageImageSlot[num].SetActive(true);
        //    }

        //    if (Image.enabled == false)
        //    {
        //        Image.enabled = true;
        //    }

        //    Image.sprite = Resources.Load<Sprite>("Image/" + StorageList[num]);

        //}
    }

    public void TransItem()
    {
        
        int transnumber = 1;

        storageside = ItemInformation.sidetoken;

        //창고 to 인벤토리
        if (storageside == true)
        {
            for(int i = 0; i < StorageList.Count; i++)
            {
                //선택 위치 찾기
                if (ItemInformation.slot_Select.transform.position.ToString() == StorageSlot[i].transform.position.ToString())
                {
                    //같은거 정보 찾기
                    for (int j = 0; j < InventorySystem.ItemDB.Count; j++)
                    {
                        if (StorageList[i + pagecalcul] == InventorySystem.ItemDB[j]["ImgName"].ToString())
                        {
                            //컨트롤키 눌러져 있을 시
                            if (InteractionSystem.max_Trans_toggle == true)
                            {
                                transnumber = StorageCountList[i + pagecalcul];

                                Debug.Log("StorageList[i + pagecalcul]" + StorageList[i + pagecalcul]);
                                Debug.Log("StorageCountList[i + pagecalcul] : " + StorageCountList[i + pagecalcul]);
                            }

                            InventorySystem.AddInventory(InventorySystem.ItemDB[j]["ImgName"], transnumber);

                            break;
                        }
                    }

                    if (InventorySystem.FullInventory == false || (InventorySystem.FullInventory == true && InventorySystem.fullActive_toggle == true))
                    {
                        StorageUse(i + pagecalcul, transnumber);
                    }

                    break;
                }
            }
        }

        else
        {
            Debug.Log("ItemInformation.slot_Select.transform.position : " + ItemInformation.slot_Select.transform.position.ToString("N10"));
            for (int i = 0; i < InventorySlot.Count; i++)
            {
                
                Debug.Log("InventorySlot[" + i + "].transform.position : " + InventorySlot[i].transform.position.ToString("N10"));


                //인벤토리 to 창고
                if (ItemInformation.slot_Select.transform.position.ToString() == InventorySlot[i].transform.position.ToString())
                {
                    if (InteractionSystem.max_Trans_toggle == true)
                    {
                        transnumber = InventorySystem.CountList[i];
                        Debug.Log("transnumber : " + transnumber);
                    }

                    AddStorage(InventorySystem.InventoryList[i], transnumber);
                    InventoryUse(i, transnumber);

                    break;
                }

            }

        }

        Reload_Info();
        //토큰 설정밎 포지션 리셋
        //if (StorageList.Any() == false)
        //{
        //    storageside = false;
        //    //ResetPosition();
        //}

        //else if (InventorySystem.InventoryList.Any() == false)
        //{
        //    storageside = true;
        //    //ResetPosition();
        //}
    }

    public void InventoryUse(int number, int transnumber)
    {
        quick_toggle = false;

        for (int i = 0; i < InventorySystem.QuickSlotList.Count; i++)
        {
            //퀵슬롯에 지정되어 있는 아이템이 창고에 옮겨졌을 떄
            if (InventorySystem.QuickSlotList[i] == InventorySystem.InventoryList[number] && InventorySystem.QuickSlotNumberList[i].text == InventorySystem.CountList[number].ToString())
            {
                //퀵슬롯 리셋 작동 토글
                quick_toggle = true;
            }
        }

        InventorySystem.CountList[number] -= transnumber;
        InventorySystem.NumberList[number].text = InventorySystem.CountList[number].ToString();

        //변경된 인벤토리 정보를 퀵슬롯에 연동. 퀵슬롯에 없을 경우 아무 변동없이 빠져나갈 것.
        if(quick_toggle == true)
        {
            Debug.Log("퀵슬롯 정보 갱신");
            InventorySystem.Quick_Load_Info();
        }    

        if (InventorySystem.CountList[number] == 0)
        {
            InventorySystem.DeleteItem(InventorySystem.InventoryList[number], number);
            InventoryImageSlot[number].SetActive(false);

        }

        if (storageUI.activeSelf == true)
        {
            LinkInventory();
        }

        if (InventoryImageSlot[number].activeSelf == true && ItemInformation.slot_Select.activeSelf == true)
        {
            ItemInformation.Load_Information(UISystem.overObject);
        }

        

        //if (InventoryImageSlot[number].GetComponent<Image>().enabled == false && number != 0)
        //{
        //    Slot_Select.transform.position = InventorySlot[number - 1].transform.position;
        //}
    }

    public void StorageUse(int number, int transnumber)
    {
        StorageCountList[number] -= transnumber;

        //StorageNumberList[number].text = StorageCountList[number].ToString();
        if (StorageCountList[number] == 0)
        {
            Delete_Storage_Item(StorageList[number], number);
           
        }

        if (storageUI.activeSelf == true)
        {
            LinkInventory();
        }

    }

    public void Delete_Storage_Item(string name, int position)
    {
        for (int i = 0; i < StorageList.Count; i++)
        {
            if (StorageList[position] == name)
            {
                //아이템 사용으로 인벤토리에 아무것도 안남을때
                if (StorageList.Count == 1)
                {
                    StorageNumberList[position].text = null;
                    StorageNumberList[position].gameObject.SetActive(false);
                    StorageImageSlot[position].GetComponent<Image>().sprite = null;
                    StorageImageSlot[position].GetComponent<Image>().enabled = false;
                    StorageImageSlot[position].SetActive(false);
                    UISystem.clicktoggle = false;

                    //ResetPosition();
                    //Reset_Information();
                }

                //그 마지막 칸이 2페이지 이상 첫번째 칸일 때
                //else if (pageNumber >= 2 && position - pagecalcul == 0)
                //{
                //    pageNumber--;
                //    pageText.text = pageNumber.ToString();
                //}


                else
                {
                    Debug.Log("position : " + position);

                    for (int j = position; j < StorageList.Count; j++)
                    {
                        //마지막 칸이 아닐 때 다음 슬롯 정보 땡겨오기
                        if ((j + 1) < StorageList.Count)
                        {
                            if (StorageImageSlot[j - pagecalcul].activeSelf == false)
                            {
                                StorageImageSlot[j - pagecalcul].SetActive(true);
                            }
                            StorageNumberList[j - pagecalcul].text = StorageCountList[(j - pagecalcul) + 1].ToString();
                            StorageImageSlot[j - pagecalcul].GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/" + StorageList[j + 1]);
                        }

                        //삭제되는게 마지막 칸일때
                        else if (position + 1 == StorageList.Count && (j - pagecalcul) + 1 <= 12*pageNumber)
                        {
                            Debug.Log("j + 1 : " + (j + 1));
                            Debug.Log("StorageList.Count : " + StorageList.Count);

                            //그 마지막 칸이 2페이지 이상 첫번째 칸일 때
                            if (pageNumber >= 2 && position - pagecalcul == 0)
                            {
                                Debug.Log("이거 작동?중");
                                pageNumber--;
                                pageText.text = pageNumber.ToString();

                                break;
                            }

                            StorageNumberList[j - pagecalcul].text = null;
                            StorageNumberList[j - pagecalcul].gameObject.SetActive(false);
                            StorageImageSlot[j - pagecalcul].GetComponent<Image>().enabled = false;
                            StorageImageSlot[j - pagecalcul].SetActive(false);
                            break;
                        }

                       
                        if ((j - pagecalcul) + 1 == StorageImageSlot.Count)
                        {
                            Debug.Log("이미지 슬롯이 더 없어 강제 종료");
                            break;
                        }
                    }

                    ItemInformation.Load_Information(UISystem.overObject);
                }


                StorageList.RemoveAt(position);
                StorageCountList.RemoveAt(position);
                

                if (StorageImageSlot[position - pagecalcul].activeSelf == false)
                {
                    ItemInformation.slot_Select.SetActive(false);
                    UISystem.clicktoggle = false;
                }

                pagecalcul = (12 * (pageNumber - 1));

                break;
            }
        }
    }

    public void Reload_Info()
    {
        Debug.Log("스토리지 카운트 : " + StorageList.Count);
        
        int count = 0;

        //스토리지 정보 갱신
        for (int i = 0; i < StorageImageSlot.Count; i++)
        {
            //스토리지 정보가 켜져 있을때만 지우기
            if (StorageNumberList[i].enabled == true)
            {
                StorageImageSlot[i].GetComponent<Image>().sprite = null;
                StorageImageSlot[i].SetActive(false);
                StorageNumberList[i].text = null;
            }
        }

        //페이지에 맞춰 정보 리로드
        for (int i = 0 + pagecalcul; i < StorageList.Count; i++)
        {
            //갯수 텍스트 키기
            if (StorageNumberList[i - pagecalcul].gameObject.activeSelf == false)
            {
                StorageNumberList[i - pagecalcul].gameObject.SetActive(true);
            }

            StorageNumberList[i - pagecalcul].text = StorageCountList[i].ToString();

            //이미지 연동
            Image Image = StorageImageSlot[i - pagecalcul].GetComponent<Image>();

            if (StorageImageSlot[i - pagecalcul].activeSelf == false)
            {
                StorageImageSlot[i - pagecalcul].SetActive(true);
            }

            if (Image.enabled == false)
            {
                Image.enabled = true;
            }

            Image.sprite = Resources.Load<Sprite>("Image/" + StorageList[i]);

            count ++;

            if(count == 12)
            {
                break;
            }
        }

    }

    public void Next_Page()
    {
        if (StorageList.Count > 12 + pagecalcul)
        {
            pageNumber++;
            pagecalcul = (12 * (pageNumber - 1));
            pageText.text = pageNumber.ToString();
            Reload_Info();
        }
    }

    public void Back_Page()
    {
        if (pageNumber != 1)
        {
            pageNumber--;
            pagecalcul = (12 * (pageNumber - 1));
            pageText.text = pageNumber.ToString();
            Reload_Info();
        }
    }



    //폐기된 시스템 혹시 몰라 백업해놈

    //public void Load_Information()
    //{
    //    //Image ChooseImage = ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>();


    //    for (int i = 0; i < InventorySlot.Count; i++)
    //    {
    //        if (ItemInformation.slot_Select.transform.position == InventorySlot[i].transform.position)
    //        {
    //            Positioncount = i;
    //            storageside = false;
    //            Debug.Log((i + 1) + "번쨰 인벤토리쪽");
    //        }

    //        else if (ItemInformation.slot_Select.transform.position == StorageSlot[i].transform.position)
    //        {
    //            Positioncount = i;
    //            storageside = true;
    //            Debug.Log((i + 1) + "번쨰 창고쪽");
    //        }

    //        if (ItemInformation.slot_Select.transform.position == InventorySlot[i].transform.position || ItemInformation.slot_Select.transform.position == StorageSlot[i].transform.position)
    //        {
    //            Positioncount = i;

    //            if (ChooseImage.enabled == false)
    //            {
    //                ChooseImage.enabled = true;
    //            }

    //            if (Slot_Select.transform.position == InventorySlot[i].transform.position)
    //            {
    //                ChooseImage.sprite = InventoryImageSlot[i].GetComponent<Image>().sprite;
    //            }
    //            else if (Slot_Select.transform.position == StorageSlot[i].transform.position)
    //            {
    //                ChooseImage.sprite = StorageImageSlot[i].GetComponent<Image>().sprite;
    //            }

    //            for (int j = 0; j < InventorySystem.ItemDB.Count; j++)
    //            {
    //                if (ChooseImage.sprite.name.ToString() == InventorySystem.ItemDB[j]["ImgName"].ToString())
    //                {
    //                    Content.text = InventorySystem.ItemDB[j]["Content"].ToString();

    //                    break;
    //                }
    //            }

    //            break;
    //        }

    //    }
    //}

    //슬롯 선택 오브젝트 움직이기
    //public void Select_Move()
    //{
    //    for (int i = 0; i < StorageSlot.Count; i++)
    //    {
    //        if (Slot_Select.transform.position == InventorySlot[i].transform.position)
    //        {
    //            storageside = false;
    //            break;
    //        }

    //        else if (Slot_Select.transform.position == StorageSlot[i].transform.position)
    //        {
    //            storageside = true;
    //            break;
    //        }
    //    }

    //    if (storageUI.activeSelf == true)
    //    {
    //        if (Input.GetButtonDown("Jump"))
    //        {
    //            Debug.Log(Positioncount);
    //        }

    //        if (Input.GetButtonDown("Left"))
    //        {
    //            if (Positioncount != 0)
    //            {
    //                Positioncount--;
    //                SetPosition();
    //            }
    //        }

    //        else if (Input.GetButtonDown("Right"))
    //        {
    //            if (Positioncount < InventorySystem.SetSize && InventoryImageSlot[Positioncount + 1].GetComponent<Image>().enabled == true && storageside == false)
    //            {
    //                Positioncount++;
    //                Slot_Select.transform.position = InventorySlot[Positioncount].transform.position;
    //            }

    //            else if (StorageImageSlot[Positioncount + 1].GetComponent<Image>().enabled == true && storageside == true)
    //            {
    //                Positioncount++;
    //                Slot_Select.transform.position = StorageSlot[Positioncount].transform.position;
    //            }
    //        }

    //        else if (Input.GetButtonDown("Up"))
    //        {
    //            if ((Positioncount - storagearray) >= 0)
    //            {
    //                Positioncount -= storagearray;
    //                SetPosition();
    //            }
    //        }

    //        else if (Input.GetButtonDown("Down"))
    //        {
    //            if ((Positioncount + storagearray) < InventorySystem.SetSize && InventoryImageSlot[Positioncount + storagearray].GetComponent<Image>().enabled == true && storageside == false)
    //            {
    //                Positioncount += storagearray;
    //                Slot_Select.transform.position = InventorySlot[Positioncount].transform.position;
    //            }

    //            else if ((Positioncount + storagearray) < StorageSlot.Count && StorageImageSlot[Positioncount + storagearray].GetComponent<Image>().enabled == true & storageside == true)
    //            {
    //                Positioncount += storagearray;
    //                Slot_Select.transform.position = StorageSlot[Positioncount].transform.position;
    //            }
    //        }

    //        Load_Information();
    //    }
    //}

    //public void Load_Information()
    //{

    //    for (int i = 0; i < InventorySlot.Count; i++)
    //    {
    //        if (ItemInformation.slot_Select.transform.position == InventorySlot[i].transform.position)
    //        {
    //            Positioncount = i;
    //            storageside = false;
    //            Debug.Log((i + 1) + "번쨰 인벤토리쪽");
    //            break;
    //        }
    //    }


    //    for (int i = 0; i < StorageSlot.Count; i++)
    //    {
    //        if (ItemInformation.slot_Select.transform.position == StorageSlot[i].transform.position)
    //        {
    //            Positioncount = i;
    //            storageside = true;
    //            Debug.Log((i + 1) + "번쨰 창고쪽");
    //            break;
    //        }
    //    }
    //}

    //public void Reset_Information()
    //{
    //    Image ChooseImage = ChooseItem.transform.Find("ChooseItemImage").GetComponent<Image>();
    //    ChooseImage.enabled = false;
    //    ChooseImage.sprite = null;
    //    Content.text = null;
    //}
}
