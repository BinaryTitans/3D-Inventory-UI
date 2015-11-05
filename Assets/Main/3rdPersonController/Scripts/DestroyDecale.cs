using UnityEngine;
using System.Collections;

public class DestroyDecale : MonoBehaviour
{

    #region Public Fields & Properties
    public float time = 5f;
    #endregion

    #region Private Fields & Properties
    private float alpha = 1f;
    #endregion

    #region System Methods
    // Update is called once per frame
    private void Update()
    {
        time -= Time.deltaTime;
        if(time < 0f)
        {
            alpha -= (Time.deltaTime / 3f);
            Color textureColor = GetComponent<Renderer>().material.color;
            textureColor.a = alpha;
            GetComponent<Renderer>().material.color = textureColor;

            if(alpha < 0f)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
    #endregion
}
