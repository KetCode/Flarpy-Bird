using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
    public GameObject pipe;
    public float spawnRate = 2;
    private float _timer = 0;
    public float heightOffset = 10;
    void Start()
    {
        SpawnPipe();
    }

    void Update()
    {
        if (_timer < spawnRate)
        {
            _timer += Time.deltaTime;
        } else
        {
            SpawnPipe();
            _timer = 0;
        }
    }
    
    void SpawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;
        
        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowestPoint, highestPoint), 0), transform.rotation);
    }
}
