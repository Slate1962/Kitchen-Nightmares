using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed = 5f;
    private Crate nearbyCrate; 
    private Ingredient heldIngredient;
    public Transform frontPoint;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnTriggerEnter(Collider other)
    {
        Crate crate = other.GetComponent<Crate>();
        if (crate != null)
        {
            nearbyCrate = crate;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Crate crate = other.GetComponent<Crate>();
        if (crate != null)
        {
            nearbyCrate = null;
        }
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
            if (heldIngredient == null && nearbyCrate != null)
            {
                GameObject newIngredientObject = Instantiate(nearbyCrate.ingredientPrefab);
                heldIngredient = newIngredientObject.GetComponent<Ingredient>();

                newIngredientObject.transform.SetParent(frontPoint);
                newIngredientObject.transform.localPosition = Vector3.zero;
                newIngredientObject.transform.localRotation = Quaternion.identity;

                heldIngredient.type = nearbyCrate.ingredientType;
                heldIngredient.needsChopping = nearbyCrate.needsChopping;
                heldIngredient.needsCooking = nearbyCrate.needsCooking;
            }
        }
    }
}