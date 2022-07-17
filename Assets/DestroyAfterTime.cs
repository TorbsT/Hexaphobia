using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [Range(0f, 120f)] public float maxLife;
    public float life;

    private void OnEnable()
    {
        life = 0f;
    }
    private void Update()
    {
        life += Time.deltaTime;
        if (life > maxLife) PoolSystem.Instance.Enpool(gameObject);
    }
}
