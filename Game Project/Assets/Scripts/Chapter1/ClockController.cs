using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    public float speed;
    public bool isStatic = true;
    private bool isOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovePointers();
    }

    private void MovePointers() {
        if(!isOn) {
            return;
        } 
        Vector3 hourRotation = transform.Find("hour_pointer").eulerAngles;
        hourRotation.z -= speed * Time.deltaTime / 12;
        transform.Find("hour_pointer").eulerAngles = hourRotation;
        Vector3 minuteRotation = transform.Find("minute_pointer").eulerAngles;
        minuteRotation.z -= speed * Time.deltaTime;
        transform.Find("minute_pointer").eulerAngles = minuteRotation;
    }

    public void TurnOn(float time) {
        isOn = true;
        StartCoroutine(OnForMinutes(time));
    }

    IEnumerator OnForMinutes(float minutes) {
        yield return new WaitForSeconds(minutes);
        isOn = false; 
        if(!isStatic) {
            transform.parent.gameObject.SetActive(false);
        }
    }
}
