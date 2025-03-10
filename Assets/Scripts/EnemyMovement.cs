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
            anim.SetFloat("speed_f", navMeshAgent.speed);
        }
    }

    // Update is called once per frame
    void Update() {
        if (player != null) {
            navMeshAgent.SetDestination(player.position);
        }
    }
}