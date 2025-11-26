using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{

    [SerializeField] private TMP_Text bossName;
    [SerializeField] private Slider slider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBossName(string name)
    {
        bossName.text = name;
    }
    public void SetMaxHealth(int maxHealth)
    {
        if (slider != null) {
            slider.maxValue = maxHealth;
        }
    }
    public void SetHealth(int health)
    {
        if (slider != null)
        {
            slider.value = health;
        }
    }
}
