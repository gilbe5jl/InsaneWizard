using UnityEngine;
using UnityEngine.UI;

public class HealthMaskController : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("References")]
    public RectTransform healthMask; // Assign the HealthBarMask object
    public Image maskImage;          // Assign the Image component on the mask
    public float fullMaskWidth = 200f; // Max width of the mask when health = 0

    void Start()
    {
        UpdateMask(); // ensure starting state is correct
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(5f);
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            Heal(100f);
        }
    }

    void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateMask();
    }
    void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateMask();
    }
    void UpdateMask()
    {
        float missingHealth = maxHealth - currentHealth;
        float percentMissing = missingHealth / maxHealth;
        float newWidth = fullMaskWidth * percentMissing;

        // Set mask width
        if (healthMask != null)
        {
            Vector2 size = healthMask.sizeDelta;
            size.x = newWidth;
            healthMask.sizeDelta = size;
        }

        // Set mask transparency (alpha)
        if (maskImage != null)
        {
            Color color = maskImage.color;
            color.a = percentMissing; // 0 when full health, 1 when empty
            maskImage.color = color;
        }
    }
}