using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{

    [SerializeField]
    GameObject end;
    [SerializeField]
    GameObject[] nextPossiblePart;
    bool alreadyGenerated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!alreadyGenerated && Camera.main.transform.position.x > transform.position.x)
        {
            GameObject newPart = Instantiate(nextPossiblePart[Random.Range(0, nextPossiblePart.Length)]);
            newPart.transform.position = end.transform.position;
            alreadyGenerated = true;
        }
        if(Camera.main.transform.position.x - Camera.main.aspect * Camera.main.orthographicSize > end.transform.position.x)
        {
            Destroy(gameObject);
        }
    }
}
