using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour{
    [SerializeField] GameObject bothPanel;
    [SerializeField] GameObject logOutPanel;
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;
    [SerializeField] bool showPassword=false;
    [SerializeField] public static readonly List<string> developerNicknames=new List<string>(){"Hyper","Hyper!","HyperGamesDev","HyperLemon","HyperLemon","HaipaRemon","Hyperek","Hajper","Hajperek","HyperLemonStudios"};
    [SerializeField] public static readonly string developerNicknameColor="#c74b3e";
    void Update(){
        if(SaveSerial.instance.hyperGamerLoginData.loggedIn){if(!logOutPanel.activeSelf){logOutPanel.SetActive(true);bothPanel.SetActive(false);}}
        else{if(!bothPanel.activeSelf){bothPanel.SetActive(true);logOutPanel.SetActive(false);}}
        if(showPassword&&password.contentType!=TMP_InputField.ContentType.Standard){password.contentType=TMP_InputField.ContentType.Standard;password.ForceLabelUpdate();}
        else if(!showPassword&&password.contentType!=TMP_InputField.ContentType.Password){password.contentType=TMP_InputField.ContentType.Password;password.ForceLabelUpdate();}
    }
    public void Register(){
        if(username.text!=""&&password.text!=""){
            if(!developerNicknames.Contains(username.text)||(developerNicknames.Contains(username.text)&&password.text==gitignoreScript.developerPasswordHyperLogin)){
                DBAccess.instance.RegisterHyperGamer(username.text,password.text);
                username.text="";password.text="";
                username.text=SaveSerial.instance.hyperGamerLoginData.username;password.text=SaveSerial.instance.hyperGamerLoginData.password;
            }else{DBAccess.instance.SetLoginMessage("Um thats taken by the developer!");}
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
    //public void ToggleShowPassword(){showPassword=!showPassword;return;}
    public void ToggleShowPassword(bool isOn){showPassword=isOn;}
}
