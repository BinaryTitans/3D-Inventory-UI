using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsController : MonoBehaviour {
    /* Master Volume */
    public Text masterVolumeText;
    public Slider masterVolumeSlider;

    /* Sound Effects */
    public Text soundEffectsText;
    public Slider soundEffectsSlider;

    /* Test Toggle */
    public Button testToggleButton;
    public Sprite toggleOn;
    public Sprite toggleOff;
    [HideInInspector] public bool testToggleBool = true;

    void Update()
    {
        /* Master Volume */
        masterVolumeText.text = "" + Mathf.Round(masterVolumeSlider.value * 100);

        /* Sound Effects */
        soundEffectsText.text = "" + Mathf.Round(soundEffectsSlider.value * 100);
    }

    /* Test Toggle Image */
    public void testToggleImage()
    {
        if (testToggleButton.GetComponent<Image>().overrideSprite == toggleOn)
        {
            testToggleButton.GetComponent<Image>().overrideSprite = toggleOff;
            testToggleBool = false;
        }

        else if (testToggleButton.GetComponent<Image>().overrideSprite == toggleOff)
        {
            testToggleButton.GetComponent<Image>().overrideSprite = toggleOn;
            testToggleBool = true;
        }
    }
}
