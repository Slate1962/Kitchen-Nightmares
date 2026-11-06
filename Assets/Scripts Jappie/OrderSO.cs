using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrderSO", menuName = "jasper Sandbox/OrderSO")]
public class OrderSO : ScriptableObject
{
    public string orderName;
    public List<FoodSO> foods;   // what has to be on the plate, in any order
    public int reward = 20;
}