using UnityEngine;

public class TargetGizmo : MonoBehaviour
{
    public GameObject ghost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ghost.GetComponent<AI>().targetTile != null)
        {
            Vector3 pos = new Vector3(ghost.GetComponent<AI>().targetTile.X,
                ghost.GetComponent<AI>().targetTile.Y, 0f);

            transform.position = pos;
        }
    }
}
