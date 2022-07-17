using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradePopup : MonoBehaviour
{
    public string title;
    public string desc;

    [Range(0f, 10f)] public float maxLife;
    public AnimationCurve titleFadeCurve = new();
    public AnimationCurve descFadeCurve = new();
    private new RectTransform transform;
    public TextMeshProUGUI titleMesh;
    public TextMeshProUGUI descMesh;
    public Image image;

    public float life;
    private void Awake()
    {
        transform = GetComponent<RectTransform>();
    }


    private void OnEnable()
    {
        life = 0f;
    }
    private void Update()
    {
        life += Time.deltaTime;
        if (life > maxLife) return;
        titleMesh.text = title;
        descMesh.text = desc;

        titleMesh.color = new(titleMesh.color.r, titleMesh.color.g, titleMesh.color.b, titleFadeCurve.Evaluate(life / maxLife));
        descMesh.color = new(descMesh.color.r, descMesh.color.g, descMesh.color.b, descFadeCurve.Evaluate(life / maxLife));
        
        if (image != null && image.isActiveAndEnabled) image.color = new(titleMesh.color.r, titleMesh.color.g, titleMesh.color.b, titleFadeCurve.Evaluate(life / maxLife));
    
    }
}
