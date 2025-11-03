using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 120.0f;

    private CharacterController controller;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Input básico
        float move = Input.GetAxis("Vertical");    // W/S
        float turn = Input.GetAxis("Horizontal");  // A/D

        // Movimiento hacia adelante/atrás
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        controller.SimpleMove(forward * move * moveSpeed);

        // Rotación en el eje Y
        transform.Rotate(0, turn * rotationSpeed * Time.deltaTime, 0);

        // Animaciones
        // Speed = magnitud del movimiento (positivo = caminar, 0 = idle)
        animator.SetFloat("speed", Mathf.Abs(move));

        // Turn = se puede usar para blend si tenés animaciones de giro
        animator.SetFloat("turn", turn);
    }



    public void Inspect()
    {
        animator.SetBool("inspecting", true);

        // volver a Idle después de 2 segundos
        Invoke("StopInspecting", 2.0f);
    }

    private void StopInspecting()
    {
        animator.SetBool("inspecting", false);
    }
}

