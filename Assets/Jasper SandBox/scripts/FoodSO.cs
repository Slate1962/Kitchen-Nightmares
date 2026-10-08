using UnityEngine;





[CreateAssetMenu(fileName = "FoodSO", menuName = "jasper Sandbox/FoodSO")]
public class FoodSO : ScriptableObject
{
    public Transform prefab;
    public string foodName;
    public Sprite foodSprite;
    public float cookTime;
    public float burnTime;
    public FoodSO slicedVersion;
    public int cuttingProgressMax = 3;

}
