using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour{
    [SerializeField] TextMeshProUGUI registerUsername;
    [SerializeField] TextMeshProUGUI registerPassword;
    [SerializeField] TextMeshProUGUI loginUsername;
    [SerializeField] TextMeshProUGUI loginPassword;
    [SerializeField] bool loggedIn;
    DBAccess db;
    void Start(){
        db=FindObjectOfType<DBAccess>();
    }
    public void Register(){db.RegisterHyperGamer(registerUsername.text,registerPassword.text);}
}
