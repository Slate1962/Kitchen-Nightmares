using UnityEngine;

public abstract class InteractableCC : MonoBehaviour
{
    public abstract bool CanInteract(PlayerMovement player);
    public abstract void Interact(PlayerMovement player);

    public virtual bool CanInteractAlternate(PlayerMovement player) { return false; }
    public virtual void InteractAlternate(PlayerMovement player) { }
}