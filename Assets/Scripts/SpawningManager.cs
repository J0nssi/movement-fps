using System.Collections.Generic;
using UnityEngine;
using System;


public class SpawningManager : MonoBehaviour
{

    public static SpawningManager instance;

    public float respawnTime = 3f;

    public GameObject[] characters;

    SortedList<float, string> spawnQueue = new SortedList<float, string>();

    SpawnPoint[] spawnPoints;

    System.Random rnd;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;

        spawnPoints = new SpawnPoint[transform.childCount];
        rnd = new System.Random();


        int i = 0;
        foreach (Transform spawnLocation in transform)
        {
            SpawnPoint s = new SpawnPoint();
            s.transform = spawnLocation;
            spawnPoints[i] = s;
            i++;
        }
    }

    void Start()
    {
        // Spawn all characters at Random spawn points
        SpawnEveryone();
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn character from spawnQueue if it's cooldown has ended
        SpawnCharacters();
    }

    void SpawnEveryone()
	{
        SpawnPoint[] activeSpawnPoints = Array.FindAll(spawnPoints, s => !s.OnCooldown());
        activeSpawnPoints = ShuffleSpawnPoints(activeSpawnPoints);
        int i = 0;
        foreach (GameObject character in characters)
        {   
            activeSpawnPoints[i].Spawn(character);
            i++;
        }
    }

    void SpawnCharacters()
    {
        if (spawnQueue.Count > 0 && spawnQueue.Keys[0] < Time.time)
        {
            string characterName = spawnQueue.Values[0];
            spawnQueue.RemoveAt(0);

            SpawnPoint[] activeSpawnPoints = Array.FindAll(spawnPoints, s => !s.OnCooldown());
            int x = rnd.Next(activeSpawnPoints.Length);
            activeSpawnPoints[x].Spawn(Resources.Load(characterName) as GameObject);

        }
    }

    SpawnPoint[] ShuffleSpawnPoints(SpawnPoint[] activeSpawnPoints)
    {
        SpawnPoint tempSpawn;
        for (int i = 0; i < activeSpawnPoints.Length; i++)
        {
            int rnd = UnityEngine.Random.Range(0, activeSpawnPoints.Length);
            tempSpawn = activeSpawnPoints[rnd];
            activeSpawnPoints[rnd] = activeSpawnPoints[i];
            activeSpawnPoints[i] = tempSpawn;
        }
        return activeSpawnPoints;
    }
        public void QueueToSpawn(GameObject character)
	{
        float time = Time.time + respawnTime;
        spawnQueue.Add(time, character.name);
        
	}
}
