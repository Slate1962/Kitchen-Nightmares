using UnityEngine;
using UnityEngine.AI;

public class NPCMoveToTarget : MonoBehaviour
{
    public bool Available = false;
    public Transform targetlocation;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {

        if (targetlocation != null && Available == true)
        {
            agent.SetDestination(targetlocation.position);
        }
    }
}
