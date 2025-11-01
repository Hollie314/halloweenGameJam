using Unity.Cinemachine;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public LayerMask layerMaskInteractible;
    public Animator playerAnimator;
    public GameObject StateDrivenCamera;
    private GameObject HitObject;
    private GameObject Autel;
    private InputManager inputManager;
    private Transform holeCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = FindFirstObjectByType<InputManager>();
        StateDrivenCamera = FindFirstObjectByType<CinemachineStateDrivenCamera>().gameObject;
        Autel = GameObject.FindGameObjectsWithTag("Autel")[0];
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

            // Highlight the hole
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
            Debug.Log("Did not Hit");

            if (HitObject != null)
            {
                // stop highlighting the hole
            }

            HitObject = null;
        }
    }

    public void Interact()
    {
        Debug.Log("interacting");
        if (HitObject != null)
        {
            Debug.Log("interacting with something");
            if (HitObject.CompareTag("Hole") && !playerAnimator.GetBool("Through hole"))
            {
                Debug.Log("interact with hole");
                playerAnimator.SetBool("Through hole", true);
                inputManager.canMove = false;

                StateDrivenCamera.transform.GetChild(0).gameObject.GetComponent<PlayerInteract>().enabled = false;
                holeCamera = HitObject.transform.GetChild(0);
                holeCamera.SetParent(StateDrivenCamera.transform, true);
                holeCamera.gameObject.SetActive(true);
                StateDrivenCamera.GetComponent<CinemachineStateDrivenCamera>().Instructions[2].Camera = holeCamera.GetComponent<CinemachineCamera>();
            }
            else if (HitObject.CompareTag("CanTake") && playerAnimator.GetBool("Through hole"))
            {
                Debug.Log("object Taken");
                GameObject newObject = Instantiate(HitObject, Autel.transform.GetChild(0));
                Destroy(HitObject);
                HitObject = null;
            }
        }
    }

    public void QuitHole()
    {
        if(playerAnimator.GetBool("Through hole") == true)
        {
            playerAnimator.SetBool("Through hole", false);
            inputManager.canMove = true;
            holeCamera.SetParent(HitObject.transform, true);
            holeCamera.SetAsFirstSibling();
            holeCamera.gameObject.SetActive(false);
            StateDrivenCamera.transform.GetChild(0).gameObject.GetComponent<PlayerInteract>().enabled = true;
        }
    }
}
