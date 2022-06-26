using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacdotSpawner : MonoBehaviour
{
    public GameObject pacdot;
    public float interval;
    public float startOffset;
    public float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time + startOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if(startTime.time > startTime + interval)
        {
            GameObject obj = (GameObject)Instantiate(pacdot, transform.position, Quaternion.identity);
            obj.transform.parent = transform;

            startTime = Time.time;
        }
    }
}
