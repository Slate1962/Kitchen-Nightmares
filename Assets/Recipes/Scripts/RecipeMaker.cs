using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RecipeMaker : MonoBehaviour
{
    System.Random rng = new System.Random();
    int maxBurgerIngredients = 13;
    List<string> burgerIngredients = new List<string>();
    public string recipe;
    void Start()
    {
        burgerIngredients.Add("Lettuce");
        burgerIngredients.Add("Onion");
        burgerIngredients.Add("Tomato");
        burgerIngredients.Add("Cheese");
        burgerIngredients.Add("Burger");
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            recipe = "Bread+";
            int ingredientCount = rng.Next(2, maxBurgerIngredients);
            int burgerPos = rng.Next(0, ingredientCount);
            for (int i = 0; i < ingredientCount; i++)
            {
                if (i == burgerPos)
                {
                    recipe = recipe + "Burger+";
                }
                else
                {
                    recipe = recipe + burgerIngredients[rng.Next(0, burgerIngredients.Count)] + "+";
                }
            }
            recipe = recipe + "Bread";
            Debug.Log(recipe);
            GameObject.Find("Canvas").GetComponent<RecipeDisplay>().UpdateRecipe(recipe);
        }
                
    }
}
