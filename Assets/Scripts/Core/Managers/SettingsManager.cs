using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Button _muteMusicButton;
    [SerializeField] Button _muteEffectsButton;
    [SerializeField] AudioClip _applayClip;

    [SerializeField] Sprite _OnMuteSprite;
    [SerializeField] Sprite _NonMuteSprite;

    private ApplicationData _appData;

    private void Start()
    {
        _appData = ApplicationData.Instance;

        _muteMusicButton.onClick.AddListener(ToggleMusicMute);
        _muteEffectsButton.onClick.AddListener(ToggleEffectsMute);
        SwitchMuteSprite();
    }

    private void ToggleMusicMute()
    {
        AudioManager.Instance.PlayOneShotSound(_applayClip);
        _appData.IsMusicMute = !_appData.IsMusicMute;
        AudioManager.Instance.MuteMusic(_appData.IsMusicMute);
        SwitchMuteSprite();
    }

    private void ToggleEffectsMute()
    {
        AudioManager.Instance.PlayOneShotSound(_applayClip);
        _appData.IsEffectsMute = !_appData.IsEffectsMute;
        SwitchMuteSprite();
    }

    private void SwitchMuteSprite()
    {
        _muteMusicButton.GetComponent<Image>().sprite = _appData.IsMusicMute ? _OnMuteSprite : _NonMuteSprite;
        _muteEffectsButton.GetComponent<Image>().sprite = _appData.IsEffectsMute ? _OnMuteSprite : _NonMuteSprite;
    }
}

