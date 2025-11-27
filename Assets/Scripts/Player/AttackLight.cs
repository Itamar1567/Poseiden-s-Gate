using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AttackLight : MonoBehaviour
{
    public event Action<AttackLight> OnDestroy;

    [SerializeField] private float damageTimeWhenLightIsIdle = 0.5f;
    [SerializeField] private float startBlinkTime = 1f;
    [SerializeField] int damage = 1;
    [SerializeField] private AudioClip shotSound;

    private Light2D light2D;
    private CircleCollider2D circleCollider;
    private AudioSource audioSource;
    

    private float initialLightIntensity;

    private bool canBlink = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        light2D = GetComponent<Light2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;
        FitColliderToLight();

        initialLightIntensity = light2D.intensity;

        StartCoroutine(BlinkLightUntilStop());

        

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FitColliderToLight()
    {
        circleCollider.radius = light2D.pointLightInnerRadius;
    }

    private IEnumerator BlinkLightUntilStop()
    {

        int timeDecreaseMultiplier = 15;
        float newBlinkTime = startBlinkTime;

        while(newBlinkTime > 0f)
        {

            //Checks what state the light is in: On or Off, and assigns the opposite of that state
            light2D.intensity = light2D.intensity > 0f ? 0f : initialLightIntensity;

            audioSource.PlayOneShot(shotSound);

            yield return new WaitForSeconds(newBlinkTime);

            newBlinkTime = Mathf.MoveTowards(newBlinkTime, 0, Time.deltaTime * timeDecreaseMultiplier);
        }


        

        canBlink = false;

        StartCoroutine(DamageGivingTime());

        yield return null;
    }

    private IEnumerator DamageGivingTime()
    {
        light2D.intensity = initialLightIntensity;
        circleCollider.enabled = true;

        yield return new WaitForSeconds(damageTimeWhenLightIsIdle);
        OnDestroySelf();

    }

    private void OnDestroySelf()
    {
        OnDestroy.Invoke(this);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null) 
        { 
        
            if(collision.TryGetComponent(out Damageable damageable))
            {
                damageable.TakeDamage(damage);
                OnDestroySelf();
            }

        }
    }
}
