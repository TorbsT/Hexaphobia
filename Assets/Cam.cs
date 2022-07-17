using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public static Cam Instance { get; private set; }

    [Range(1f, 10f)] public float minZoom;
    [Range(5f, 20f)] public float maxZoom;
    [Range(0f, 5f)] public float zoomSpeed;
    public new Camera camera;
    public Animator animator;

    private void Awake()
    {
        camera = GetComponent<Camera>();
        animator = GetComponent<Animator>();
        Instance = this;
    }

    private void Start()
    {
        
    }


    private void Update()
    {
        float scroll = -Input.mouseScrollDelta.y*Time.deltaTime;
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize + zoomSpeed*scroll*50f, minZoom, maxZoom);
    }

    private void FixedUpdate()
    {
        Vector3 pos = Player.Instance.transform.position;
        pos.z = -10;
        transform.position = pos;
    }
}
