﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public float turnDelay = 0.1f;
    public float levelStartDelay = 2f;

    public BoardManager boardScript;
    public int level = 1;
    public int playerFoodPoints = 100;
    public bool doingSetup = false;
    

    [HideInInspector] public bool playersTurn = true;
    [HideInInspector] public bool gameOver = false;

    private List<Enemy> enemies;
    private bool enemiesMoving = false;
    private GameObject levelImage;
    private GameObject restartButton;
    private Text levelText;

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
        doingSetup = true;

#if !(UNITY_STANDALONE || UNITY_WEBGL)
        Text restartButtonText = GameObject.Find("LevelText").GetComponent<Text>();
        restartButtonText.text = "Touch me to Restart";
#endif

        levelImage = GameObject.Find("LevelImage");
        restartButton = GameObject.Find("RestartButton");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;

        restartButton.SetActive(false);
        levelImage.SetActive(true);


        enemies.Clear();
        boardScript.SetupScene(level);

        Invoke("HideLevelImage", levelStartDelay);
    }

    private void HideLevelImage()
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }

    public void GameOver()
    {
        levelText.text = "You died after " + level + " days.";
        levelImage.SetActive(true);
        restartButton.SetActive(true);
        //enabled = false;
        gameOver = true;
    }


    public void Restart()
    {
        playerFoodPoints = 100;
        level = 1;
        gameOver = false;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

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
        if(!playersTurn && !enemiesMoving && !doingSetup)
        {
            StartCoroutine(MoveEnemies());
        }
    }

    public void AddEnemyToList(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        InitGame();
        level++;
    }
}
