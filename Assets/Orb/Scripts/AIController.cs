﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public GameObject explosion;
    public GameObject target;
    public GameObject bullet;

    NavMeshAgent agent;
    enum STATE { WANDER, CHASE, ATTACK, DEAD };
    STATE state = STATE.WANDER;

    float DistanceToPlayer()
    {
        return Vector3.Distance(target.transform.position, this.transform.position);
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        agent.baseOffset = Random.Range(0.01f, 0.08f);
        target = GameObject.FindGameObjectWithTag("Player");
    }

    public void BlowUp()
    {
        GameObject explode = Instantiate(explosion, this.transform.position + new Vector3(0, agent.baseOffset, 0), explosion.transform.rotation);
        Destroy(explode, 2);
        Destroy(this.gameObject);
    }

    bool canShoot = true;
    void Shoot()
    {
        if (!canShoot) return;
        GameObject bulletObj = Instantiate(bullet, this.transform.position + this.transform.forward, this.transform.rotation);
        bulletObj.GetComponent<Rigidbody>().AddForce(this.transform.forward * 1000);
        canShoot = false;
        Invoke("CanShoot", 1.5f);
    }

    void CanShoot()
    {
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case STATE.WANDER:
                if (CanSeePlayer())
                {
                    state = STATE.CHASE;
                    agent.stoppingDistance = 5;
                }
                else if (!agent.hasPath)
                {
                    float newX = this.transform.position.x + Random.Range(-50, 50);
                    float newZ = this.transform.position.z + Random.Range(-50, 50);
                    NavMeshHit hit;
                    Vector3 randomPoint = new Vector3(newX, 0, newZ);
                    if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                    {
                        Vector3 dest = hit.position;
                        agent.SetDestination(dest);
                        agent.stoppingDistance = 0;
                    }

                }
                break;
            case STATE.CHASE:
                agent.SetDestination(target.transform.position);

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    state = STATE.ATTACK;
                }
                break;
            case STATE.ATTACK:
                this.transform.LookAt(target.transform.position + Vector3.up);
                Shoot();
                if (DistanceToPlayer() > agent.stoppingDistance + 2)
                {
                    state = STATE.CHASE;
                }
                break;
        }

    }

    bool CanSeePlayer()
    {
        return target.CompareTag("Player");
    }
}
