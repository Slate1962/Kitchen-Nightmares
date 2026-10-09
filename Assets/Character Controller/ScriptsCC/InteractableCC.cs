using UnityEngine;

public abstract class InteractableCC : MonoBehaviour
{
    public abstract bool CanInteract(PlayerMovement player);
    public abstract void Interact(PlayerMovement player);
}