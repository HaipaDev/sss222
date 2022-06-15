using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;


public class DBAccess : MonoBehaviour{      public static DBAccess instance;
    /*const*/ string MONGO_URI = gitignoreScript.mongoDBString;//= "";
    MongoClient client_SSS222;
    const string DATABASE_NAME_SSS222 = "sss222";
    IMongoDatabase db_SSS222;
    const string DATABASE_NAME_hyperGamers = "hyperGamerLogin";
    MongoClient client_hyperGamers;
    IMongoDatabase db_hyperGamers;
    
    IMongoCollection<Model_Score> scores_arcade;
    IMongoCollection<Model_Score> scores_classic;
    IMongoCollection<Model_Score> scores_meteormadness;
    IMongoCollection<Model_Score> scores_hardcore;
    IMongoCollection<HyperGamer> hyperGamers;
    public string loginMessage;
    public string loggedInMessage;
    public string submitMessage;

    public string hyperLastLoginAppDisplay="SSS222";

    void Awake(){
        if(DBAccess.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}
        MONGO_URI=gitignoreScript.mongoDBString;
    }
    void Start(){
        client_SSS222=new MongoClient(MONGO_URI);
        db_SSS222=client_SSS222.GetDatabase(DATABASE_NAME_SSS222);
        scores_arcade=db_SSS222.GetCollection<Model_Score>("scores_arcade");
        scores_hardcore=db_SSS222.GetCollection<Model_Score>("scores_hardcore");
        scores_classic=db_SSS222.GetCollection<Model_Score>("scores_classic");
        scores_meteormadness=db_SSS222.GetCollection<Model_Score>("scores_meteormadness");

        
        client_hyperGamers=new MongoClient(MONGO_URI);
        db_hyperGamers=client_hyperGamers.GetDatabase(DATABASE_NAME_hyperGamers);
        hyperGamers=db_hyperGamers.GetCollection<HyperGamer>("userdata");

        //LoginHyperGamer(SaveSerial.instance.hyperGamerLoginData.username,SaveSerial.instance.hyperGamerLoginData.password);

        //SaveScoreToDB("testname",100);
        //GetScoresFromDB();
    }

    public async void SaveScoreToDB(string name, Highscore highscore){
        var scores=GetGamemodeCollection();
        var sameNameScore=await scores.FindAsync(e => e.name==name);
        //if(sameIDscore.ToList().Count>0){Debug.Log(id);}else{Debug.Log("Score with name "+name+" not found");}
        if(sameNameScore.ToList().Count>0){
            sameNameScore=await scores.FindAsync(e => e.name==name);
            if(highscore.score==0){SetSubmitMessage("Score is equals 0!");}
            else if(highscore.score<=sameNameScore.ToList()[0].score){SetSubmitMessage("Score is lower or equals to submitted");}
            else{await scores.FindOneAndUpdateAsync(e=>e.name==name,Builders<Model_Score>.Update.Set(e=>e.score,highscore.score));SetSubmitMessage("Score overwritten!");}
        }else{if(highscore.score!=0){
            Model_Score document=new Model_Score{name=name,score=highscore.score,
            playtime=highscore.playtime,
            version=highscore.version,build=highscore.build,
            date=highscore.date
            };
            await scores.InsertOneAsync(document);
            SetSubmitMessage("New score submitted!");
        }else{SetSubmitMessage("Score is equals 0!");}}
    }
    public async Task<List<Model_Score>> GetScoresFromDB(){
        var scores=GetGamemodeCollection();
        var allScoresTask=scores.FindAsync<Model_Score>(FilterDefinition<Model_Score>.Empty);
        var scoresAwaited=await allScoresTask;
        List<Model_Score> highscores=new List<Model_Score>();
        foreach(var score in scoresAwaited.ToList()){
            //Debug.Log(score.ToString());
            highscores.Add(score);
            //highscores.Add(Deserialize(score.ToString()));
        }
        //Debug.Log("Highscores: "+highscores.ToString());
        return highscores;
    }
    IMongoCollection<Model_Score> GetGamemodeCollection(){
        var collection=scores_arcade;
        if(GameSession.instance.CheckGamemodeSelected("Classic")){collection=scores_classic;}
        else if(GameSession.instance.CheckGamemodeSelected("Hardcore")){collection=scores_hardcore;}
        else if(GameSession.instance.CheckGamemodeSelected("Meteor")){collection=scores_meteormadness;}
        return collection;
    }



