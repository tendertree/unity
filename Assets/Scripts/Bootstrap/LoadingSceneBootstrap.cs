using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using com.example;
using Cysharp.Threading.Tasks;
using Supabase.Gotrue.Exceptions;
using TMPro;
using UnityEngine;

public class SignInWithEmailPassword : MonoBehaviour
{
    // Public Unity References
    public TMP_InputField EmailInput = null!;
    public TMP_InputField PasswordInput = null!;
    public TMP_Text ErrorText = null!;
    public SupabaseManager SupabaseManager = null!;

    // Private implementation
    private bool _doSignIn;
    private bool _doSignOut;


    private async UniTaskVoid Start()
    {
        Debug.Log("login");

        await UniTask.WaitUntil(() => SupabaseManager != null && SupabaseManager.Supabase() != null);

        try
        {
            var email = "earst@arstars.com";
            var passwd = "arstrst";
            var session = await SupabaseManager.Supabase().Auth.SignIn(email, passwd);
            Debug.Log($"Signed up successfully: {session.User?.Email}");
            ErrorText.text = $"Success! Signed Up as {session.User?.Email}";
        }
        catch (GotrueException goTrueException)
        {
            ErrorText.text = $"{goTrueException.Reason} {goTrueException.Message}";
            //       Debug.LogException(goTrueException, gameObject);
        }
        catch (Exception e)
        {
            ErrorText.text = "An unexpected error occurred";
            Debug.LogException(e, gameObject);
        }

        LoadScene().Forget();
    }


    [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
    private async void Update()
    {
        // Unity does not allow async UI events, so we set a flag and use Update() to do the async work
        if (_doSignOut)
        {
            _doSignOut = false;
            await SupabaseManager.Supabase()!.Auth.SignOut();
            _doSignOut = false;
        }

        // Unity does not allow async UI events, so we set a flag and use Update() to do the async work
        if (_doSignIn)
        {
            _doSignIn = false;
            await PerformSignIn();
            _doSignIn = false;
        }
    }

    // Unity does not allow async UI events, so we set a flag and use Update() to do the async work
    public void SignIn()
    {
        _doSignIn = true;
    }

    // Unity does not allow async UI events, so we set a flag and use Update() to do the async work
    public void SignOut()
    {
        _doSignOut = true;
    }

    // This is where we do the async work and handle exceptions
    [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeInvocation")]
    private async Task PerformSignIn()
    {
        try
        {
            var email = "earst@arstars.com";
            var passwd = "arstrst";
            var session = await SupabaseManager.Supabase()!.Auth.SignUp(email, passwd)!;
            ErrorText.text = $"Success! Signed Up as {session.User?.Email}";
        }
        catch (GotrueException goTrueException)
        {
            ErrorText.text = $"{goTrueException.Reason} {goTrueException.Message}";
            Debug.Log(goTrueException.Message, gameObject);
            Debug.LogException(goTrueException, gameObject);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message, gameObject);
            Debug.Log(e, gameObject);
        }
    }

    public async UniTaskVoid LoadScene()
    {
        Debug.Log("Starting the game...");
        await SceneLoader.Instance.LoadScene("MainGame");
        Debug.Log("Game scene loaded successfully!");
    }
}