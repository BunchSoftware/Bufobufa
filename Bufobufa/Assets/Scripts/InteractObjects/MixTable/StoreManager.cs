using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public List<Ingredient> TypesIngredients = new();

    public static StoreManager Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    public void AddIngridient(string nameIngridient)
    {
        for (int i = 0; i < TypesIngredients.Count; i++)
        {
            if (TypesIngredients[i].name == nameIngridient)
            {
                TypesIngredients[i].Spawner.GetComponent<Spawner>().count++;
            }
        }
    }

    [Serializable]
    public class Ingredient
    {
        public string name;
        public GameObject Spawner;
    }
}
