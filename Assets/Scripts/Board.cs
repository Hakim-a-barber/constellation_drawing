using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    private string[,] board;
    public Vector2Int size = new Vector2Int(100, 100);

    // Start is called before the first frame update
    void Start()
    {
        board = new string[size.y, size.x];

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        (int, int) playerCord = transform2boardCord(player.transform.position);
        board[playerCord.Item1, playerCord.Item2] = "P";


        GameObject[] stars = GameObject.FindGameObjectsWithTag("Star");
        foreach (GameObject star in stars) {
            (int, int) starCord = transform2boardCord(star.transform.position);
            board[starCord.Item1, starCord.Item2] = "S";
        }

        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject obstacle in obstacles) {
            (int, int) obstacleCord = transform2boardCord(obstacle.transform.position);
            board[obstacleCord.Item1, obstacleCord.Item2] = "O";
        }

        GameObject[] relics = GameObject.FindGameObjectsWithTag("Relic");
        foreach (GameObject relic in relics) {
            (int, int) relicCord = transform2boardCord(relic.transform.position);
            board[relicCord.Item1, relicCord.Item2] = "R";
        }
    }

    public void drawConnection(Vector3 A, Vector3 B) {
        (int, int) ACord = transform2boardCord(A);
        (int, int) BCord = transform2boardCord(B);

        float deltaY = ACord.Item1 - BCord.Item1;
        float deltaX = ACord.Item2 - BCord.Item2;

        float largerDistance = deltaY;
        if (Mathf.Abs(deltaX) > Mathf.Abs(deltaY)) {
            largerDistance = deltaX;
        }

        float yStep = deltaY / largerDistance;
        if(deltaY > 0) {
            yStep = -1 * Mathf.Abs(yStep);
        }
        float xStep = deltaX / largerDistance;
        if (deltaX > 0) {
            xStep = -1 * Mathf.Abs(xStep);
        }

        List<float> yValues = new List<float>();
        for (float i = ACord.Item1 + yStep; (int) Mathf.Round(i) != BCord.Item1; i += yStep) {
            yValues.Add(i);
        }

        List<float> xValues = new List<float>();
        for (float j = ACord.Item2 + xStep; (int) Mathf.Round(j) != BCord.Item2; j += xStep) {
            xValues.Add(j);
        }

        if (yValues.Count < xValues.Count) {
            int n = xValues.Count - yValues.Count;
            for (int i = 0; i < n; i++) {
                yValues.Add(BCord.Item1);
            }
            
        } else {
            int n = yValues.Count - xValues.Count;
            for (int i = 0; i < n; i++) {
                xValues.Add(BCord.Item2);
            }
        }

        for( int i = 0; i < yValues.Count; i++) {
            // print("y: " + yValues[i] + " x: " + xValues[i]);

            int y = (int) Mathf.Round(yValues[i]);
            int x = (int) Mathf.Round(xValues[i]);
            board[y, x] = "X";
        }
        drawBoard();
    }

    public string[,] getBoard() {
        return board;
    }

    public void drawBoard() {
        string output = "";
        for (int i = 0; i < board.GetLength(0); i++) {
            for (int j = 0; j < board.GetLength(1); j++) {
                if (board[i, j] == null) {
                    output += "-";
                } else {
                    output += board[i, j];
                }
            }
            output += "\n";
        }
        print(output);
    }


    public (int, int) transform2boardCord(Vector3 pos) {
        int x = (int) (Mathf.Round(pos.x) + (size.x / 2));
        int y = (int) (Mathf.Round(pos.y) + (size.y / 2));

        return (y, x);
    }
}
