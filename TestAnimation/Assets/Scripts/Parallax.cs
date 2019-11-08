using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    Camera camera;
    [SerializeField]
    float factor;
    float initX;
    float initXCamera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        initX = transform.position.y;
        initXCamera = camera.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(initX + factor*(camera.transform.position.x - initXCamera), transform.position.y, transform.position.z);
    }
}
