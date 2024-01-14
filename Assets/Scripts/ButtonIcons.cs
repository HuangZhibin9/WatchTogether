using UnityEngine;
using UnityEngine.UI;

public class ButtonIcons : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;

    //[Button]
    //public void SavePng()
    //{
    //    Texture2D _playIcon;
    //    Texture2D _pauseIcon;
    //    _playIcon = SdfIcons.CreateTransparentIconTexture(SdfIconType.PlayFill, Color.white, 120, 120, 0);
    //    _pauseIcon = SdfIcons.CreateTransparentIconTexture(SdfIconType.PauseFill, Color.white, 120, 120, 0);
    //    byte[] playIconData = _playIcon.EncodeToPNG();
    //    byte[] pauseIconData = _pauseIcon.EncodeToPNG();
    //    File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "playIconData.png"), playIconData);
    //    File.WriteAllBytes(Path.Combine(Application.persistentDataPath, "pauseIconData.png"), pauseIconData);
    //    Debug.Log(Application.persistentDataPath);
    //}
}