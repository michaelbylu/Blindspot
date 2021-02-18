using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class PipeController : MonoBehaviour
{
    public bool isStatic;
    //1 = L, 2 = T, 3 = I
    public int pipeShape;
    public Sprite emptyPipe;
    public Sprite filledPipe;
    private int[] rotations = new int[] {0, 90, 180, 270};
    //1 = top, 2 = right, 3 = bottom, 4 = left
    private int[,] neighbors;
    // Start is called before the first frame update
    void Start()
    {
        if(pipeShape == 1){
            neighbors = new int[,] {{1, 2}, {4, 1}, {3, 4}, {2, 3}};
        }
        else if(pipeShape == 2){
            neighbors = new int[,] {{1, 2, 4}, {1, 4, 3}, {2, 3, 4}, {2, 3, 1}};
        }
        else if(pipeShape == 3){
            neighbors = new int[,] {{1, 3}, {2, 4}, {1, 3}, {2, 4}};
        }
        if(isStatic) {
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<int> GetNeighbors() {
        int index = Array.IndexOf(rotations, Mathf.RoundToInt(gameObject.transform.eulerAngles.z));
        print("index :" + index);
        print(Mathf.RoundToInt(gameObject.transform.eulerAngles.z));
        List<int> res = new List<int>();
        for(int i=0; i<neighbors.GetLength(1); i++){
            res.Add(neighbors[index,i]);
        }
        return res;
    }

    public void SwitchState(bool isConnected) {
        gameObject.GetComponent<SpriteRenderer>().sprite = isConnected? filledPipe : emptyPipe;
    }

    private void OnMouseDown() {
        gameObject.transform.Rotate(new Vector3(0, 0, 90));
        GameObject.FindGameObjectWithTag("StartPoint").GetComponent<PipeManager>().StartBfs();
    }
}
