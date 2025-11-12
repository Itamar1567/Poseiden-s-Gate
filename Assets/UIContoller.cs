using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContoller : MonoBehaviour
{

    PlayerController playerController;

    [SerializeField] private Image shootSideImage;

    [SerializeField] private Animations heartPrefab;

    [SerializeField] private GameObject healthGrid;

    private float shootSide = -1;



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
    public void DestroyHearts(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
           Destroy(healthGrid.transform.GetChild(healthGrid.transform.childCount - 1).gameObject);
        }
    }
    public void GenerateHearts(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            Animations animation = Instantiate(heartPrefab, healthGrid.transform);
            Debug.Log(animation.transform.localPosition);
            //animation.oscillateOnY = true;
        }
    }


}
