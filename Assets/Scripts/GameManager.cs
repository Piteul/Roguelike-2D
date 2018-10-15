﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float turnDelay = 0.1f;
    public static GameManager instance = null;
    public BoardManager boardScript;
    public int playerFoodPoints = 100;
    [HideInInspector]
    public bool playersTurn = true;

    private int level = 3;
    private List<Enemy> enemies;
    private bool enemiesMoving;

    private void Awake() {

        if(instance == null) {
            instance = this;
        }
        else if(instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Call this to add the passed in Enemy to the List of Enemy objects.
    public void AddEnemyToList(Enemy script) {
        //Add Enemy to List enemies.
        enemies.Add(script);
    }

    public void GameOver() {

        enabled = false;
    }

    void InitGame() {
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    //Coroutine to move enemies in sequence.
    IEnumerator MoveEnemies() {
        //While enemiesMoving is true player is unable to move.
        enemiesMoving = true;

        //Wait for turnDelay seconds, defaults to .1 (100 ms).
        yield return new WaitForSeconds(turnDelay);

        //If there are no enemies spawned (IE in first level):
        if (enemies.Count == 0) {
            //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
            yield return new WaitForSeconds(turnDelay);
        }

        //Loop through List of Enemy objects.
        for (int i = 0; i < enemies.Count; i++) {
            //Call the MoveEnemy function of Enemy at index i in the enemies List.
            enemies[i].MoveEnemy();

            //Wait for Enemy's moveTime before moving next Enemy, 
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        //Once Enemies are done moving, set playersTurn to true so player can move.
        playersTurn = true;

        //Enemies are done moving, set enemiesMoving to false.
        enemiesMoving = false;
    }
}
