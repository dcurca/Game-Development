using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour
{
    [SerializeField] private RectTransform healthBarRect;
    [SerializeField] private Text healthText;

    public void SetHealth(int current, int max)
    {
        float value = (float)current / max;

        healthBarRect.localScale = new Vector3(value, healthBarRect.localScale.y, healthBarRect.localScale.z);
        healthText.text = current + "/" + max + "HP";

        Image image = healthBarRect.GetComponent<Image>();
        if(current <= 0.25 * max)
        {
            image.color = new Color(255, 0, 0);
        }
        else if(current <= 0.5 * max)
        {
            image.color = new Color(229, 207, 0);
        }
    }
}
