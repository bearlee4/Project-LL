using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.PlasticSCM.Editor.WebApi;

public class AlchemySystem : MonoBehaviour
{
    StorageSystem StorageSystem;
    ItemInformation ItemInformation;
    InventorySystem InventorySystem;

    private GameObject canvas;
    UISystem UISystem;

    //연금 아이템 담아두기 위한 리스트
    public List<string> AlchemyList = new List<string>();

    public List<string> Mix_ItemList = new List<string>();

    //연금쪽 슬롯
    public List<GameObject> AlchemySlot = new List<GameObject>();
    public List<GameObject> AlchemyImageSlot = new List<GameObject>();
    public GameObject resultImageSlot;

    //창고측 슬롯
    public List<GameObject> StorageSlot = new List<GameObject>();
    public List<GameObject> StorageImageSlot = new List<GameObject>();
    public List<Text> StorageNumberList = new List<Text>();

    public int alchemySize;
    public GameObject alchemyUI;
    public bool alchemyside;
    public bool fullAlchemy_Slot;
    private bool alchemySusccess;
    private bool storage_side;

    public GameObject mix_Button;
    public GameObject get_Button;
    public Button inventory_Button;
    public Button storage_Button;

    private int pageNumber;
    private int pagecalcul;
    public Text pageText;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        UISystem = canvas.GetComponent<UISystem>();
        StorageSystem = this.GetComponent<StorageSystem>();
        ItemInformation = this.GetComponent<ItemInformation>();
        InventorySystem = this.GetComponent<InventorySystem>();

        alchemySize = 2;
        alchemyside = false;
        storage_side = true;

        mix_Button.SetActive(false);
        get_Button.SetActive(false);

        if (alchemyUI.activeSelf == true)
        {
            alchemyUI.SetActive(false);
        }

