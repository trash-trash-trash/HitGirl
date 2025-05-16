using System;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public PlayerInputHandler playerInputs;
    
    public Transform cameraArm;
    public float mouseSensitivity = 1.5f;

    public void Awake()
    {
        playerInputs.AnnounceLookVector2 += HandleLook;
    }

    private void HandleLook(Vector2 lookInput)
    {
        if (cameraArm == null) return;

        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        cameraArm.Rotate(Vector3.up, mouseX, Space.World);
        cameraArm.Rotate(Vector3.forward, mouseY, Space.Self);
    }

    void OnDisable()
    {
        playerInputs.AnnounceLookVector2 -= HandleLook;
    }
}
