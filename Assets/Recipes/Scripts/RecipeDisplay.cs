using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class RecipeDisplay : MonoBehaviour
{
    //public string input = "Bread+Onion+Lettuce+Tomato+Burger+Bread";
    public List<RawImage> ingredientPrefabs = new List<RawImage>();
    List<RawImage> activeIngredientPrefabs = new List<RawImage>();
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void UpdateRecipe(string input)
    {
        List<string> activeIngredients = input.Split('+').ToList<string>();

        float stackPosition = 0;
        foreach(RawImage active in activeIngredientPrefabs)
        {
            Destroy(active.gameObject);      
        }
        activeIngredientPrefabs.Clear();

        foreach (string ingredient in activeIngredients)
        {
            RawImage ingredientImage = ingredientPrefabs.FirstOrDefault(img => img.name.StartsWith(ingredient) && img.name.EndsWith("Burger"));
            RawImage ingredientPrefab = Instantiate(ingredientImage, this.transform);
            float ingredientHeight = ingredientPrefab.GetComponent<RectTransform>().rect.height;
            ingredientPrefab.GetComponent<RectTransform>().localPosition = Vector3.zero;
            ingredientPrefab.GetComponent<RectTransform>().localPosition = new Vector3(this.transform.localPosition.x, stackPosition + ingredientHeight / 2f, this.transform.localPosition.z);
            stackPosition += ingredientHeight;
            activeIngredientPrefabs.Add(ingredientPrefab);
        }
        foreach(RawImage active in activeIngredientPrefabs)
        {
            active.GetComponent<RectTransform>().localPosition = new Vector3(active.transform.localPosition.x, active.transform.localPosition.y - stackPosition / 2f, active.transform.localPosition.z);
        }
        
    }
}
