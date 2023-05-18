using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissleTarget : MonoBehaviour
{
    public Vector3 p0;
    public Vector3 p1;
    private float timeStart;
    private float duration = 0.01f;
    private bool targetFound = false;
    GameObject rootGO;
    private void Start()
    {
        rootGO = transform.root.gameObject;
    }
    private void Update()
    {
        if (targetFound)
        {
            float u = (Time.time - timeStart) / duration;
            if (u>=1)
            {
                targetFound = false;
            }
            rootGO.transform.position = (1 - u) * p0 + u * p1;
        }
        
    }
    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        if (go.tag == "Enemy")
        {
            rootGO.transform.position = go.transform.position;
            /*
            targetFound = true;
            rootGO = transform.root.gameObject;
            p0 = rootGO.transform.position;
            p1 = go.transform.position;
            timeStart = Time.time;*/
        }
    }
}
