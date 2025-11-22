using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIContoller : MonoBehaviour
{

    PlayerController playerController;


    private Item currentUsedProjectile;

    [SerializeField] private float promptDecayTime = 3f;

    [SerializeField] private Image shootSideImage;
    [SerializeField] private Sprite transparent;
    [SerializeField] private TMP_Text prompt;

    //Prefabs
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject shieldPrefab;

    //Grids
    [SerializeField] private Transform healthGrid;
    [SerializeField] private Transform ammoGrid;


    //Texts and respective requierments
    [SerializeField] private TMP_Text plankAmountDisplay;
    [SerializeField] private Item plank;

    [SerializeField] private TMP_Text enemiesTxt;
    [SerializeField] private TMP_Text roundTxt;


    [SerializeField] private TMP_Text coinAmountDisplay;
    [SerializeField] private Item coin;

    //Delay Sliders
    [SerializeField] private Slider repairSlider;
    [SerializeField] private Slider shootSlider;

    //Coroutine references
    private Coroutine textFadeOut;

    readonly private Dictionary<Item, TMP_Text> itemText = new();
    private Dictionary<Item, int> itemAmounts = new();


    private void Awake()
    {
        itemText.Add(coin, coinAmountDisplay);
        itemText.Add(plank, plankAmountDisplay);
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

        PlayerHealth health = player.GetComponent<PlayerHealth>();
        health.OnHealthChanged += GenerateHealthDisplay;
        health.OnRepair += DisplayShieldRepairDelay;
        health.OnPrompt += SetPrompt;
        health.GetInitialHealth();

        Attack attack = player.GetComponent<Attack>();
        attack.OnChangeSide += DisplayShootSide;
        attack.OnShoot += DisplayShootWaitTime;
        attack.OnChangeProjectile += GenerateAmmoDisplay;

        Inventory inventory = player.GetComponent<Inventory>();
        inventory.OnPrompt += SetPrompt;
        inventory.OnInventoryChange += UpdateItemInInventoryUI;
        itemAmounts = inventory.GetInventory();

        //For not initialization of UI
        foreach (var item in itemAmounts)
        {
            UpdateUI(item.Key, item.Value);
        }
    }

    public void UpdateUI(Item item, int amount)
    {
        switch (item.categories)
        {
            case ItemCategories.Projectile:
                {
                    if (item == currentUsedProjectile || currentUsedProjectile == null)
                    {
                        Debug.Log("Hello");
                        GenerateAmmoDisplay(item, amount);
                    }
                }

                break;
            case ItemCategories.Material: DisplayItemAmount(item, itemAmounts[item]);
                break;

        }
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

    private void DisplayShieldRepairDelay(float delayTime)
    {
        StartCoroutine(FillSlider(delayTime, repairSlider));
    }

    // Fills up the slider's value from 0.1 to 1 gradaully (Based on a given time)
    private IEnumerator FillSlider(float delayTime, Slider slider)
    {

        slider.value = 0;
        float fullVal = 1;

        while (!Mathf.Approximately(slider.value, fullVal))
        {
            slider.value = Mathf.MoveTowards(slider.value, fullVal, Time.deltaTime / delayTime);
            yield return null;
        }

        yield return new WaitForSeconds(delayTime);
    }

    //Inventory


    public void UpdateItemInInventoryUI(Item item, int amount)
    {
        if (itemAmounts.ContainsKey(item)) 
        {
            itemAmounts[item] = amount;
            UpdateUI(item, itemAmounts[item]);
            
        }
        else { Debug.Log("Could not find provided item in UI inventory"); }
        
    }

    public void DisplayItemAmount(Item item, int amount)
    {
        if(itemText.ContainsKey(item))
        {
            itemText[item].text = amount.ToString();
        }
        else
        {
            Debug.Log("Could not find Item type: " + item);
        }
        
    }

    //Attack

    public void DisplayShootWaitTime(float delayTime)
    {
        StartCoroutine(FillSlider(delayTime, shootSlider));
    }

    public void GenerateAmmoDisplay(Item ammoType, int amount)
    {

        currentUsedProjectile = ammoType;

        
        ResetAmmoDisplay();

        Debug.Log("Ammo");

        foreach (Transform slot in ammoGrid)
        {
            if((amount - 1) >= 0)
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

    //Convinience

    public void UpdateEnemyCount(int count)
    {
        enemiesTxt.text = "Enemies: " + count.ToString();
    }
    public void UpdateRoundNumber(int round)
    {
        roundTxt.text = "Round: " + round.ToString();
    }
    //Destroyes all children of a given grid
    public void ResetGridUI(Transform grid)
    {
        foreach (Transform obj in grid)
        {
            Destroy(obj.gameObject);
        }
    }

    public void SetPrompt(string text)
    {
        prompt.text = text;
        if (textFadeOut != null) { StopCoroutine(textFadeOut); }
        FadeOutText(promptDecayTime, prompt);
    }
    public void FadeOutText(float timeToFade, TMP_Text text)
    {
        textFadeOut = StartCoroutine(FadeOutNumeratorText(timeToFade, text));
    }

    private IEnumerator FadeOutNumeratorText(float time, TMP_Text text)
    {

        text.color = Color.white;

        Color initialColor = text.color;
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(initialColor.a, 0f, elapsed / time);
            text.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        // Ensure fully transparent at the end
        text.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
    }


    private void OnEnable()
    {
        GameManager.instance.OnRoundChanged += UpdateRoundNumber;
        GameManager.instance.OnEnemyCountChanged += UpdateEnemyCount;
    }
    private void OnDisable()
    {
        GameManager.instance.OnRoundChanged -= UpdateRoundNumber;
        GameManager.instance.OnEnemyCountChanged -= UpdateEnemyCount;
    }

}

