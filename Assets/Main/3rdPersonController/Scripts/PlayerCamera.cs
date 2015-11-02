using UnityEngine;
using System.Collections;
using Globals;

public class PlayerCamera : MonoBehaviour
{
    
    #region Public Fields & Properties
    //Camera rotation in the Y axis
    public float yMinLimit = -85f;
    public float yMaxLimit = 85f;

    //Camera FOV
    public float normalFOV = 60f;
    public float zoomFOV   = 30f;

    //Speed of the zoom from and to Zoomed and unzoomed
    public float lerpSpeed = 8.0f;

    //Camera position on the Y axis, currently aim height working not normalHeight
    public float positionlerp = 6f;
    public float normalHeight = 1.5f;
    public float normalAimHeight = 1.8f;

    //Camera Position on the X axis 
    public float normalDistance = 2.5f;
    public float normalAimDistance = 1f;

    //Has something to do with the enemy targeting with the ray from the camera
    public float minHeight = 0.5f;
    public float maxHeight = 2f;

    //Has something to do with the enemy targeting with the ray from the camera
    public float minDistance = 0.2f;
    public float maxDistance = 2.5f;

    //allows the player to look around himself if not moving
    public bool orbit;

    public Transform target;
    public Transform player;

    //I think Speed = camera turn speed, aimSpeed = player turn speed, and max speed is how fast its allowed to go
    public Vector2 speed = new Vector2(135f, 135f);
    public Vector2 aimSpeed = new Vector2(100f, 100f);
    public Vector2 maxSpeed = new Vector2(100f, 100f);

    public LayerMask hitLayer;

    //Think of the camera on a circular rig around the player. Where the camra sits X, and Z are editable, Y is not.
    public Vector3 normalDirection = new Vector3(-1f, 0f, 0.3f);
    public Vector3 aimDirection = new Vector3(-1f, 0f, 0.7f);

    public Camera theCam;
    #endregion

    #region Private Fields & Properties
    private float x = 0.0f;
    private float y = 0.0f;

    private float deltaTime;
    private float targetDistance;
    private float targetHeight;

    private Transform camTransform;

    private Vector3 position;
    private Vector3 camDir;
    private Vector3 campPos;

    private Quaternion rotation;

    private PlayerController playerController;
    #endregion

    #region Getters & Setters
    public float X { get { return x; } }
    public float Y { get { return y; } }
    #endregion

    #region System Methods
    private void Start()
    {
        theCam = gameObject.GetComponent<Camera>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if(target == null || player == null)
        {
            Destroy(this);
            return;
        }

        target.parent = null;

        camTransform = transform;

        Vector3 angles = camTransform.eulerAngles;

        x = angles.y;
        y = angles.x;

        playerController = player.GetComponent<PlayerController>();

        targetDistance = normalDistance;

        campPos = player.position + new Vector3(0, normalHeight, 0);
    }

    // Update is called once per frame
    private void Update()
    {
        //This hides cursor in game unless you input cancel
        if (Input.GetButtonDown(PlayerInput.Cancel))
        {
            Cursor.visible = !Cursor.visible;
            Cursor.lockState = CursorLockMode.None;
        }

        //Toggles camera orbit on and off
        if (Input.GetAxis(PlayerInput.Horizontal) != 0.0 || Input.GetAxis(PlayerInput.Veritical) != 0.0 || playerController.aim)
        {
            GoToOrbitMode(false);
        }

        if(!orbit && playerController.idleTimer > 0.1)
        {
            GoToOrbitMode(true);
        }
       
    }

    public void LateUpdate()
    {
        deltaTime = Time.deltaTime;

        GetInput();
        RotatePlayer();
        CameraMovement();
    }
    #endregion

    #region Custom Methods
    private void GoToOrbitMode(bool state)
    {
        orbit = state;
        playerController.idleTimer = 0.0f;
    }

    private void GetInput()
    {
        //This checks to see if the player controller is aimed. If it is aimed, then set it to aimspeed. If it isnt set it to speed
        Vector2 a = playerController.aim ? aimSpeed : speed;

        x += Mathf.Clamp(Input.GetAxis(PlayerInput.MouseX) * a.x,  -maxSpeed.x, maxSpeed.x) * deltaTime;
        y -= Mathf.Clamp(Input.GetAxis(PlayerInput.MouseY) * a.y,  -maxSpeed.y, maxSpeed.y) * deltaTime;
        y = ClampAngle(y, yMinLimit, yMaxLimit);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    private void RotatePlayer()
    {
        if (!orbit) playerController.targetYRotation = x;
    }

    public void CameraMovement()
    {
        //aim checking
        if(playerController.aim)
        {
            theCam.fieldOfView = Mathf.Lerp(theCam.fieldOfView, zoomFOV, deltaTime * lerpSpeed);

            camDir = (aimDirection.x * target.forward) + (aimDirection.z * target.right);
            targetHeight = normalAimHeight;
            targetDistance = normalAimDistance;
        }
        else
        {
            theCam.fieldOfView = Mathf.Lerp(theCam.fieldOfView, normalFOV, deltaTime * lerpSpeed);

            camDir = (normalDirection.x * target.forward) + (normalDirection.z * target.right);
            targetHeight = normalAimHeight;
            targetDistance = normalAimDistance;
        }

        camDir = camDir.normalized;

        campPos = player.position + new Vector3(0, targetHeight, 0);

        RaycastHit hit;
        if(Physics.Raycast(campPos, camDir, out hit, targetDistance + 0.2f, hitLayer))
        {
            float t = hit.distance - 0.1f;
            t -= minDistance;
            t /= (targetDistance - minDistance);
            targetHeight = Mathf.Lerp(maxHeight, targetHeight, Mathf.Clamp(t, 0.0f, 1.0f));
            campPos = player.position + new Vector3(0, targetHeight, 0);

            targetDistance = hit.distance - 0.1f;
        }

        Vector3 lookPoint = campPos;
        lookPoint += (target.right * Vector3.Dot((camDir * targetDistance), target.right));

        camTransform.position = campPos + (camDir * targetDistance);
        camTransform.LookAt(lookPoint);

        target.position = campPos;
        target.rotation = Quaternion.Euler(y, x, 0);
    }
    #endregion
}
