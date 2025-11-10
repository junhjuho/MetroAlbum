using ARLocation.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestFirebase : MonoBehaviour
{

    void Start()
    {
        Texture2D texture2D = CreateTestTexture();
        StorageManager.Instance.UploadGetDownLink("test1107.jpg", texture2D.EncodeToJPG(),
            (link) => { Debug.Log("Link is : " + link); });
    }
    Texture2D CreateTestTexture()
    {
        // 256x256 빨간색 이미지 생성
        Texture2D tex = new Texture2D(256, 256);
        Color[] colors = new Color[256 * 256];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.red;
        }

        tex.SetPixels(colors);
        tex.Apply();

        return tex;
    }
}
