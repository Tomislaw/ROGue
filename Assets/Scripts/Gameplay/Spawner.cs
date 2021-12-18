using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Prefab;
    public float delayBetweenSpawns;
    public int objectsToSpawn;

    private float timeToNextSpawn;


    void Start()
    {
        timeToNextSpawn = delayBetweenSpawns;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(objectsToSpawn > 0)
        {
            timeToNextSpawn -= Time.fixedDeltaTime;

            if(timeToNextSpawn <= 0)
            {
                var spawnedObject = Instantiate(Prefab);
                spawnedObject.transform.position = transform.position;
                timeToNextSpawn = delayBetweenSpawns;
                objectsToSpawn--;
            }
        }
    }
}
