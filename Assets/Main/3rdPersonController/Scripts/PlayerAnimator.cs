using UnityEngine;
using System.Collections;
using Globals;

public class PlayerAnimator : MonoBehaviour
{

    #region Public Fields & Properties

    #endregion

    #region Private Fields & Properties
    private float airVelocity;

	private Animator animator;
	private PlayerController playerController;
    #endregion

    #region Getters & Setters

    #endregion

    #region System Methods
    // Use this for initialization
    private void Start()
    {
		animator = GetComponent<Animator> ();
		playerController = GetComponent<PlayerController> ();
    }

    // Update is called once per frame
    private void Update()
    {
		animator.SetBool (AnimatorCondition.Grounded, playerController.grounded);
        animator.SetFloat(AnimatorCondition.Speed, Input.GetAxis(PlayerInput.Veritical));
        animator.SetFloat(AnimatorCondition.Direction, Input.GetAxis(PlayerInput.Horizontal));

        if(playerController.grounded)
        {
            airVelocity = 0f;
        }
        else
        {
            airVelocity -= Time.time;
        }

        animator.SetFloat(AnimatorCondition.AirVelocity, airVelocity);
    }
    #endregion

    #region Custom Methods

    #endregion
}
