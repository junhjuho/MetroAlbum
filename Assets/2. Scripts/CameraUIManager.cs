using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraUIManager : MonoBehaviour
{
    [Header("UI References")]
    public Button captureButton;
    public Button cameraModeButton;
    public Button backButton;
    public RawImage previewImage;
    public GameObject previewPanel;
    public Button closeButton;
    public Button saveButton;
    public Text statusText;
    public GameObject menu;
    public GameObject cameraMode;

    [Header("Managers")]
    public PhotoCaptureManager captureManager;
    public PhotoStorageService storageService;

    void Start()
    {
        cameraModeButton.onClick.AddListener(OpenCameraMode);
        captureButton.onClick.AddListener(TakePhoto);
        closeButton.onClick.AddListener(ClosePreview);
        backButton.onClick.AddListener(CloseCameraMode);
        saveButton.onClick.AddListener(SavePhoto);

        captureManager.OnCaptureStarted += OnCaptureStarted;
        captureManager.OnPhotoCaptured += OnPhotoCaptured;
        storageService.OnSaveComplete += OnSaveComplete;

        previewPanel.SetActive(false);
    }

    void OnDestroy()
    {
        // �̺�Ʈ ���� ���� (�޸� ���� ����)
        captureManager.OnCaptureStarted -= OnCaptureStarted;
        captureManager.OnPhotoCaptured -= OnPhotoCaptured;
        storageService.OnSaveComplete -= OnSaveComplete;
    }

    public void OpenCameraMode() // UI ��ȯ
    {
        menu.SetActive(false);
        cameraMode.SetActive(true);
    }

    public void CloseCameraMode() // UI ��ȯ
    {
        menu.SetActive(true);
        cameraMode.SetActive(false);
    }

    public void ClosePreview() // UI ��ȯ
    {
        previewPanel.SetActive(false);
        cameraMode.SetActive(true);

        captureManager.ClearCapturedPhoto();
    }

    void TakePhoto()
    {
        captureManager.CapturePhoto();
    }

    void SavePhoto()
    {
        Texture2D photo = captureManager.GetLastCapturedPhoto();
        StorageManager.Instance.UploadGetDownLink($"Photo_{System.DateTime.Now:yyyyMMdd_HHmmss}.jpg", photo.EncodeToJPG(),
          (link) => { Debug.Log("Link is : " + link); });
    }

    // ===== �̺�Ʈ �ݹ� =====
    void OnCaptureStarted()
    {
        cameraMode.SetActive(false);
        if (statusText != null) statusText.text = "�Կ� ��...";
    }

    void OnPhotoCaptured(Texture2D photo)
    {
        previewImage.texture = photo;
        previewPanel.SetActive(true);
        if (statusText != null) statusText.text = "";

    }

    void OnSaveComplete(string filePath)
    {
        if (statusText != null)
        {
            statusText.text = "���� �Ϸ�!";
            StartCoroutine(ClearStatusAfterDelay(2f));
        }
    }

    IEnumerator ClearStatusAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (statusText != null) statusText.text = "";
    }
}
