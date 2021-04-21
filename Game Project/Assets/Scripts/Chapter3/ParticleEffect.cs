using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    public Vector3 posRange;
    public Vector3 scaleRange;
    public float duration;
    private ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, duration + ps.main.startLifetime.constant);
    }

    // Update is called once per frame
    void Update()
    {
        var shape = ps.shape;
        shape.position += posRange * Time.deltaTime / duration;
        shape.scale += scaleRange * Time.deltaTime / duration;
    }
}
