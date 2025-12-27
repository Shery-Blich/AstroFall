using UnityEngine;

public class NyanCatSpawn : MonoBehaviour
{
    [SerializeField] 
    GameObject nyanCatPrefab;
    [SerializeField]
    float spawnInterval;

   
    [SerializeField]
    float spawnY = 25f;


    private float spawnTime;

    private void Start()
    {
        spawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= spawnTime)
        {

            SpawnNyanCat();
            spawnTime = Time.time + spawnInterval;
        }

    }

    private void SpawnNyanCat()
    {
        float randomX = Random.Range(-ScreenController.Instance.ScreenBounds.x, ScreenController.Instance.ScreenBounds.x);
        Vector3 spawnPos = new Vector3(randomX, spawnY, 0f);

        NyanCatMove nyanCat =
            Instantiate(nyanCatPrefab, spawnPos, Quaternion.identity)
            .GetComponent<NyanCatMove>();

        nyanCat.direction = Vector3.up;
    }
}

