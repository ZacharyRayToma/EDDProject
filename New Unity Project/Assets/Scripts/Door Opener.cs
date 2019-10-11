using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    int enemycounter;
    bool Iscomplete;
    // Start is called before the first frame update
    void Start()
    {
        //initial variables
        

    }

    // Update is called once per frame
    void Update()
    {
        if(Iscomplete == false)
        {
            
        } else
        {
            Destroy(this);
        }
    }
}
