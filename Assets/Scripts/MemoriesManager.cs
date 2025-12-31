using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemoriesManager : MonoBehaviour
{
    public static MemoriesManager Instance;
    public int CollectedMemories;

    [SerializeField]
    public MemoryCollectibleController[] memoryCollectibleControllers;

    [SerializeField]
    public float MinSpawnDelay = 2.5f;

    [SerializeField]
    public float MaxSpawnDelay = 3.5f;

    [SerializeField]
    private float currSpawnDelay = 0.0f;

    [SerializeField]
    private float currSpawnDelayTimeLapse = 0.0f;

    [SerializeField]
    private float HorizontalSpawnRateMultiplier = 2.2f;

    private Queue<MemoryCollectibleController> inactiveMemories;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        inactiveMemories = new Queue<MemoryCollectibleController>(memoryCollectibleControllers);
    }

    void Start()
    {
        currSpawnDelay = Random.Range(MinSpawnDelay, MaxSpawnDelay);
        currSpawnDelayTimeLapse = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        currSpawnDelayTimeLapse += Time.deltaTime;
        if (currSpawnDelay < currSpawnDelayTimeLapse)
        {
            if (!inactiveMemories.Any())
            {
                return;
            }

            inactiveMemories.Dequeue().RespawnCoin();
            currSpawnDelay = Random.Range(MinSpawnDelay, MaxSpawnDelay);
            currSpawnDelayTimeLapse = 0.0f;
        }
    }

    public void QueueMemoryForRespawn(MemoryCollectibleController memory)
    {
        if (inactiveMemories.Count == memoryCollectibleControllers.Length)
        {
            return;
        }

        this.inactiveMemories.Enqueue(memory);
    }
}
