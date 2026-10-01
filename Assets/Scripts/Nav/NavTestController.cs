using System.Threading;
using Unity.GraphToolkit.Editor.GraphVisualization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NavTestController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;

    // Assign your Pointer Position action here in the inspector
    public InputActionReference pointerPosition;

    // This is the function you will assign in the Player Input Unity Events
    public void OnClickToMove(InputAction.CallbackContext context)
    {
        // Only run the movement logic when the click is actually performed
        if (context.performed)
        {
            // Read the current position of the pointer
            Vector2 screenPos = pointerPosition.action.ReadValue<Vector2>();

            Ray ray = cam.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}