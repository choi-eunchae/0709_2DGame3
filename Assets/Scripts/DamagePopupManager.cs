using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopupManager: MonoBehaviour
{
    public static DamagePopupManager Instance { get; private set; }
    public RectTransform canvasRect;
    public GameObject damageTextPrefab;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void CreateDamageText(int damage, Vector3 worldPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        GameObject textObj = Instantiate(damageTextPrefab, canvasRect);
        textObj.GetComponent<RectTransform>().position = screenPos;

        textObj.GetComponent<DamageText>().Show(damage);
    }
}