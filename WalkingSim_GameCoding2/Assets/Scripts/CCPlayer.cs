using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CCPlayer : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 9f;
    public float jumpHeight = 5f;

    public Transform cameraTransform;
    public float lookSensitivity = 1f;

    private CharacterController cc;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float verticalVelocity; //current upward/downward speed when we jump
    private float gravity = -20f; //constant downward acceleration

    private float pitch; //up & down

    //interaction variables
    private GameObject currentTarget;
    public Image reticleImage;
    public bool interactPressed;
    public static event Action<NPCData> OnDialogueRequested;
    public static event Action<Item> OnCheckItem;
    private Interactable currentInteractable;

    private bool isSprinting;
    private bool isJumping;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();

        //optional cursor locking
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //find reticle
        reticleImage = GameObject.Find("Reticle").GetComponent<Image>();
        reticleImage.color = new Color(0, 0, 0, 0.7f); //slightly transparent black
    }

    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        { HandleLook(); }
        DialogueManager diaM = FindAnyObjectByType<DialogueManager>();
        if (!diaM.isActive)
        {
            HandleMovement();
        }
        CheckInteract();
        HandleInteract();

    }
    void Start()
    {

    }


    private bool GetKeyDown(KeyCode escape)
    {
        throw new NotImplementedException();
    }

    private void HandleLook()
    {
        //horizontal movement rotates player
        float yaw = lookInput.x * lookSensitivity;
        //vertical mouse movement rotates camera
        float pitchDelta = lookInput.y * lookSensitivity;
        
        transform.Rotate(Vector3.up * yaw);

        //accumulate vertical rotation
        pitch -= pitchDelta;
        //clamp it so we dont flip upside down
        pitch = Mathf.Clamp(pitch, -90, 90);

        cameraTransform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
    private void HandleMovement()
    {
        //updating our bool to be true or false if the player is grounded
        bool grounded = cc.isGrounded;
        //Debug.Log("is grounded: " + grounded);

        //this keeps the character controller snapped to the ground
        if(grounded && verticalVelocity <= 0)
        {
            verticalVelocity = -2f;
        }

        float currentSpeed = walkSpeed;

        //if running is true, set current speed to runSpeed
        if(isSprinting)
        {
            currentSpeed = sprintSpeed;
        } //if it is false set it back to walkSpeed
        else if (!isSprinting)
        {
            currentSpeed = walkSpeed;
        }

        Vector3 move = transform.right * moveInput.x * currentSpeed + transform.forward * moveInput.y * currentSpeed;

        //if jumping is true & we are grounded
        if(isJumping && grounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else
        {
            isJumping = false;
        }

        //apply gravity to every frame
        verticalVelocity += gravity * Time.deltaTime;
        //convert verticalVelocity into movement vector
        Vector3 velocity = Vector3.up * verticalVelocity;
        //now we are finally MOVING our PLAYER

        cc.Move((move + velocity) * Time.deltaTime);

    }

    void CheckInteract()
    {
        //reset reticle image to normal color first
        if (reticleImage != null) reticleImage.color = new Color(100, 100, 100, .5f);
        //make a ray that goes straight out of the camera(center of screen)
        //players eyesight
        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        //RaycastHit hit;
        //asking unity if it hit something within 3 units
        //hit stores what we hit like the collider
        //bool didHit = Physics.Raycast(ray, out hit, 3);
        //if (!didHit) return;//if we didn't hit anything start here
        //if we hit something tagged interactable
        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {

            currentInteractable = hit.collider.GetComponentInParent<Interactable>();
            if(currentInteractable != null && reticleImage != null)
            {
                reticleImage.color = Color.red;
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3, Color.blue);
            }
            else
            {
                Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3, Color.blue);
            }
        }

        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * 3, Color.blue);
    }

    void HandleInteract()
    {
        //if the player did not press interact this frame do nothing
        if (!interactPressed) return;
        //consume the input so one click only triggers one interactions
        //this changes next frame
        interactPressed = false;
        if(currentInteractable == null)
        {
            return;
        }
        currentInteractable.Interact(this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //if we are actually hitting the key, isJumping = true
        if(context.performed)
        {
            isJumping = true;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();

    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("OnInteract fired. performed= " + context.performed);
        if (context.performed) interactPressed = true;
    }

    public void CursorLock(InputAction.CallbackContext context)
    {
        //if cursor IS locked, unlock it
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            //unlock it
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return; //Return bc otherwise it would lock it again on next line
        }
        //if cursor IS NOT locked, lock it
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }
    }

    public void RequestDialogue(NPCData npcData)
    {
        OnDialogueRequested?.Invoke(npcData);
    }

    //example
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("CC collided with: " + hit.gameObject.name);
    }

    public void CheckRequest(Item item)
    {
        OnCheckItem?.Invoke(item);
    }
    


}
