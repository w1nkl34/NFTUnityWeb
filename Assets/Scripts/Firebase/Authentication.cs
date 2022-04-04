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
    public TMP_InputField tmpEmailRegister;
    public TMP_InputField tmpPasswordRegister;
    UIController uIController;
    GameManager gm;
    PopUpController popUpController;
    public bool authModeRegister = false;
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

    public void Register()
    {
        gm.OpenCloseConnecttingBar(true);
        StartCoroutine(RegisterUser());     
    }
    public void SignInWithEmailPassword(string email,string pass)
    {
        StartCoroutine(SignUserEmailPassCor(email,pass));
    }

    public IEnumerator RegisterUser()
    {
        var auth = FirebaseAuth.DefaultInstance;
        var signTask = auth.CreateUserWithEmailAndPasswordAsync(tmpEmailRegister.text,tmpPasswordRegister.text);
        yield return new WaitUntil(predicate: () => signTask.IsCompleted);
        if(signTask.Exception != null)
        {
            Debug.Log("Failed to register");
            popUpController.OpenInfoPop(signTask.Exception.InnerException.InnerException.ToString());
            gm.OpenCloseConnecttingBar(false);
        }
        else
        {
            Debug.Log("successfully registered user!" + signTask.Result.Email);
            yield return StartCoroutine(GetUserData());
            gm.OpenCloseConnecttingBar(false);
            uIController.ShowAuthenticationScreen(false);
        }
    }
    

    public IEnumerator SignUser()
    {
        var auth = FirebaseAuth.DefaultInstance;
        var signTask = auth.SignInWithEmailAndPasswordAsync(tmpEmail.text,tmpPassword.text);
        yield return new WaitUntil(predicate: () => signTask.IsCompleted);
        if(signTask.Exception != null)
        {
            Debug.Log("Failed to sign");
            popUpController.OpenInfoPop(signTask.Exception.InnerException.InnerException.ToString());
            gm.OpenCloseConnecttingBar(false);
        }
        else
        {
            Debug.Log("successfully signed user!" + signTask.Result.Email);
            yield return StartCoroutine(GetUserData());
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
            yield return StartCoroutine(GetUserData());
            gm.OpenCloseConnecttingBar(false);
            uIController.ShowAuthenticationScreen(false);
        }
    }

    public void ChangeAuthMode()
    {
        authModeRegister = !authModeRegister;
        if(authModeRegister)
        {
            uIController.registerPage.SetActive(true);
            uIController.signPage.SetActive(false);
        }
        else
        {
            uIController.registerPage.SetActive(false);
            uIController.signPage.SetActive(true);           
        }
    }
    private int tries = 0;
    public IEnumerator GetUserData() {
        var functions = FirebaseFunctions.DefaultInstance;
        var function = functions.GetHttpsCallable("getUserData");
        var task = function.CallAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        if(task.Exception != null)
        {
            Debug.Log("Failed To Get User Data!");
            if(tries < 10)
            {
            yield return new WaitForSeconds(1f);
            tries++;
            yield return GetUserData();
            }
            else
            {
            yield return null;
            }

        }
        else
        {
            tries = 10;
            gm.StartTown(task.Result.Data.ToString());
            Constants.authenticated = true;
        }
    }
}
