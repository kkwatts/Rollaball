using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour {
    public Transform player;

    private NavMeshAgent navMeshAgent;
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if (anim) {
            anim.SetFloat("speed_f", navMeshAgent.velocity.x + navMeshAgent.velocity.z);
        }
    }

    // Update is called once per frame
    void Update() {
        anim.SetFloat("speed_f", navMeshAgent.velocity.x + navMeshAgent.velocity.z);
        if (Vector3.Distance(transform.position, player.position) <= 20f) {
            navMeshAgent.SetDestination(player.position);
        }
        Debug.Log(anim.GetFloat("speed_f"));
    }
}