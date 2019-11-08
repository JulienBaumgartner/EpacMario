using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    GameObject[] toSpawn;
    [SerializeField]
    bool isInfiniteSpawner;
    [SerializeField]
    float probability;
    float chrono = 0f;
    float nextWaitTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if(Random.Range(0f, 1f) < probability)
        {
            Instantiate(toSpawn[Random.Range(0,toSpawn.Length)], transform);
        }
        if (isInfiniteSpawner)
        {
            nextWaitTime = Random.Range(5f, 10f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        chrono += Time.deltaTime;
        if(isInfiniteSpawner && chrono > nextWaitTime)
        {
            if (Random.Range(0f, 1f) < probability)
            {
                Instantiate(toSpawn[Random.Range(0, toSpawn.Length)], transform);
            }
            chrono = 0;
            nextWaitTime = Random.Range(5f, 10f);
        }
    }
}
