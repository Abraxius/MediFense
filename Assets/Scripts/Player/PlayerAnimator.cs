using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Sorgt dafür das der Player animiert läuft
public class PlayerAnimator : MonoBehaviour
{

    const float locomationAnimationSmoothTime = .1f;

    NavMeshAgent agent;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("speedPercent",speedPercent, locomationAnimationSmoothTime, Time.deltaTime);
    }
}
