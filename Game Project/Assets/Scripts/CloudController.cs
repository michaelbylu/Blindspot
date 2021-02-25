using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    // Start is called before the first frame update
    public float liveSpan;
    public float moveSpeed;
    private SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Destroy(gameObject, liveSpan);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = gameObject.transform.position;
        pos.x += moveSpeed * Time.deltaTime;
        gameObject.transform.position = pos;
    }

    public void Reverse() {
        moveSpeed *= -1f;
    }

    void FadeOut() {
        Color cur = spriteRenderer.color;
        cur.a -= 1 / liveSpan * Time.deltaTime;
        spriteRenderer.color = cur;
    }
}
