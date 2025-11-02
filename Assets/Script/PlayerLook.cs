using Unity.Cinemachine;
using UnityEngine;

public class PlayerLook : MonoBehaviour

{
    public CinemachineCamera cam;
    private float xRotation = 0f;
    private float yRotation = 0f;
    public bool clampLeftRight = false;
    public float clampLeft = -80f;
    public float clampright = 80f;
    public float clampdowm = -80f;
    public float clampup = 80f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public void ProcessLook(Vector2 input)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        float mouseX = input.x;
        float mouseY = input.y;
        //calculate camera rotation for looking up and down
        yRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        yRotation = Mathf.Clamp(yRotation, clampdowm, clampup);

        if (clampLeftRight)
        {
            //calculate camera rotation for looking Left and right
            xRotation += (mouseX * Time.deltaTime) * xSensitivity;
            xRotation = Mathf.Clamp(xRotation, clampLeft, clampright);
            //apply this to our camera transform.
            cam.transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0f);
        }
        else
        {
            cam.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }
    }

    public void ResetCam()
    {
        cam.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
    }
}
