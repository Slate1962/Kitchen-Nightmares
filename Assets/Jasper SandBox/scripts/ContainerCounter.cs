using UnityEngine;
using UnityEngine.InputSystem;

public class ContainerCounter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private FoodSO kitchenObjectSO;            // sleep hier je FoodSO asset in
    [SerializeField] private Transform SpawnPoint;  // waar het object verschijnt

    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SpawnFood();
        }

    }

    public void SpawnFood()
    {



        Transform KitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, SpawnPoint);
        KitchenObjectTransform.localPosition = Vector3.zero; 
        Debug.Log(KitchenObjectTransform.GetComponent<FoodObjects>().GetKitchenObjectSO().foodName);
        
    }

}
