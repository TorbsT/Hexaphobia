using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    public GameObject prefab;
    public List<Upgrade> upgradePrefabs = new();
    public List<Transform> upgradeSpawns;
    public int spawncount;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        Invoke(nameof(SpawnShit), 0.1f);
    }

    public void SpawnShit()
    {

        for (int i = 0; i < spawncount; i++)
        {
            GameObject go = Spawn();
            Die die = go.GetComponent<Die>();
            die.cooldown = 4f;
            if (i == 0) die.Set(6);
            if (i == 1) die.Set(1);
        }

        DiceSystem.Instance.SetShit();
    }

    public GameObject Spawn()
    {
        GameObject go = PoolSystem.Instance.Depool(prefab);
        Vector2 pos = Player.Instance.transform.position;
        go.transform.position = pos+Random.insideUnitCircle;
        return go;
    }
    public void SpawnUpgrade()
    {
        int i = Random.Range(0, upgradePrefabs.Count);
        Upgrade upgrade = upgradePrefabs[i];
        if (upgrade.oneTime) upgradePrefabs.RemoveAt(i);
        GameObject go = PoolSystem.Instance.Depool(upgrade.gameObject);

        Vector2 bestSpawn = Vector2.zero;
        Vector2 playerPos = Player.Instance.transform.position;
        float bestDistance = (bestSpawn - playerPos).sqrMagnitude;
        foreach (Transform spawn in upgradeSpawns)
        {
            Vector2 pos = spawn.position;
            float distance = (pos - playerPos).sqrMagnitude;
            if (distance > bestDistance)
            {
                bestDistance = distance;
                bestSpawn = pos;
            }
        }

        go.transform.position = bestSpawn;
    }
}
