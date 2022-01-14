using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour{
    [SerializeField] GameObject bothPanel;
    [SerializeField] GameObject logOutPanel;
    [SerializeField] TMPro.TMP_InputField username;
    [SerializeField] TMPro.TMP_InputField password;
    void Update(){
        if(SaveSerial.instance.hyperGamerLoginData.loggedIn){if(!logOutPanel.activeSelf){logOutPanel.SetActive(true);bothPanel.SetActive(false);}}
        else{if(!bothPanel.activeSelf){bothPanel.SetActive(true);logOutPanel.SetActive(false);}}
    }
    public void Register(){
        if(username.text!=""&&password.text!=""){
            DBAccess.instance.RegisterHyperGamer(username.text,password.text);
            username.text="";password.text="";
            username.text=SaveSerial.instance.hyperGamerLoginData.username;password.text=SaveSerial.instance.hyperGamerLoginData.password;
        }
        else if(username.text!=""&&password.text==""){DBAccess.instance.SetLoginMessage("bruh make a password");}
        else if(username.text==""&&password.text!=""){DBAccess.instance.SetLoginMessage("bruh you cant register with just a password");}
        else{DBAccess.instance.SetLoginMessage("Empty fields");}
    }
    public void LogIn(){
        if(username.text!=""&&password.text!=""){DBAccess.instance.LoginHyperGamer(username.text,password.text);}
        else if(username.text!=""&&password.text==""){DBAccess.instance.SetLoginMessage("bruh type your password");}
        else if(username.text==""&&password.text!=""){DBAccess.instance.SetLoginMessage("bruh you cant login with just a password");}
        else{DBAccess.instance.SetLoginMessage("Empty fields");}
    }
    public void LogOut(){SaveSerial.instance.LogOut();}
}
