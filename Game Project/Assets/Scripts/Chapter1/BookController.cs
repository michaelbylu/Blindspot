using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour
{
    public Sprite openedBook;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableClick() {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<FadeInOut>().StartBlinking();
    }

    private void OnMouseUp() {
        GetComponent<FadeInOut>().StopBlinking();
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = openedBook;
        StartCoroutine(Hold(2));
    }

    IEnumerator Hold(float time) {
        yield return new WaitForSeconds(time);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>().ChangeStage();
    }
}
