using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
    public Rigidbody2D rb;
    [Range(0f, 100f)] public float maxStartTorque;
    public Die die;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        die = GetComponent<Die>();
    }

    private void OnEnable()
    {
        rb.velocity = Random.insideUnitCircle.normalized * 1f;
        rb.angularVelocity = Random.Range(-maxStartTorque, maxStartTorque);
        Rect spawnRect = new(0f, 0f, 15f, 7f);
        float x = Random.Range(spawnRect.min.x, spawnRect.max.x);
        float y = Random.Range(spawnRect.min.y, spawnRect.max.y);
        transform.position = new Vector2(x, y);
    }

    private void FixedUpdate()
    {
        float eyeShit = 1f;
        if (die != null) eyeShit = DiceSystem.Instance.eyeSpeedCurve.Evaluate(die.eyes);
        rb.velocity = 100 * eyeShit * DiceSystem.Instance.speed * Time.fixedDeltaTime * rb.velocity.normalized;
    }

    public void Flee()
    {
        Vector2 ppos = Player.Instance.transform.position;
        Vector2 pos = transform.position;
        rb.velocity = (pos - ppos).normalized * 1f;
    }
}
