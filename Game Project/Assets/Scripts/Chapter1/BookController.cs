using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController : MonoBehaviour
{
    public Sprite openedBook;
    public Sprite closedBook;
    public AudioClip closeSFX;
    public AudioClip flipSFX;
    public AudioClip openSFX;
    private AudioSource audioSource;
    private int count = 0;
    private bool clickable = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableClick() {
        GetComponent<FadeInOut>().StartBlinking();
        clickable = true;
    }

    private void OnMouseDown() {
        if(!clickable){
            return;
        }
        GetComponent<FadeInOut>().StopBlinking();
        clickable = false;
        GetComponent<SpriteRenderer>().sprite = openedBook;
        audioSource.clip = (count == 0)? openSFX : flipSFX;
        audioSource.Play();
        StartCoroutine(Hold(2));
    }

    public void CloseBook() {
        audioSource.clip = closeSFX;
        audioSource.Play();
        GetComponent<SpriteRenderer>().sprite = closedBook;
    }

    public void FlipBook() {
        audioSource.clip = flipSFX;
        audioSource.Play();
    }

    IEnumerator Hold(float time) {
        yield return new WaitForSeconds(time);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>().ChangeStage();
    }
}
