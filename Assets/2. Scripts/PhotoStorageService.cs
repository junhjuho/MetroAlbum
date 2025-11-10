using System.Collections;
using System.IO;
using UnityEngine;

public class PhotoStorageService : MonoBehaviour
{
    public event System.Action<string> OnSaveComplete;

    public void SavePhotoLocally(Texture2D photo)
    {
        if (photo == null)
        {
            Debug.LogWarning("저장할 사진이 없습니다.");
            return;
        }

        byte[] bytes = photo.EncodeToPNG();
        string fileName = $"ARPhoto_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        File.WriteAllBytes(filePath, bytes);

        Debug.Log($"사진 저장됨: {filePath}");

        // 갤러리 저장 (NativeGallery 플러그인 사용 시)
        // NativeGallery.SaveImageToGallery(bytes, "MyARApp", fileName);

        OnSaveComplete?.Invoke(filePath);
    }

    // 나중에 Firebase 업로드 함수 추가 예정
    // public async void UploadToFirebase(Texture2D photo) { }
}
