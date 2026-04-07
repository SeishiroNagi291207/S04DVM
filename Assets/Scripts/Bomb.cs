using UnityEngine;
using UnityEngine.AI;

public class Bomb : MonoBehaviour
{
    public float tiempoExplosion = 2f;
    public float radio = 5f;

    void Start()
    {
        Invoke(nameof(Explotar), tiempoExplosion);
    }

    void Explotar()
    {
        Debug.Log("BOOM");
        Collider[] objetos = Physics.OverlapSphere(transform.position, radio);

        foreach (Collider obj in objetos)
        {
            if (obj.CompareTag("Enemy"))
            {
                NavMeshAgent agent = obj.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.isStopped = true;
                }

                Destroy(obj.gameObject);
            }
        }

        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Explotar();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}