    public async void RegisterHyperGamer(string username,string password){
        System.Threading.CancellationToken cancellationToken=System.Threading.CancellationToken.None;
        var loginUsername=await hyperGamers.FindAsync(e=>/*GameAssets.CaseInsStrCmpr(e.username,username)*/e.username==username,null,cancellationToken);
        //if(loginData.ToList().Count>0){
            //loginData=await hyperGamers.FindAsync(e => e.username==username&&e.password==password);
        bool collectionBiggerThan0=false;
        if(loginUsername.ToList().Count>0){collectionBiggerThan0=true;}
        loginUsername=await hyperGamers.FindAsync(e=>e.username==username);
        if(collectionBiggerThan0&&loginUsername.ToList()[0].username==username){
            SetLoginMessage("You cant register an account that already exists");
        }else if(SaveSerial.instance.hyperGamerLoginData.registeredCount>=SaveSerial.instance.maxRegisteredHyperGamers){
            SetLoginMessage("You cant register more than 3 accounts per device");
        }else{
            HyperGamer document=new HyperGamer{username=username,password=password,
                dateRegister=System.DateTime.Now,dateLastLogin=System.DateTime.Now,
                appRegistered=hyperLastLoginAppDisplay,appLastLogin=hyperLastLoginAppDisplay,
                isSteam=GameSession.instance.isSteam,/*steamID=Steamworks.SteamClient.SteamId,*/sss222_customizationData=customizationData(),sss222_overlayColors=overlayColors()};
            await hyperGamers.InsertOneAsync(document);
            string _pass=password;if(FindObjectOfType<Login>()!=null){if(!FindObjectOfType<Login>()._rememberPassword())_pass="";}
            SaveSerial.instance.SetLogin(username,_pass);SaveSerial.instance.SaveLogin();
            SaveSerial.instance.hyperGamerLoginData.registeredCount++;
        }
    }
    public async void LoginHyperGamer(string username,string password){
        System.Threading.CancellationToken cancellationToken=System.Threading.CancellationToken.None;
        var loginUsername=await hyperGamers.FindAsync(e=>e.username==username,null,cancellationToken);
        bool collectionBiggerThan0=false;
        if(loginUsername.ToList().Count>0){collectionBiggerThan0=true;
            loginUsername=await hyperGamers.FindAsync(e=>e.username==username,null,cancellationToken);
            if(collectionBiggerThan0&&loginUsername.ToList()[0].password!=password){
                SetLoginMessage("Wrong password");if(SaveSerial.instance.hyperGamerLoginData.loggedIn)SaveSerial.instance.LogOut();
            }
            loginUsername=await hyperGamers.FindAsync(e=>e.username==username,null,cancellationToken);
            if(collectionBiggerThan0&&loginUsername.ToList()[0].password==password){
                loginUsername=await hyperGamers.FindAsync(e=>e.username==username,null,cancellationToken);
                hyperGamers.FindOneAndUpdate(e=>e.username==username,Builders<HyperGamer>.Update.Set(e=>e.dateLastLogin,System.DateTime.Now));
                hyperGamers.FindOneAndUpdate(e=>e.username==username,Builders<HyperGamer>.Update.Set(e=>e.appLastLogin,hyperLastLoginAppDisplay));
                await hyperGamers.FindOneAndUpdateAsync(e=>e.username==username,Builders<HyperGamer>.Update.Set(e=>e.sss222_customizationData,customizationData()));
                await hyperGamers.FindOneAndUpdateAsync(e=>e.username==username,Builders<HyperGamer>.Update.Set(e=>e.sss222_overlayColors,overlayColors()));
                string _pass=password;if(FindObjectOfType<Login>()!=null){if(!FindObjectOfType<Login>()._rememberPassword())_pass="";}
                SaveSerial.instance.SetLogin(username,_pass);SaveSerial.instance.SaveLogin();
            }
        }else{SetLoginMessage("Login not found");if(SaveSerial.instance.hyperGamerLoginData.loggedIn)SaveSerial.instance.LogOut();}
    }
    public string[] customizationData(){var pd=SaveSerial.instance.playerData;return new string[]{pd.skinName,pd.trailName,pd.flaresName,pd.deathFxName};}
    public float[] overlayColors(){var pd=SaveSerial.instance.playerData;return new float[]{pd.overlayColor[0],pd.overlayColor[1],pd.overlayColor[2]};}
    public async void UpdateCustomizationData(){
        System.Threading.CancellationToken cancellationToken=System.Threading.CancellationToken.None;
        var loginUsername=await hyperGamers.FindAsync(e=>e.username==SaveSerial.instance.hyperGamerLoginData.username,null,cancellationToken);
        if(loginUsername.ToList().Count>0){
            loginUsername=await hyperGamers.FindAsync(e=>e.username==SaveSerial.instance.hyperGamerLoginData.username,null,cancellationToken);
            
            await hyperGamers.FindOneAndUpdateAsync(e=>e.username==SaveSerial.instance.hyperGamerLoginData.username,Builders<HyperGamer>.Update.Set(e=>e.sss222_customizationData,customizationData()));
            await hyperGamers.FindOneAndUpdateAsync(e=>e.username==SaveSerial.instance.hyperGamerLoginData.username,Builders<HyperGamer>.Update.Set(e=>e.sss222_overlayColors,overlayColors()));
            SetLoggedInMessage("Customization Data updated");Debug.Log("Customization Data updated");
        }else{SetLoggedInMessage("Login not found");if(SaveSerial.instance.hyperGamerLoginData.loggedIn)SaveSerial.instance.LogOut();}
    }
    public async Task<string[]> GetUsersCustomizationData(string username){
        System.Threading.CancellationToken cancellationToken=System.Threading.CancellationToken.None;
        var loginUsername=await hyperGamers.FindAsync(e=>e.username==username,null,cancellationToken);
        if(loginUsername.ToList().Count>0){
            loginUsername=await hyperGamers.FindAsync(e=>e.username==username,null,cancellationToken);
            return loginUsername.First().sss222_customizationData;
        }else{SetLoggedInMessage("User not found");return null;}
    }
    public async void ChangePassHyperGamer(string password,string newPass){
        System.Threading.CancellationToken cancellationToken=System.Threading.CancellationToken.None;
        var loginUsername=await hyperGamers.FindAsync(e=>e.username==SaveSerial.instance.hyperGamerLoginData.username,null,cancellationToken);
        bool collectionBiggerThan0=false;
        if(loginUsername.ToList().Count>0){collectionBiggerThan0=true;
            loginUsername=await hyperGamers.FindAsync(e=>e.username==SaveSerial.instance.hyperGamerLoginData.username,null,cancellationToken);
            if(collectionBiggerThan0&&loginUsername.ToList()[0].password!=password){
                SetLoggedInMessage("Wrong password");
            }
            loginUsername=await hyperGamers.FindAsync(e=>e.username==SaveSerial.instance.hyperGamerLoginData.username,null,cancellationToken);
            if(collectionBiggerThan0&&loginUsername.ToList()[0].password==password){
                loginUsername=await hyperGamers.FindAsync(e=>e.username==SaveSerial.instance.hyperGamerLoginData.username,null,cancellationToken);
                SaveSerial.instance.SetLogin(SaveSerial.instance.hyperGamerLoginData.username,newPass);SaveSerial.instance.SaveLogin();
                await hyperGamers.FindOneAndUpdateAsync(e=>e.username==SaveSerial.instance.hyperGamerLoginData.username,Builders<HyperGamer>.Update.Set(e=>e.password,newPass));
                SetLoggedInMessage("Password changed");
            }
        }else{SetLoggedInMessage("Login not found");if(SaveSerial.instance.hyperGamerLoginData.loggedIn)SaveSerial.instance.LogOut();}
    }
    public void DeleteHyperGamer(string password){
        hyperGamers.FindOneAndDelete(e=>e.username==SaveSerial.instance.hyperGamerLoginData.username);SaveSerial.instance.LogOut();SetLoginMessage("Account deleted");
    }


