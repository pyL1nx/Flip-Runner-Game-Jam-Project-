using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool useWorldSpace = true;

    [Header("Lane clamp")]
    [SerializeField] private bool clampX = true;
    [SerializeField] private float minX = -2.5f;
    [SerializeField] private float maxX =  2.5f;

    [Header("Flip indicator")]
    [SerializeField] private GameObject flipIndicator; // assign UI element
    [SerializeField] private float indicatorDuration = 2f;

    [Header("Flip sound")]
    [SerializeField] private AudioClip flipSound; // assign flip sound clip
    private AudioSource audioSource;

    [Header("TMPro texts")]
    [SerializeField] private TextMeshProUGUI aKeyText;
    [SerializeField] private TextMeshProUGUI dKeyText;
    [SerializeField] private TextMeshProUGUI wKeyText;
    [SerializeField] private TextMeshProUGUI sKeyText;

    private Animator anim;
    private int runHash;

    private enum ControlMapping
    {
        LeftARightD = 1,    // 1 -> a = left d = right
        LeftDRightA,        // 2 -> a = right d = left
        LeftWRightS,        // 3 -> w = left s = right
        LeftSRightW,        // 4 -> s = left w = right
        LeftWLeftA,         // 5 -> w = left a = right
        LeftDLeftS          // 6 -> d = left s = right
    }

    private ControlMapping currentMapping = ControlMapping.LeftARightD;

    private Coroutine flipLoop;
    private Coroutine indicatorRoutine;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        runHash = Animator.StringToHash("Run");

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        anim.Play(runHash, 0, 0f);

        if (flipLoop != null) StopCoroutine(flipLoop);
        flipLoop = StartCoroutine(FlipControlsLoop());
    }

    private void OnDisable()
    {
        if (flipLoop != null) StopCoroutine(flipLoop);
        if (indicatorRoutine != null) StopCoroutine(indicatorRoutine);

        currentMapping = ControlMapping.LeftARightD;

        if (flipIndicator) flipIndicator.SetActive(false);
        ClearKeyTexts();
    }

    private void ClearKeyTexts()
    {
        if (aKeyText) aKeyText.text = "A: -";
        if (dKeyText) dKeyText.text = "D: -";
        if (wKeyText) wKeyText.text = "W: -";
        if (sKeyText) sKeyText.text = "S: -";
    }

    private void Update()
    {
        float horiz = 0f;
        UpdateKeyTexts();

        bool aDown = Input.GetKey(KeyCode.A);
        bool dDown = Input.GetKey(KeyCode.D);
        bool wDown = Input.GetKey(KeyCode.W);
        bool sDown = Input.GetKey(KeyCode.S);

        switch (currentMapping)
        {
            case ControlMapping.LeftARightD:
                if (aDown) horiz = -1f;
                else if (dDown) horiz = 1f;
                break;

            case ControlMapping.LeftDRightA:
                if (aDown) horiz = 1f;
                else if (dDown) horiz = -1f;
                break;

            case ControlMapping.LeftWRightS:
                if (wDown) horiz = -1f;
                else if (sDown) horiz = 1f;
                break;

            case ControlMapping.LeftSRightW:
                if (sDown) horiz = -1f;
                else if (wDown) horiz = 1f;
                break;

            case ControlMapping.LeftWLeftA:
                if (wDown) horiz = -1f;
                else if (aDown) horiz = 1f;
                break;

            case ControlMapping.LeftDLeftS:
                if (dDown) horiz = -1f;
                else if (sDown) horiz = 1f;
                break;
        }

        Vector3 delta = new Vector3(horiz, 0f, 0f) * moveSpeed * Time.deltaTime;

        if (useWorldSpace)
            transform.position += delta;
        else
            transform.Translate(delta, Space.Self);

        if (clampX)
        {
            Vector3 p = transform.position;
            p.x = Mathf.Clamp(p.x, minX, maxX);
            transform.position = p;
        }
    }

    private void UpdateKeyTexts()
    {
        if (!aKeyText || !dKeyText || !wKeyText || !sKeyText) return;

        switch (currentMapping)
        {
            case ControlMapping.LeftARightD:
                aKeyText.text = "A = Left";
                dKeyText.text = "D = Right";
                wKeyText.text = "W = ";
                sKeyText.text = "S = ";
                break;
            case ControlMapping.LeftDRightA:
                aKeyText.text = "A = Right";
                dKeyText.text = "D = Left";
                wKeyText.text = "W = ";
                sKeyText.text = "S = ";
                break;
            case ControlMapping.LeftWRightS:
                aKeyText.text = "A = ";
                dKeyText.text = "D = ";
                wKeyText.text = "W = Left";
                sKeyText.text = "S = Right";
                break;
            case ControlMapping.LeftSRightW:
                aKeyText.text = "A = ";
                dKeyText.text = "D = ";
                wKeyText.text = "W = Right";
                sKeyText.text = "S = Left";
                break;
            case ControlMapping.LeftWLeftA:
                aKeyText.text = "A = Right";
                dKeyText.text = "D = ";
                wKeyText.text = "W = Left";
                sKeyText.text = "S = ";
                break;
            case ControlMapping.LeftDLeftS:
                aKeyText.text = "A = ";
                dKeyText.text = "D = Left";
                wKeyText.text = "W = ";
                sKeyText.text = "S = Right";
                break;
        }
    }

    private System.Collections.IEnumerator FlipControlsLoop()
    {
        while (true)
        {
            // Randomly choose a mapping from 1 to 6
            int randValue = Random.Range(1, 7);
            currentMapping = (ControlMapping)randValue;

            ShowFlipIndicator();
            PlayFlipSound();

            yield return new WaitForSeconds(indicatorDuration);

            if (flipIndicator) flipIndicator.SetActive(false);

            yield return new WaitForSeconds(Random.Range(5f, 10f));
        }
    }

    private void PlayFlipSound()
    {
        if (flipSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(flipSound);
        }
    }

    private void ShowFlipIndicator()
    {
        if (!flipIndicator) return;
        if (indicatorRoutine != null) StopCoroutine(indicatorRoutine);
        indicatorRoutine = StartCoroutine(ShowIndicatorRoutine());
    }

    private System.Collections.IEnumerator ShowIndicatorRoutine()
    {
        flipIndicator.SetActive(true);
        yield return new WaitForSeconds(indicatorDuration);
        flipIndicator.SetActive(false);
        indicatorRoutine = null;
    }
}
