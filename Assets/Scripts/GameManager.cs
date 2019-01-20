using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public BoardManager boardScript;
    public int difficultyLevel = 0;

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
}
