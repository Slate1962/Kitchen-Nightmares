using UnityEngine;

public class Crate : InteractableCC
{
    public FoodSO food;
    [SerializeField] private Animator animator;

    public override bool CanInteract(PlayerMovement player)
    {
        return player.HeldItem == null;
    }

    public override void Interact(PlayerMovement player)
    {
        if (food == null || food.prefab == null)
        {
            Debug.LogError(name + " has no food or no prefab assigned");
            return;
        }

        Transform foodTransform = Instantiate(food.prefab);
        FoodObjects foodObject = foodTransform.GetComponent<FoodObjects>();

        if (foodObject == null)
        {
            Debug.LogError(food.name + " prefab has no FoodObjects component");
            Destroy(foodTransform.gameObject);
            return;
        }

        player.PickUp(foodObject);

        Debug.Log("Crate Interact reached, animator = " + animator, this);

        if (animator == null)
        {
            Debug.LogWarning(name + " has no Animator assigned", this);
        }
        else
        {
            animator.SetTrigger("OpenClose");
        }
    }
}