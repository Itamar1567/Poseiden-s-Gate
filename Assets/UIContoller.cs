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

    private float shootSide = -1;



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

    public void DisplayTakeDamage(int amount)
    {

        for (int i = 0; i < amount; i++)
        {
            Transform currentChild = healthGrid.GetChild((healthGrid.transform.childCount - 1) - i);

            

                if (currentChild.childCount <= 0)
                {
                    Destroy(currentChild.gameObject);
                }
                else
                {
                    Destroy(currentChild.GetChild(0).gameObject);
                }
            
        }
    }


    public void GenerateHearts(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            Instantiate(heartPrefab, healthGrid);
        }
    }

    public void GenerateShields(int amount)
    {
        for (int i = 0; i < healthGrid.transform.childCount; i++)
        {
            Transform currentChild = healthGrid.GetChild(i);

            if (currentChild.childCount <= 0)
            {
                Instantiate(shieldPrefab, currentChild.transform);
            }
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
   

    public void RemoveAmmoFromDisplay(int amount)
    {
        int amountToRemove = amount >= 0 ? amount : 0;
        
        for(int i = ammoGrid.childCount - 1; i >= 0; i--)
        {

            if(amountToRemove <= 0)
            {
                break;
            }

            Image img = ammoGrid.GetChild(i).GetChild(0).GetComponent<Image>();

            if (img.sprite == transparent)
            {
                continue;
            }
            else
            {
                img.sprite = transparent;
                amountToRemove -= 1;
            }
            
        }
    }
    public void GenerateAmmoDisplay(int amount, Item ammoType)
    {
        ResetAmmoDisplay();

        int maxSlots = ammoGrid.childCount - 1;
        amount = Mathf.Min(amount, maxSlots);


        for (int i = 0; i <= amount; i++)
        {
            Image img = ammoGrid.GetChild(i).GetChild(0).GetComponent<Image>();
            img.sprite = ammoType.icon;

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
