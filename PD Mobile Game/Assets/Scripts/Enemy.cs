using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] GameObject target;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = target.transform.position;
    }
}
