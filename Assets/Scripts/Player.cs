using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject
{
    public int damagePower = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public float nextLevelDelay = 1f;
    public Text foodText;

    [Header("Sounds")]
    public AudioClip chopSound1;
    public AudioClip chopSound2;
    public AudioClip moveSound1;
    public AudioClip moveSound2;
    public AudioClip drinkSound1;
    public AudioClip drinkSound2;
    public AudioClip eatSound1;
    public AudioClip eatSound2;
    public AudioClip gameOverSound;


    private Animator animator;
    private int food;

#if !(UNITY_STANDALONE || UNITY_WEBGL)
    private Vector2 touchOrigin = - Vector2.one;
#endif

    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        base.Awake();
    }

    protected override void Start()
    {
        food = GameManager.instance.playerFoodPoints;
        UpdateFoodText();
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
            food = 0;
            UpdateFoodText();
            foodText.enabled = false;
            SoundManager.instance.PlaySingle(gameOverSound);
            GameManager.instance.GameOver();
        }
    }

    void UpdateFoodText()
    {
        foodText.text = "Food: " + food;
        foodText.GraphicUpdateComplete();
    }

    protected override bool AttempMove(int xDir, int yDir)
    {
        food--;
        UpdateFoodText();
        SoundManager.instance.RandomizeSFX(moveSound1, moveSound2);
        bool canMove = base.AttempMove(xDir, yDir);
        CheckIfGameOver();
        GameManager.instance.playersTurn = false;

        return canMove;
    }

    protected override void OnMovementFail(GameObject obstacle)
    {
        Wall hitWall = obstacle.GetComponent<Wall>();

        if(hitWall != null)
        {
            hitWall.DamageWall(damagePower);
            SoundManager.instance.RandomizeSFX(chopSound1, chopSound2);
            animator.SetTrigger("playerChop");
        }
    }

    private void Update()
    {
        if (GameManager.instance.playersTurn && !GameManager.instance.gameOver && !isMoving)
        {
            int horizontal = 0;
            int vertical = 0;

#if UNITY_STANDALONE || UNITY_WEBGL
            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxisRaw("Vertical");
#else
            if (Input.touchCount > 0) {
                Touch userTouch = Input.touches[0];

                if(userTouch.phase == TouchPhase.Began)
                {
                    touchOrigin = userTouch.position;
                } else if(userTouch.phase == TouchPhase.Ended && touchOrigin != -Vector2.one)
                {
                    Vector2 touchEnd = userTouch.position;
                    float x = touchEnd.x - touchOrigin.x;
                    float y = touchEnd.y - touchOrigin.y;

                    if(x != 0 || y != 0)
                    {
                        if (Mathf.Abs(x) > Mathf.Abs(y))
                        {
                            horizontal = (int)Mathf.Sign(x);
                        }
                        else if (Mathf.Abs(x) < Mathf.Abs(y))
                        {
                            vertical = (int)Mathf.Sign(y);
                        }
                    }
                }
            };
#endif
            if ((horizontal != 0) || (vertical != 0))
            {
                // Block Diagonal Movement
                if (horizontal != 0)
                {
                    vertical = 0;
                }

                if(AttempMove(horizontal, vertical))
                {
                    SoundManager.instance.RandomizeSFX(moveSound1, moveSound2);
                }
            }
        } else if (GameManager.instance.gameOver && Input.GetKeyDown(KeyCode.R))
        {
            Invoke("StartNewGame", nextLevelDelay);
        }
    }

    public void StartNewGame()
    {
        GameManager.instance.Restart();
        NextLevel();
    }

    public void LoseFood(int loss)
    {
        food -= loss;
        UpdateFoodText();
        animator.SetTrigger("playerHit");
        CheckIfGameOver();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Exit"))
        {
            Invoke("NextLevel", nextLevelDelay);
            enabled = false;
        }
        else if (other.CompareTag("Food"))
        {
            food += pointsPerFood;
            SoundManager.instance.RandomizeSFX(eatSound1, eatSound2);
            UpdateFoodText();
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("Soda"))
        {
            food += pointsPerSoda;
            SoundManager.instance.RandomizeSFX(drinkSound1, drinkSound2);
            other.gameObject.SetActive(false);
        }
    }

    public void NextLevel()
    {
        food = GameManager.instance.playerFoodPoints;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
