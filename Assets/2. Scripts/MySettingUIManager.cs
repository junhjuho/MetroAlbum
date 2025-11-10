using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MySettingUIManager : MonoBehaviour
{
    [Header("UI References")]
    public Button settingButton;
    public Button backButton;
    public GameObject setting;
    public GameObject menu;

    // Start is called before the first frame update
    void Start()
    {
        settingButton.onClick.AddListener(OpenSetting);
        backButton.onClick.AddListener(CloseSetting);
    }

    public void OpenSetting()
    {
        setting.SetActive(true);
        menu.SetActive(false);
    }

    public void CloseSetting()
    {
        setting.SetActive(false);
        menu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
