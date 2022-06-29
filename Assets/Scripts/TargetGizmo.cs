using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGizmo : MonoBehaviour
{
    public GmaeObject ghost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ghost.GetComponent<AI>().targetTile != null)
        {
            Vector pos = new Vector3(ghost.GetComponent<AI>().targetTile.x,
                ghost.GetComponent<AI>().targetTile.y, 0f);

            transform.position = pos;
        }
    }
}
