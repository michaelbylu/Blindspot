using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public GameObject[] bubbles;
    public float interval = 5f;
    private float lastSpawn;
    private float nextInterval;
    // Start is called before the first frame update
    void Start()
    {
        lastSpawn = Time.time;
        nextInterval = interval;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastSpawn > nextInterval) {
            StartCoroutine(ShowBubble());
            lastSpawn = Time.time;
            nextInterval = Random.Range(0.8f * interval, 1.2f * interval);
        }
    }

    public void FadeOut() {
        lastSpawn = Time.time + 100f;
        StopAllCoroutines();
        foreach(FadeInOut f in GetComponentsInChildren<FadeInOut>()) {
            f.StartFadingOut();
        }
    }

    IEnumerator ShowBubble() {
        int i = Random.Range(0, bubbles.Length);
        while(bubbles[i].activeSelf) {
            i = Random.Range(0, bubbles.Length);
            yield return new WaitForSeconds(0.1f);
        }
        bubbles[i].SetActive(true);
        bubbles[i].GetComponent<FadeInOut>().StartFadingIn();
        yield return new WaitForSeconds(2f);
        bubbles[i].GetComponent<FadeInOut>().StartFadingOut();
        yield return new WaitForSeconds(1f);
        bubbles[i].SetActive(false);
    }
}
