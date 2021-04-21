using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatPuzzleController : MonoBehaviour
{
    public int index;
    public float dir = 0;
    public float duration;
    public bool hasRotation = false;
    public Transform destination;
    public Vector3[] path;
    private Vector3 screenPoint;
    private Vector3 offset;
    private Tween myTween;
    private string text;
    // Start is called before the first frame update
    void Start()
    {
        path[0] = new Vector3(path[0].x, transform.position.y + Random.Range(dir - 0.5f, dir + 0.5f), path[0].z);
        for(int i = 1; i < path.Length; i++) {
            path[i] = new Vector3(path[i].x, transform.position.y + Random.Range(2, -2), path[i].z);
        }
        myTween = transform.DOPath(path, Random.Range(duration, 1.25f * duration), PathType.CatmullRom);
        if(hasRotation) {
            transform.eulerAngles += new Vector3(0, 0, Random.Range(0f, 360f));
        }
        StartCoroutine(SpawnNext());
    }

    // Update is called once per frame
    void Update()
    {
        if(hasRotation) {
            transform.eulerAngles += new Vector3(0, 0, Random.Range(10 * Time.deltaTime, 15 * Time.deltaTime));
        }
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
        FloatingTextManager ftm = GameObject.FindGameObjectWithTag("Respawn").GetComponent<FloatingTextManager>();
        if(destination != null && Vector3.Distance(gameObject.transform.position, destination.position) < 0.7f && ftm.PuzzlePlaced(index)) {
            transform.position = destination.position;
            transform.localScale = new Vector3(1f, 1f, 1f);
            GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sortingOrder--;
            myTween.Kill();
            if(hasRotation) {
                hasRotation = false;
                transform.rotation = Quaternion.identity;
            }
        }
        else {
            myTween.Play();
        }
    }

    private void OnMouseOver() {
        GetComponent<SpriteRenderer>().material.SetFloat("_Thickness", 0.05f);
    }

    private void OnMouseExit() {
        GetComponent<SpriteRenderer>().material.SetFloat("_Thickness", 0f);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.name.StartsWith("Edge")) {
            StartCoroutine(StartFadingOut());
        }
    }

    IEnumerator SpawnNext() {
        yield return new WaitForSeconds(2f);
        if(destination == null) {
            GetComponent<FadeInOut>().StartFadingOut(0.2f);
        }
        yield return new WaitForSeconds(4f);
        GameObject.FindGameObjectWithTag("Respawn").GetComponent<FloatingTextManager>().StartSpawn();
    }

    IEnumerator StartFadingOut() {
        GetComponent<FadeInOut>().StartFadingOut();
        yield return new WaitForSeconds(1f);
        if(destination != null) {
            GameObject.FindGameObjectWithTag("Respawn").GetComponent<FloatingTextManager>().SpawnText(transform.position, text);
        }
        Destroy(gameObject, 8f);
    }

    public void SetText(string floatingText) {
        text = floatingText;
    }
}
