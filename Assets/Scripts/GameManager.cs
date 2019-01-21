using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public BoardManager boardScript;
    public int difficultyLevel = 0;
    public int playerFoodPoints = 100;

    [HideInInspector] public bool playersTurn = true;

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
    }

    private void Start()
    {
        boardScript.SetupScene(difficultyLevel);
    }

    public void GameOver()
    {
        enabled = false;
    }
}
