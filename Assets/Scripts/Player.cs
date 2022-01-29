using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Controls controls;

    public float speed;
    public float turnSpeed;

    public Vector2 controllerInput;
    private void Awake()
    {
        controls = new Controls();

        controls.Racing.Move.performed += ctx => controllerInput = ctx.ReadValue<Vector2>();
        controls.Racing.Move.canceled += ctx => controllerInput = Vector2.zero;

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.fixedDeltaTime * speed);

        if (controllerInput.x >= 0.2f)
        {
            transform.Translate(Vector2.right * Time.fixedDeltaTime * turnSpeed);
        }

        if (controllerInput.x <= -0.2f)
        {
            transform.Translate(Vector2.left * Time.fixedDeltaTime * turnSpeed);
        }
    }

    private void OnEnable()
    {
        controls.Racing.Enable();
    }

    private void OnDisable()
    {
        controls.Racing.Disable();
    }
}
