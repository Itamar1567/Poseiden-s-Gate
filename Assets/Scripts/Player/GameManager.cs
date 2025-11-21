using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    static GameManager instance;

    [SerializeField] private int maxSpawnedEnemies = 3;

    //Prefabs
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private GameObject playerPrefab;

    //Camera
    [SerializeField] private Camera mainCam;
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private float lensSize = 5;

    private int enemies = 0;
    private int round = 0;

    //Player reference
    private GameObject player;

    //Tilemaps

    private Tilemap enemyTilemap;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetPlayer();
        SetPlayerCamera();
        enemyTilemap = FindEnemySpawnAreas();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemies == 0)
        {
            SpawnEnemies();
            round++;
        }
    }

    private Tilemap FindEnemySpawnAreas()
    {
        if (GameObject.Find("EnemySpawnAreas").TryGetComponent<Tilemap>(out Tilemap map))
        {
            return map;
        }

        return null;
    }

    //Used each round change;
    private void SpawnEnemies()
    {
        while(enemies < maxSpawnedEnemies)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {

        if (enemyTilemap == null) { return; }
        if (enemies >= maxSpawnedEnemies) { return; }

        BoundsInt bounds = enemyTilemap.cellBounds;

        Vector3Int cell = GetRandomEnemyCell(bounds, enemyTilemap);
        Vector3 worldPos = enemyTilemap.GetCellCenterWorld(cell);
        GameObject enemy = Instantiate(basicEnemyPrefab, worldPos, Quaternion.identity);

        if(enemy.TryGetComponent(out Enemy enemyScript)) { enemyScript.SetTarget(player); }

        if(enemy.TryGetComponent(out EnemyHealth enemyHealth)){ enemyHealth.OnEnemyDeath += HandleEnemyDeath; }

        enemies++;
        

    }
    private void HandleEnemyDeath(EnemyHealth enemyHealth)
    {

        enemies--;
        if (enemyHealth != null) { enemyHealth.OnEnemyDeath -= HandleEnemyDeath; }

    }

    //Returnes a valid spawn point for an enemy
    private Vector3Int GetRandomEnemyCell(BoundsInt bounds, Tilemap tilemap)
    {
        Vector3Int cell;

        do
        {
            cell = new Vector3Int(
                Random.Range(bounds.xMin, bounds.xMax),
                Random.Range(bounds.yMin, bounds.yMax),
                0
            );
        }
        while (!tilemap.HasTile(cell));

        return cell;
    }

    private Collider2D FindCameraConfiner()
    {
        if (GameObject.Find("CineBorder").TryGetComponent<PolygonCollider2D>(out PolygonCollider2D border))
        {
            return border;
        }
        
        return null;
    }

    private void SetPlayer()
    {
        player = Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity);
    }
    private void SetPlayerCamera()
    {
        mainCam = Instantiate(mainCam, new Vector3(0, 0, 0), Quaternion.identity);
        playerCam = Instantiate(playerCam, new Vector3(0, 0, 0), Quaternion.identity);

        if (player == null || playerCam == null) { return; } 

        playerCam.LookAt = player.transform;
        playerCam.Follow = player.transform;
        playerCam.m_Lens.OrthographicSize = lensSize;
        //playerCam
        if(playerCam.TryGetComponent<CinemachineConfiner2D>(out CinemachineConfiner2D confiner))
        {
            confiner.m_BoundingShape2D = FindCameraConfiner();
        }
    }
}