        pageNumber = 1;
        pagecalcul = 12 * (pageNumber - 1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //창고 UI 열기
    public void OpenAlchemy()
    {
        Debug.Log("Alchemy open");
        LinkStorage();
        alchemyUI.SetActive(true);

        //연금 슬롯 사이즈에 맞게 인벤토리 슬롯 생성
        for (int num = alchemySize; AlchemySlot.Count > num; num++)
        {
            AlchemySlot[num].SetActive(false);
        }

        if (mix_Button.activeSelf == true && AlchemyList.Count < 2)
        {
            mix_Button.SetActive(false);
        }

        if (storage_side == true)
        {
            Change_Storage();
            LinkStorage();
        }

        else
        {
            Change_Inventory();
            LinkInventory();
        }

    }

    //창고 UI 닫기
    public void CloseAlchemy()
    {
        Debug.Log("Alchemy close");
        //연금 슬롯에 넣어둔게 있을때
        if(AlchemyList.Any())
        {
            for (int i = 0; i < AlchemyList.Count; i++)
            {
                StorageSystem.AddStorage(AlchemyList[i], 1);
                AlchemyImageSlot[i].SetActive(false);
            }

            AlchemyList.Clear();
            fullAlchemy_Slot = false;
        }

        alchemyUI.SetActive(false);
    }


    public void Slot_Reset()
    {
        for (int i = 0; i < StorageSlot.Count; i++)
        {
            Image Image = StorageImageSlot[i].GetComponent<Image>();
            Image.sprite = null;
            Image.enabled = false;
            StorageNumberList[i].text = null;
        }
    }

    public void LinkStorage()
    {
        int count = 0;

        Slot_Reset();
        if (StorageSystem.StorageList.Any() == true)
        {
            for (int i = 0 + pagecalcul; i < StorageSystem.StorageList.Count; i++)
            {
                //이미지 연동
                Image Image = StorageImageSlot[i - pagecalcul].GetComponent<Image>();
                //Image StorageImage = StorageSystem.StorageImageSlot[i].GetComponent<Image>();
                if (StorageImageSlot[i - pagecalcul].activeSelf == false)
                {
                    StorageImageSlot[i - pagecalcul].SetActive(true);
                }

                if (Image.enabled == false)
                {
                    Image.sprite = Resources.Load<Sprite>("Image/" + StorageSystem.StorageList[i]);
                    Image.enabled = true;
                }

                //텍스트 연동
                StorageNumberList[i - pagecalcul].text = StorageSystem.StorageCountList[i].ToString();

                count++;

                if(count == 12)
                {
                    break;
                }
            }
        }
    }

    public void LinkInventory()
    {
        int count = 0;

        Slot_Reset();
        if (InventorySystem.InventoryList.Any() == true)
        {
            for (int i = 0 + pagecalcul; i < InventorySystem.InventoryList.Count; i++)
            {
                //이미지 연동
                Image Image = StorageImageSlot[i - pagecalcul].GetComponent<Image>();
                //Image StorageImage = StorageSystem.StorageImageSlot[i].GetComponent<Image>();
                if (StorageImageSlot[i - pagecalcul].activeSelf == false)
                {
                    StorageImageSlot[i - pagecalcul].SetActive(true);
                }

                if (Image.enabled == false)
                {
                    Image.sprite = Resources.Load<Sprite>("Image/" + InventorySystem.InventoryList[i]);
                    Image.enabled = true;
                }

                //텍스트 연동
                StorageNumberList[i - pagecalcul].text = InventorySystem.CountList[i].ToString();

                count++;

                if (count == 12)
                {
                    break;
                }
            }
        }
    }

    public void TransItem()
    {
        int transnumber = 1;

        alchemyside = ItemInformation.sidetoken;
        Debug.Log("현재 스토리지 사이드" + alchemyside);

        //연금솥 to 창고
        if (alchemyside == true)
        {
            for (int i = 0; i < AlchemyList.Count; i++)
            {
                Debug.Log("연금솥->창고 쪽 이동 작동중");
                if (ItemInformation.slot_Select.transform.position.ToString() == AlchemySlot[i].transform.position.ToString())
                {

                    for (int j = 0; j < InventorySystem.ItemDB.Count; j++)
                    {
                        if (AlchemyList[i] == InventorySystem.ItemDB[j]["ImgName"].ToString())
                        {
                            if (storage_side == true)
                            {
                                StorageSystem.AddStorage(InventorySystem.ItemDB[j]["ImgName"].ToString(), transnumber);
                                AlchemyUse(i);
                                break;
                            }

                            else
                            {
                                InventorySystem.AddInventory(InventorySystem.ItemDB[j]["ImgName"].ToString(), transnumber);
                                AlchemyUse(i);
                                break;
                            }
                            
                        }
                    }
                }
            }

            if(alchemyUI.activeSelf == true && AlchemyList.Count < 2)
            {
                mix_Button.SetActive(false);
            }
        }

        //창고 to 연금솥
        else
        {
            if(storage_side == true)
            {
                for (int i = 0 + pagecalcul; i < StorageSystem.StorageList.Count; i++)
                {
                    if (ItemInformation.slot_Select.transform.position.ToString() == StorageSlot[i - pagecalcul].transform.position.ToString() && fullAlchemy_Slot == false)
                    {


                        AddAlchemy(StorageSystem.StorageList[i]);
                        StorageSystem.StorageUse(i, transnumber);
                        LinkStorage();

                        if (StorageImageSlot[i - pagecalcul].activeSelf == true && StorageImageSlot[i - pagecalcul].GetComponent<Image>().sprite != null)
                        {
                            ItemInformation.Load_Information(UISystem.overObject);
                        }

                        break;
                    }

                }
            }

            else
            {
                for (int i = 0 + pagecalcul; i < InventorySystem.InventoryList.Count; i++)
                {
                    if (ItemInformation.slot_Select.transform.position.ToString() == StorageSlot[i - pagecalcul].transform.position.ToString() && fullAlchemy_Slot == false)
                    {


                        AddAlchemy(InventorySystem.InventoryList[i]);
                        StorageSystem.InventoryUse(i, transnumber);
                        LinkInventory();

                        if (StorageImageSlot[i - pagecalcul].activeSelf == true && StorageImageSlot[i - pagecalcul].GetComponent<Image>().sprite != null)
                        {
                            ItemInformation.Load_Information(UISystem.overObject);
                        }

                        break;
                    }

                }
            }
            

            //버튼 숨긴거 꺼내기
            if (alchemyUI.activeSelf == true && AlchemyList.Count >= alchemySize)
            {
                mix_Button.SetActive(true);
            }
        }
    }

    //아이템 추가
    public void AddAlchemy(string name)
    {
        //연금솥 공간이 부족할때
        if (AlchemyList.Count == alchemySize)
        {
            fullAlchemy_Slot = true;
            Debug.Log("공간이 부족합니다.");
        }

        //부족하지 않으면 실행
        if (fullAlchemy_Slot == false)
        {
            AlchemyList.Add(name);
        }

        Debug.Log("Add Alchemy");

        // 이미지 생성
        for (int num = 0; num < AlchemyList.Count; num++)
        {
            //이미지 연동
            Image Image = AlchemyImageSlot[num].GetComponent<Image>();

            if (AlchemyImageSlot[num].activeSelf == false)
            {
                AlchemyImageSlot[num].SetActive(true);
            }

            if (Image.enabled == false)
            {
                Image.enabled = true;
            }

            Image.sprite = Resources.Load<Sprite>("Image/" + AlchemyList[num]);

        }

        if (AlchemyList.Count == alchemySize)
        {
            fullAlchemy_Slot = true;
        }
    }

    public void AlchemyUse(int i)
    {
        //아이템 사용으로 인벤토리에 아무것도 안남을때
        if (AlchemyList.Count == 1)
        {
            AlchemyImageSlot[i].GetComponent<Image>().sprite = null;
            AlchemyImageSlot[i].GetComponent<Image>().enabled = false;
            AlchemyImageSlot[i].SetActive(false);
            UISystem.clicktoggle = false;

        }

        else
        {

            for (int j = i; j < AlchemyList.Count; j++)
            {
                //마지막 칸이 아닐 때 다음 슬롯 정보 땡겨오기
                if ((j + 1) < AlchemyList.Count)
                {
                    if (AlchemyImageSlot[j].activeSelf == false)
                    {
                        AlchemyImageSlot[j].SetActive(true);
                    }

                    AlchemyImageSlot[j].GetComponent<Image>().sprite = AlchemyImageSlot[j + 1].GetComponent<Image>().sprite;
                    ItemInformation.Load_Information(UISystem.overObject);
                }

                //삭제되는게 마지막 칸일때
                else if ((j + 1) == AlchemyList.Count)
                {
                    AlchemyImageSlot[j].GetComponent<Image>().enabled = false;
                    AlchemyImageSlot[j].SetActive(false);
                    break;
                }


            }
        }

        AlchemyList.RemoveAt(i);
        fullAlchemy_Slot = false;

        if (AlchemyImageSlot[i].activeSelf == false)
        {
            ItemInformation.slot_Select.SetActive(false);
            UISystem.clicktoggle = false;
        }

        if (storage_side == true)
        {
            LinkStorage();
        }
        
        else
        {
            LinkInventory();
        }
    }

    public void Delete_All()
    {
        int AlchemyCount = AlchemyList.Count;

        for (int i = 0; i < AlchemyCount; i++)
        {
            AlchemyImageSlot[i].GetComponent<Image>().sprite = null;
            AlchemyImageSlot[i].GetComponent<Image>().enabled = false;
            AlchemyImageSlot[i].SetActive(false);
        }

        AlchemyList.Clear();
        fullAlchemy_Slot = false;
        UISystem.clicktoggle = false;
        Debug.Log("모든 아이템이 창고로 옮겨졌습니다.");
    }

    public void Item_Mix()
    {
        alchemySusccess = false;
        Mix_ItemList.Clear();

        if (AlchemyList.Count == 2)
        {
            Mix_ItemList.Add(AlchemyList[0] + "+" + AlchemyList[1]);
            Mix_ItemList.Add(AlchemyList[1] + "+" + AlchemyList[0]);

            Debug.Log("아이템 리스트 목록1" + Mix_ItemList[0]);
            Debug.Log("아이템 리스트 목록2" + Mix_ItemList[1]);
        }

        Delete_All();

        

        for (int i = 0; i < Mix_ItemList.Count; i++)
        {
            
            for (int j = 0; j < InventorySystem.ItemDB.Count; j++)
            {
                if (Mix_ItemList[i] == InventorySystem.ItemDB[j]["Recipe"].ToString())
                {
                    

                    //이미지 연동
                    Image Image = resultImageSlot.GetComponent<Image>();
                    if (resultImageSlot.activeSelf == false)
                    {
                        resultImageSlot.SetActive(true);
                    }

                    if (Image.enabled == false)
                    {
                        Image.enabled = true;  
                    }

                    Image.sprite = Resources.Load<Sprite>("Image/" + InventorySystem.ItemDB[j]["ImgName"].ToString());

                    Debug.Log("합성 성공");
                    Debug.Log(InventorySystem.ItemDB[j]["ImgName"]);
                    alchemySusccess = true;

                    break;
                }

                else
                {
                    //이미지 연동
                    Image Image = resultImageSlot.GetComponent<Image>();
                    if (resultImageSlot.activeSelf == false)
                    {
                        resultImageSlot.SetActive(true);
                    }

                    if (Image.enabled == false)
                    {
                        Image.enabled = true;
                    }

                    Image.sprite = Resources.Load<Sprite>("Image/Fail_potion");
                    Debug.Log("합성 실패. 맞는 레시피가 없습니다.");
                    
                }

            }

            if(alchemySusccess == true)
            {
                break;
            }
        }

        mix_Button.SetActive(false);
        get_Button.SetActive(true);
    }

    public void Get_ResultItem()
    {
        Debug.Log(resultImageSlot.GetComponent<Image>().sprite.ToString());
        for (int i =0; i < InventorySystem.ItemDB.Count; i ++ )
        {
            if (resultImageSlot.GetComponent<Image>().sprite.name == InventorySystem.ItemDB[i]["ImgName"].ToString())
            {
                Debug.Log("같은거 찾음");
                StorageSystem.AddStorage(InventorySystem.ItemDB[i]["ImgName"].ToString(), 1);
                resultImageSlot.SetActive(false);
                get_Button.SetActive(false);
                if (storage_side == true)
                {
                    LinkStorage();
                }
            }
        }
    }

    public void Next_Page()
    {
        if (StorageSystem.StorageList.Count > 12 + pagecalcul)
        {
            pageNumber++;
            pagecalcul = (12 * (pageNumber - 1));
            pageText.text = pageNumber.ToString();
            LinkStorage();
        }
    }

    public void Back_Page()
    {
        if (pageNumber != 1)
        {
            pageNumber--;
            pagecalcul = (12 * (pageNumber - 1));
            pageText.text = pageNumber.ToString();
            LinkStorage();
        }
    }

    public void Change_Inventory()
    {
        pageNumber = 0;

        Color color = storage_Button.GetComponent<Image>().color;
        color.a = 0.5f;
        storage_Button.GetComponent<Image>().color = color;
        color = inventory_Button.GetComponent<Image>().color;
        color.a = 1.0f;
        inventory_Button.GetComponent<Image>().color = color;

        LinkInventory();

        storage_side = false;

        Debug.Log("인벤토리 변경");
    }

    public void Change_Storage()
    {
        pageNumber = 0;

        Color color = inventory_Button.GetComponent <Image>().color;
        color.a = 0.5f;
        inventory_Button.GetComponent<Image>().color = color;
        color = storage_Button.GetComponent<Image>().color;
        color.a = 1.0f;
        storage_Button.GetComponent<Image>().color = color;

        LinkStorage();

        storage_side = true;

        Debug.Log("창고 변경");
    }
}
