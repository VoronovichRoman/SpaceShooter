using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set in Inspector: Enemy_2")]
    public float sinEccentricity = 0.6f;
    public float lifeTime = 10;

    [Header("Set Dynamically: Enemy_2")]
    public Vector3 �0;
    public Vector3 p1;
    public float birthTime;
    void Start()
    {
        �0 = Vector3.zero;
        �0.x = -bndCheck.camWidth - bndCheck.radius;
        �0.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
        p1 = Vector3.zero;
        p1.x = bndCheck.camWidth + bndCheck.radius;
        p1.y = Random.Range(-bndCheck.camHeight, bndCheck.camHeight);
        if (Random.value > 0.5f)
        {
            �0.x *= -1;
            p1.x *= -1;
        }
        birthTime = Time.time;
    }
    public override void Move()
    {
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1)
        {
            Destroy(this.gameObject);
            return;
        }
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));
        pos = (1 - u) * �0 + u * p1;
    }
}
