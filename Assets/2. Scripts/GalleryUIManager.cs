using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryUIManager : MonoBehaviour
{
    [Header("UI References")]
    public Button galleryButton;
    public Button backButton;
    public GameObject galleryView;
    public GameObject menu;

    [Header("Image Display")]
    public Image displayImage;

     [Header("WebView Settings")]
    public string webViewURL = "https://github.com/junhjuho/MetroAlbum";
    
    private WebViewObject webView;  // ✅ WebView 변수 선언

    void Start()
    {
        galleryButton.onClick.AddListener(OpenGalleryView);
        backButton.onClick.AddListener(CloseGalleryView);
    }

    public void OpenGalleryView()
    {
        galleryView.SetActive(true);
        menu.SetActive(false);

        InitializeWebView();

        // // Firebase Storage에서 모든 이미지 파일 목록을 가져옵니다
        // StorageManager.Instance.ListAllFiles((files) =>
        // {
        //     if (files != null && files.Count > 0)
        //     {
        //         // 첫 번째 이미지를 다운로드하여 표시합니다
        //         foreach (string file in files)
        //         {
        //             if (file.ToLower().EndsWith(".jpg") || file.ToLower().EndsWith(".jpeg") || file.ToLower().EndsWith(".png"))
        //             {
        //                 StorageManager.Instance.DownloadImage(file, (texture) =>
        //                 {
        //                     if (texture != null)
        //                     {
        //                         Sprite sprite = Sprite.Create(
        //                             texture,
        //                             new Rect(0, 0, texture.width, texture.height),
        //                             new Vector2(0.5f, 0.5f)
        //                         );
        //                         displayImage.sprite = sprite;
        //                     }
        //                 });
        //                 break; // 첫 번째 이미지만 표시
        //             }
        //         }
        //     }
        // });
    }
    void InitializeWebView()
    {
        // 이미 WebView가 있으면 재사용
        if (webView != null)
        {
            webView.SetVisibility(true);
            return;
        }

        // WebView 생성
        webView = (new GameObject("WebViewObject")).AddComponent<WebViewObject>();
        
        // WebView 초기화
        webView.Init(
            cb: (msg) => Debug.Log($"WebView 메시지: {msg}"),
            err: (msg) => Debug.LogError($"WebView 에러: {msg}"),
            started: (msg) => Debug.Log("WebView 로드 시작"),
            ld: (msg) => Debug.Log("WebView 로드 완료")
        );

        // 여백 설정 (left, top, right, bottom)
        webView.SetMargins(0, 150, 0, 150);
        
        // 표시
        webView.SetVisibility(true);
        
        // URL 로드
        webView.LoadURL(webViewURL);
        
        Debug.Log($"WebView 초기화 완료: {webViewURL}");
    }
    public void CloseGalleryView()
    {
        galleryView.SetActive(false);
        menu.SetActive(true);

        if (webView != null)
        {
            webView.SetVisibility(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        if (webView != null)
        {
            Destroy(webView.gameObject);
        }
    }
}
