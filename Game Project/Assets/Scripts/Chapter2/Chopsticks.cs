using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopsticks : MonoBehaviour
{
    public SpriteRenderer leftChopstick;
    public Transform endPos;
    public Transform plate;
    public List<Transform> destinations;
    private Vector3 initialPos;
    private GameObject selectedDumpling = null;
    private bool hasComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckComplete();
        MoveWithMouse();
        if(Input.GetMouseButtonDown(0)) {
            if(selectedDumpling == null) {
                return;
            }
            initialPos = selectedDumpling.transform.position;
            selectedDumpling.transform.position = endPos.position;
            selectedDumpling.transform.SetParent(endPos);
            leftChopstick.sortingOrder = 1;
        }
        else if(Input.GetMouseButtonUp(0)) {
            if(endPos.childCount == 0) {
                return;
            }
            selectedDumpling = endPos.GetChild(0).gameObject;
            leftChopstick.sortingOrder = 3;
            float minDis = 10f;
            Transform dest = null;
            for(int i = 0; i < destinations.Count; i++) {
                if(Vector3.Distance(selectedDumpling.transform.position, destinations[i].transform.position) < minDis) {
                    minDis = Vector3.Distance(selectedDumpling.transform.position, destinations[i].transform.position);
                    dest = destinations[i];
                }
            }
            if(minDis <= 0.5f) {
                selectedDumpling.transform.SetParent(plate);
                selectedDumpling.transform.position = dest.position;
                dest.gameObject.SetActive(false);
                destinations.Remove(dest);
                selectedDumpling.GetComponent<PolygonCollider2D>().enabled = false;
                GetComponent<AudioSource>().Play();
                selectedDumpling = null;     
            }
            else {
                selectedDumpling.transform.SetParent(plate);
                selectedDumpling.transform.position = initialPos;
                selectedDumpling = null;
            }
        }
    }

    IEnumerator StartFadeOut() {
        yield return new WaitForSeconds(2f);
        foreach(FadeInOut f in transform.parent.GetComponentsInChildren<FadeInOut>()) {
            f.StartFadingOut();
        }
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter2Manager>().ChangeStage();
    }

    private void CheckComplete() {
        if(destinations.Count == 0 && !hasComplete) {
            hasComplete = true;
            StartCoroutine(StartFadeOut());
        }
    }

    private void MoveWithMouse() {
        Vector3 cursorScreenPoint = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (cursorScreenPoint);
        transform.position = new Vector3(cursorPosition.x, cursorPosition.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name.StartsWith("dumpling") && !Input.GetMouseButton(0)) {
            if(selectedDumpling == null || Vector3.Distance(other.transform.position, endPos.position) <
            Vector3.Distance(selectedDumpling.transform.position, endPos.position)) {
                selectedDumpling = other.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.name.StartsWith("dumpling") && !Input.GetMouseButton(0)) {
            if(other.gameObject == selectedDumpling) {
                selectedDumpling = null;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.name.StartsWith("dumpling") && !Input.GetMouseButton(0)) {
            if(selectedDumpling == null || Vector3.Distance(other.transform.position, endPos.position) <
            Vector3.Distance(selectedDumpling.transform.position, endPos.position)) {
                selectedDumpling = other.gameObject;
            }
        }
    }
}
