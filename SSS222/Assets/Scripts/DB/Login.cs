using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour{
    [SerializeField] GameObject bothPanel;
    [SerializeField] GameObject logInPanel;
    [SerializeField] GameObject registerPanel;
    [SerializeField] GameObject logOutPanel;
    [SerializeField] TMPro.TMP_InputField registerUsername;
    [SerializeField] TMPro.TMP_InputField registerPassword;
    [SerializeField] TMPro.TMP_InputField loginUsername;
    [SerializeField] TMPro.TMP_InputField loginPassword;
    void Update(){
        if(SaveSerial.instance.hyperGamerLoginData.loggedIn){if(!logOutPanel.activeSelf){logOutPanel.SetActive(true);bothPanel.SetActive(false);}}
        else{if(!bothPanel.activeSelf){bothPanel.SetActive(true);logOutPanel.SetActive(false);}}
    }
    public void Register(){
        if(registerUsername.text!=""&&registerPassword.text!=""){
            DBAccess.instance.RegisterHyperGamer(registerUsername.text,registerPassword.text);
            registerUsername.text="";registerPassword.text="";
            loginUsername.text=SaveSerial.instance.hyperGamerLoginData.username;loginPassword.text=SaveSerial.instance.hyperGamerLoginData.password;
        }
        else if(registerUsername.text!=""&&registerPassword.text==""){DBAccess.instance.SetLoginMessage("bruh make a password");}
        else if(registerUsername.text==""&&registerPassword.text!=""){DBAccess.instance.SetLoginMessage("bruh you cant register with just a password");}
        else{DBAccess.instance.SetRegisterMessage("You cant register an ampty account");}
    }
    public void LogIn(){
        if(loginUsername.text!=""&&loginPassword.text!=""){DBAccess.instance.LoginHyperGamer(loginUsername.text,loginPassword.text);}
        else if(loginUsername.text!=""&&loginPassword.text==""){DBAccess.instance.SetLoginMessage("bruh type your password");}
        else if(loginUsername.text==""&&loginPassword.text!=""){DBAccess.instance.SetLoginMessage("bruh you cant login with just a password");}
        else if(loginUsername.text==""&&loginPassword.text==""){DBAccess.instance.SetLoginMessage("You cant login to an empty account");}
    }
    public void LogOut(){SaveSerial.instance.LogOut();}
    public void LoginPanel(){logInPanel.SetActive(true);registerPanel.SetActive(false);}
    public void RegisterPanel(){logInPanel.SetActive(false);registerPanel.SetActive(true);}
}
