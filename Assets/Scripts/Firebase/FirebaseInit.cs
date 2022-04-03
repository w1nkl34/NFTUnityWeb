using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;

public class FirebaseInit : MonoBehaviour
{

    public UIController uIController;
    public GameManager gm;
    public Authentication auth;


    void Start()
    {
        StartCoroutine(InitCor());
        
    }

    IEnumerator InitCor()
    {
        var task = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(predicate: () => task.IsCompleted);
        if(task.Exception != null)
        {
            Debug.Log("Firebase Init Error!");
        }
        else
        {
            UserIsAnonymousConnect();
        }
    }

    public void UserIsNotAnonymousConnect()
    {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            uIController.ShowAuthenticationScreen(true);
            auth.SignInWithEmailPassword("a@a.com","aaaaaa");
    }

    public void UserIsAnonymousConnect()
    {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
            gm.OpenCloseConnecttingBar(false);
            uIController.ShowAuthenticationScreen(true);
    }
}
