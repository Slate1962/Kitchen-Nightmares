using UnityEngine;
using UnityEngine.InputSystem;

public class ContainerCounter : MonoBehaviour
{


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