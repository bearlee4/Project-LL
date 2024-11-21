using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour
{
    public GameObject recipe_UI;
    public GameObject recipeBook_UI;
    public GameObject recipe_content;
    public GameObject recipe_Slot;
    public GameObject recipes;
    //public Text pageText;
    public List<string> recipeList = new List<string>();

    InventorySystem InventorySystem;


    public bool open_recipe_ui;
    private int max_page;
    private int current_page;
    private int quotient;
    private int remainder;
    private int recipe_Count;

    

    // Start is called before the first frame update
    void Start()
    {
        recipe_Count = recipes.transform.childCount;
        Debug.Log("recipe_Count : " + recipe_Count);
        current_page = 1;
        max_page = 1;
        InventorySystem = this.GetComponent<InventorySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open_Recipebook()
    {
        recipe_UI.SetActive(true);
        open_recipe_ui = true;
        Set_Recipe();
    }

    public void Close_Recipebook()
    {
        recipe_UI.SetActive(false);
        open_recipe_ui = false;
    }

    public void Test_Open_Recipebook()
    {
        recipeBook_UI.SetActive(true);
        open_recipe_ui = true;
        Reload_Information();
    }
    public void Test_Close_Recipebook()
    {
        recipeBook_UI.SetActive(false);
        open_recipe_ui = false;
    }

    public void Add_Recipe(string name)
    {
        recipeList.Add(name);
        Instantiate(recipe_Slot, recipe_content.transform);
        Debug.Log("레시피 추가");
    }

    public void Set_Recipe()
    {
        for (int i = 0; i < recipeList.Count; i++)
        {
            for (int n = 0; n < InventorySystem.ItemDB.Count; n++)
            {
                if (recipeList[i] == InventorySystem.ItemDB[n]["ImgName"].ToString())
                {
                    string[] cut_Recipe = InventorySystem.ItemDB[n]["Recipe"].ToString().Split("+");
                    Image Image1 = recipe_content.transform.GetChild(i).Find("Slot1").Find("Image").GetComponent<Image>();
                    Image Image2 = recipe_content.transform.GetChild(i).Find("Slot2").Find("Image").GetComponent<Image>();
                    Image ResultImage = recipe_content.transform.GetChild(i).Find("ResultSlot").Find("Image").GetComponent<Image>();

                    if (recipe_content.transform.GetChild(i).Find("Slot1").Find("Image").gameObject.activeSelf == false)
                    {
                        recipe_content.transform.GetChild(i).Find("Slot1").Find("Image").gameObject.SetActive(true);
                    }

                    if (Image1.enabled == false)
                    {
                        Image1.enabled = true;
                    }

                    if (Image2.enabled == false)
                    {
                        Image2.enabled = true;
                    }

                    if (ResultImage.enabled == false)
                    {
                        ResultImage.enabled = true;
                    }

                    Image1.sprite = Resources.Load<Sprite>("Image/" + cut_Recipe[0]);
                    Image2.sprite = Resources.Load<Sprite>("Image/" + cut_Recipe[1]);
                    ResultImage.sprite = Resources.Load<Sprite>("Image/" + recipeList[i]);
                    break;
                }
            }


        }
    }

    public void Check_Recipe()
    {
        if (recipeList.Count != recipe_content.transform.childCount)
        {
            for (int i = recipeList.Count; i == recipe_content.transform.childCount; i++)
            {
                Instantiate(recipe_Slot, recipe_content.transform);
            }
        }
    }

    public void Reload_Information()
    {
        
        if(recipeList.Count < recipe_Count)
        {
            quotient = 0;
        }

        else
        {
            quotient = recipeList.Count / recipe_Count;
            max_page = quotient;
        }

        Debug.Log("quotient : " + quotient);

        remainder = recipeList.Count % recipe_Count;

        for (int i = 0 + ((current_page - 1) * recipe_Count); i < recipeList.Count; i++)
        {
            for (int n = 0; n < InventorySystem.ItemDB.Count; n++)
            {
                if (recipeList[i] == InventorySystem.ItemDB[n]["ImgName"].ToString())
                {
                    string[] cut_Recipe = InventorySystem.ItemDB[n]["Recipe"].ToString().Split("+");
                    Image Image1 = recipes.transform.GetChild(i).Find("Slot1").Find("Image").GetComponent<Image>();
                    Image Image2 = recipes.transform.GetChild(i).Find("Slot2").Find("Image").GetComponent<Image>();
                    Image ResultImage = recipes.transform.GetChild(i).Find("ResultSlot").Find("Image").GetComponent<Image>();

                    if (recipes.transform.GetChild(i).Find("Slot1").Find("Image").gameObject.activeSelf == false)
                    {
                        recipes.transform.GetChild(i).Find("Slot1").Find("Image").gameObject.SetActive(true);
                    }

                    if (Image1.enabled == false)
                    {
                        Image1.enabled = true;
                    }

                    if (Image2.enabled == false)
                    {
                        Image2.enabled = true;
                    }

                    if (ResultImage.enabled == false)
                    {
                        ResultImage.enabled = true;
                    }

                    Image1.sprite = Resources.Load<Sprite>("Image/" + cut_Recipe[0]);
                    Image2.sprite = Resources.Load<Sprite>("Image/" + cut_Recipe[1]);
                    ResultImage.sprite = Resources.Load<Sprite>("Image/" + recipeList[i]);
                    break;
                }
            }

            Debug.Log("i : " + i);

            //문제점
            //if (i == current_page * recipe_Count)
            //{
            //    break;
            //}
        }
    }

    public void Next_Page()
    {
        if (max_page > current_page)
        {
            current_page++;
            //pageText.text = current_page.ToString();
            Reload_Information();
        }
    }

    public void Back_Page()
    {
        if (current_page != 1)
        {
            current_page--;
           // pageText.text = current_page.ToString();
            Reload_Information();
        }
    }
}
