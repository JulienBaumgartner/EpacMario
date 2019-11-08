using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackground : MonoBehaviour
{
    [SerializeField]
    GameObject go1;
    [SerializeField]
    GameObject go2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Camera.main.transform.position.x > go2.transform.position.x)
        {
            float delta = go2.transform.position.x - go1.transform.position.x;
            go1.transform.position += new Vector3(delta, 0, 0);
            go2.transform.position += new Vector3(delta, 0, 0);
        }
        if (Camera.main.transform.position.x < go1.transform.position.x)
        {
            float delta = go2.transform.position.x - go1.transform.position.x;
            go1.transform.position -= new Vector3(delta, 0, 0);
            go2.transform.position -= new Vector3(delta, 0, 0);
        }
    }
}
