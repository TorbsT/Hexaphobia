using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipSystem : MonoBehaviour
{
    public static TipSystem Instance { get; private set; }

    public GameObject wasdTip;
    public GameObject collectTip;
    public GameObject powerTip;
    public GameObject surviveTip;
    public GameObject upgradeTip;

    public bool moved;
    public bool collected;
    public int poweredCount;
    public int upgradesSpawned;
    public int upgradesDespawned;


    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Time.timeScale == 0f) return;
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            moved = true;
        }
        if (DiceSystem.Instance.score > 0) collected = true;
        if (Input.GetKeyDown(KeyCode.Space)) poweredCount++;


        if (moved)
        {
            wasdTip.SetActive(false);
        }
        if (collected && moved)
        {
            collectTip.SetActive(false);
        }
        if (poweredCount > 0)
        {
            powerTip.SetActive(false);
        }
        if (poweredCount > 1)
        {
            surviveTip.SetActive(false);
        }
        upgradeTip.SetActive(upgradesSpawned > upgradesDespawned && upgradesSpawned == 1);
    }
}
