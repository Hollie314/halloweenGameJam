using UnityEngine;

public class FaceCam : MonoBehaviour
{
    public GameObject player;
    // Update is called once per frame

    void Update()
    {
        transform.LookAt(player.transform.position);
    }
}
