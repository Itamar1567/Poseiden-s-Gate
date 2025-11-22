using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public event Action<int> OnRoundChanged;
    public event Action<int> OnEnemyCountChanged;


    public static GameManager instance;

    [SerializeField] private int maxSpawnedEnemies = 3;

    //Prefabs
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<GameObject> pickupables; 

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
    private Tilemap pickupAblesTilemap;

    //Required Variables
    [SerializeField] private float timeBetweenItemSpawn = 3f;
    private bool canSpawnItem = true;

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
        pickupAblesTilemap = FindPickupablesSpawnAreas();
    }

    // Update is called once per frame
    void Update()
    {
        

        if(enemies == 0)
        {
            SpawnEnemies();
            maxSpawnedEnemies += 2;
            round++;
            OnRoundChanged.Invoke(round);
        }

        if (canSpawnItem)
        {
            SpawnPickupAbles();
        }
    }

    private Tilemap FindPickupablesSpawnAreas()
    {
        if (GameObject.Find("PickupablesSpawnAreas").TryGetComponent<Tilemap>(out Tilemap map))
        {
            return map;
        }

        return null;
    }

    private void SpawnPickupAbles()
    {
        
        if (pickupables.Count <= 0) { return; }

        BoundsInt bounds = pickupAblesTilemap.cellBounds;
        
        int randomIndex = UnityEngine.Random.Range(0, pickupables.Count);
        GameObject itemToSpawn = pickupables[randomIndex];

        Vector3Int cell = GetRandomTilemapCell(bounds, pickupAblesTilemap);
        Vector3 worldPos = pickupAblesTilemap.GetCellCenterWorld(cell);

        Instantiate(itemToSpawn, worldPos, Quaternion.identity);

        StartCoroutine(ItemSpawnTimer());

    }

    private IEnumerator ItemSpawnTimer()
    {
        canSpawnItem = false;
        yield return new WaitForSeconds(timeBetweenItemSpawn);
        canSpawnItem = true;
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

        Vector3Int cell = GetRandomTilemapCell(bounds, enemyTilemap);
        Vector3 worldPos = enemyTilemap.GetCellCenterWorld(cell);
        GameObject enemy = Instantiate(basicEnemyPrefab, worldPos, Quaternion.identity);

        if(enemy.TryGetComponent(out Enemy enemyScript)) { enemyScript.SetTarget(player); }

        if(enemy.TryGetComponent(out EnemyHealth enemyHealth)){ enemyHealth.OnEnemyDeath += HandleEnemyDeath; }

        enemies++;

        OnEnemyCountChanged.Invoke(enemies);

    }
    private void HandleEnemyDeath(EnemyHealth enemyHealth)
    {

        enemies--;
        if (enemyHealth != null) { enemyHealth.OnEnemyDeath -= HandleEnemyDeath; }
        OnEnemyCountChanged.Invoke(enemies);

    }

    //Returnes a valid spawn point for an enemy
    private Vector3Int GetRandomTilemapCell(BoundsInt bounds, Tilemap tilemap)
    {
        Vector3Int cell;

        do
        {
            cell = new Vector3Int(
                UnityEngine.Random.Range(bounds.xMin, bounds.xMax),
                UnityEngine.Random.Range(bounds.yMin, bounds.yMax),
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
