using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeScript : MonoBehaviour
{
    public GameObject dummyObject;
    public Rigidbody dummyRb;

    void Start()
    {
        GameObject newObject1 = Instantiate(dummyObject);

        Rigidbody newObject2 = Instantiate(dummyRb);
        
        newObject1.transform.position = new Vector3(0, 0, 0);
        newObject1.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
