using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSpawner : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform[] spawnPoints;
    public int maxMissiles = 2;

    private int currentMissiles = 0;
    private bool canSpawnMissile = true;

    private void Update()
    {
    }
    public void Attack()
    {
        if (canSpawnMissile && currentMissiles < maxMissiles)
        {
            if (Input.GetKeyDown(KeyCode.H))
            {
                SpawnMissile();
            }
        }
    }

    private void SpawnMissile()
    {
        Transform spawnPoint = GetAvailableSpawnPoint();
        if (spawnPoint != null)
        {
            GameObject missileObject = Instantiate(missilePrefab, spawnPoint.position, spawnPoint.rotation);
            Missile missileComponent = missileObject.GetComponent<Missile>();
            missileComponent.OnMissileDestroyed += OnMissileDestroyed;
            currentMissiles++;

            if (currentMissiles >= maxMissiles)
            {
                canSpawnMissile = false;
            }
        }
    }

    private void OnMissileDestroyed(Missile destroyedMissile)
    {
        destroyedMissile.OnMissileDestroyed -= OnMissileDestroyed;
        currentMissiles--;
        canSpawnMissile = true;
    }

    private Transform GetAvailableSpawnPoint()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint.childCount == 0)
            {
                return spawnPoint;
            }
        }

        return null;
    }
}
