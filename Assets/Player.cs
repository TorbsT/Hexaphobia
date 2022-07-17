using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [Range(0f, 10f)] public float acceleration;
    [Range(0f, 100f)] public float topSpeed;
    public GameObject pointPrefab;

    public Transform leftEye;
    public Transform rightEye;

    public Animator animator;

    public float inputHor;
    public float inputVer;

    Rigidbody2D rb;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Die die = collision.collider.GetComponent<Die>();
        if (die == null) return;

        if (die.lifetime < 1f) return;

        bool charging = die.charging;
        if (charging) return;
        int eyes = die.eyes;
        if (eyes != 6)
        {
            animator.SetTrigger("scored");
            DiceSystem.Instance.Score(eyes);
            GameObject pointGO = PoolSystem.Instance.Depool(pointPrefab);
            Point p = pointGO.GetComponent<Point>();
            p.startPos = die.transform.position;
            p.Set(eyes);
            die.cooldown = 10f;
        } else
        {
            animator.SetTrigger("died");
            DiceSystem.Instance.Died();
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        inputHor = Input.GetAxisRaw("Horizontal");
        inputVer = Input.GetAxisRaw("Vertical");

        //Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = pos;
        Die lookingAt = null;
        float leastSqrMg = float.MaxValue;
        foreach (Die die in DiceSystem.Instance.dice)
        {
            if (die.eyes == 6)
            {
                float sqrMg = (transform.position - die.transform.position).sqrMagnitude;
                if (sqrMg < leastSqrMg)
                {
                    lookingAt = die;
                    leastSqrMg = sqrMg;
                }
            }
        }
        //animator.SetBool("looking", lookingAt != null);
        if (lookingAt != null)
        {
            leftEye.transform.right = lookingAt.transform.position - leftEye.transform.position;
            rightEye.transform.right = lookingAt.transform.position - rightEye.transform.position;
        }
    }

    private void FixedUpdate()
    {
        Vector2 vel = rb.velocity;
        Vector2 inputVel = new(inputHor, inputVer);
        float tickAcc = acceleration * Time.fixedDeltaTime * 50f;
        if (inputVel.x == 0 && inputVel.y == 0)
        {
            // Not moving
            float velMag = vel.magnitude;
            if (velMag < tickAcc)
            {
                rb.velocity = Vector2.zero;
            }
            else rb.velocity = vel.normalized * (velMag - tickAcc);
        }
        else if ((vel + inputVel*tickAcc).magnitude > topSpeed)
        {
            rb.velocity = (vel + inputVel*tickAcc).normalized * topSpeed;
        } else
        {
            rb.velocity = vel + inputVel*tickAcc;
        }
    }
    public void Died()
    {
        
    }


}
