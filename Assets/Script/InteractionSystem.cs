using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{

    private string currentSceneName;

    public GameObject Manager;
    InventorySystem InventorySystem;
    ItemInformation ItemInformation;
    FieldSystem FieldSystem;
    StorageSystem StorageSystem;
    AlchemySystem AlchemySystem;
    RequestSystem RequestSystem;
    ShopSystem ShopSystem;
    RecipeBook RecipeBook;

    public GameObject SaveData_Manager;
    SaveData SaveData;

    public GameObject BackGround_Audio;
    BackGroundController BackGroundController;

    public GameObject Pause_UI;
    public GameObject Death_UI;

    private GameObject canvas;
    UISystem UISystem;

    public bool UItoken;
    private bool IsPause;
    private bool forageincounter;
    private bool storageincounter;
    private bool alchemyincounter;
    private bool spawnerincounter;
    private bool requestincounter;
    private bool shopincounter;
    private bool resetrequestincounter;
    private bool potalincounter;
    private string potal_Value;

    public bool max_Trans_toggle;
    public bool IsdropItem;
    public GameObject enter_object;

    //테스트용
    private bool backhomeincounter;

    public List<Dictionary<string, object>> ItemDB;

    private List<string> TriggerList = new List<string>();

    // Start is called before the first frame update
    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;

        canvas = GameObject.Find("Canvas");
        UISystem = canvas.GetComponent<UISystem>();

        IsPause = false;
        UItoken = false;
        forageincounter = false;
        storageincounter = false;
        alchemyincounter = false;
        spawnerincounter = false;
        requestincounter = false;
        shopincounter = false;
        resetrequestincounter = false;
        potalincounter = false;
        InventorySystem = Manager.GetComponent<InventorySystem>();
        StorageSystem = Manager.GetComponent<StorageSystem>();
        ItemInformation = Manager.GetComponent<ItemInformation>();
        AlchemySystem = Manager.GetComponent<AlchemySystem>();
        FieldSystem = Manager.GetComponent<FieldSystem>();
        RequestSystem = Manager.GetComponent<RequestSystem>();
        ShopSystem = Manager.GetComponent<ShopSystem>();
        RecipeBook = Manager.GetComponent<RecipeBook>();
        BackGroundController = BackGround_Audio.GetComponent<BackGroundController>();

        SaveData_Manager = GameObject.Find("SaveData_Manager");
        SaveData = SaveData_Manager.GetComponent<SaveData>();

        ItemDB = CSVReader.Read("ItemDB");

        max_Trans_toggle = false;

        backhomeincounter = false;

        FieldSystem.Forage_Classification();
    }


    // Update is called once per frame
    void Update()
    {
        CheckButton();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        enter_object = col.gameObject;

        if (col.gameObject.name == "Slot_Plus")
        {
            AlchemySystem.AlchemySlot_Plus();
            Debug.Log("슬롯 추가");
        }

        //채집물 접촉
        if (col.gameObject.tag == "Forage")
        {
            Debug.Log("채집물");
            forageincounter = true;
            TriggerList.Add(col.gameObject.name);

        }

        //상호작용 가능한 오브젝트 접촉
        if (col.gameObject.tag == "InteractionObject")
        {
            if(col.gameObject.name == "Storage")
            {
                Debug.Log("창고 접촉");
                storageincounter = true;
            }

            if (col.gameObject.name == "Alchemy")
            {
                Debug.Log("연금솥 접촉");
                alchemyincounter = true;
            }

            if (col.gameObject.name == "RequestBoard")
            {
                Debug.Log("게시판 접촉");
                requestincounter = true;
            }

            if (col.gameObject.name == "Shop")
            {
                Debug.Log("상점 접촉");
                shopincounter = true;
            }

        }

        //일과 끝내기 접촉
        if (col.gameObject.name == "Back_Home")
        {
            Debug.Log("일과 끝내기 접촉");
            backhomeincounter = true;
        }

        //스포너와 접촉
        if (col.gameObject.tag == "Spawner")
        {
            Debug.Log("스포너 접촉");
            spawnerincounter = true;
        }

        //스포너리스폰과 접촉시 발동
        if (col.gameObject.name == "Test_Spawner_Respawn")
        {
            FieldSystem.Respawn_Test_spawner();
        }

        if (col.gameObject.name == "Reset_Request")
        {
            Debug.Log("의뢰 리셋 접촉");
            resetrequestincounter = true;
        }

        if(col.gameObject.tag == "Potal")
        {
            potalincounter = true;
            if (col.gameObject.name == "To Forest")
            {
                potal_Value = "Forest";
            }

            else if (col.gameObject.name == "To Forest2")
            {
                potal_Value = "Forest2";
            }

            else if (col.gameObject.name == "To Vilige")
            {
                potal_Value = "Vilige";
            }

            else if (col.gameObject.name == "To Boss")
            {
                potal_Value = "Boss";
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Forage")
        {
            forageincounter = false;
        }

        if (col.gameObject.name == "Storage")
        {
            storageincounter = false;
        }

        if (col.gameObject.name == "Alchemy")
        {
            alchemyincounter = false;
        }

        if (col.gameObject.name == "Back_Home")
        {
            backhomeincounter = false;
        }

        if (col.gameObject.tag == "Spawner")
        {
            Debug.Log("스포너 접촉");
            spawnerincounter = false;
        }

        if (col.gameObject.name == "RequestBoard")
        {
            requestincounter = false;
        }

        if (col.gameObject.name == "Shop")
        {
            shopincounter = false;
        }

        if (col.gameObject.name == "Reset_Request")
        {
            resetrequestincounter = false;
        }

        if (col.gameObject.tag == "Potal")
        {
            potalincounter = false;
        }
    }

    public void CheckButton()
    {
        //UI가 꺼져 있을때만 작동
        if (UItoken == false)
        {
            //1 버튼
            //1번 퀵슬롯에 지정해둔게 있을때
            if (Input.GetButtonDown("QuickSlot1") && InventorySystem.QuickSlotList[0] != "null")
            {
                //그 아이템 사용
                InventorySystem.UseItem(InventorySystem.QuickSlotPosition[0]);
                //정보 리로드
                //InventorySystem.Quick_Load_Info();
            }

            //2 버튼
            //2번 퀵슬롯에 지정해둔게 있을때
            if (Input.GetButtonDown("QuickSlot2") && InventorySystem.QuickSlotList[1] != "null")
            {
                //그 아이템 사용
                InventorySystem.UseItem(InventorySystem.QuickSlotPosition[1]);
                //정보 리로드
                //InventorySystem.Quick_Load_Info();
            }

            //3 버튼
            //3번 퀵슬롯에 지정해둔게 있을때
            if (Input.GetButtonDown("QuickSlot1") && InventorySystem.QuickSlotList[2] != "null")
            {
                //그 아이템 사용
                InventorySystem.UseItem(InventorySystem.QuickSlotPosition[2]);
                //정보 리로드
                //InventorySystem.Quick_Load_Info();
            }
        }

        //F버튼
        if (Input.GetButtonDown("Interaction"))
        {
            //채집물 획득
            if (InventorySystem.Inventory.activeSelf == false && forageincounter == true)
            {
                if(enter_object.tag == "Spawner")
                {
                    AddDropItem(enter_object);
                }
                
            }

            //창고 여닫기
            if (storageincounter == true)
            {
                if (StorageSystem.storageUI.activeSelf == false && UItoken == false)
                {
                    StorageSystem.OpenStorage();
                    UItoken = true;
                }

                else if (StorageSystem.storageUI.activeSelf == true && UItoken == true)
                {
                    StorageSystem.CloseStorage();
                    UISystem.clicktoggle = false;
                    if(ItemInformation.slot_Select.activeSelf == true)
                    {
                        ItemInformation.slot_Select.SetActive(false);
                    }
                    UItoken = false;
                }
            }

            //연금 여닫기
            if (alchemyincounter == true)
            {
                if (AlchemySystem.alchemyUI.activeSelf == false && UItoken == false)
                {
                    AlchemySystem.OpenAlchemy();
                    UItoken = true;
                }

                else if (AlchemySystem.alchemyUI.activeSelf == true && UItoken == true && AlchemySystem.ban_trans == false)
                {
                    AlchemySystem.CloseAlchemy();
                    UISystem.clicktoggle = false;
                    if (ItemInformation.slot_Select.activeSelf == true)
                    {
                        ItemInformation.slot_Select.SetActive(false);
                    }
                    UItoken = false;
                }
            }

            //하루 일과 끝내기
            if (backhomeincounter == true)
            {
                StorageSystem.Back_Home();
            }

            //스포너 상호작용
            if (spawnerincounter == true)
            {
                RandomItem(enter_object);
            }

            //의뢰UI 상호작용
            if (requestincounter == true)
            {
                if (RequestSystem.request_UI.activeSelf == false && UItoken == false)
                {
                    RequestSystem.Open_RequestBoard();
                    UItoken = true;
                }

                else if (RequestSystem.request_UI.activeSelf == true && UItoken == true)
                {
                    RequestSystem.request_UI.SetActive(false);
                    RequestSystem.information_toggle = false;
                    UItoken = false;
                }
            }

            //상점UI 상호작용
            if (shopincounter == true)
            {
                if (ShopSystem.shop_UI.activeSelf == false && UItoken == false)
                {
                    ShopSystem.shop_UI.SetActive(true);
                    ShopSystem.Link_Gold();
                    UItoken = true;
                }

                else if(ShopSystem.shop_UI.activeSelf == true && UItoken == true)
                {
                    ShopSystem.shop_UI.SetActive(false);
                    UItoken = false;
                }
            }

            //의뢰 리셋
            if (resetrequestincounter == true)
            {
                RequestSystem.Set_Request();
            }

            if (potalincounter == true)
            {
                if (potal_Value == "Forest")
                {
                    Change_Map("Forest");
                }

                else if (potal_Value == "Forest2")
                {
                    Change_Map("Forest2");
                }

                else if (potal_Value == "Vilige")
                {
                    Change_Map("Vilige");
                }

                else if (potal_Value == "Boss")
                {
                    Change_Map("Boss");
                }
            }
        }

        //I버튼
        if (Input.GetButtonDown("Inventory"))
        {
            if (InventorySystem.Inventory.activeSelf == false && UItoken == false)
            {
                InventorySystem.OpenInventory();
                UItoken = true;
            }

            else if (InventorySystem.Inventory.activeSelf == true && UItoken == true)
            {
                InventorySystem.CloseInventory();
                UISystem.clicktoggle = false;
                if (ItemInformation.slot_Select.activeSelf == true)
                {
                    ItemInformation.slot_Select.SetActive(false);
                }
                UItoken = false;
            }

        }

        //Z버튼
        if (Input.GetButtonDown("Confirm"))
        {
            if (InventorySystem.Inventory.activeSelf == true && ItemInformation.slot_Select.activeSelf == true)
            {
                InventorySystem.UseItem(InventorySystem.Positioncount);
            }

            if (StorageSystem.storageUI.activeSelf == true && ItemInformation.slot_Select.activeSelf == true && UISystem.clicktoggle == true)
            {
                Debug.Log("전송 버튼 작동중");
                StorageSystem.TransItem();
            }

            if (AlchemySystem.alchemyUI.activeSelf == true && ItemInformation.slot_Select.activeSelf == true && UISystem.clicktoggle == true)
            {
                Debug.Log("연금UI 전송 버튼 작동중");
                AlchemySystem.TransItem();
            }
        }

        //esc버튼
        if (Input.GetButtonDown("Cancel"))
        {
            //창고
            if (StorageSystem.storageUI.activeSelf == true && UItoken == true)
            {
                StorageSystem.CloseStorage();
                UISystem.clicktoggle = false;
                if (ItemInformation.slot_Select.activeSelf == true)
                {
                    ItemInformation.slot_Select.SetActive(false);
                }
                UItoken = false;
            }

            //인벤토리
            else if (InventorySystem.Inventory.activeSelf == true && UItoken == true)
            {
                InventorySystem.CloseInventory();
                UISystem.clicktoggle = false;
                if (ItemInformation.slot_Select.activeSelf == true)
                {
                    ItemInformation.slot_Select.SetActive(false);
                }
                UItoken = false;
            }

            //연금
            else if (AlchemySystem.alchemyUI.activeSelf == true && UItoken == true)
            {
                if (AlchemySystem.open_result_ui == true)
                {
                    AlchemySystem.Get_ResultItem();
                }

                else if (AlchemySystem.open_recipe_ui == true)
                {
                    RecipeBook.Close_Recipebook();
                }

                else
                {
                    AlchemySystem.alchemyUI.SetActive(false);
                    UISystem.clicktoggle = false;
                    if (ItemInformation.slot_Select.activeSelf == true)
                    {
                        ItemInformation.slot_Select.SetActive(false);
                    }
                    UItoken = false;
                }
            }

            //의뢰
            else if (RequestSystem.request_UI.activeSelf == true && UItoken == true)
            {
                RequestSystem.request_UI.SetActive(false);
                RequestSystem.information_toggle = false;
                UItoken = false;
            }

            //상점
            else if (ShopSystem.shop_UI.activeSelf == true && UItoken == true)
            {
                ShopSystem.shop_UI.SetActive(false);
                UItoken = false;
            }

            //퍼즈
            if (UItoken == false && IsPause == false)
            {
                Debug.Log("퍼즈 작동중");
                Open_PauseUI();
            }

            else if (UItoken == true && IsPause == true)
            {
                Close_PauseUI();
            }
        }

        //Ctrl 버튼
        if (Input.GetButton("Ctrl"))
        {
            max_Trans_toggle = true;
        }

        else if (Input.GetButtonUp("Ctrl"))
        {
            max_Trans_toggle = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            this.GetComponent<Player>().Super_Mod();
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            this.GetComponent<Player>().Normal_Mod();
        }
    }


    //아이템 추가
    public void AddDropItem(GameObject col)
    {

        for (int IDB = 0; IDB < ItemDB.Count; IDB++)
        {
            if (col.name == ItemDB[IDB]["ImgName"].ToString())
            {
                InventorySystem.AddInventory(ItemDB[IDB]["ImgName"], InventorySystem.GetCount);
                break;
            }

        }

        if (InventorySystem.FullInventory == false)
        {
            col.SetActive(false);
            //TriggerList.Remove(TriggerList[0]);
        }

    }

    

    //커먼 8, 레어 2 비율로 확률 돌리기
    public void RandomItem(GameObject col)
    {
        int random_number = Random.Range(0, 10);
        int second_random_number;
        IsdropItem = true;
        InventorySystem.GetCount = Random.Range(1, 3);

        if (random_number < 8)
        {
            Debug.Log("일반 등급 당첨!");
            second_random_number = Random.Range(0, FieldSystem.common_ForageList.Count);
            InventorySystem.AddInventory(FieldSystem.common_ForageList[second_random_number], InventorySystem.GetCount);

        }

        else if(random_number >= 8)
        {
            Debug.Log("희귀 등급 당첨!");
            second_random_number = Random.Range(0, FieldSystem.rare_ForageList.Count);
            InventorySystem.AddInventory(FieldSystem.rare_ForageList[second_random_number], InventorySystem.GetCount);
        }

        if(InventorySystem.FullInventory == false)
        {
            col.SetActive(false);
        }
    }
    public void Save_Data()
    {
        for (int i = 0; i < InventorySystem.InventoryList.Count; i++)
        {
            SaveData.Inventory.Add(InventorySystem.InventoryList[i]);
            SaveData.Inventory_CountList.Add(InventorySystem.CountList[i]) ;
        }

        for (int i = 0; i < StorageSystem.StorageList.Count; i++)
        {
            SaveData.Storage.Add(StorageSystem.StorageList[i]);
            SaveData.Storage_CountList.Add(StorageSystem.StorageCountList[i]);

        }

        SaveData.gold = this.GetComponent<Player>().gold;

        for (int i = 0; i < RequestSystem.request_List.Count; i++)
        {
            SaveData.today_request.Add(RequestSystem.request_List[i]);
            SaveData.position_Number.Add(RequestSystem.position_Number[i]);
            SaveData.image_Number.Add(RequestSystem.image_Number[i]);
        }
        
    }

    public void Change_Map(string name)
    {
        //Save_Data();
        if (name == "Forest")
        {
            this.transform.position = new Vector3(-70, 132);
            this.transform.GetChild(0).GetComponent<CameraController>().minBounds = new Vector2((float)-81.8, (float)132.8);
            this.transform.GetChild(0).GetComponent<CameraController>().maxBounds = new Vector2((float)-58.8, (float)154.5);
            FieldSystem.Respawn_spawner();
            BackGroundController.To_Forest();
        }

        if (name == "Forest2")
        {
            this.transform.position = new Vector3((float)-56.6, (float)154.5);
            this.transform.GetChild(0).GetComponent<CameraController>().minBounds = new Vector2((float)-82.7, (float)132.3);
            this.transform.GetChild(0).GetComponent<CameraController>().maxBounds = new Vector2(-58, 155);
        }

        if (name == "Vilige")
        {
            this.transform.position = new Vector3((float)-51.4, (float)175.5);
            this.transform.GetChild(0).GetComponent<CameraController>().minBounds = new Vector2(-58, (float)165.2);
            this.transform.GetChild(0).GetComponent<CameraController>().maxBounds = new Vector2((float)-45.3, (float)174.8);
            StorageSystem.Back_Home();
            RequestSystem.Set_Request();
            BackGroundController.To_Vilige();
        }

        if (name == "Boss")
        {
            this.transform.position = new Vector3((float)-73.5, (float)164);
            this.transform.GetChild(0).GetComponent<CameraController>().minBounds = new Vector2(-75, (float)164.9);
            this.transform.GetChild(0).GetComponent<CameraController>().maxBounds = new Vector2((float)-72.2, (float)169.3);
        }
    }


    public void Open_PauseUI()
    {
        Pause_UI.SetActive(true);
        IsPause = true;
        UItoken = true;
        Time.timeScale = 0;
    }

    public void Close_PauseUI()
    {
        Pause_UI.SetActive(false);
        IsPause = false;
        UItoken = false;
        Time.timeScale = 1;
    }

    public void Reset_Scene()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void Exit_Game()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }

    public void Open_Death_UI()
    {
        UItoken = true;
        Death_UI.SetActive(true);
        Time.timeScale = 0;
    }

    public void Revive()
    {
        UItoken = false;
        Death_UI.SetActive(false);
        Time.timeScale = 1;
        this.GetComponent<PlayerStatus>().currentHP = this.GetComponent<PlayerStatus>().maxHP;
        this.GetComponent<PlayerStatus>().currentMP = this.GetComponent<PlayerStatus>().maxMP;
        this.GetComponent<Player>().HP_bar.GetComponent<Slider>().value = this.GetComponent<PlayerStatus>().currentHP;
        this.GetComponent<Player>().MP_bar.GetComponent<Slider>().value = this.GetComponent<PlayerStatus>().currentMP;

        //인벤토리 리셋
        int InventoryCount = InventorySystem.InventoryList.Count;

        for (int i = 0; i < InventoryCount; i++)
        {
            InventorySystem.DeleteItem(InventorySystem.InventoryList[0], 0);
            InventorySystem.weight = 0;
            InventorySystem.weight_text.text = InventorySystem.weight.ToString() + " / " + InventorySystem.max_weight.ToString();
        }

        this.gameObject.transform.position = new Vector3 ((float)-48.5, 167, 0);
        this.transform.GetChild(0).GetComponent<CameraController>().minBounds = new Vector2(-58, (float)165.2);
        this.transform.GetChild(0).GetComponent<CameraController>().maxBounds = new Vector2((float)-45.3, (float)174.8);

        this.GetComponent<Player>().animator.SetBool("Dead", false);


    }

    public void Main_Scene()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("MainScene");
    }

    ////아이템 추가
    //public void AddDropItem()
    //{
    //    int listcount;

    //    listcount = TriggerList.Count;

    //    if (TriggerList.Any() == true)
    //    {

    //        for (int IDB = 0; IDB < ItemDB.Count; IDB++)
    //        {
    //            if (TriggerList[0] == ItemDB[IDB]["ImgName"].ToString())
    //            {
    //                InventorySystem.AddInventory(ItemDB[IDB]["ImgName"], InventorySystem.GetCount);
    //                break;
    //            }

    //        }

    //        if (InventorySystem.FullInventory == false)
    //        {
    //            GameObject.Find(TriggerList[0]).SetActive(false);
    //            //TriggerList.Remove(TriggerList[0]);
    //        }
    //    }

    //    else if (!TriggerList.Any() == false)
    //    {
    //        forageincounter = false;
    //    }

    //}
}
