using UnityEngine;
using System.Collections.Generic;

public class IngredientSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public Transform location;

        [Header("Spawn Options")]
        public bool useRandom;

        public List<GameObject> possibleIngredients; // for random
        public GameObject fixedIngredient;           // for fixed

        [Header("Spawn Rules")]
        public int spawnEveryXDays = 1;
    }

    public List<SpawnPoint> spawnPoints;
    // Track spawned ingredients
    private List<GameObject> spawnedIngredients = new List<GameObject>();
    [Header("Cleanup Settings")]
    public bool clearBeforeSpawning = true;
    public bool clearAtNight = false;

    private void OnEnable()
    {

        if (DayNightManager.Instance == null)
        {
            Debug.LogWarning("DayNightManager not ready yet!");
            return;
        }

        DayNightManager.Instance.OnNewDay += HandleNewDay;
        
        if (clearAtNight)
            DayNightManager.Instance.OnNightStart += ClearIngredients;
    }

    private void OnDisable()
    {
        if (DayNightManager.Instance != null)
        {
            DayNightManager.Instance.OnNewDay -= HandleNewDay;

            if (clearAtNight)
                DayNightManager.Instance.OnNightStart -= ClearIngredients;
        }
    }
    void HandleNewDay()
    {
        if (clearBeforeSpawning)
        {
            ClearIngredients();
        }

        SpawnIngredients();
    }
    void SpawnIngredients()
    {
        int currentDay = DayNightManager.Instance.currentDay;

        foreach (var spawnpoint in spawnPoints)
        {
            // Check day condition
            if (currentDay % spawnpoint.spawnEveryXDays != 0)
                continue;

            GameObject toSpawn = null;

            if (spawnpoint.useRandom)
            {
                int randomNumberOfIngredients = Random.Range(0, spawnpoint.possibleIngredients.Count);
                toSpawn = spawnpoint.possibleIngredients[randomNumberOfIngredients];
            }
            else
            {
                toSpawn = spawnpoint.fixedIngredient;
            }
            
            GameObject spawned = Instantiate(toSpawn, spawnpoint.location.position, Quaternion.identity);

            // 🔥 Track it
            spawnedIngredients.Add(spawned);
            // Instantiate(toSpawn, spawnpoint.location.position, Quaternion.identity);
        }
    }
    public void ClearIngredients()
    {
        for (int i = 0; i < spawnedIngredients.Count; i++)
        {
            if (spawnedIngredients[i] != null)
            {
                Destroy(spawnedIngredients[i]);
            }
        }

        spawnedIngredients.Clear();
    }
}