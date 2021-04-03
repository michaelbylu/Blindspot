using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterFood : MonoBehaviour
{
    [TextArea]
    public string lineIndex;
    public float distance = 0.3f;
    public PlayerController player;
    public JsonReader jsonReader;
    private bool hasTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.gameObject.transform.position, transform.position) < distance && !hasTriggered) {
            hasTriggered = true;
            jsonReader.ChangeLine(lineIndex);
            player.FreezeMove();
        }
    }
}
