using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCaptureManager : MonoBehaviour
{
    public event System.Action<Texture2D> OnPhotoCaptured;
    public event System.Action OnCaptureStarted;

    private Texture2D capturedPhoto;

    public void CapturePhoto()
    {
        StartCoroutine(CapturePhotoCoroutine());
    }

    IEnumerator CapturePhotoCoroutine()
    {
        OnCaptureStarted?.Invoke();

        yield return new WaitForEndOfFrame();

        // È­¸é Ä¸Ã³
        capturedPhoto = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        capturedPhoto.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        capturedPhoto.Apply();

        OnPhotoCaptured?.Invoke(capturedPhoto);

        Debug.Log("»çÁø Ä¸Ã³ ¿Ï·á!");
    }

    public Texture2D GetLastCapturedPhoto()
    {
        return capturedPhoto;
    }

    public void ClearCapturedPhoto()
    {
        if (capturedPhoto != null)
        {
            Destroy(capturedPhoto);
            capturedPhoto = null;
        }
    }
}
