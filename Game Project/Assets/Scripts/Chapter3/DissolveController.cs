using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public AudioClip[] sfx;
    public Material dissolveMat;
    public GameObject particleEffect;
    public Sprite[] sprites;
    public float[] heightRange;
    public float duration;
    private Material material;
    private int stage = 0;
    private bool isDissolving = false;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[0];
        GetComponent<SpriteRenderer>().material = new Material(dissolveMat);
        material = GetComponent<SpriteRenderer>().material;
        material.SetFloat("_CurOffHeight", heightRange[0]);
    }

    // Update is called once per frame
    void Update()
    {
        Dissolve();
    }

    private void Dissolve() {
        if(!isDissolving) {
            return;
        }
        float height = material.GetFloat("_CurOffHeight");
        height += (heightRange[1] - heightRange[0]) * Time.deltaTime / duration;
        material.SetFloat("_CurOffHeight", height);
    }

    private void OnMouseDown() {
        if(stage < sprites.Length - 1) {
            stage++;
            GetComponent<SpriteRenderer>().sprite = sprites[stage];
            GetComponent<AudioSource>().clip = sfx[0];
        }
        else {
            isDissolving = true;
            foreach(ParticleEffect p in GetComponentsInChildren<ParticleEffect>(true)) {
                p.gameObject.SetActive(true);
            }
            //particleEffect.SetActive(true);
            GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<AudioSource>().clip = sfx[1];
        }
        GetComponent<AudioSource>().Play();
    }
}
