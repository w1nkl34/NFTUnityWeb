using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;

public class FirebaseInit : MonoBehaviour
{

    public UIController uIController;
    public GameManager gm;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => 
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
        gm.OpenCloseConnecttingBar(false);
        uIController.ShowAuthenticationScreen(true);
    }

}
