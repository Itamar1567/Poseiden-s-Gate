using System.Collections;
using System.Collections.Generic;
using TMPro;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIContoller : MonoBehaviour
{

    PlayerController playerController;

    [SerializeField] private Image shootSideImage;
    [SerializeField] private Sprite transparent;

    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject shieldPrefab;

    [SerializeField] private Transform healthGrid;
    [SerializeField] private Transform ammoGrid;


    [SerializeField] private TMP_Text plankAmountDisplay;
    [SerializeField] private Item plank;


    private Dictionary<Item, TMP_Text> itemAmounts = new Dictionary<Item, TMP_Text>();


    private void Awake()
    {
        itemAmounts.Add(plank, plankAmountDisplay);
    }

    // Start is called before the first frame update
    void Start()
    {

        
        StartCoroutine(PhaseInOut(shootSideImage, 3f));
    }

    // Update is called once per frame
    void Update()
    {

        if (playerController == null)
            return;
        
    }

    public void BindToPlayer(PlayerController player)
    {
        playerController = player;

        Health health = player.GetComponent<Health>();
        health.OnHealthChanged += GenerateHealthDisplay;
        health.GetInitialHealth();

        Attack attack = player.GetComponent<Attack>();
        attack.OnChangeSide += DisplayShootSide;
        attack.OnChangeAmmo += GenerateAmmoDisplay;
        attack.GetInitialAmmo();
    }
    
    public void DisplayShootSide(float shootSide)
    {
        //Flip the arrow torwards the chose cannon side of the boat
        int angle = shootSide > 0 ? 0 : 180;

        shootSideImage.rectTransform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    //Gradually increases and decreases an Image element's alpha
    private IEnumerator PhaseInOut(Image image, float speed, float opaque = 1f, float transperent = 0.3f)
    {

        Color color = image.color;

        float targetAlpha = color.a < 1f ? opaque : transperent;

        while (true)
        {


            while (!Mathf.Approximately(color.a, targetAlpha))
            {
                color.a = Mathf.MoveTowards(color.a, targetAlpha, speed * Time.deltaTime);
                image.color = color;
                yield return null;
            }

            targetAlpha = color.a < 1f ? opaque : transperent;

            yield return new WaitForSeconds(0.1f);
        }

    }

    //Health


    private void GenerateHealthDisplay(int health, int shield)
    {

        ResetGridUI(healthGrid);

        //Generate hearts and shields after hearts because shields are children of hearts
        for (int i = 0; i < health; i++)
        {
            GameObject heart = Instantiate(heartPrefab, healthGrid);

            if(shield > 0)
            {
                Instantiate(shieldPrefab, heart.transform);
                shield -= 1;
            }
            
        }   
    }

    //Destroyes all children of a given grid
    public void ResetGridUI(Transform grid)
    {
        foreach(Transform obj in grid)
        {
            Destroy(obj.gameObject);
        }
    }

    //Inventory
  
    public void DisplayItemAmount(int amount, Item item)
    {
        if(itemAmounts.ContainsKey(item))
        {
            itemAmounts[item].text = amount.ToString();
        }
        else
        {
            Debug.Log("Could not find Item type: " + item);
        }
        
    }

    //Ammo

    public void GenerateAmmoDisplay(int amount, Item ammoType)
    {
        ResetAmmoDisplay();

        Debug.Log("Ammo");

        foreach (Transform slot in ammoGrid)
        {
            if(amount >= 0)
            {
                slot.GetChild(0).GetComponent<Image>().sprite = ammoType.icon;
                amount -= 1;
            }
        }

    }

    public void ResetAmmoDisplay()
    {
        foreach (Transform child in ammoGrid)
        {
            child.GetChild(0).GetComponent<Image>().sprite = transparent;
        }
    }
        
    
}
