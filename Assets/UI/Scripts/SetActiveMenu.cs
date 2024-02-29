using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum EMenuType
{
    Game,
    Audio,
    Video
}

public class SetActiveMenu : MonoBehaviour
{
    [SerializeField] private Button MenuButton;
    [SerializeField] private GameObject GameMenu;
    [SerializeField] private GameObject AudioMenu;
    [SerializeField] private GameObject VideoMenu;

    [SerializeField] private EMenuType OwnMenuType;

    // Start is called before the first frame update
    void Start()
    {
        this.MenuButton.onClick.AddListener(OpenMenu);
    }

    private void OpenMenu()
    {
        if (this.OwnMenuType == EMenuType.Game)
        {
            this.GameMenu.SetActive(true);
            this.AudioMenu.SetActive(false);
            this.VideoMenu.SetActive(false);
        }

        if (this.OwnMenuType == EMenuType.Audio)
        {
            this.GameMenu.SetActive(false);
            this.AudioMenu.SetActive(true);
            this.VideoMenu.SetActive(false);
        }

        if (this.OwnMenuType == EMenuType.Video)
        {
            this.GameMenu.SetActive(false);
            this.AudioMenu.SetActive(false);
            this.VideoMenu.SetActive(true);
        }

    }
}