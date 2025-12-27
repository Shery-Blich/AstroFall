using UnityEngine;

public class NyanCatSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject spawnRightPos;

    [SerializeField]
    GameObject spawnLeftPos;

    [SerializeField]
    GameObject NyanCatPrefab;

    [SerializeField]
    float spawnInterval;


    private float spawnTime;

    private void Start()
    {
        spawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= spawnTime)
        {
            bool spawnLeft = Random.value > 0.5f;
            NyanCatMove nyanCat;

            if (!spawnLeft)
            {
                nyanCat = Instantiate(NyanCatPrefab, spawnLeftPos.transform.position, Quaternion.identity).GetComponent<NyanCatMove>();
                nyanCat.sprite.flipX = true;
                nyanCat.direction = Vector3.left;
            }
            else
            {
                nyanCat = Instantiate(NyanCatPrefab, spawnRightPos.transform.position, Quaternion.identity).GetComponent<NyanCatMove>();
                nyanCat.direction = Vector3.right;
            }

            spawnTime = Time.time + spawnInterval;
        }

    }
}

