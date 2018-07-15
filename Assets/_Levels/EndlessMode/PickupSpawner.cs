using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] int pickupNum = 3;
    [SerializeField] Vector3Int spawnArea;

	void Update()
	{
        if (transform.childCount == 0)
        {
            for (int i = 0; i < pickupNum; i++)
            {
                Instantiate(
                    pickupPrefab,
                    new Vector3(Random.Range(0, spawnArea.x) * MazeCell.cellSize, spawnArea.y, Random.Range(0, spawnArea.z) * MazeCell.cellSize),
                    pickupPrefab.transform.rotation,
                    transform
                );
            }
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x * MazeCell.cellSize, spawnArea.y, spawnArea.z * MazeCell.cellSize));
    }
}
