using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cloud;
    public float[] xRange;
    public float maxY;
    public float minY;
    public float interval;
    private float lastSpawn;
    void Start()
    {
        lastSpawn = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastSpawn >= interval) {
            SpawnCloud();
            lastSpawn = Time.time;
        }
    }

    void SpawnCloud() {
        Vector3 cloudPos = new Vector3(xRange[Mathf.RoundToInt(Random.Range(0,2))],
        Random.Range(minY, maxY), 0);
        GameObject cloudSpawn = GameObject.Instantiate(cloud, cloudPos, Quaternion.identity);
        if(cloudPos.x != xRange[0]) {
            cloudSpawn.GetComponent<CloudController>().Reverse();
        }
    }
}
