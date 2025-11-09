using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionAsset playerInputActionAsset;

    [Header("UI Animations")]
    public Animator animatorPause;
    public CanvasGroup pauseCanvasGroup; // asignar canvas principal del menú
    public GameObject pauseUIGroup;

    bool isPaused = false;

    private InputAction activatePause;

    private void Awake()
    {
        playerInputActionAsset.Enable();
        activatePause = playerInputActionAsset.FindActionMap("Gameplay").FindAction("Pause");
    }

    private void Start()
    {
        pauseUIGroup.SetActive(false);
    }

    void Update()
    {
        if (activatePause.triggered) TogglePause();
    }

    public void TogglePause()
    {
        if (isPaused) 
            StartCoroutine(Unpause()); 
        else 
            StartCoroutine(Pause());
    }

    IEnumerator Pause()
    {
        pauseUIGroup.SetActive(true);
        isPaused = true;
        animatorPause.SetTrigger("Change");

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 0f;
    }


    IEnumerator Unpause()
    {
        Time.timeScale = 1f; 
        animatorPause.SetTrigger("Change");

        yield return new WaitForSecondsRealtime(1f);

        isPaused = false;
        pauseUIGroup.SetActive(false);
    }

}
