using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

    public int playerDamage;

    [Header("Sounds")]
    public AudioClip attackSound1;
    public AudioClip attackSound2;

    private Animator animator;
    private Transform target;
    private bool skipMove;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
    protected override void Start () {
        GameManager.instance.AddEnemyToList(this);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    protected override bool AttempMove(int xDir, int yDir)
    {
        bool canMove = false;

        if (!skipMove)
        {
            canMove = base.AttempMove(xDir, yDir);
        }

        skipMove = !skipMove;

        return canMove;
    }

    public void TryMove()
    {
        int horizontal = 0, vertical = 0;

        if(Mathf.Abs(target.position.x - transform.position.x) > Mathf.Epsilon)
        {
            horizontal = (target.position.x > transform.position.x) ? 1 : -1;
        } else
        {
            if (Mathf.Abs(target.position.y - transform.position.y) > Mathf.Epsilon)
            {
                vertical = (target.position.y > transform.position.y) ? 1 : -1;
            }
        }
        
        if(horizontal !=  0 || vertical != 0)
        {
            AttempMove(horizontal, vertical);
        }
    }

    protected override void OnMovementFail(GameObject obstacle)
    {
        Player hitPlayer = obstacle.GetComponent<Player>();

        if(hitPlayer != null)
        {
            hitPlayer.LoseFood(playerDamage);
            SoundManager.instance.RandomizeSFX(attackSound1, attackSound2);
            animator.SetTrigger("enemyAttack");
        }
    }
}
