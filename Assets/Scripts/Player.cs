using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    public int damagePower = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restartLevelDelay = 1f;

    private Animator animator;
    private int food;

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
    }

    protected override void Start()
    {
        food = GameManager.instance.playerFoodPoints;
        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    void CheckIfGameOver()
    {
        if(food <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    protected override void AttempMove(int xDir, int yDir)
    {
        Debug.Log("Player Attempted to Move");
        food--;
        base.AttempMove(xDir, yDir);
        CheckIfGameOver();
        GameManager.instance.playersTurn = false;
        Debug.Log("Player Finished Moving");
    }

    protected override void OnMovementFail(GameObject obstacle)
    {
        Wall hitWall = obstacle.GetComponent<Wall>();

        if(hitWall != null)
        {
            hitWall.damageWall(damagePower);
            animator.SetTrigger("playerChop");
        }
    }

    private void Update()
    {
        if (GameManager.instance.playersTurn)
        {
            int horizontal = (int)Input.GetAxisRaw("Horizontal");
            int vertical = (int)Input.GetAxisRaw("Vertical");
            /*
            Debug.Log("Horizontal: " + horizontal);
            Debug.Log("Vertical: " + vertical);
            */
            if ((horizontal != 0) || (vertical != 0))
            {
                // Block Diagonal Movement
                if (horizontal != 0)
                {
                    vertical = 0;
                }

                AttempMove(horizontal, vertical);
            }
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseFood(int loss)
    {
        food -= loss;
        animator.SetTrigger("playerHit");
        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Exit"))
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.CompareTag("Food"))
        {
            food += pointsPerFood;
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Soda"))
        {
            food += pointsPerSoda;
            other.gameObject.SetActive(false);
        }
    }
}
