using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed = 5f;
    public Transform frontPoint;
    private List<InteractableCC> nearbyInteractables = new List<InteractableCC>();

    public CarryableCC HeldItem { get; private set; }

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnTriggerEnter(Collider other)
    {

        Debug.Log("Touched: " + other.name);
        InteractableCC interactable = other.GetComponent<InteractableCC>();
        if (interactable != null)
        {
            nearbyInteractables.Add(interactable);
            Debug.Log("Added: " + interactable.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        InteractableCC interactable = other.GetComponent<InteractableCC>();
        if (interactable != null)
        {
            nearbyInteractables.Remove(interactable);
        }
    }

    public void PickUp(CarryableCC item)
    {
        HeldItem = item;
        item.transform.SetParent(frontPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    public CarryableCC Release()
    {
        CarryableCC item = HeldItem;
        HeldItem = null;
        return item;
    }

    InteractableCC GetClosestInteractable()
    {
        InteractableCC closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (InteractableCC interactable in nearbyInteractables)
        {
            if (!interactable.CanInteract(this))
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, interactable.transform.position);
            if (distance < closestDistance)
            {
                closest = interactable;
                closestDistance = distance;
            }
        }

        return closest;
    }

    void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Keyboard.current.aKey.isPressed) horizontal -= 1f;
        if (Keyboard.current.dKey.isPressed) horizontal += 1f;
        if (Keyboard.current.wKey.isPressed) vertical += 1f;
        if (Keyboard.current.sKey.isPressed) vertical -= 1f;

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        controller.Move(direction * moveSpeed * Time.deltaTime);

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            InteractableCC target = GetClosestInteractable();
            Debug.Log("Nearby: " + nearbyInteractables.Count + " | Target: " + target + " | Held: " + HeldItem);

            if (target != null)
            {
                target.Interact(this);
            }
        }
    }
}