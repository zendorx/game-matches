using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public delegate void OnInit(int matchesCount, int currentPlayer);
    public delegate void OnPlayerChanged(int currentPlayer);
    public delegate void OnGameEnded(int winner);
    public delegate void OnEnemySelect(int number);

    public event OnInit OnInitEvent;
    public event OnPlayerChanged OnPlayerChangedEvent;
    public event OnGameEnded OnGameEndedEvent;
    public event OnEnemySelect OnEnemySelectEvent;

    public TextAsset solutions;


    public const int min = 20;
    public const int max = 45;

    private int matchesCount;
    public bool cheaterAI;
    public bool myTurnFirst;
    public Game game;

    Enemy enemy;

	// Use this for initialization
	void Start () {
        game = new Game();

        int currentPlayer = 0;
        if (!myTurnFirst)
            currentPlayer = 1;

        if (cheaterAI)
        {
            enemy = new CheaterEnemy(solutions.text);
        }
        else
        {
            enemy = new RandomEnemy();
        }

        matchesCount = Random.Range(min, max + 1);
        game.Init(matchesCount, currentPlayer);

        if (OnInitEvent != null)
            OnInitEvent(matchesCount, currentPlayer);

        if (currentPlayer == 1)
        {
            MakeAITurn();
        }
    }

    void MakeAITurn()
    {
        int enemyNumber = enemy.DoTurn(game.GetLastNumber(), game.GetLeft(), game.GetMin(), game.GetMax());
        if (OnEnemySelectEvent != null)
            OnEnemySelectEvent(enemyNumber);
    }
	
    public void DoTurn(int number)
    {
        game.MakeTurn(number);
        if (game.IsGameOver())
        {
            if (OnGameEndedEvent != null)
                OnGameEndedEvent(game.GetCurrentPlayer());
        }
        else
        {
            if (OnPlayerChangedEvent != null)
                OnPlayerChangedEvent(game.GetCurrentPlayer());

            if (game.GetCurrentPlayer() == 1)
            {
                MakeAITurn();
            }
        }
    }
}
