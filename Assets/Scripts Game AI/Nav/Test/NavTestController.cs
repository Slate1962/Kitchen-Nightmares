using System.Threading;
using Unity.GraphToolkit.Editor.GraphVisualization;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class NavTestController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public InputActionReference pointerPosition;
    public void OnClickToMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 screenPos = pointerPosition.action.ReadValue<Vector2>();

            Ray ray = cam.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                agent.SetDestination(hit.point);
            }
        }
    }
}