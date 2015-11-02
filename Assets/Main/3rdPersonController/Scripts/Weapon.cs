using UnityEngine;
using System.Collections;
using Globals;

public class Weapon : MonoBehaviour {

    #region Public Fields & Properties
    public float fireDelay;

    public Transform gunRig;
    public Transform muzzle;

    public GameObject bulletHole;
    public GameObject muzzleFlash;

    public Camera cam;
    #endregion

    #region Private Fields & Properties
    private float fireCounter;

    private PlayerController playerController;

    private Ray ray;
    #endregion

    #region Getters & Setters

    #endregion

    #region System Methods
    // Use this for initialization
    private void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        ray = cam.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f));

        if(playerController.aim)
        {
            gunRig.forward = ray.direction;
        }
        else
        {
            gunRig.forward = transform.forward;
        }

        if(Input.GetButton(PlayerInput.Fire1) && fireCounter > fireDelay)
        {
            muzzle.GetComponent<AudioSource>().Play();
            fireCounter = 0f;

            RaycastHit hit;
            if(playerController.aim)
            {
                if(Physics.Raycast(ray, out hit, 100f))
                {
                    Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                }
            }
            else
            {
                if(Physics.Raycast(muzzle.position, muzzle.forward, out hit, 100f))
                {
                    Instantiate(bulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                }
            }

            StartCoroutine(MuzzleFlash());
        }

        fireCounter += Time.deltaTime;
    }
    #endregion

    #region Custom Methods
    private IEnumerator MuzzleFlash()
    {
        //16 minutes vid 7 if this rotation doesnt work
        GameObject go = (GameObject)(Instantiate(muzzleFlash, muzzle.position, muzzle.rotation));
        go.transform.parent = muzzle;
        yield return new WaitForSeconds(go.GetComponent<ParticleSystem>().duration + 0.05f);
        Destroy(go);
    }
    #endregion
}
