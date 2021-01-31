using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour {

    public GameObject player;

    public Button start_button;

    private Board board;

    private List<int> actions = new List<int>();

    private int counter = 0;

    // Start is called before the first frame update
    void Start() {

        board = GetComponent<Board>();
        start_button.onClick.AddListener(startButtonClick);

    }

    void startButtonClick() {
        Debug.Log("You have clicked the button!");

        (int, int) start = board.transform2boardCord(player.transform.position);

        GameObject relic = GameObject.FindGameObjectsWithTag("Relic")[0];
        (int, int) relicCord = board.transform2boardCord(relic.transform.position);

        actions = A_Star(new Vector2(start.Item2, start.Item1), new Vector2(relicCord.Item2, relicCord.Item1));

        foreach (int action in actions) {
            print(num2string(action));
        }
    }



    // Update is called once per frame
    void Update()
    {
        counter += 1;
        if (actions.Count != 0) {
            if (counter % 50 == 0) {
                counter = 0;
                Vector2 action = num2vec2(actions[0]);
                player.transform.position += new Vector3(action.x, action.y, 0);
                actions.RemoveAt(0);
            }
        }
    }

    private List<int> A_Star(Vector2 start, Vector2 end) {
        // The Queue used in BFS
        Queue<Vector2> queue = new Queue<Vector2>();

        // The already visited tiles
        List<Vector2> discovered = new List<Vector2>() { start };

        // The actions to get to a tile
        Hashtable actionTable = new Hashtable();

        // Start looking at the start tile
        actionTable.Add(start, new List<int>());
        queue.Enqueue(start);

        // While there are still tiles to look at
        while (queue.Count != 0) {

            // Check tile
            Vector2 tile = queue.Dequeue();

            // If we reached pacman
            if (tile == end) {
                // Return the actions needed to get to the end tile
                return (List<int>) actionTable[tile];
            }

            List<int> choices = new List<int> { 0, 1, 2, 3 };
            foreach (int direction in choices) {

                // New tile have the possible move
                Vector2 new_tile = tile + num2vec2(direction);

                // Checks valid move
                if (!validMove(new_tile)) {
                    continue;
                }

                // Checks if it has already been visited
                if (discovered.Contains(new_tile)) {
                    continue;
                }

                // Add the tile to the queue and lists
                discovered.Add(new_tile);                

                List<int> temp = new List<int>((List<int>) actionTable[tile]);
                temp.Add(direction);
                actionTable.Add(new_tile, temp);

                queue.Enqueue(new_tile);
            }
        }
        Debug.Log("There is no valid path");
        return new List<int>();
    }


    private bool validMove(Vector2 pos) {

        int x = (int) Mathf.Round(pos.x);
        int y = (int) Mathf.Round(pos.y);

        if (x < 0 || x >= board.getBoard().GetLength(1) || y < 0 || y >= board.getBoard().GetLength(0)) {
            return false;
        }

        return board.getBoard()[y, x] != "X" && board.getBoard()[y, x] != "S" && board.getBoard()[y, x] != "O";
    }

    // Returns a Vector2 assosiated with the action
    public Vector2 num2vec2(int n) {
        switch (n) {
            case 0:
                return new Vector2(0, -1);
            case 1:
                return new Vector2(1, 0);
            case 2:
                return new Vector2(0, 1);
            case 3:
                return new Vector2(-1, 0);
            default:    // should never happen
                return new Vector2(0, 0);
        }
    }

    public string num2string(int n) {
        switch (n) {
            case 0:
                return "Up";
            case 1:
                return "Right";
            case 2:
                return "Down";
            case 3:
                return "Left";
            default:    // should never happen
                return "None";
        }
    }
}
