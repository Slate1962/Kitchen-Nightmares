using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// World-space order bubble. Put this on the root of a prefab with a World Space Canvas.
public class OrderBubbleUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform iconContainer;   // object with a Horizontal Layout Group
    [SerializeField] private Image iconPrefab;          // a simple UI Image used as template
    [SerializeField] private TMP_Text orderNameText;    // optional

    private readonly List<GameObject> spawnedIcons = new List<GameObject>();
    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        if (iconPrefab != null) iconPrefab.gameObject.SetActive(false);
    }

    // Always face the camera
    private void LateUpdate()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        transform.forward = cam.transform.forward;
    }

    public void Show(OrderSO order)
    {
        Clear();
        if (order == null) return;

        if (orderNameText != null) orderNameText.text = order.orderName;

        foreach (FoodSO food in order.foods)
        {
            if (food == null || food.foodSprite == null) continue;

            Image icon = Instantiate(iconPrefab, iconContainer);
            icon.sprite = food.foodSprite;
            icon.gameObject.SetActive(true);
            spawnedIcons.Add(icon.gameObject);
        }

        gameObject.SetActive(true);
    }

    public void Clear()
    {
        foreach (GameObject go in spawnedIcons) Destroy(go);
        spawnedIcons.Clear();
    }
}