using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject; // sleep hier je counter in
    [SerializeField] private Image barImage;
    [SerializeField] private GameObject barVisual;

    private IHasProgress hasProgress;

    void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError(hasProgressGameObject.name + " heeft geen IHasProgress component!");
            return;
        }

        hasProgress.OnProgressChanged += OnProgressChanged;
        hasProgress.OnFoodPresenceChanged += OnFoodPresenceChanged;

        barImage.fillAmount = 0f;
        barVisual.SetActive(false);
    }

    void OnDestroy()
    {
        if (hasProgress != null)
        {
            hasProgress.OnProgressChanged -= OnProgressChanged;
            hasProgress.OnFoodPresenceChanged -= OnFoodPresenceChanged;
        }
    }

    private void OnProgressChanged(float progress)
    {
        barImage.fillAmount = progress;
    }

    private void OnFoodPresenceChanged(bool show)
    {
        barVisual.SetActive(show);
    }

    void LateUpdate()
    {
        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;
    }
}
