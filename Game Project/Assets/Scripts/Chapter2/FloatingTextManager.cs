using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public int spawnAmount;
    public float spawnInterval;
    [TextArea]
    public string[] sentences;
    public GameObject inTextPrefab;
    public GameObject inTextEmpPrefab;
    public GameObject outTextPrefab;
    public GameObject outTextEmpPrefab;
    public Transform puzzleParent;
    public GameObject[] floatingPuzzlePrefab;
    public GameObject[] miscPrefab;
    public Transform[] floatingPuzzleDest;
    private int spawnCount;
    private int waveIndex;
    private float lastSpawn;
    private float interval;
    private bool isSpawning;
    private string[] currentText;
    private List<int> puzzleIndex;

    // Start is called before the first frame update
    void Start()
    {
        waveIndex = 0;
        lastSpawn = Time.time + 2f;
        interval = spawnInterval;
        spawnCount = 0;
        isSpawning = true;
        puzzleIndex = new List<int>();
        for(int i = 0; i < floatingPuzzlePrefab.Length; i++) {
            puzzleIndex.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpawnText(Vector3.zero, "");
    }

    public void SpawnText(Vector3 initPos, string floatingText) {  
        if(initPos == Vector3.zero) {
            if(Time.time - lastSpawn <= interval || !isSpawning || spawnCount == spawnAmount) {
                return;
            }
            if(spawnCount == 0) {
                currentText = sentences[waveIndex].Split('/');
                Debug.Log(waveIndex);
                spawnAmount = currentText.Length;
            }
            GameObject text = GameObject.Instantiate(inTextPrefab, inTextPrefab.transform.position, Quaternion.identity);  
            if(currentText[spawnCount].StartsWith("*")) {
                Destroy(text);
                text = GameObject.Instantiate(inTextEmpPrefab, inTextPrefab.transform.position, Quaternion.identity);
                text.GetComponent<TMPro.TextMeshProUGUI>().text = currentText[spawnCount].Substring(1);
            }
            else {
                text.GetComponent<TMPro.TextMeshProUGUI>().text = currentText[spawnCount];
            }
            text.GetComponent<FloatingScript>().dir = 3f * (float)spawnCount / spawnAmount;
            text.transform.SetParent(gameObject.transform);
            text.GetComponent<RectTransform>().anchoredPosition = new Vector2(-960, 460 - 300f * (float)spawnCount / spawnAmount);
            text.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
            lastSpawn = Time.time;
            interval = Random.Range(0.8f * spawnInterval, 1.2f * spawnInterval);
            spawnCount++;
            if(spawnAmount == spawnCount) {
                text.GetComponent<FloatingScript>().spawnPuzzleAfter = true;
                isSpawning = false;
            }
        }
        else {
            GameObject text = GameObject.Instantiate(outTextPrefab, outTextPrefab.transform.position, Quaternion.identity);
            if(floatingText.StartsWith("*")) {
                Destroy(text);
                text = GameObject.Instantiate(outTextEmpPrefab, outTextEmpPrefab.transform.position, Quaternion.identity);
                text.GetComponent<TMPro.TextMeshProUGUI>().text = floatingText.Substring(1);
            }
            else {
                text.GetComponent<TMPro.TextMeshProUGUI>().text = floatingText;
            }
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

    public bool PuzzlePlaced(int index) {
        if(puzzleIndex.Contains(index)) {
            puzzleIndex.Remove(index);
            GetComponent<AudioSource>().Play();
            if(puzzleIndex.Count == 0) {
                StopAllCoroutines();
                StartCoroutine(FadeOut());
            }
            return true;
        }
        else {
            return false;
        }
    } 

    public void StartSpawn() {
        if(isSpawning) {
            return;
        }
        isSpawning = true;
        spawnCount = 0;
        waveIndex++;
        if(waveIndex >= sentences.Length) {
            waveIndex = 0;
        }
    }

    IEnumerator FadeOut() {
        foreach(FloatingScript f in transform.GetComponentsInChildren<FloatingScript>()){
            f.StopAllCoroutines();
        }
        yield return new WaitForSeconds(2f);
        foreach(FadeInOut f in transform.parent.GetComponentsInChildren<FadeInOut>()){
            f.StartFadingOut();
        }
        foreach(FadeInOut f in puzzleParent.GetComponentsInChildren<FadeInOut>()) {
            f.StartFadingOut();
        }
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter2Manager>().ChangeStage();
        puzzleParent.gameObject.SetActive(false);
    }

    IEnumerator Spawn(Vector3 pos) {
        yield return new WaitForSeconds(0.5f);
        if(puzzleIndex.Count == 0) {
            
        }
        else if(puzzleIndex.Count == 1) {
            GameObject[] puzzles = new GameObject[spawnCount];
            GameObject puzzle = GameObject.Instantiate(floatingPuzzlePrefab[puzzleIndex[0]], pos, Quaternion.identity);
            puzzle.GetComponent<FloatPuzzleController>().destination = floatingPuzzleDest[puzzleIndex[0]];
            puzzle.GetComponent<FloatPuzzleController>().SetText(currentText[0]);
            puzzles[0] = puzzle;
            int[] shuffle = Shuffle(miscPrefab.Length);
            for(int i = 1; i < spawnCount; i++) {
                GameObject misc = GameObject.Instantiate(miscPrefab[shuffle[i - 1]], pos, Quaternion.identity);
                misc.GetComponent<FloatPuzzleController>().SetText(currentText[i]);
                puzzles[i] = misc;
            }
            shuffle = Shuffle(spawnCount);
            for(int i = 0; i < spawnAmount; i++) {
                puzzles[i].transform.SetParent(puzzleParent);
                puzzles[shuffle[i]].GetComponent<FloatPuzzleController>().dir = 2 - 4f / spawnCount * i;
                if(puzzles[i].GetComponent<FloatPuzzleController>().destination != null) {
                    puzzles[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }
        }
        else {
            GameObject[] puzzles = new GameObject[spawnCount];
            int[] shuffle = Shuffle(puzzleIndex.Count);
            GameObject puzzle1 = GameObject.Instantiate(floatingPuzzlePrefab[puzzleIndex[shuffle[0]]], pos, Quaternion.identity);
            puzzle1.GetComponent<FloatPuzzleController>().destination = floatingPuzzleDest[puzzleIndex[shuffle[0]]];
            puzzle1.GetComponent<FloatPuzzleController>().SetText(currentText[0]);
            puzzles[0] = puzzle1;
            GameObject puzzle2 = GameObject.Instantiate(floatingPuzzlePrefab[puzzleIndex[shuffle[1]]], pos, Quaternion.identity);
            puzzle2.GetComponent<FloatPuzzleController>().destination = floatingPuzzleDest[puzzleIndex[shuffle[1]]];
            puzzle2.GetComponent<FloatPuzzleController>().SetText(currentText[1]);
            puzzles[1] = puzzle2;
            shuffle = Shuffle(miscPrefab.Length);
            for(int i = 2; i < spawnCount; i++) {
                GameObject misc = GameObject.Instantiate(miscPrefab[shuffle[i - 1]], pos, Quaternion.identity);
                misc.GetComponent<FloatPuzzleController>().SetText(currentText[i]);
                puzzles[i] = misc;
            }
            shuffle = Shuffle(spawnCount);
            for(int i = 0; i < spawnAmount; i++) {
                puzzles[i].transform.SetParent(puzzleParent);
                puzzles[shuffle[i]].GetComponent<FloatPuzzleController>().dir = 2 - 4f / spawnCount * i;
                if(puzzles[i].GetComponent<FloatPuzzleController>().destination != null) {
                    puzzles[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                }
            }
        }
    }

    private int[] Shuffle(int Count)
    {
        int[] texts = new int[Count];
        for(int i = 0; i < Count; i ++) {
            texts[i] = i;
        }
        for (int t = 0; t < texts.Length; t++)
        {
            int tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
        return texts;
    }

}
