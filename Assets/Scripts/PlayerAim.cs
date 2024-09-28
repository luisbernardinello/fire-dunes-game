using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("Aim control")]
    [SerializeField] public Transform aim;

    [SerializeField] private bool isAimingPrecisly;
    [SerializeField] private bool isLockingToTarget;





    [Header("Camera control")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float minCameraDistance = 1.5f;
    [SerializeField] private float maxCameraDistance = 4f;
    [SerializeField] private float cameraSensetivity = 5f;




    [SerializeField] private LayerMask aimLayerMask;
    //private Vector3 lookingDirection;

    private Vector2 mouseInput;
    private RaycastHit lastKnowMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            isAimingPrecisly = !isAimingPrecisly;

        if (Input.GetKeyDown(KeyCode.L))
            isLockingToTarget = !isLockingToTarget;

        UpdateAimPosition();


        //aim.position = new Vector3(GetMousePosition().x, transform.position.y + 1, GetMousePosition().z);
        UpdateCameraPosition();

    }

  

    public Transform Target()
    {
        Transform target = null;

        if(GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;
        }

        return target;
    }

    private void UpdateCameraPosition()
    {
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), cameraSensetivity * Time.deltaTime);
    }

    private void UpdateAimPosition()
    {

        Transform target = Target();

        if(target != null && isLockingToTarget)
        {
            if (target.GetComponent<Renderer>() != null)
            {
                aim.position = target.GetComponent<Renderer>().bounds.center;
            }
            else
            {
                aim.position = target.position;
            }
            
            return;
        }



        aim.position = GetMouseHitInfo().point;

        if (!isAimingPrecisly)
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
    }

    public Transform Aim() => aim;

    public bool CanAimPrecisly() => isAimingPrecisly;


    private Vector3 DesiredCameraPosition()
    {
        //float actualMaxCameraDistance;
        //bool movingDownwards = player.movement.moveInput.y < -.5f;

        //if (movingDownwards)
        //{
        //    actualMaxCameraDistance = minCameraDistance;
        //}
        //else
        //{
        //    actualMaxCameraDistance = maxCameraDistance;
        //}

        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;


        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);

        float clampedDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualMaxCameraDistance);
        desiredCameraPosition = transform.position + aimDirection * clampedDistance;
        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;

    }
    public RaycastHit GetMouseHitInfo()
    {

        Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnowMouseHit = hitInfo;
            return hitInfo;
        }
        return lastKnowMouseHit;
    }

    private void AssignInputEvents()
    {
        controls = player.controls;


        controls.Character.Aim.performed += context => mouseInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => mouseInput = Vector2.zero;
    }
}

