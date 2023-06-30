using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonnusController : MonoBehaviour
{
    int v_Bonnus = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < -11.2f)
        {
            Destroy(gameObject);
        }
    }
    public int GetBonnus()
    {
        return v_Bonnus;
    }
}
