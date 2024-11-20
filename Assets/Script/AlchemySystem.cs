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
    
    [Header("Object")]
    public GameObject alchemyUI;
    //private bool storage_side;

    public GameObject Result_UI;
    public GameObject mix_UI;
    public GameObject mix_Button;
    public Text mix_Text;

    public GameObject get_Button;
    public Button inventory_Button;
    public Button storage_Button;
    public Text pageText;
    private int pageNumber;
    private int pagecalcul;
    private int transnumber;


    [Header("ItemSlot")]
    //연금쪽 슬롯
    public List<GameObject> AlchemySlot = new List<GameObject>();
    public List<GameObject> AlchemyImageSlot = new List<GameObject>();
    public GameObject resultImageSlot;
    //public Text resultNumber;

    //창고측 슬롯
    public List<GameObject> StorageSlot = new List<GameObject>();
    public List<GameObject> StorageImageSlot = new List<GameObject>();
    public List<Text> StorageNumberList = new List<Text>();

    [Header("Recipe")]
    //레시피 관련
    //public GameObject recipe_UI;
    //public GameObject recipe_content;
    //public GameObject recipe_Slot;
    //public List<string> recipeList = new List<string>();

    RecipeBook RecipeBook;


    [Header("Don't Touch in Unity")]
    //연금 아이템 관련
    public List<string> AlchemyList = new List<string>();
    public List<int> AlchemyNumber = new List<int>();
    public List<string> Mix_ItemList = new List<string>();
    public int alchemySize;
    private int max_Count;
    public bool alchemyside;
    public bool fullAlchemy_Slot;
    public bool ban_trans;
    private bool alchemySusccess;
    public bool open_result_ui;
    public bool open_recipe_ui;

    private void Awake()
    {
        alchemySize = 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        UISystem = canvas.GetComponent<UISystem>();
        StorageSystem = this.GetComponent<StorageSystem>();
        ItemInformation = this.GetComponent<ItemInformation>();
        InventorySystem = this.GetComponent<InventorySystem>();
        RecipeBook = this.GetComponent<RecipeBook>();
        
        alchemyside = false;
        //storage_side = true;

        mix_UI.SetActive(false);
        //get_Button.SetActive(false);

        if (Result_UI.activeSelf == true)
        {
            Result_UI.SetActive(false);
        }

        if (alchemyUI.activeSelf == true)
        {
            alchemyUI.SetActive(false);
        }

        pageNumber = 1;
        pagecalcul = StorageSlot.Count * (pageNumber - 1);
        max_Count = 10;

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

        Color color;

        for (int i = 0; i < AlchemySlot.Count; i++)
        {
            color = AlchemySlot[i].GetComponent<Image>().color;
            color.a = 1.0f;
            AlchemySlot[i].GetComponent<Image>().color = color;
        }

        //연금 슬롯 사이즈에 맞게 인벤토리 슬롯 생성
        for (int num = alchemySize; AlchemySlot.Count > num; num++)
        {
            //AlchemySlot[num].SetActive(false);
            color = AlchemySlot[num].GetComponent<Image>().color;
            color.a = 0.5f;
            AlchemySlot[num].GetComponent<Image>().color = color;
        }

        if (mix_UI.activeSelf == true && AlchemyList.Count < 2)
        {
            mix_UI.SetActive(false);
        }

        //Change_Storage();
        LinkStorage();

        //if (storage_side == true)
        //{
        //    Change_Storage();
        //    LinkStorage();
        //}

        //else
        //{
        //    Change_Inventory();
        //    LinkInventory();
        //}

        //갯수 1로 리셋
        transnumber = 1;

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
                StorageSystem.AddStorage(AlchemyList[i], transnumber);
                AlchemyImageSlot[i].SetActive(false);
                AlchemySlot[i].transform.Find("Text").GetComponent<Text>().text = null;
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

                if(count == StorageSlot.Count)
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

                if (count == StorageSlot.Count)
                {
                    break;
                }
            }
        }
    }

    public void TransItem()
    {
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
                            StorageSystem.AddStorage(InventorySystem.ItemDB[j]["ImgName"].ToString(), transnumber);
                            AlchemyUse(i, transnumber);
                            break;

                            //if (storage_side == true)
                            //{
                            //    StorageSystem.AddStorage(InventorySystem.ItemDB[j]["ImgName"].ToString(), transnumber);
                            //    AlchemyUse(i);
                            //    break;
                            //}

                            //else
                            //{
                            //    InventorySystem.AddInventory(InventorySystem.ItemDB[j]["ImgName"].ToString(), transnumber);
                            //    AlchemyUse(i);
                            //    break;
                            //}
                            
                        }
                    }
                }
            }

            if(alchemyUI.activeSelf == true && AlchemyList.Count < 2)
            {
                mix_UI.SetActive(false);
            }
        }

        //창고 to 연금솥
        else
        {
            for (int i = 0 + pagecalcul; i < StorageSystem.StorageList.Count; i++)
            {
                if (ItemInformation.slot_Select.transform.position.ToString() == StorageSlot[i - pagecalcul].transform.position.ToString() && fullAlchemy_Slot == false)
                {
                    if (AlchemyList.Contains(StorageSystem.StorageList[i]) && mix_UI.activeSelf == false)
                    {
                        Debug.Log("못들어감");
                        break;
                    }

                    else
                    {
                        AddAlchemy(StorageSystem.StorageList[i], transnumber);
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

            //버튼 숨긴거 꺼내기
            if (alchemyUI.activeSelf == true && AlchemyList.Count >= alchemySize)
            {
                mix_UI.SetActive(true);
                mix_Text.text = transnumber.ToString();
            }

            //if (storage_side == true)
            //{
            //    for (int i = 0 + pagecalcul; i < StorageSystem.StorageList.Count; i++)
            //    {
            //        if (ItemInformation.slot_Select.transform.position.ToString() == StorageSlot[i - pagecalcul].transform.position.ToString() && fullAlchemy_Slot == false)
            //        {

            //            AddAlchemy(StorageSystem.StorageList[i], transnumber);
            //            StorageSystem.StorageUse(i, transnumber);
            //            LinkStorage();

            //            if (StorageImageSlot[i - pagecalcul].activeSelf == true && StorageImageSlot[i - pagecalcul].GetComponent<Image>().sprite != null)
            //            {
            //                ItemInformation.Load_Information(UISystem.overObject);
            //            }

            //            break;
            //        }

            //    }
            //}

            //else
            //{
            //    for (int i = 0 + pagecalcul; i < InventorySystem.InventoryList.Count; i++)
            //    {
            //        if (ItemInformation.slot_Select.transform.position.ToString() == StorageSlot[i - pagecalcul].transform.position.ToString() && fullAlchemy_Slot == false)
            //        {


            //            AddAlchemy(InventorySystem.InventoryList[i], transnumber);
            //            StorageSystem.InventoryUse(i, transnumber);
            //            LinkInventory();

            //            if (StorageImageSlot[i - pagecalcul].activeSelf == true && StorageImageSlot[i - pagecalcul].GetComponent<Image>().sprite != null)
            //            {
            //                ItemInformation.Load_Information(UISystem.overObject);
            //            }

            //            break;
            //        }

            //    }
            //}

        }
    }

    //아이템 추가
    public void AddAlchemy(string name, int transnumber)
    {
        //연금솥 공간이 부족할때
        if (AlchemyList.Count == alchemySize && !AlchemyList.Contains(name))
        {
            fullAlchemy_Slot = true;
            Debug.Log("공간이 부족합니다.");
        }

        //부족하지 않으면 실행
        if (fullAlchemy_Slot == false || AlchemyList.Contains(name))
        {
            if (AlchemyList.Contains(name))
            {
                for (int i = 0; i < AlchemyList.Count; i++)
                {
                    if (AlchemyList[i] == name)
                    {
                        AlchemyNumber[i] += transnumber;
                    }
                }
            }

            else
            {
                AlchemyList.Add(name);
                AlchemyNumber.Add(transnumber);
            }
        }

        else
        {
            Debug.Log("같은 종류끼리 합성할 수 없습니다. 다른 아이템을 넣어주세요.");
        }

        Debug.Log("Add Alchemy");

        // 이미지 생성
        for (int num = 0; num < AlchemyList.Count; num++)
        {
            //이미지 연동
            Image Image = AlchemyImageSlot[num].GetComponent<Image>();
            Text text = AlchemySlot[num].transform.Find("Text").GetComponent<Text>();

            if (AlchemyImageSlot[num].activeSelf == false)
            {
                AlchemyImageSlot[num].SetActive(true);
            }

            if (Image.enabled == false)
            {
                Image.enabled = true;
            }

            Image.sprite = Resources.Load<Sprite>("Image/" + AlchemyList[num]);
            text.text = AlchemyNumber[num].ToString();

        }

        if (AlchemyList.Count == alchemySize)
        {
            fullAlchemy_Slot = true;
        }
    }

    public void AlchemyUse(int i, int number)
    {
        //0개로 사라져야하지 않을때
        if (AlchemyNumber[i] - number != 0)
        {
            AlchemyNumber[i] -= number;
            AlchemySlot[i].transform.Find("Text").GetComponent<Text>().text = AlchemyNumber[i].ToString();

        }

        //사라져야 할 때
        else
        {
            //아이템 사용으로 인벤토리에 아무것도 안남을때
            if (AlchemyList.Count == 1)
            {
                AlchemyImageSlot[i].GetComponent<Image>().sprite = null;
                AlchemyImageSlot[i].GetComponent<Image>().enabled = false;
                AlchemyImageSlot[i].SetActive(false);
                AlchemySlot[i].transform.Find("Text").GetComponent<Text>().text = null;
                UISystem.clicktoggle = false;

            }

            else
            {


                for (int j = i; j < AlchemyList.Count; j++)
                {
                    //마지막 칸이 아닐 때 다음 슬롯 정보 땡겨오기
                    if ((j + 1) < AlchemyList.Count)
                    {
                        Text text1 = AlchemySlot[j].transform.Find("Text").GetComponent<Text>();
                        Text text2 = AlchemySlot[j + 1].transform.Find("Text").GetComponent<Text>();

                        if (AlchemyImageSlot[j].activeSelf == false)
                        {
                            AlchemyImageSlot[j].SetActive(true);
                        }

                        AlchemyImageSlot[j].GetComponent<Image>().sprite = AlchemyImageSlot[j + 1].GetComponent<Image>().sprite;
                        text1.text = text2.text;
                        ItemInformation.Load_Information(UISystem.overObject);
                    }

                    //삭제되는게 마지막 칸일때
                    else if ((j + 1) == AlchemyList.Count)
                    {
                        AlchemyImageSlot[j].GetComponent<Image>().enabled = false;
                        AlchemySlot[j].transform.Find("Text").GetComponent<Text>().text = null;
                        AlchemyImageSlot[j].SetActive(false);
                        break;
                    }


                }
            }

            AlchemyList.RemoveAt(i);
            AlchemyNumber.RemoveAt(i);
            fullAlchemy_Slot = false;

            Count_Reset();
            Debug.Log("transnumber : " + transnumber);

            if (AlchemyImageSlot[i].activeSelf == false)
            {
                ItemInformation.slot_Select.SetActive(false);
                UISystem.clicktoggle = false;
            }

            LinkStorage();

            //if (storage_side == true)
            //{
            //    LinkStorage();
            //}

            //else
            //{
            //    LinkInventory();
            //}
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
            AlchemySlot[i].transform.Find("Text").GetComponent <Text>().text = null;
        }

        AlchemyList.Clear();
        AlchemyNumber.Clear();
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

                    Open_Result_UI(true, InventorySystem.ItemDB[j]["ImgName"].ToString(), InventorySystem.ItemDB[j]["Name"].ToString());

                    break;
                }

                else
                {

                    Open_Result_UI(false, "Fail_potion", "정체모를 포션");

                }

            }

            if (alchemySusccess == true)
            {
                break;
            }
        }

        //resultNumber.text = transnumber.ToString();

        mix_UI.SetActive(false);
        ban_trans = true;
    }

    public void Open_Result_UI(bool Success, string name, string kor_name)
    {
        Debug.Log("합성 창 진입 성공");
        Result_UI.SetActive(true);

        Text truefail = Result_UI.transform.Find("Result").GetComponent<Text>();
        Text content = Result_UI.transform.Find("Content").GetComponent<Text>();

        if (Success == true)
        {
            truefail.text = "합성 성공!";
            truefail.color = Color.yellow;

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

            Image.sprite = Resources.Load<Sprite>("Image/" + name);

            Debug.Log("합성 성공");
            Debug.Log(name);
            alchemySusccess = true;

            if (!RecipeBook.recipeList.Contains(name))
            {
                RecipeBook.Add_Recipe(name);
            }
        }

        else
        {
            truefail.text = "합성 실패...";
            truefail.color = Color.gray;

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

        content.text = kor_name + " " + transnumber + "개를 획득하셨습니다.";

        open_result_ui = true;
        //get_Button.SetActive(true);
    }

    public void Get_ResultItem()
    {
        Debug.Log(resultImageSlot.GetComponent<Image>().sprite.ToString());
        for (int i =0; i < InventorySystem.ItemDB.Count; i ++ )
        {
            if (resultImageSlot.GetComponent<Image>().sprite.name == InventorySystem.ItemDB[i]["ImgName"].ToString())
            {
                Debug.Log("같은거 찾음");
                StorageSystem.AddStorage(InventorySystem.ItemDB[i]["ImgName"].ToString(), transnumber);
                resultImageSlot.SetActive(false);
                //resultNumber.text = null;
                Result_UI.SetActive(false);
                open_result_ui = false;
                //get_Button.SetActive(false);

                LinkStorage();

                transnumber = 1;

                //if (storage_side == true)
                //{
                //    LinkStorage();
                //}
            }
        }

        ban_trans = false;
    }

    public void Next_Page()
    {
        if (StorageSystem.StorageList.Count > StorageSlot.Count + pagecalcul)
        {
            pageNumber++;
            pagecalcul = (StorageSlot.Count * (pageNumber - 1));
            pageText.text = pageNumber.ToString();
            LinkStorage();
        }
    }

    public void Back_Page()
    {
        if (pageNumber != 1)
        {
            pageNumber--;
            pagecalcul = (StorageSlot.Count * (pageNumber - 1));
            pageText.text = pageNumber.ToString();
            LinkStorage();
        }
    }

    public void Change_Inventory()
    {
        pageNumber = 1;

        Color color = storage_Button.GetComponent<Image>().color;
        color.a = 0.5f;
        storage_Button.GetComponent<Image>().color = color;
        color = inventory_Button.GetComponent<Image>().color;
        color.a = 1.0f;
        inventory_Button.GetComponent<Image>().color = color;

        LinkInventory();

        //storage_side = false;

        Debug.Log("인벤토리 변경");
    }

    public void Change_Storage()
    {
        pageNumber = 1;

        Color color = inventory_Button.GetComponent <Image>().color;
        color.a = 0.5f;
        inventory_Button.GetComponent<Image>().color = color;
        color = storage_Button.GetComponent<Image>().color;
        color.a = 1.0f;
        storage_Button.GetComponent<Image>().color = color;

        LinkStorage();

        //storage_side = true;

        Debug.Log("창고 변경");
    }

    public void Plus_Item()
    {
        int change_Count;
        int count_Toggle = 0;

        change_Count = 1;
        int[] test = new int[AlchemyList.Count];
        
        for (int i = 0; i < AlchemyList.Count; i++)
        {
            if (StorageSystem.StorageList.Contains(AlchemyList[i]))
            {
                for (int j = 0; j < StorageSystem.StorageList.Count; j++)
                {
                    if (AlchemyList[i] == StorageSystem.StorageList[j] && StorageSystem.StorageCountList[j] >= (transnumber - AlchemyNumber[i]))
                    {
                        //AddAlchemy(StorageSystem.StorageList[i], transnumber);
                        //StorageSystem.StorageUse(i, transnumber);
                        //LinkStorage();
                        test[i] = j;
                        count_Toggle ++;
                        break;
                    }
                }
            }
        }

        if (count_Toggle == AlchemyList.Count && transnumber < max_Count)
        {
            transnumber++;
            mix_Text.text = transnumber.ToString();

            for (int i = 0; i < AlchemyList.Count;i++)
            {
                AddAlchemy(AlchemyList[i], change_Count);
                StorageSystem.StorageUse(test[i], change_Count);
            }

            LinkStorage();
        }

        else
        {
            Debug.Log("작동할 수 없습니다.");
        }
    }

    public void Minus_Item()
    {
        int change_Count;
        change_Count = 1;

        if (transnumber <= 1)
        {
            Debug.Log("더 이상 내릴 수 없습니다.");
        }

        else
        {
            transnumber--;
            mix_Text.text = transnumber.ToString();

            for (int i = 0; i < AlchemyList.Count;i++)
            {
                AlchemyUse(i, change_Count);
                StorageSystem.AddStorage(AlchemyList[i], change_Count);
            }

            LinkStorage();
        }
        
    }

    public void Count_Reset()
    {
        int return_Count;
        transnumber = 1;

        for(int i = 0; i < AlchemyList.Count; i++)
        {
            if (AlchemyNumber[i] > 1)
            {
                return_Count = AlchemyNumber[i] - 1;
                AlchemyUse(i, return_Count);
                StorageSystem.AddStorage(AlchemyList[i], return_Count);
                AlchemySlot[i].transform.Find("Text").GetComponent<Text>().text = 1.ToString();

                Debug.Log("AlchemyNumber[" + i + "] : " + AlchemyNumber[i]);
            }
            
        }
    }

}
