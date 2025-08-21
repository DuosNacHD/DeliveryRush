using UnityEngine;
using Pathfinding;
public class AIAgent : MonoBehaviour
{
    AIPath path;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform target;

    private void Start()
    {
        path = GetComponent<AIPath>();
    }
    private void Update()
    {
        path.maxSpeed = moveSpeed;

        path.destination = target.position;
    }
}
