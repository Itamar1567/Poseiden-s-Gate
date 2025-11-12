using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIContoller : MonoBehaviour
{

    PlayerController playerController;

    [SerializeField] private Image shootSideImage;

    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject shieldPrefab;

    [SerializeField] private GameObject healthGrid;


    [SerializeField] private TMP_Text logAmountDisplay;

    private Dictionary<string, TMP_Text> itemAmounts = new Dictionary<string, TMP_Text>();

    private float shootSide = -1;



    // Start is called before the first frame update
    void Start()
    {
        itemAmounts.Add("log", logAmountDisplay);
        itemAmounts.Add("coin", logAmountDisplay);

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
            Transform currentChild = healthGrid.transform.GetChild((healthGrid.transform.childCount - 1) - i);

            

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
            Instantiate(heartPrefab, healthGrid.transform);
        }
    }

    public void GenerateShields(int amount)
    {
        for (int i = 0; i < healthGrid.transform.childCount; i++)
        {
            Transform currentChild = healthGrid.transform.GetChild(i);

            if (currentChild.childCount <= 0)
            {
                Instantiate(shieldPrefab, currentChild.transform);
            }
        }
    }

    //Inventory
  
    public void DisplayItemAmount(int amount, string type)
    {
        if(itemAmounts.ContainsKey(type))
        {
            itemAmounts[type].text = amount.ToString();
        }
        else
        {
            Debug.Log("Could not find Item type: " + type);
        }
        
    }

}
