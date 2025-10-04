using UnityEngine;
using UnityEngine.EventSystems;

public class MenuPanelSwitcher : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;

    [Header("Optional selection")]
    [SerializeField] private GameObject firstSelectOnOptions;
    [SerializeField] private GameObject firstSelectOnMenu;

    private void OnEnable()
    {
        // Ensure something is selected when menu shows (keyboard/gamepad)
        SetFirstSelected(firstSelectOnMenu);
    }

    // Hook this to Options button OnClick
    public void OpenOptions()
    {
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (optionsPanel)  optionsPanel.SetActive(true);
        SetFirstSelected(firstSelectOnOptions);
    }

    // Hook this to Back button OnClick inside Options
    public void BackToMenu()
    {
        if (optionsPanel)  optionsPanel.SetActive(false);
        if (mainMenuPanel) mainMenuPanel.SetActive(true);
        SetFirstSelected(firstSelectOnMenu);
    }

    private void SetFirstSelected(GameObject go)
    {
        if (!go) return;
        var es = EventSystem.current;
        if (!es) return;
        es.SetSelectedGameObject(null);
        es.SetSelectedGameObject(go);
    }
}
