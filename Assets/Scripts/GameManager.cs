using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public float turnDelay = 0.1f;
    public float levelStartDelay = 2f;

    public BoardManager boardScript;
    public int difficultyLevel = 0;
    public int playerFoodPoints = 100;
    
    [HideInInspector] public bool playersTurn = true;

    private List<Enemy> enemies;
    private bool enemiesMoving = false;
    private GameObject levelImage;
    private Text levelText;
    //private Text foodText;


    private void Awake()
    {
        //Make the GameManager a Singleton
        if(GameManager.instance == null){
            GameManager.instance = this;
        } else if (GameManager.instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<BoardManager>();
        enemies = new List<Enemy>();
    }

    void InitGame()
    {
        boardScript.SetupScene(difficultyLevel);
        enemies.Clear();
    }

    private void Start()
    {
        InitGame();
    }

    public void GameOver()
    {
        enabled = false;
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;

        yield return new WaitForSeconds(turnDelay);

        if(enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].TryMove();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        playersTurn = true;
        enemiesMoving = false;
    }

    private void Update()
    {
        if(!playersTurn && !enemiesMoving)
        {
            StartCoroutine(MoveEnemies());
        }
    }

    public void AddEnemyToList(Enemy enemy)
    {
        enemies.Add(enemy);
    }
}
