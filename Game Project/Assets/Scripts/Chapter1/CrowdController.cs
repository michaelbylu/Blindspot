using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdController : MonoBehaviour
{
    public GameObject[] crowd;
    public float interval = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableCrowd() {
        StartCoroutine(CrowdEnable());
    }

    IEnumerator CrowdEnable() {
        for(int i=0; i<crowd.Length;i++) {
            yield return new WaitForSeconds(interval);
            crowd[i].SetActive(true);
        }
        yield return new WaitForSeconds(interval);
        GameObject.FindGameObjectWithTag("GameController").GetComponent<Chapter1Manager>().ChangeStage();
    }
}
