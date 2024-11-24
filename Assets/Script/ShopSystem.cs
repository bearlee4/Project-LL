using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    InventorySystem InventorySystem;
    Player Player;

    private GameObject player;

    public GameObject shop_UI;
    public GameObject shop_Slot;
    public GameObject content;
    public Text Gold_Text;

    //아이템DB 가져오기
    public List<Dictionary<string, object>> ItemDB;

    private List<string> shop_List = new List<string>();
    private List<int> shop_Cost = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        ItemDB = CSVReader.Read("ItemDB");
        player = GameObject.Find("Player");

        InventorySystem = this.GetComponent<InventorySystem>();
        Player = player.GetComponent<Player>();
        

        //살 수 있는 품목 리스트에 저장
        for (int i = 0; i < ItemDB.Count; i++)
        {
            if (ItemDB[i]["Buy"].ToString() != "X")
            {
                shop_List.Add(ItemDB[i]["ImgName"].ToString());
                shop_Cost.Add((int)ItemDB[i]["Buy"]);
                Debug.Log("shop_List.Count : " + shop_List.Count);
            }
        }

        for (int i = 0; i < shop_List.Count; i++)
        {
            Instantiate(shop_Slot, content.transform);
        }

        List_Renewal();
        //자식 개수 찾아내는 코드
        //Debug.Log("content.transform.childCount : " + content.transform.childCount);

        if (shop_UI.activeSelf == true)
        {
            shop_UI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void List_Renewal()
    {
        //정보 리셋
        for (int i = 0; i < content.transform.childCount; i++)
        {
            content.transform.GetChild(i).Find("Gold").Find("Gold_Text").GetComponent<Text>().text = null;
            if(content.transform.GetChild(i).Find("Item").Find("ItemImage").gameObject.activeSelf == true)
            {
                content.transform.GetChild(i).Find("Item").Find("ItemImage").gameObject.SetActive(false);
            }
            
        }

        for (int i = 0; i < shop_List.Count; i++)
        {
            for (int n = 0; n < ItemDB.Count; n++)
            {
                if (shop_List[i] == ItemDB[n]["ImgName"].ToString())
                {
                    content.transform.GetChild(i).Find("Gold").Find("Gold_Text").GetComponent<Text>().text = ItemDB[n]["Buy"].ToString();
                    Image Image = content.transform.GetChild(i).Find("Item").Find("ItemImage").GetComponent<Image>();

                    //이미지 키기
                    if (content.transform.GetChild(i).Find("Item").Find("ItemImage").gameObject.activeSelf == false)
                    {
                        content.transform.GetChild(i).Find("Item").Find("ItemImage").gameObject.SetActive(true);
                    }
                    if (Image.enabled == false)
                    {
                        Image.enabled = true;
                    }
                    Image.sprite = Resources.Load<Sprite>("Image/" + shop_List[i]);

                    int temp = i;
                    content.transform.GetChild(temp).Find("Buy_Button").GetComponent<Button>().onClick.AddListener(() => Buy_Item(temp));
                    break;
                }
            }
        }
    }

    public void Check(int num)
    {
        Debug.Log("버튼 작동중");
        Debug.Log("num : " + num);
    }

    public void Buy_Item(int num)
    {
        int transnumber = 1;
        //골드가 충분할경우
        if (Player.gold >= shop_Cost[num])
        {
            //인벤토리가 꽉차있지 않을 경우
            if (InventorySystem.FullInventory == false)
            {
                Player.Use_Gold(shop_Cost[num]);
                Link_Gold();
                InventorySystem.AddInventory(shop_List[num], transnumber);
            }

            //인벤토리가 꽉차있을 경우
            else
            {
                //인벤이 꽉차 있어도 같은 종류의 아이템이 있고 합친 갯수가 99개를 넘지 않을경우
                if (InventorySystem.InventoryList.Contains(shop_List[num]))
                {
                   for (int i = 0; i < InventorySystem.InventoryList.Count; i++)
                   {
                        if (InventorySystem.InventoryList[i] == shop_List[num])
                        {
                            if (InventorySystem.CountList[i] + transnumber <= 99)
                            {
                                Player.Use_Gold(shop_Cost[num]);
                                Link_Gold();
                                InventorySystem.AddInventory(shop_List[num], transnumber);
                                break;
                            }

                            else
                            {
                                Debug.Log("공간 부족");
                                break;
                            }
                        }
                   }
                }

                else
                {
                    Debug.Log("공간 부족");
                }
            }
        }

        else
        {
            Debug.Log("돈이 부족함");
        }
    }

    public void Link_Gold()
    {
        Gold_Text.text = Player.gold_Text.text;
    }
}
