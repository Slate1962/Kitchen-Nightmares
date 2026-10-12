using UnityEngine;


[CreateAssetMenu(fileName = "FoodSO", menuName = "jasper Sandbox/FoodSO")]
public class FoodSO : ScriptableObject
{
    public Transform prefab;
    public string foodName;
    public Sprite foodSprite;
    public FoodSO friedVersion;
    public FoodSO slicedVersion;
    public int cuttingProgressMax = 3;

}

