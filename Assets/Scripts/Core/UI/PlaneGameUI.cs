using UnityEngine;
using UnityEngine.UI;

public class PlaneGameUI : MonoBehaviour
{
    [SerializeField] Button _pauseButton;
    [SerializeField] Button _mainMenuButton;

    private void Start()
    {
        _mainMenuButton.onClick.AddListener(() => { OnMainMenu(); });
    }

    private void OnMainMenu()
    {
        SceneLoader.Instance.LoadScene(SceneLoader.Scene.MainScene);
    }
}
