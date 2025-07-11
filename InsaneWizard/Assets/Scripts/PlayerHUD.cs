using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("HUD References")]
    public Image healthFill;


    [Header("Player Stats")]
    public float maxHealth = 100f;


    [HideInInspector] public float currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
        UpdateHUD();
    }

    void Update()
    {
        // Example controls (testing only)
        if (Input.GetKeyDown(KeyCode.H)) currentHealth -= 10f;


        // Clamp values and update HUD
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (healthFill != null)
            healthFill.fillAmount = currentHealth / maxHealth;

        
    }
}