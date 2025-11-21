using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    static GameManager instance;

    //Prefabs
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private GameObject playerPrefab;

    //Camera
    [SerializeField] private Camera mainCam;
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private float lensSize = 5;

    private int enemies = 0;
    private int round = 1;

    //Player reference
    private GameObject player;

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
