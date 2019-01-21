using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

    public int playerDamage;

    private Animator animator;
    private Transform target;
    private bool skipMove;

    protected override void Awake()
    {
        animator.GetComponent<Animator>();
        base.Awake();
    }

    // Use this for initialization
    protected override void Start () {
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player").transform;
	}

    protected override void AttempMove(int xDir, int yDir)
    {
        if (!skipMove)
        {
            base.AttempMove(xDir, yDir);
        }

        skipMove = !skipMove;
    }

    public void Move()
    {
        int horizontal = 0, vertical = 0;

        if(Mathf.Abs(target.position.x - transform.position.x) > Mathf.Epsilon)
        {
            vertical = (target.position.x > transform.position.x) ? 1 : -1;
        } else
        {
            horizontal = (target.position.y > transform.position.y) ? 1 : -1;
        }

        AttempMove(horizontal, vertical);
    }

    protected override void OnMovementFail(GameObject obstacle)
    {
        Player hitPlayer = obstacle.GetComponent<Player>();

        if(hitPlayer != null)
        {
            hitPlayer.LoseFood(playerDamage);
            animator.SetTrigger("enemyAttack");
        }
    }
}
