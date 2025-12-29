using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MemoriesManager : MonoBehaviour
{
    public static MemoriesManager Instance;

    [SerializeField]
    public MemoryCollectibleController[] memoryCollectibleControllers;

    [SerializeField]
    public float MinSpawnDelay = 0.2f;

    [SerializeField]
    public float MaxSpawnDelay = 2f;

    [SerializeField]
    private float currSpawnDelay = 0.0f;

    [SerializeField]
    private float currSpawnDelayTimeLapse = 0.0f;

    public Queue<MemoryCollectibleController> InactiveMemories { get; private set; }

    // SerializeField to show in inspector for debugging purposes
    // Remove once UI for memories is implemented
    [SerializeField]
    public int CollectedMemories;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        InactiveMemories = new Queue<MemoryCollectibleController>(memoryCollectibleControllers);
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
            if (!InactiveMemories.Any())
            {
                return;
            }

            InactiveMemories.Dequeue().RespawnCoin();
            currSpawnDelay = Random.Range(MinSpawnDelay, MaxSpawnDelay);
            currSpawnDelayTimeLapse = 0.0f;
        }
    }

    private void QueueMemoryForRespawn(MemoryCollectibleController memory)
    {
        if (InactiveMemories.Count == memoryCollectibleControllers.Length)
        {
            return;
        }

        this.InactiveMemories.Enqueue(memory);
    }
}
