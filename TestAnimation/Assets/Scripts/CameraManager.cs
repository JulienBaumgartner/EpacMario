using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    float minY = 0;
    float minX;
    [SerializeField]
    GameObject mario;
    // Start is called before the first frame update
    void Start()
    {

        minX = mario.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        minX = Mathf.Max(minX, mario.transform.position.x);
        transform.position = new Vector3(minX, Mathf.Max(minY, mario.transform.position.y),transform.position.z);
    }
}
