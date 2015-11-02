using UnityEngine;
using System.Collections;
using Globals;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(HeadLookController))]
[RequireComponent(typeof(CharacterMotor))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(Weapon))]

public class PlayerController : MonoBehaviour
{

    #region Public Fields & Properties
	public float idleTimer;
	public float runSpeed 			= 4.6f;
	public float runStrafeSpeed 	= 3.07f;
	public float walkSpeed 			= 1.22f;
	public float walkStrafeSpeed 	= 1.22f;
	public float maxRotationSpeed 	= 540f;

	//Public variables that are hidden in the inspector.
	[HideInInspector]
	public float targetYRotation;
	[HideInInspector]
	public bool walk;
	[HideInInspector]
	public bool aim;
	[HideInInspector]
	public bool inAir;
	[HideInInspector]
	public bool grounded;
	[HideInInspector]
	public Vector3 moveDir;
    #endregion

    #region Private Fields & Properties
	private Transform playerTransform;
	private CharacterController controller;
	private CharacterMotor motor;
    #endregion

    #region Getters & Setters

    #endregion

    #region System Methods
    // Use this for initialization
    private void Start()
    {
		idleTimer = 0f;

		playerTransform = transform;

		walk = true;
		aim = false;

		controller = GetComponent<CharacterController> ();
		motor = GetComponent<CharacterMotor> ();

		controller.center = new Vector3 (0f, 1f, 0f); //Sets the center of the Player controller(can be done in inspector)
    }

    // Update is called once per frame
    private void Update()
    {
		GetUserInput ();

		if (!motor.canControl) 
		{
			motor.canControl = true;
		}

		moveDir = new Vector3(Input.GetAxis(PlayerInput.Horizontal), 0f, Input.GetAxis(PlayerInput.Veritical));

		if (moveDir.sqrMagnitude > 1f)moveDir = moveDir.normalized;

		motor.inputMoveDirection = playerTransform.TransformDirection (moveDir);
		motor.inputJump = Input.GetButtonDown (PlayerInput.Jump);
		motor.movement.maxForwardSpeed = (walk) ? walkSpeed : runSpeed;
		motor.movement.maxBackwardsSpeed = motor.movement.maxForwardSpeed;
		motor.movement.maxSidewaysSpeed = (walk) ? walkStrafeSpeed : runStrafeSpeed;

		if (moveDir != Vector3.zero)idleTimer = 0f;

		inAir = !motor.grounded;
		grounded = !inAir;

		float currentAngle = playerTransform.localRotation.eulerAngles.y;
		float delta = Mathf.Repeat ((targetYRotation - currentAngle), 360f);

		if (delta > 180f)delta -= 360f;

		float newYRot = Mathf.MoveTowards (currentAngle, currentAngle + delta, Time.deltaTime * maxRotationSpeed);
		Vector3 newLocalRot = new Vector3 (playerTransform.localRotation.eulerAngles.x, newYRot, playerTransform.localRotation.eulerAngles.z);
		playerTransform.localRotation = Quaternion.Euler (newLocalRot);
    }
    #endregion

    #region Custom Methods
    private void GetUserInput()
	{
		aim = Input.GetButton (PlayerInput.Fire2);

		idleTimer += Time.deltaTime;

		walk =(!Input.GetButton(PlayerInput.Run) || moveDir == Vector3.zero || Input.GetAxis(PlayerInput.Veritical) < 0f); //Makes the player walk unless he presses shift. Also forces player to walk if going backwards
	}
    #endregion
}
