using Unity.Cinemachine;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public LayerMask layerMaskInteractible;
    public Animator playerAnimator;
    public GameObject StateDrivenCamera;
    private GameObject HitObject;
    private InputManager inputManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = FindFirstObjectByType<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        detectionRaycast();
    }

    private void detectionRaycast()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMaskInteractible))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            Debug.Log("Did Hit");
            HitObject = hit.transform.gameObject;
            Debug.Log(HitObject.name);
            Debug.Log(HitObject == null);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
            Debug.Log("Did not Hit");
            //HitObject = null;
        }
    }

    public void Interact()
    {
        Debug.Log("interacting");
        if (HitObject != null)
        {
            Debug.Log("interacting with something");
            if (HitObject.CompareTag("Hole"))
            {
                Debug.Log("interact with hole");
                playerAnimator.SetBool("Through hole", true);
                inputManager.canMove = false;
                Transform holeCamera = HitObject.transform.GetChild(0);
                holeCamera.SetParent(StateDrivenCamera.transform, true);
                holeCamera.gameObject.SetActive(true);
                StateDrivenCamera.GetComponent<CinemachineStateDrivenCamera>().Instructions[2].Camera = holeCamera.GetComponent<CinemachineCamera>();
            }
            else if (HitObject.CompareTag("CanTake"))
            {
                Debug.Log("object Taken");
            }
        }
    }

    public void QuitHole()
    {

    }
}
