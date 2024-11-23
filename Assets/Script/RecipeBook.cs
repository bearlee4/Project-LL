using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour
{
    public GameObject recipe_UI;
    //이게 진짜
    public GameObject recipeBook_UI;
    public GameObject triple_recipieBook_UI;
    public GameObject recipe_content;
    public GameObject recipe_Slot;
    public GameObject recipes;
    //public Text pageText;
    public List<string> recipeList = new List<string>();
    public List<string> triple_recipeList = new List<string>();

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
        Set_Recipe(2);
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

    public void Open_Triple_Recipebook()
    {
        triple_recipieBook_UI.SetActive(true);
        open_recipe_ui = true;
        Set_Recipe(3);
    }

    public void Close_Triple_Recipebook()
    {
        triple_recipieBook_UI.SetActive(false);
        open_recipe_ui = false;
    }

    public void Add_Recipe(string name)
    {
        for (int i = 0; i < InventorySystem.ItemDB.Count; i++)
        {
            if (name == InventorySystem.ItemDB[i]["ImgName"].ToString())
            {
                string[] cut_Recipe = InventorySystem.ItemDB[i]["Recipe"].ToString().Split("+");
                if (cut_Recipe.Length == 2)
                {
                    recipeList.Add(name);
                    Instantiate(recipe_Slot, recipe_content.transform);
                    break;
                }

                else if (cut_Recipe.Length == 3)
                {
                    triple_recipeList.Add(name);
                    break;
                }
            }
        }
        Debug.Log("레시피 추가");
    }

    public void Set_Recipe(int count)
    {
        if (count == 2)
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

        else if (count == 3)
        {
            for (int i = 0; i < triple_recipeList.Count; i++)
            {
                for (int n = 0; n < InventorySystem.ItemDB.Count; n++)
                {
                    if (triple_recipeList[i] == InventorySystem.ItemDB[n]["ImgName"].ToString())
                    {
                        string[] cut_Recipe = InventorySystem.ItemDB[n]["Recipe"].ToString().Split("+");
                        Image Image1 = triple_recipieBook_UI.transform.Find("Recipe List").GetChild(i).Find("Slot1").Find("Image").GetComponent<Image>();
                        Image Image2 = triple_recipieBook_UI.transform.Find("Recipe List").GetChild(i).Find("Slot2").Find("Image").GetComponent<Image>();
                        Image Image3 = triple_recipieBook_UI.transform.Find("Recipe List").GetChild(i).Find("Slot3").Find("Image").GetComponent<Image>();
                        Image ResultImage = triple_recipieBook_UI.transform.Find("Recipe List").GetChild(i).Find("ResultSlot").Find("Image").GetComponent<Image>();

                        if (triple_recipieBook_UI.transform.Find("Recipe List").GetChild(i).Find("Slot1").Find("Image").gameObject.activeSelf == false)
                        {
                            triple_recipieBook_UI.transform.Find("Recipe List").GetChild(i).Find("Slot1").Find("Image").gameObject.SetActive(true);
                        }

                        if (Image1.enabled == false)
                        {
                            Image1.enabled = true;
                        }

                        if (Image2.enabled == false)
                        {
                            Image2.enabled = true;
                        }

                        if (Image3.enabled == false)
                        {
                            Image3.enabled = true;
                        }

                        if (ResultImage.enabled == false)
                        {
                            ResultImage.enabled = true;
                        }

                        Image1.sprite = Resources.Load<Sprite>("Image/" + cut_Recipe[0]);
                        Image2.sprite = Resources.Load<Sprite>("Image/" + cut_Recipe[1]);
                        Image3.sprite = Resources.Load<Sprite>("Image/" + cut_Recipe[2]);
                        ResultImage.sprite = Resources.Load<Sprite>("Image/" + triple_recipeList[i]);
                        break;

                    }
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

        if (remainder != 0)
        {
            max_page++;
        }

        for (int i = 0 + ((current_page - 1) * recipe_Count); i < recipeList.Count; i++)
        {
            for (int n = 0; n < InventorySystem.ItemDB.Count; n++)
            {
                if (recipeList[i] == InventorySystem.ItemDB[n]["ImgName"].ToString())
                {
                    string[] cut_Recipe = InventorySystem.ItemDB[n]["Recipe"].ToString().Split("+");
                    Image Image1 = recipes.transform.GetChild(i - ((current_page - 1) * recipe_Count)).Find("Slot1").Find("Image").GetComponent<Image>();
                    Image Image2 = recipes.transform.GetChild(i - ((current_page - 1) * recipe_Count)).Find("Slot2").Find("Image").GetComponent<Image>();
                    Image ResultImage = recipes.transform.GetChild(i - ((current_page - 1) * recipe_Count)).Find("ResultSlot").Find("Image").GetComponent<Image>();

                    if (recipes.transform.GetChild(i - ((current_page - 1) * recipe_Count)).Find("Slot1").Find("Image").gameObject.activeSelf == false)
                    {
                        recipes.transform.GetChild(i - ((current_page - 1) * recipe_Count)).Find("Slot1").Find("Image").gameObject.SetActive(true);
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

            //문제점
            if (i == (current_page * recipe_Count - 1))
            {
                break;
            }
        }
    }

    public void Image_Reset()
    {
        for (int i = 0; i < recipes.transform.childCount; i ++)
        {
            Image Image1 = recipes.transform.GetChild(i).Find("Slot1").Find("Image").GetComponent<Image>();
            Image Image2 = recipes.transform.GetChild(i).Find("Slot2").Find("Image").GetComponent<Image>();
            Image ResultImage = recipes.transform.GetChild(i).Find("ResultSlot").Find("Image").GetComponent<Image>();

            Image1.sprite = null;
            Image1.enabled = false;
            Image2.sprite = null;
            Image2.enabled = false;
            ResultImage.sprite = null;
            ResultImage.enabled = false;
        }
    }

    public void Next_Page()
    {
        if (max_page > current_page)
        {
            current_page++;
            Image_Reset();
            //pageText.text = current_page.ToString();
            Reload_Information();
        }
    }

    public void Back_Page()
    {
        if (current_page != 1)
        {
            current_page--;
            Image_Reset();
            // pageText.text = current_page.ToString();
            Reload_Information();
        }
    }

    public void Change_Double()
    {
        Close_Triple_Recipebook();
        Test_Open_Recipebook();
    }

    public void Change_Triple()
    {
        Test_Close_Recipebook();
        Open_Triple_Recipebook();
    }
}
