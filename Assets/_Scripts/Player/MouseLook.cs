using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public PlayerInputHandler playerInputs;
    
    public Transform cameraArm;
    public float mouseSensitivity = 1.5f;

    public bool canLook = true;

    public void Awake()
    {
        playerInputs.AnnounceLookVector2 += HandleLook;

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void HandleLook(Vector2 lookInput)
    {
        if (cameraArm == null) return;

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        if (canLook)
        {
            cameraArm.Rotate(Vector3.up, mouseX, Space.World);
            cameraArm.Rotate(Vector3.forward, mouseY, Space.Self);
        }
    }

    void OnDisable()
    {
        playerInputs.AnnounceLookVector2 -= HandleLook;
    }
}
