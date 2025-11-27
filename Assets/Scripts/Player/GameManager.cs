using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public event Action<string, int, int> OnInitiateBoss;
    public event Action<int> OnRoundChanged;
    public event Action<int> OnFadeInOrOutRequest;
    public event Action<int> OnEnemyCountChanged;
    public event Action<string,int,int> OnBossHealthChange;
    public event Action OnBossDeath;


    public static GameManager instance;

    [SerializeField] private int maxSpawnedEnemiesAtStart = 3;
    private int maxSpawnedEnemiesForRound;


    [SerializeField] private Light2D Sun;

    //Prefabs
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private List<GameObject> bossEnemies;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private List<GameObject> pickupables; 

    //Camera
    [SerializeField] private Camera mainCam;
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private float lensSize = 5;

    //Location References
    [SerializeField] private GameObject shopPoint;
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject BossSpawnPoint;

    private bool isInShop = false;
    private bool GameEnded = false;
    private bool isPlayerDead = false;

    private int highestRoundAchieved;

    private int enemies = 0;
    private int round = 0;
    private int currentBossIndex = 0;

    //Player reference
    private GameObject player;

    //Tilemaps
    private Tilemap enemyTilemap;
    private Tilemap pickupAblesTilemap;

    //Required Variables
    [SerializeField] private float timeBetweenItemSpawn = 3f;
    private bool canSpawnItem = true;
    private List<GameObject> allEnemiesRef = new List<GameObject>();




    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxSpawnedEnemiesForRound = maxSpawnedEnemiesAtStart;

        SetPlayer();
        SetPlayerCamera();
        enemyTilemap = FindEnemySpawnAreas();
        pickupAblesTilemap = FindPickupablesSpawnAreas();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInShop) { return; }
        if (GameEnded) { return; }

        if(enemies == 0)
        {
            round++;
            highestRoundAchieved = round;
            OnRoundChanged?.Invoke(round);

            //Multiples of 5 only
            bool isBossRound = (round % 5 == 0 && round != 1);

            if (isBossRound)
            {
                SpawnBossEnemy();
            }
            else
            {
                SpawnEnemies();
                maxSpawnedEnemiesForRound += 2;
            }


                
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

    //Used every 5 rounds
    private void SpawnBossEnemy()
    {

        switch (round)
        {
            case 5:
                currentBossIndex = 0; 
                break;
            case 10:
                currentBossIndex = 1;
                break;
            case 15:
                currentBossIndex = 2;
                Sun.intensity = 0.1f;
                break;
            default:
                currentBossIndex = 0;
                break;
        }

        GameObject newBoss = Instantiate(bossEnemies[currentBossIndex], BossSpawnPoint.transform.position, Quaternion.Euler(0,0,-90));

        int bossMaxHealth = 0;
        int bossHealth = 0;
        string name = "";

        if(newBoss.TryGetComponent(out Boss boss)){ boss.SetPlayerTransformReference(player.transform); name = boss.GetBossName(); }

        if (newBoss.TryGetComponent(out EnemyHealth health)) 
        {
            health.OnHealthChanged += OnBossDamagedDisplay;
            
            health.OnEnemyDeath += HandleEnemyDeath; 
            bossHealth = health.GetCurrentHealth();
            bossMaxHealth = health.GetMaxHealh();
        }

        allEnemiesRef.Add(newBoss);

        enemies++;
        OnInitiateBoss.Invoke(name, bossHealth, bossMaxHealth);
        OnEnemyCountChanged?.Invoke(enemies);
    }

    //Used each round change;
    private void SpawnEnemies()
    {
        while(enemies < maxSpawnedEnemiesForRound)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {

        if (enemyTilemap == null) { return; }
        if (enemies >= maxSpawnedEnemiesForRound) { return; }

        BoundsInt bounds = enemyTilemap.cellBounds;

        Vector3Int cell = GetRandomTilemapCell(bounds, enemyTilemap);
        Vector3 worldPos = enemyTilemap.GetCellCenterWorld(cell);
        GameObject enemy = Instantiate(basicEnemyPrefab, worldPos, Quaternion.identity);

        if(enemy.TryGetComponent(out Enemy enemyScript)) { enemyScript.SetTarget(player); }

        if(enemy.TryGetComponent(out EnemyHealth enemyHealth)){ enemyHealth.OnEnemyDeath += HandleEnemyDeath; }

        allEnemiesRef.Add(enemy);

        enemies++;

        //Null check if all enemies have been destroyed
        OnEnemyCountChanged?.Invoke(enemies);

    }
    private void HandleEnemyDeath(EnemyHealth enemyHealth)
    {
        enemies--;

        if (enemyHealth.gameObject.TryGetComponent(out Boss boss)) 
        { 
            OnBossDeath.Invoke();
             
            if (currentBossIndex == bossEnemies.Count - 1 && isPlayerDead != true) { 
                EndGame();
            }
                
        }
        if (enemyHealth != null) { enemyHealth.OnEnemyDeath -= HandleEnemyDeath; }
        allEnemiesRef.Remove(enemyHealth.gameObject);

        //Null check if all enemies have been destroyed
        OnEnemyCountChanged?.Invoke(enemies);

    }

    private IEnumerator EndGameProcedure()
    {
        GameEnded = true;
        OnFadeInOrOutRequest.Invoke(1);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Credits");
    }
    private void EndGame()
    {
        StartCoroutine(EndGameProcedure());   
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

    private Collider2D FindCameraConfiner(string confinerName)
    {
        if (GameObject.Find(confinerName).TryGetComponent<PolygonCollider2D>(out PolygonCollider2D border))
        {
            return border;
        }
        
        return null;
    }
    private void SetPlayer()
    {
        player = Instantiate(playerPrefab, startPoint.transform.position, Quaternion.identity);
        if(player.TryGetComponent(out PlayerHealth health)) { health.OnDie += OnPlayerDeath; }
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
            confiner.m_BoundingShape2D = FindCameraConfiner("CineBorder");
        }
    }
    private void OnPlayerDeath()
    {

        isPlayerDead = true;

        Sun.intensity = 1;

        OnFadeInOrOutRequest.Invoke(1);

        if (playerCam.TryGetComponent<CinemachineConfiner2D>(out CinemachineConfiner2D confiner))
        {
            confiner.m_BoundingShape2D = FindCameraConfiner("CineBorderShop");
        }

        isInShop = true;
        if(player.TryGetComponent(out PlayerController playerController))
        {
            playerController.OnPlayerDeath();
        }


        player.transform.position = shopPoint.transform.position;
        ResetLevel();
    }

    public void OnReplay()
    {

        isPlayerDead = false;

        OnFadeInOrOutRequest.Invoke(0);

        isInShop = false;

        if(playerCam.TryGetComponent(out CinemachineConfiner2D confiner)){

            confiner.m_BoundingShape2D = FindCameraConfiner("CineBorder");

        }
        if(player.TryGetComponent(out PlayerHealth health))
        {
            health.ResetHealthSystem();
        }

        player.transform.position = startPoint.transform.position;
    }

    private void OnBossDamagedDisplay(string name, int health, int maxHealth)
    {
        Debug.Log(maxHealth);
        OnBossHealthChange.Invoke(name, health, maxHealth);
    }
    private void ResetLevel() 
    {
        round = 0;

        //Iterates over a copy of allEnemies because it is being modified as the loop goes
        foreach (GameObject enemy in new List<GameObject>(allEnemiesRef))
        {
            if (enemy.TryGetComponent(out EnemyHealth enemyHealth))
            {
                HandleEnemyDeath(enemyHealth);
            }

            Destroy(enemy);

        }

        allEnemiesRef.Clear();

        enemies = 0;
        maxSpawnedEnemiesForRound = maxSpawnedEnemiesAtStart;

    }
}
