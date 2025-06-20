using System.Collections;
using System.Collections.Generic;
using Firebase.Extensions;
using Google;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

public class LoginWithGoogle : MonoBehaviour
{
    private string GoogleAPI = "730146566611-oeii35cfcv7l57c7bkmm93gh984r5g1v.apps.googleusercontent.com";
    private GoogleSignInConfiguration configuration;

    Firebase.Auth.FirebaseAuth auth;

    private string imageUrl;
    private bool isGoogleSignInInitialized = false;
    private void Start()
    {
        InitFirebase();
    }

    void InitFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void Logout()
    {
        if (auth.CurrentUser != null)
            auth.SignOut();

        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void Login(Action<Firebase.Auth.FirebaseUser> onLogin, Action<Sprite> onImageLaoded, Action onLoginFailed)
    {
#if UNITY_EDITOR
        Debug.Log("Simulating Google login in Editor.");
        onLogin.Invoke(null);
        onImageLaoded.Invoke(null);
        return;
#endif

            if (!isGoogleSignInInitialized)
            {
                GoogleSignIn.Configuration = new GoogleSignInConfiguration
                {
                    RequestIdToken = true,
                    WebClientId = GoogleAPI,
                    RequestEmail = true
                };
                isGoogleSignInInitialized = true;
            }

            var signIn = GoogleSignIn.DefaultInstance.SignIn();

            signIn.ContinueWith(task =>
            {

                if (task.IsCanceled || task.IsFaulted)
                {
                    Debug.Log("Sign-in failed.");
                    onLoginFailed.Invoke();
                    return;
                }

                var credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
                {
                    if (authTask.IsCanceled || authTask.IsFaulted)
                    {
                        Debug.LogError(authTask.Exception.ToString());
                        foreach (var e in authTask.Exception.Flatten().InnerExceptions)
                            Debug.LogError("Auth Error: " + e.Message);
                        onLoginFailed.Invoke();
                        return;
                    }

                    var user = auth.CurrentUser;
                    onLogin.Invoke(user);
                    StartCoroutine(_loadImage(_checkImageUrl(user.PhotoUrl.ToString()), onImageLaoded));
                });
            });

    }

    private string _checkImageUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }
        return imageUrl;
    }

    private IEnumerator _loadImage(string imageUri,Action<Sprite> onImageLoaded)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUri);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            // Use the loaded texture here
            Debug.Log("Image loaded successfully");
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            onImageLoaded?.Invoke(sprite);
        }
        else
        {
            Debug.Log("Error loading image: " + www.error);
        }


    }
}