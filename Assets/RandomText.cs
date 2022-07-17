using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomText : MonoBehaviour
{
    public List<string> texts = new();
    public TextMeshProUGUI mesh;
    // Start is called before the first frame update
    void OnEnable()
    {
        mesh.text = texts[Random.Range(0, texts.Count)];
    }
}
