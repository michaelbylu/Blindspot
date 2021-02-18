using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    [System.Serializable]
    public struct gridRow {
        public GameObject[] row; 
    }
    public gridRow[] pipes;

    private bool[,] visited;
    // number of rows in the grid
    private int length;
    // number of colunms in the grid
    private int height;

    // Start is called before the first frame update
    void Start()
    {
        height = pipes[0].row.Length;
        length = pipes.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBfs() {
        visited = new bool [length, height];
        visited[0, 0] = true;
        Bfs(1, 0, 3);
        ChangeColor();
    }

    public void Bfs(int x, int y, int direction) {
        if(x >= length || x <0 || y >= height || y <0) {
            return;
        }
        // if visited, return
        if(visited[x, y]) {
            return;
        }
        // if no pipe there, return
        if(pipes[x].row[y] == null) {
            return;
        }
        // if this block is not connected to the previous one, return
        print("Getting the neighbors of " + x +" , " + y);
        List<int> neighbors = pipes[x].row[y].GetComponent<PipeController>().GetNeighbors();
        int reverseDir = 6 - 2 * (direction % 2) - direction;
        if(!neighbors.Contains(reverseDir)) {
            print("neighbors not connected, " + reverseDir + " not found.");
            return;
        }
        visited[x, y] = true;
        // go through each neighbors except the predecessor
        foreach(int neighbor in neighbors) {
            if(neighbor != reverseDir) {
                if(neighbor == 1) {
                    Bfs(x - 1, y, 1);
                }
                else if(neighbor == 2) {
                    Bfs(x, y + 1, 2);
                }
                else if(neighbor == 3) {
                    Bfs(x + 1, y, 3);
                }
                else if(neighbor == 4) {
                    Bfs(x, y - 1, 4);
                }
            }
        }
    }

    public void ChangeColor() {
        for(int i=0; i<length; i++) {
            for(int j=0; j<height; j++) {
                if(pipes[i].row[j] != null) {
                    pipes[i].row[j].GetComponent<PipeController>().SwitchState(visited[i, j]);
                }
            }
        }
    }
}
