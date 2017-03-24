using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameViewController : MonoBehaviour {

    public Transform field;
    public GameObject matchPrefab;

    public Text leftText;
    public Text lastNumberText;
    public Text selectedText;
    public GameObject doTurnBtn;

    public GameObject yourTurnHolder;
    public GameObject enemyTurnHolder;

    public GameObject winHolder;
    public GameObject loseHolder;


    List<MatchView> matches = new List<MatchView>();
    GameController controller;
    Game game;

    int selectedCount = 0;

    void Awake()
    {
        controller = GetComponent<GameController>();
        controller.OnInitEvent += OnInit;
        controller.OnPlayerChangedEvent += OnPlayerChanged;
        controller.OnGameEndedEvent += OnGameEnded;
        controller.OnEnemySelectEvent += OnEnemySelect;
    }

    void OnInit(int matchesCount, int currentPlayer)
    {
        game = controller.game;

        const float cell = 0.7f;

        for (int i = 0; i < matchesCount; i++)
        {
            GameObject go = Instantiate(matchPrefab);
            go.transform.SetParent(field);
            Vector3 pos = new Vector3();
            pos.x = cell * i - (matchesCount * cell / 2);
            go.transform.localPosition = pos;
            matches.Add(go.GetComponent<MatchView>());
        }



        //Simple fit camera size
        Camera.main.orthographicSize = 6 + (matchesCount - 20) / 5;

        UpdateInfo();
        OnPlayerChanged(game.GetCurrentPlayer());
    }

    void OnEnemySelect(int number)
    {
        MatchView view;
        if (number >= matches.Count)
        {
            if (matches.Count == 0)
            {
                Debug.LogError("Matches not found!");
                return;
            }

            view = matches[matches.Count - 1];
        }
        else
        {
            view = matches[number - 1];
        }

        selectedCount = SelectMatches(view);
        if (selectedCount == 0)
        {
            Debug.LogError("Something wrong!");
        }
        else
        {
            StartCoroutine("DelayTurn");
        }
    }

    IEnumerator DelayTurn()
    {
        yield return new WaitForSeconds(1);

        DoTurn();
    }

    void Update()
    {
        if (game.GetCurrentPlayer() == 0)
        {
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Match")
                {
                    GameObject go = hit.transform.gameObject;
                    selectedCount = SelectMatches(go.GetComponent<MatchView>());
                    UpdateInfo();
                }

            }
        }
    }

    void UnSelect()
    {
        foreach (MatchView view in matches)
        {
            view.SetColor(Color.white);
        }
    }

    int SelectMatches(MatchView last)
    {
        UnSelect();

        int max = game.GetMax();
        int min = game.GetMin();
        int count = 0;
        int selected = 0;
        bool afterSelected = false;

        Color select = Color.green;
        if (game.GetCurrentPlayer() == 1)
            select = Color.cyan;

        foreach (MatchView view in matches)
        {
            Color color = select;
            
            if (count < min)
            {
                selected++;
            }
            else if (count < max && !afterSelected)
            {
                selected++;
            }
            else
            {
                if (afterSelected)
                    color = Color.white;
                else
                    color = Color.red;
            }

            count++;

            view.SetColor(color);
            if (view == last)
                afterSelected = true;
        }

        return selected;
    }

    public void DoTurn()
    {
        if (selectedCount > 0)
        {
            for (int i = 0; i < selectedCount; i++)
            {
                if (matches.Count == 0)
                {
                    Debug.LogError("Array is empty!");
                    break;
                }
                Destroy(matches[0].gameObject);
                matches.RemoveAt(0);
            }

            controller.DoTurn(selectedCount);
        }
    }
    
    void UpdateInfo()
    {
        leftText.text = "Осталось: " + game.GetLeft();
        lastNumberText.text = "Убрано: " + game.GetLastNumber();

        doTurnBtn.SetActive(selectedCount > 0);
        selectedText.text = "Выбрано: " + selectedCount;
    }

    void OnPlayerChanged(int currentPlayer)
    {
        selectedCount = 0;
        yourTurnHolder.SetActive(currentPlayer == 0);
        enemyTurnHolder.SetActive(currentPlayer == 1);
        
        UpdateInfo();
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void OnGameEnded(int winner)
    {
        UpdateInfo();
        yourTurnHolder.SetActive(false);
        enemyTurnHolder.SetActive(false);

        winHolder.SetActive(winner == 0);
        loseHolder.SetActive(winner == 1);

    }
}
