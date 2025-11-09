using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource; // loop de fondo
    public AudioSource sfxSource;   // sonido de botón (usa PlayOneShot o clip)

    [Header("UI Sound")]
    public Slider musicSlider;
    public Slider sfxSlider;

    const string MUSIC_KEY = "musicVolume";
    const string SFX_KEY = "sfxVolume";

    void Start()
    {
        float mv = PlayerPrefs.HasKey(MUSIC_KEY) ? PlayerPrefs.GetFloat(MUSIC_KEY) : 1f;
        float sv = PlayerPrefs.HasKey(SFX_KEY) ? PlayerPrefs.GetFloat(SFX_KEY) : 1f;
        musicSlider.value = mv; sfxSlider.value = sv;
        ApplyMusic(mv); ApplySfx(sv);
        musicSlider.onValueChanged.AddListener(ApplyMusic);
        sfxSlider.onValueChanged.AddListener(ApplySfx);
    }

    public void ApplyMusic(float v)
    {
        musicSource.volume = v;
        PlayerPrefs.SetFloat(MUSIC_KEY, v);
    }
    public void ApplySfx(float v)
    {
        sfxSource.volume = v;
        PlayerPrefs.SetFloat(SFX_KEY, v);
    }

    public void PlayButtonSound(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
