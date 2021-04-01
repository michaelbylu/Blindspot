using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public int spawnAmount;
    public float spawnInterval;
    public GameObject inTextPrefab;
    public GameObject outTextPrefab;
    public GameObject floatingPuzzlePrefab;
    private int spawnCount;
    private float lastSpawn;
    private float interval;
    private bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawn = Time.time;
        interval = spawnInterval;
        spawnCount = 0;
        isSpawning = true;
    }

    // Update is called once per frame
    void Update()
    {
        SpawnText(Vector3.zero);
    }

    public void SpawnText(Vector3 initPos) {  
        if(initPos == Vector3.zero) {
            if(Time.time - lastSpawn <= interval || !isSpawning || spawnCount == spawnAmount) {
                return;
            }
            GameObject text = GameObject.Instantiate(inTextPrefab, inTextPrefab.transform.position, Quaternion.identity);
            text.GetComponent<TMPro.TextMeshProUGUI>().text = "testbug";
            text.transform.SetParent(gameObject.transform);
            text.GetComponent<RectTransform>().anchoredPosition = new Vector2(-960, Random.Range(160, 460));
            text.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            lastSpawn = Time.time;
            interval = Random.Range(0.6f * spawnInterval, 1.2f * spawnInterval);
            spawnCount++;
            if(spawnAmount == spawnCount) {
                text.GetComponent<FloatingScript>().spawnPuzzleAfter = true;
            }
        }
        else {
            GameObject text = GameObject.Instantiate(outTextPrefab, outTextPrefab.transform.position, Quaternion.identity);
            text.GetComponent<TMPro.TextMeshProUGUI>().text = "I'm out";
            text.GetComponent<RectTransform>().anchoredPosition = initPos;
            text.transform.SetParent(gameObject.transform);
            text.GetComponent<FloatingScript>().spawnPuzzleAfter = false;
            text.GetComponent<FloatingScript>().lifeSpan = 15f;
            text.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        }
        
    }

    public void SpawnPuzzle(Vector3 pos) {
        StartCoroutine(Spawn(pos));
    }

    public void StartSpawn() {
        isSpawning = true;
        spawnCount = 0;
    }

    IEnumerator Spawn(Vector3 pos) {
        yield return new WaitForSeconds(1f);
        GameObject puzzle1 = GameObject.Instantiate(floatingPuzzlePrefab, pos, Quaternion.identity);
        GameObject puzzle2 = GameObject.Instantiate(floatingPuzzlePrefab, pos, Quaternion.identity);
        GameObject puzzle3 = GameObject.Instantiate(floatingPuzzlePrefab, pos, Quaternion.identity);
        puzzle1.GetComponent<FloatPuzzleController>().dir = 1;
        puzzle2.GetComponent<FloatPuzzleController>().dir = 0;
        puzzle3.GetComponent<FloatPuzzleController>().dir = -1;
    }
}
