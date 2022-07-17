using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public string id;
    public string title = "Upgrade get!";
    public string desc;
    public bool oneTime;
    public GameObject poofPrefab;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Player>() != null)
        {
            DiceSystem.Instance.UpgradePopup(this);
            PoolSystem.Instance.Enpool(gameObject);
        }
    }
    private void OnEnable()
    {
        TipSystem.Instance.upgradesSpawned++;
    }
    private void OnDisable()
    {
        GameObject poof = PoolSystem.Instance.Depool(poofPrefab);
        poof.transform.position = transform.position;
        TipSystem.Instance.upgradesDespawned++;
    }
}
