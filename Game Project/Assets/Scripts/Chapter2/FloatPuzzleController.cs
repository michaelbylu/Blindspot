using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatPuzzleController : MonoBehaviour
{
    public float moveSpeed;
    public int dir = 0;
    public Vector3[] path;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Tween myTween;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < path.Length; i++) {
            path[i] = new Vector3(path[i].x, transform.position.y + Random.Range(2 * dir - 1f, 2 * dir + 1f), path[i].z);
        }
        myTween = transform.DOPath(path, 8, PathType.CatmullRom);
        StartCoroutine(SpawnNext());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() {
        myTween.Pause();
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - 
        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    private void OnMouseDrag() {
        Vector3 cursorScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (cursorScreenPoint) + offset;
        transform.position = cursorPosition;
    }

    private void OnMouseUp() {
        myTween.Play();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        print(other.name);
        if(other.name.StartsWith("Edge")) {
            GameObject.FindGameObjectWithTag("Respawn").GetComponent<FloatingTextManager>().SpawnText(transform.position);
            Destroy(gameObject);
        }
    }

    IEnumerator SpawnNext() {
        yield return new WaitForSeconds(4f);
        GameObject.FindGameObjectWithTag("Respawn").GetComponent<FloatingTextManager>().StartSpawn();
    }
}