    public void SetLoginMessage(string msg){StartCoroutine(SetLoginMessageI(msg));}
    IEnumerator SetLoginMessageI(string msg){DBAccess.instance.loginMessage=msg;yield return new WaitForSecondsRealtime(2f);if(DBAccess.instance.loginMessage==msg)DBAccess.instance.loginMessage="";}
    public void SetLoggedInMessage(string msg){StartCoroutine(SetLoggedInMessageI(msg));}
    IEnumerator SetLoggedInMessageI(string msg){DBAccess.instance.loggedInMessage=msg;yield return new WaitForSecondsRealtime(2f);if(DBAccess.instance.loggedInMessage==msg)DBAccess.instance.loggedInMessage="";}
    public void SetSubmitMessage(string msg){StartCoroutine(SetSubmitMessageI(msg));}
    IEnumerator SetSubmitMessageI(string msg){DBAccess.instance.submitMessage=msg;yield return new WaitForSecondsRealtime(2f);if(DBAccess.instance.submitMessage==msg)DBAccess.instance.submitMessage="";}
}
// Model_User Sample
[System.Serializable]
public class Model_Score {
    public ObjectId _id { set; get; }
    
    //public int id { set; get; }
    public string name {  set; get; }
    public int score { set; get; }
    public float playtime { set; get; }
    public string version { set; get; }
    public float build { set; get; }
    public System.DateTime date { set; get; }
    
    //Possible Methods ...
        
}
[System.Serializable]
public class HyperGamer {
    public ObjectId _id { set; get; }
    

    public string username {  set; get; }
    public string password { set; get; }
    public bool isSteam { set; get; }
    //public Steamworks.SteamId steamID { set; get; }
    public string[] sss222_customizationData { set; get; }
    public float[] sss222_overlayColors { set; get; }
    public string appRegistered { set; get; }
    public string appLastLogin { set; get; }
    public System.DateTime dateLastLogin { set; get; }
    public System.DateTime dateRegister { set; get; }
}