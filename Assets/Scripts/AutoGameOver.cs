using UnityEngine;
using TMPro;

public class AutoGameOver : MonoBehaviour
{
    private GameObject gameOverPanel;
    private TMP_Text healthText;
    private bool isGameOver = false;

    void Start()
    {
        Time.timeScale = 1f;

        gameOverPanel = FindChildByName(transform, "GameOverPanel")?.gameObject;

        Transform healthTransform = FindChildByName(transform, "HealthText");

        if (healthTransform != null)
        {
            healthText = healthTransform.GetComponent<TMP_Text>();
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("GameOverPanel not found");
        }

        if (healthText == null)
        {
            Debug.LogError("HealthText not found");
        }
    }

    void Update()
    {
        if (isGameOver || healthText == null) return;

        string text = healthText.text; // مثال: 100 / 100
        string[] parts = text.Split('/');

        if (parts.Length > 0)
        {
            int health;

            if (int.TryParse(parts[0].Trim(), out health))
            {
                if (health <= 0)
                {
                    ShowGameOver();
                }
            }
        }
    }

    void ShowGameOver()
    {
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    Transform FindChildByName(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child;
            }

            Transform result = FindChildByName(child, childName);

            if (result != null)
            {
                return result;
            }
        }

        return null;
    }
}