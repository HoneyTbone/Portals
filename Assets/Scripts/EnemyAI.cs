using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private GameObject Player;
    public float Distance;

    public NavMeshAgent _agent;

    public float attackRange = 2.5f;

    // Update is called once per frame

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {      
            Distance = Vector3.Distance(Player.transform.position, this.transform.position);
                _agent.SetDestination(Player.transform.position);

        if (Distance < attackRange)
        {
            //Debug.Log("Attacking");
            GetComponentInChildren<Animator>().SetTrigger("Attack");
        }
    }
}