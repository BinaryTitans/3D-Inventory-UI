using UnityEngine;
using System.Collections;
using Globals;

public class PlayerTarget : MonoBehaviour
{

    #region Public Fields & Properties
    public Texture2D target;
    public Texture2D targetOver;

    public bool overEnemy;
    public bool aim;

    public LayerMask enemyLayer;
    public LayerMask otherLayer;

    public float enemyDistance = 50.0f;

    public Camera playerCam;

    public Transform playerTarget;

    public PlayerController playerController;
    public PlayerCamera playerCamera;
    #endregion

    #region Private Fields & Properties
    private bool _overEnemy;
    private bool _aim;

    private GUITexture gui;

    #endregion

    #region Getters & Setters

    #endregion

    #region System Methods
    // Use this for initialization
    private void Start()
    {
        //clears parent for playerTarget
        playerTarget.parent = null;

        //Gui layout
        gui = GetComponent<GUITexture>();
        gui.pixelInset = new Rect(-target.width * 0.5f, -target.height * 0.5f, target.width, target.height);
        gui.texture = target;
        gui.color = new Color(0.5f, 0.5f, 0.5f, 0.15f);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!playerCam.gameObject.activeSelf)
        {
            gui.color = new Color(0.5f, 0.5f, 0.5f, 0.0f);
            return;
        }

        //aim = fire2 input
        aim = Input.GetButton(PlayerInput.Fire2);

        //Creates a ray from camera to the center of the screen
        Ray ray = playerCam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));

        RaycastHit hit1;
        RaycastHit hit2;

        //Causes the ray to return information if the reticle is over an enemy
        overEnemy = Physics.Raycast(ray.origin, ray.direction, out hit1, enemyDistance, enemyLayer);

        if (overEnemy)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit2, enemyDistance, otherLayer))
            {
                overEnemy = hit1.distance < hit2.distance;
            }
        }

        float delta = 1.0f - ((playerCamera.Y + 85) * 0.0058823529f);

        //changes where reticle sits in aim vs not aimed
        if (playerController.aim)
        {
            playerTarget.position = playerCam.ScreenToWorldPoint(new Vector3(Screen.width * 0.7f, Screen.height * (0.3f + (delta * 0.24f)), 10f));
        }
        else
        {
            playerTarget.position = playerCam.ScreenToWorldPoint(new Vector3(Screen.width * 0.7f, Screen.height * (0.4f + (delta * 0.16f)), 10f));
        }

        //changes the gui to show that the player is aimed at the target or is not aimed at target
        if(overEnemy != _overEnemy)
        {
            _overEnemy = overEnemy;

            if (overEnemy)
            {
                gui.texture = targetOver;
            }
            else
            {
                gui.texture = target;
            }
        }

        //changes the color of the retical if aim (r,g,b,a)
        if (aim != _aim)
        {
            _aim = aim;

            if (aim)
            {
                gui.color = new Color(0.5f, 0.5f, 0.5f, 0.75f); 
            }
            else
            {
                gui.color = new Color(0.5f, 0.5f, 0.5f, 0.15f);
            }
        }

    }
    #endregion

    #region Custom Methods

    #endregion
}
