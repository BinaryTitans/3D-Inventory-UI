using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsController : MonoBehaviour {
    /* Master Volume */
    public Text masterVolumeText;
    public Slider masterVolumeSlider;

    /* Sound Effects */
    public Text soundEffectsText;

    /* Toggle Music */
    public Button toggleMusicButton;
    bool musicEnabled = true;

    /* Sprites */
    public Sprite toggleOn;
    public Sprite toggleOff;

    /* Save */
    float savedMasterVolume;
    int savedMusicToggle;

    void Start()
    {
        /* Load Master Volume */
        savedMasterVolume = PlayerPrefs.GetFloat("Master Volume");
        masterVolumeSlider.value = savedMasterVolume;

        /* Load Music Enabled */
        savedMusicToggle = PlayerPrefs.GetInt("Music Toggle");
        if (savedMusicToggle == 1)
        {
            musicEnabled = true;
            toggleMusicButton.GetComponent<Image>().overrideSprite = toggleOn;
        } else if (savedMusicToggle == 0)
        {
            musicEnabled = false;
            toggleMusicButton.GetComponent<Image>().overrideSprite = toggleOff;
        }
    }

    void Update()
    {
        /* Master Volume */
        masterVolumeText.text = "" + Mathf.Round(masterVolumeSlider.value * 100);

        /* Save */
        //Master Volume
        if (masterVolumeSlider.value != savedMasterVolume)
        {
            PlayerPrefs.SetFloat("Master Volume", masterVolumeSlider.value);
            savedMasterVolume = masterVolumeSlider.value;
        }
        
        //Music
        if (musicEnabled == true && savedMusicToggle == 0)
        {
            PlayerPrefs.SetInt("Music Toggle", 1);
            savedMusicToggle = 1;
        }
        if (musicEnabled == false && savedMusicToggle == 1)
        {
            PlayerPrefs.SetInt("Music Toggle", 0);
            savedMusicToggle = 0;
        }
    }

    /* Toggle Music Image */
    public void toggleMusicImage()
    {
        if (toggleMusicButton.GetComponent<Image>().overrideSprite == toggleOn)
        {
            toggleMusicButton.GetComponent<Image>().overrideSprite = toggleOff;
            musicEnabled = false;
        }

        else if (toggleMusicButton.GetComponent<Image>().overrideSprite == toggleOff)
        {
            toggleMusicButton.GetComponent<Image>().overrideSprite = toggleOn;
            musicEnabled = true;
        }
    }
}
