using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ThingsInTableMix : MonoBehaviour
{
    [SerializeField] Button MixIngredientsButton;

    public List<GameObject> IngredientsIn = new();
    public List<string> ingredients = new();
    public List<Recipe> Recipes = new();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Ingredient>())
        {
            IngredientsIn.Add(collision.gameObject);
            collision.gameObject.GetComponent<Ingredient>().InTableMix = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Ingredient>())
        {
            IngredientsIn.Remove(collision.gameObject);
            collision.gameObject.GetComponent<Ingredient>().InTableMix = false;
        }
    }
    private void Start()
    {
        MixIngredientsButton.onClick.AddListener(MixIngredients);
    }
    private void MixIngredients()
    {
        ingredients.Clear();
        for (int i = 0; i < IngredientsIn.Count; i++)
        {
            ingredients.Add(IngredientsIn[i].GetComponent<Ingredient>().IngredientName);
        }
        ingredients.Sort();
        for (int i = 0; i < Recipes.Count; i++)
        {
            List<string> IngredientStrings = new();
            for (int k = 0; k < Recipes[i].IngredientsForRecipe.Count; k++)
            {
                IngredientStrings.Add(Recipes[i].IngredientsForRecipe[k].GetComponent<Ingredient>().IngredientName);
            }
            IngredientStrings.Sort();
            if (ingredients.SequenceEqual(IngredientStrings))
            {
                if (Recipes[i].OutPut != null)
                {
                    Instantiate(Recipes[i].OutPut, IngredientsIn[i].transform.position, IngredientsIn[i].transform.rotation, transform.parent);
                    for (int j = IngredientsIn.Count - 1; j >= 0; j--)
                    {
                        Destroy(IngredientsIn[j]);
                    }
                    
                    break;
                }
            }
        }
    }
    [Serializable]
    public class Recipe
    {
        public List<GameObject> IngredientsForRecipe;
        public GameObject OutPut;
    }
}
