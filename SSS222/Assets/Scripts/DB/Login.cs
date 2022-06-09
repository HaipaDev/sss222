using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class Login : MonoBehaviour{
    [Header("Panels & Fields")]
    [SerializeField] GameObject bothPanel;
    [SerializeField] GameObject logOutPanel;
    [SerializeField] TMP_InputField username;
    [SerializeField] TMP_InputField password;
    [SerializeField] TMP_InputField passwordLoggedIn;
    [SerializeField] TMP_InputField passwordNew;
    [SerializeField] Button changePassButton;
    [SerializeField] Button deleteButton;
    [Header("Variables")]
    [SerializeField] bool showPassword=false;
    [SerializeField] bool rememberPassword=true;
    [DisableInEditorMode][SerializeField] bool changePassConfirm=false;
    [DisableInEditorMode][SerializeField] bool deleteConfirm=false;
    [SerializeField] public static readonly List<string> developerNicknames=new List<string>(){"Hyper","Hyper!","HyperGamesDev","HyperLemon","HyperLemon","HaipaRemon","Hyperek","Hajper","Hajperek","HyperLemonStudios"};
    [SerializeField] public static readonly string developerNicknameColor="#c74b3e";
    //public static bool _isDeveloperNick(string str){return str.IndexOf("Hyper", StringComparison.OrdinalIgnoreCase) >= 0;}
    //public static bool _isDeveloperNick(string str){foreach(string dn in developerNicknames){return str.IndexOf(dn, StringComparison.OrdinalIgnoreCase) >= 0;}return false;}
    public static bool _isDeveloperNick(string str){foreach(string dn in developerNicknames){return GameAssets.CaseInsStrCmpr(str,dn);}return false;}
    void Start(){
        username.text=SaveSerial.instance.hyperGamerLoginData.username;
    }
    void Update(){
        if(SaveSerial.instance.hyperGamerLoginData.loggedIn){if(!logOutPanel.activeSelf){logOutPanel.SetActive(true);bothPanel.SetActive(false);}}
        else{if(!bothPanel.activeSelf){bothPanel.SetActive(true);logOutPanel.SetActive(false);}}

        if(showPassword&&password.contentType!=TMP_InputField.ContentType.Standard){
            password.contentType=TMP_InputField.ContentType.Standard;password.ForceLabelUpdate();
            passwordLoggedIn.contentType=TMP_InputField.ContentType.Standard;passwordLoggedIn.ForceLabelUpdate();
            passwordNew.contentType=TMP_InputField.ContentType.Standard;passwordNew.ForceLabelUpdate();
        }
        else if(!showPassword&&password.contentType!=TMP_InputField.ContentType.Password){
            password.contentType=TMP_InputField.ContentType.Password;password.ForceLabelUpdate();
            passwordLoggedIn.contentType=TMP_InputField.ContentType.Password;passwordLoggedIn.ForceLabelUpdate();
            passwordNew.contentType=TMP_InputField.ContentType.Password;passwordNew.ForceLabelUpdate();
        }
        
        if(Input.GetKeyDown(KeyCode.Tab)){
            if(username.isFocused){password.Select();return;}
            else if(password.isFocused){username.Select();return;}
            
            if(passwordLoggedIn.isFocused){passwordNew.Select();return;}
        }
        if(Input.GetKeyDown("enter")||Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter)){
            //if(username.isFocused){password.Select();return;}
            //else if(password.isFocused){Debug.Log("Pre");password.ActivateInputField();Debug.Log("Mid");password.Select();LogIn();Debug.Log("Post");return;}

            //if(passwordLoggedIn.isFocused){passwordNew.Select();return;}
            //else if(passwordNew.isFocused){passwordNew.ActivateInputField();passwordNew.Select();ChangePassHyperGamer();return;}

            if(username.text!=""&&password.text!=""){LogIn();return;}
            if(passwordLoggedIn.text!=""&&passwordNew.text!=""){ChangePassHyperGamer();return;}
        }

        if(GSceneManager.EscPressed()&&_inputsSelected){SelectVal(false);}
        if(GSceneManager.EscPressed()&&!_inputsSelected){GSceneManager.instance.LoadSocialsScene();}
        //if(GSceneManager.EscPressed()&&!InputsSelected()){GSceneManager.instance.LoadSocialsScene();}
    }
    bool _inputsSelected;
    public bool InputsSelected(){return username.isFocused||password.isFocused||passwordLoggedIn.isFocused||passwordNew.isFocused;}
    public void SelectVal(bool val){if(val==true){_inputsSelected=true;}else{StartCoroutine(SelectValFalseI());}}
    IEnumerator SelectValFalseI(){yield return new WaitForSecondsRealtime(0.02f);_inputsSelected=false;}

    public void Register(){
        if(username.text!=""&&password.text!=""){
            if(!_isDeveloperNick(username.text)||(_isDeveloperNick(username.text)&&password.text==gitignoreScript.developerPasswordHyperLogin)){
                DBAccess.instance.RegisterHyperGamer(username.text,password.text);
                //username.text="";password.text="";
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
    public void ChangePassHyperGamer(){
        if(passwordNew.text!=""&&passwordLoggedIn.text!=""
        &&passwordNew.text!=passwordLoggedIn.text
        &&passwordLoggedIn.text==SaveSerial.instance.hyperGamerLoginData.password){
            var colors=deleteButton.colors;
            if(!changePassConfirm){changePassConfirm=true;
                colors.normalColor=new Color(195,195,195);colors.selectedColor=Color.white;colors.highlightedColor=colors.selectedColor;changePassButton.colors=colors;
                return;
            }
            else{
                colors.normalColor=new Color(86,86,86);colors.selectedColor=new Color(100,100,100);colors.highlightedColor=colors.selectedColor;changePassButton.colors=colors;
                changePassConfirm=false;DBAccess.instance.ChangePassHyperGamer(passwordLoggedIn.text,passwordNew.text);
                passwordLoggedIn.text="";passwordNew.text="";
                return;
            }
        }else{
            if(passwordNew.text==""){DBAccess.instance.SetLoggedInMessage("bruh type a new password");}
            else if(passwordNew.text==passwordLoggedIn.text){DBAccess.instance.SetLoggedInMessage("bruh the passwords are the same");}
        }
    }
    public void DeleteHyperGamer(){
        if(passwordLoggedIn.text==SaveSerial.instance.hyperGamerLoginData.password){
            var colors=deleteButton.colors;
            if(!deleteConfirm){deleteConfirm=true;
                colors.normalColor=new Color(195,195,195);colors.selectedColor=Color.white;colors.highlightedColor=colors.selectedColor;deleteButton.colors=colors;
                return;
            }
            else{
                colors.normalColor=new Color(86,86,86);colors.selectedColor=new Color(100,100,100);colors.highlightedColor=colors.selectedColor;deleteButton.colors=colors;
                deleteConfirm=false;DBAccess.instance.DeleteHyperGamer(passwordLoggedIn.text);if(SaveSerial.instance.hyperGamerLoginData.registeredCount>0)SaveSerial.instance.hyperGamerLoginData.registeredCount--;
                passwordLoggedIn.text="";passwordNew.text="";
                return;
            }
        }
    }
    public void LogOut(){SaveSerial.instance.LogOut();}
    public void ToggleShowPassword(bool isOn){showPassword=isOn;}
    public void ToggleRememberPassword(bool isOn){rememberPassword=isOn;}
    public bool _rememberPassword(){return rememberPassword;}
}
