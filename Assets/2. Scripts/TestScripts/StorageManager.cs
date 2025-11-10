using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Extensions;
using Firebase.Storage;
using Firebase.Firestore;
using System.Threading.Tasks;
using System;
using Unity.VisualScripting;

public class StorageManager : SingletonBehaviour<StorageManager>
{
    public FirebaseStorage storage;
    public StorageReference reference;
    public Text text;

    private bool isFirebaseInitialized = false;

    FirebaseFirestore db;

    void Start() 
    {
        text.text = "Initializing Firebase...";
        Debug.Log("Starting Firebase initialization");

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;

            text.text = $"Dependency Status: {dependencyStatus}";
            Debug.Log($"Firebase Dependency Status: {dependencyStatus}");

            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError($"Could not resolve Firebase dependencies: {dependencyStatus}");
                text.text = $"Firebase Error: {dependencyStatus}";
            }
        });
    }
    void InitializeFirebase()
    {
        try // google-services.json 
        {
            var options = new Firebase.AppOptions
            {
                ApiKey = "AIzaSyDJ9gyLYAMW-9cB-5UJIkRQ9bZ21Yom48E",           // google-services.json�� current_key
                AppId = "1:617759240959:android:a8532c1a9c4f55b77052ca",             // google-services.json�� mobilesdk_app_id
                ProjectId = "metrofunar-home",     // google-services.json�� project_id
                StorageBucket = "metrofunar-home.firebasestorage.app"  // storage_bucket
            };

            var app = FirebaseApp.Create(options);

            storage = FirebaseStorage.DefaultInstance;
            reference = storage.GetReferenceFromUrl("gs://metrofunar-home.firebasestorage.app");

            isFirebaseInitialized = true;

            Debug.Log("INIT Firebase Success");
            text.text = "INIT Firebase Success";
        }
        catch (Exception e)
        {
            Debug.LogError($"Firebase initialization failed: {e.Message}");
            text.text = $"Firebase Error: {e.Message}";
        }

        db = FirebaseFirestore.DefaultInstance;
    }


    public void UploadGetDownLink(string name, byte[] data, Action<string> SendLink = null)
    {
        if (!isFirebaseInitialized)
        {
            Debug.LogError("Firebase not initialized yet!");
            text.text = "Firebase not ready";
            return;
        }

        StorageReference fileRef = reference.Child(name);
        fileRef.PutBytesAsync(data).ContinueWithOnMainThread((Task<StorageMetadata> task) =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception.ToString());
            }
            else
            {
                StorageMetadata metadata = task.Result;
                Debug.Log("Finished uploading");
                text.text = $"Finished uploading";

                fileRef.GetDownloadUrlAsync().ContinueWithOnMainThread((Task<Uri> uriTask) =>
                {
                    string uri = uriTask.Result.ToString();
                    SendLink?.Invoke(uri);
                });
            }
        });
    }

    public void DownloadImage(string fileName, Action<Texture2D> OnDownloadComplete = null)
    {
        if (!isFirebaseInitialized) return;

        StorageReference fileRef = reference.Child(fileName);
        text.text = "Downloading...";

        const long maxAllowedSize = 10 * 1024 * 1024; // 10MB

        fileRef.GetBytesAsync(maxAllowedSize).ContinueWithOnMainThread((Task<byte[]> task) =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError($"Download failed: {task.Exception}");
                text.text = "Download failed";
            }
            else
            {
                byte[] imageData = task.Result;
                text.text = "Download complete";

                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(imageData);

                OnDownloadComplete?.Invoke(texture);
            }
        });
    }

    public void UploadImageWithMetadata(string fileName, byte[] imageData, Action<bool> callback = null)
    {
        if (!isFirebaseInitialized)
        {
            Debug.LogError("Firebase not initialized!");
            callback?.Invoke(false);
            return;
        }

        StorageReference fileRef = reference.Child(fileName);

        // 1. Storage에 이미지 업로드
        fileRef.PutBytesAsync(imageData).ContinueWithOnMainThread(uploadTask =>
        {
            if (uploadTask.IsFaulted || uploadTask.IsCanceled)
            {
                Debug.LogError("Upload failed");
                callback?.Invoke(false);
                return;
            }

            // 2. 다운로드 URL 가져오기
            fileRef.GetDownloadUrlAsync().ContinueWithOnMainThread(urlTask =>
            {
                if (urlTask.IsFaulted || urlTask.IsCanceled)
                {
                    callback?.Invoke(false);
                    return;
                }

                string downloadUrl = urlTask.Result.ToString();
                Debug.Log($"Download URL: {downloadUrl}");

                // 3. Firestore에 메타데이터 저장
                var postData = new Dictionary<string, object>
                {
                    { "fileName", fileName },
                    { "imageUrl", downloadUrl },
                    { "userName", "User" + UnityEngine.Random.Range(1000, 9999) },
                    { "likes", 0 },
                    { "createdAt", Timestamp.GetCurrentTimestamp() }
                };

                db.Collection("posts").AddAsync(postData).ContinueWithOnMainThread(firestoreTask =>
                {
                    if (firestoreTask.IsCompleted && !firestoreTask.IsFaulted)
                    {
                        Debug.Log("Firestore 저장 완료!");
                        text.text = "Upload complete!";
                        callback?.Invoke(true);
                    }
                    else
                    {
                        Debug.LogError($"Firestore 저장 실패: {firestoreTask.Exception}");
                        callback?.Invoke(false);
                    }
                });
            });
        });
    }
}
