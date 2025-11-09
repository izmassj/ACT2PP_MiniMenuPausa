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
    //public float fadeDuration = 0.25f;
    bool isPaused = false;

    private InputAction activatePause;

    private void Start()
    {
        activatePause = playerInputActionAsset.FindActionMap("Gameplay").FindAction("Pause");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

    public void TogglePause()
    {
        if (isPaused) StartCoroutine(Unpause()); else StartCoroutine(Pause());
    }

    IEnumerator Pause()
    {
        isPaused = true;
        animatorPause.SetTrigger("Change");

        yield return null;
        yield return new WaitUntil(() => animatorPause.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        Time.timeScale = 0f;
    }


    IEnumerator Unpause()
    {
        Time.timeScale = 1f; 
        animatorPause.SetTrigger("Change");

        yield return new WaitUntil(() => animatorPause.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        isPaused = false;
    }

}
