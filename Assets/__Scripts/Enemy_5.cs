using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_5 : Enemy
{
    [Header("Set in Inspector: Enemy_5")]
    private float timeStart;
    private float duration = 4f;
    public Vector3 p0;
    public Vector3 p1;
    void Start()
    {
        p0 = p1 = pos;
        InitMovement();
    }
    void InitMovement()
    {
        p0 = p1;
        float widMinRad = bndCheck.camWidth - bndCheck.radius;
        float hgtMinRad = bndCheck.camHeight - bndCheck.radius;
        if (p0.x == widMinRad)
        {
            p1.x = -widMinRad;
        }
        else
        {
            p1.x = widMinRad;
        }
        p1.y = Random.Range(-hgtMinRad, hgtMinRad);
        timeStart = Time.time;
    }
    public override void Move()
    {
        float u = (Time.time - timeStart) / duration;
        if (u > 1)
        {
            InitMovement();
            u = 0;
        }
        u = 1 - Mathf.Pow(1 - u, 2);
        pos = (1 - u) * p0 + u * p1;
    }
    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        if (go.tag == "PowerUp")
        {
            Destroy(go);
        }
    }
}
