using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ValueManager : MonoBehaviour {
    public Text masterVolumeText;
    public Slider masterVolumeSlider;

    void Update()
    {
        masterVolumeText.text = "" + Mathf.Round(masterVolumeSlider.value * 100);
    }
}
