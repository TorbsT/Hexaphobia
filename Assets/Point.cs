using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public AnimationCurve posCurve;
    public AnimationCurve fadeCurve;
    [Range(0f, 10f)] public float maxPos;
    public List<Sprite> sprites = new();
    public Vector2 startPos;
    public float maxLife;
    public float life;


    public SpriteRenderer rend;
    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        life = 0f;
        transform.position = Vector2.one * 100;
    }
    private void Update()
    {
        life += Time.deltaTime;
        if (life > maxLife) PoolSystem.Instance.Enpool(gameObject);
        rend.color = new(rend.color.r, rend.color.g, rend.color.b, fadeCurve.Evaluate(life / maxLife));
        Vector2 pos = startPos + new Vector2(0f, posCurve.Evaluate(life / maxLife)*maxPos);
        transform.position = pos;
    }
    public void Set(int points)
    {
        rend.sprite = sprites[points - 1];
    }
}
