using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInteract : MonoBehaviour
{
    public LayerMask layerMaskInteractible;
    public InputActionReference interactAction;
    public Animator playerAnimator;
    public GameObject StateDrivenCamera;
    public Material light_Material;
    private Material[] originalMaterials;
    private GameObject HitObject;
    private GameObject Autel;
    private InputManager inputManager;
    private Transform holeCamera;
    private PlayerLook look;
    private UpdateUI updateUI;

    public bool clampLeftRight = false;
    public float clampLeft = -80f;
    public float clampright = 80f;
    public float clampdowm = -80f;
    public float clampup = 80f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputManager = FindFirstObjectByType<InputManager>();
        playerAnimator = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Animator>();
        updateUI = FindFirstObjectByType<UpdateUI>();
        StateDrivenCamera = FindFirstObjectByType<CinemachineStateDrivenCamera>().gameObject;
        Autel = GameObject.FindGameObjectsWithTag("Autel")[0];
    }

    private void Awake()
    {
        interactAction.action.performed += ctx => Interact();
        look = FindFirstObjectByType<PlayerLook>();
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
            if(HitObject != hit.transform.gameObject)
            {
                if (HitObject != null)
                {
                    // stop highlighting the hole
                    if (HitObject.CompareTag("Hole"))
                    {
                        HitObject.transform.GetChild(2).GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = originalMaterials;
                    }
                    else
                    {
                        HitObject.gameObject.GetComponent<MeshRenderer>().materials = originalMaterials;
                    }
                }

                if (hit.transform.CompareTag("Hole"))
                {
                    // Highlight the hole
                    originalMaterials = hit.transform.GetChild(2).GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;
                    Material[] mats = new Material[originalMaterials.Length + 1];
                    for (int i = 0; i < originalMaterials.Length; i++)
                        mats[i] = originalMaterials[i];

                    mats[mats.Length - 1] = light_Material;
                    hit.transform.GetChild(2).GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = mats;
                }
                else
                {
                    // Highlight the hole
                    originalMaterials = hit.transform.gameObject.GetComponent<MeshRenderer>().materials;
                    Material[] mats = new Material[originalMaterials.Length + 1];
                    for (int i = 0; i < originalMaterials.Length; i++)
                        mats[i] = originalMaterials[i];

                    mats[mats.Length - 1] = light_Material;
                    hit.transform.gameObject.GetComponent<MeshRenderer>().materials = mats;
                }
            }
            HitObject = hit.transform.gameObject;
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
            Debug.Log("Did not Hit");

            if (HitObject != null)
            {
                // stop highlighting the hole
                if (HitObject.CompareTag("Hole"))
                {
                    HitObject.transform.GetChild(2).GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = originalMaterials;
                }
                else
                {
                    HitObject.gameObject.GetComponent<MeshRenderer>().materials = originalMaterials;
                }
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
                //holeCamera.SetParent(StateDrivenCamera.transform, true);
                holeCamera.gameObject.SetActive(true);
                StateDrivenCamera.GetComponent<CinemachineStateDrivenCamera>().Instructions[2].Camera = holeCamera.GetComponent<CinemachineCamera>();

                look.cam = holeCamera.GetComponent <CinemachineCamera>();
                look.clampLeftRight = true;
                look.clampLeft = holeCamera.GetComponent<PlayerInteract>().clampLeft;
                look.clampright = holeCamera.GetComponent<PlayerInteract>().clampright;
                look.clampdowm = holeCamera.GetComponent<PlayerInteract>().clampdowm;
                look.clampup = holeCamera.GetComponent<PlayerInteract>().clampup;
            }
            else if (HitObject.CompareTag("Bed") && !playerAnimator.GetBool("Through hole"))
            {
                SceneManager.LoadScene("Main menu");
            }
            else if (HitObject.CompareTag("CanTake"))
            {
                Debug.Log("object Taken");
                GameObject newObject = Instantiate(HitObject, Autel.transform.GetChild(0).position, Quaternion.identity);
                Destroy(HitObject);
                HitObject = null;
                updateUI.UpdateText();
            }
        }
    }

    public void QuitHole()
    {
        if(playerAnimator.GetBool("Through hole") == true)
        {
            playerAnimator.SetBool("Through hole", false);
            StateDrivenCamera.GetComponent<CinemachineStateDrivenCamera>().Instructions[2].Camera = null;
            holeCamera.gameObject.SetActive(false);
            inputManager.canMove = true;
            StateDrivenCamera.transform.GetChild(0).gameObject.GetComponent<PlayerInteract>().enabled = true;

            look.cam = StateDrivenCamera.transform.GetChild(0).gameObject.GetComponent<CinemachineCamera>();
            look.clampLeftRight = false;
            look.clampLeft = -80f;
            look.clampright = 80f;
            look.clampdowm = -80f;
            look.clampup = 80f;
        }
    }
}
