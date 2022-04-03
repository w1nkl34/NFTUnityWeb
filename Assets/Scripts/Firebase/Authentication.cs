using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using Firebase.Functions;
using Newtonsoft.Json;
using Firebase.Extensions;

public class Authentication : MonoBehaviour
{
    public TMP_InputField tmpEmail;
    public TMP_InputField tmpPassword;
    Button signInButton;
    UIController uIController;
    GameManager gm;
    PopUpController popUpController;

    public void Awake()
    {
        uIController = FindObjectOfType<UIController>();    
        gm = FindObjectOfType<GameManager>();
        popUpController = FindObjectOfType<PopUpController>();
    }

    public void SignIn()
    {
        gm.OpenCloseConnecttingBar(true);
        StartCoroutine(SignUser());
    }
    public void SignInWithEmailPassword(string email,string pass)
    {
        StartCoroutine(SignUserEmailPassCor(email,pass));
    }
    public IEnumerator SignUser()
    {
        var auth = FirebaseAuth.DefaultInstance;
        var signTask = auth.SignInWithEmailAndPasswordAsync(tmpEmail.text,tmpPassword.text);
        yield return new WaitUntil(predicate: () => signTask.IsCompleted);
        if(signTask.Exception != null)
        {
            Debug.Log("Failed to sign");
            popUpController.OpenInfoPop("User Is Not Valid!");
            gm.OpenCloseConnecttingBar(false);
        }
        else
        {
            Debug.Log("successfully signed user!" + signTask.Result.Email);
            Constants.authenticated = true;
            yield return StartCoroutine(getUserData());
            gm.OpenCloseConnecttingBar(false);
            uIController.ShowAuthenticationScreen(false);
        }
    }

    public IEnumerator SignUserEmailPassCor(string email,string pass)
    {
        var auth = FirebaseAuth.DefaultInstance;
        var signTask = auth.SignInWithEmailAndPasswordAsync(email,pass);
        yield return new WaitUntil(predicate: () => signTask.IsCompleted);
        if(signTask.Exception != null)
        {
            Debug.Log("Failed to sign");
            popUpController.OpenInfoPop("User Is Not Valid!");
            gm.OpenCloseConnecttingBar(false);
        }
        else
        {
            Debug.Log("successfully signed user!" + signTask.Result.Email);
            Constants.authenticated = true;
            yield return StartCoroutine(getUserData());
            gm.OpenCloseConnecttingBar(false);
            uIController.ShowAuthenticationScreen(false);
        }
    }

    public IEnumerator getUserData() {
        var functions = FirebaseFunctions.DefaultInstance;
        var function = functions.GetHttpsCallable("getUserData");
        var task = function.CallAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        if(task.Exception != null)
        {
            Debug.Log("fault");
        }
        else
        {
            gm.StartTown(task.Result.Data.ToString());
        }
    }
}
