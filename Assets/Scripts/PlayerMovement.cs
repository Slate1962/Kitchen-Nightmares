using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float moveSpeed = 5f;
    private List<Crate> nearbyCrates = new List<Crate>();
    private List<TableCC> nearbyTables = new List<TableCC>();
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
            nearbyCrates.Add(crate);
        }

        TableCC table = other.GetComponent<TableCC>();
        if (table != null)
        {
            nearbyTables.Add(table);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Crate crate = other.GetComponent<Crate>();
        if (crate != null)
        {
            nearbyCrates.Remove(crate);
        }

        TableCC table = other.GetComponent<TableCC>();
        if (table != null)
        {
            nearbyTables.Remove(table);
        }
    }

    Crate GetClosestCrate()
    {
        Crate closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Crate crate in nearbyCrates)
        {
            float distance = Vector3.Distance(transform.position, crate.transform.position);
            if (distance < closestDistance)
            {
                closest = crate;
                closestDistance = distance;
            }
        }

        return closest;
    }

    TableCC GetClosestTable()
    {
        TableCC closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (TableCC table in nearbyTables)
        {
            float distance = Vector3.Distance(transform.position, table.transform.position);
            if (distance < closestDistance)
            {
                closest = table;
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
            Crate nearbyCrate = GetClosestCrate();
            TableCC nearbyTable = GetClosestTable();

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
            else if (heldIngredient != null && nearbyTable != null && nearbyTable.heldIngredient == null)
            {
                heldIngredient.transform.SetParent(nearbyTable.placePoint);
                heldIngredient.transform.localPosition = Vector3.zero;
                heldIngredient.transform.localRotation = Quaternion.identity;

                nearbyTable.heldIngredient = heldIngredient;
                heldIngredient = null;
            }
            else if (heldIngredient == null && nearbyTable != null && nearbyTable.heldIngredient != null)
            {
                heldIngredient = nearbyTable.heldIngredient;
                nearbyTable.heldIngredient = null;

                heldIngredient.transform.SetParent(frontPoint);
                heldIngredient.transform.localPosition = Vector3.zero;
                heldIngredient.transform.localRotation = Quaternion.identity;
            }
        }
    }
}