using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{
    public List<Sprite> sprites = new();

    [Range(0f, 5f)] public float delay;
    [Range(0f, 100f)] public float maxStartTorque;
    [Range(5f, 100f)] public float lineRange;

    public Color defaultColor = Color.white;
    public Color killColor = Color.red;
    public Color scoreColor = Color.green;
    public Color killChargeColor = Color.magenta;
    public Color scoreChargeColor = Color.yellow;

    public float cooldown = 1f;
    public int eyes = -1;
    public SpriteRenderer leadRend;
    public SpriteRenderer rend;
    public SpriteRenderer colorRend;
    public Rigidbody2D rb;
    public LineRenderer line;
    public bool charging;
    public float lifetime;
    public bool lead;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        defaultColor = rend.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Invoke(nameof(UpdateLine), 0f);
        /*
        if (!DiceSystem.Instance.started) return;

        if (timeSinceLast < delay) return;
        bool collidedWithDie = collision.collider.GetComponent<Die>() != null;
        if (!collidedWithDie)
        {
            DiceSystem.Instance.Score(eyes);
            //Set(eyes + Random.Range(1, 6));
        }
        */
    }

    private void OnEnable()
    {
        Set(Random.Range(1, 7));
        rb.velocity = Random.insideUnitCircle.normalized*DiceSystem.Instance.speed;
        rb.angularVelocity = Random.Range(-maxStartTorque, maxStartTorque);
        rb.mass = DiceSystem.Instance.diceMass;
        Rect spawnRect = new(0f, 0f, 15f, 7f);
        float x = Random.Range(spawnRect.min.x, spawnRect.max.x);
        float y = Random.Range(spawnRect.min.y, spawnRect.max.y);
        transform.position = new Vector2(x, y);
        DiceSystem.Instance.dice.Add(this);
    }
    private void OnDisable()
    {
        DiceSystem.Instance.dice.Remove(this);
    }

    // Update is called once per frame
    void Update()
    {
        lifetime += Time.deltaTime;
        cooldown -= Time.deltaTime;
        bool newCharging = (cooldown > 0f);
        if (!charging && newCharging)
        {
            if (eyes == 6) colorRend.color = killChargeColor;
            if (eyes != 6) colorRend.color = scoreChargeColor;
        }
        else if (charging && !newCharging)
        {
            if (eyes == 6) colorRend.color = killColor;
            if (eyes != 6) colorRend.color = scoreColor;
        }
        if (lead != DiceSystem.Instance.leadSkin)
        {
            lead = DiceSystem.Instance.leadSkin;
            leadRend.gameObject.SetActive(lead);
        }
        charging = newCharging;
    }

    private void FixedUpdate()
    {
        rb.velocity = 100 * DiceSystem.Instance.eyeSpeedCurve.Evaluate(eyes) * DiceSystem.Instance.speed * Time.fixedDeltaTime * rb.velocity.normalized;
    }
    public void UpdateLine()
    {
        if (eyes != 6)
        {
            line.positionCount = 0;
            return;
        }
        line.positionCount = 2;
        Vector2 pos = transform.position;
        line.SetPosition(0, pos);
        line.SetPosition(1, pos+rb.velocity.normalized*lineRange);
    }
    public void Set(int eyes)
    {
        if (eyes > 6) eyes -= 6;
        this.eyes = eyes;
        rend.sprite = sprites[eyes - 1];
        cooldown = delay;
        charging = cooldown <= 0f;
        colorRend.color = defaultColor;
    }
    public void Flee()
    {
        Vector2 ppos = Player.Instance.transform.position;
        Vector2 pos = transform.position;
        rb.velocity = (pos - ppos).normalized * 1f;
    }
}
