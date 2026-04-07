using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public Transform Player;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.speed = Random.Range(1, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null)
        {
            agent.SetDestination(Player.position);
            //agent.

        }
    }
    public void HasPath()
    {
        print(agent.hasPath);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (agent == null || agent.path == null) return;

        Vector3[] corners = agent.path.corners;


        for (int i = 0; i < corners.Length - 1; i++)
        {
            Gizmos.DrawLine(corners[i], corners[i + 1]);
            Gizmos.DrawSphere(corners[i], 0.2f);
        }
    }
}