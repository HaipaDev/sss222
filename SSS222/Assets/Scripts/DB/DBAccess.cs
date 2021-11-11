using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;


public class DBAccess : MonoBehaviour{
    public static DBAccess instance;
    const string MONGO_URI = "mongodb+srv://sss222db:ElBpPfSw8tHOEYi2@cluster0.9rrjl.mongodb.net/Cluster0";
    const string DATABASE_NAME_SSS222 = "sss222";
    MongoClient client_SSS222;
    IMongoDatabase db_SSS222;
    const string DATABASE_NAME_hyperGamers = "hyperGamerLogin";
    MongoClient client_hyperGamers;
    IMongoDatabase db_hyperGamers;
    
    IMongoCollection<Model_Score> scores_arcade;
    IMongoCollection<Model_Score> scores_classic;
    IMongoCollection<Model_Score> scores_meteormadness;
    IMongoCollection<Model_Score> scores_hardcore;
    IMongoCollection<HyperGamer> hyperGamers;
    public string registerMessage;
    public string loginMessage;
    public string submitMessage;
    void Awake(){instance=this;SetUpSingleton();}
    void SetUpSingleton(){int numberOfObj=FindObjectsOfType<GameSession>().Length;if(numberOfObj>1){Destroy(gameObject);}else{DontDestroyOnLoad(gameObject);}}
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

        //SaveScoreToDB("testname",100);
        //GetScoresFromDB();
    }

    public async void SaveScoreToDB(string name, int score){
        var scores=GetGamemodeCollection();
        var sameNameScore=await scores.FindAsync(e => e.name==name);
        //if(sameIDscore.ToList().Count>0){Debug.Log(id);}else{Debug.Log("Score with name "+name+" not found");}
        if(sameNameScore.ToList().Count>0){
            sameNameScore=await scores.FindAsync(e => e.name==name);
            if(score==0){SetSubmitMessage("Score is equals 0!");}
            else if(score<sameNameScore.ToList()[0].score){SetSubmitMessage("Score is lower than submitted");}
            else{await scores.FindOneAndUpdateAsync(e => e.name==name, Builders<Model_Score>.Update.Set(e => e.score, score));SetSubmitMessage("Score overwritten!");}
        }else{if(score!=0){
            Model_Score document=new Model_Score { name=name, score=score, version=GameSession.instance.gameVersion, date=System.DateTime.Now };
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
        if(GameSession.instance.CheckGameModeSelected("Classic")){collection=scores_classic;}
        else if(GameSession.instance.CheckGameModeSelected("Hardcore")){collection=scores_hardcore;}
        else if(GameSession.instance.CheckGameModeSelected("Meteor")){collection=scores_meteormadness;}
        return collection;
    }



    public async void RegisterHyperGamer(string username, string password){
        System.Threading.CancellationToken cancellationToken=System.Threading.CancellationToken.None;
        var loginData=await hyperGamers.FindAsync(e => e.username==username, null, (System.Threading.CancellationToken)cancellationToken);
        //if(loginData.ToList().Count>0){
            //loginData=await hyperGamers.FindAsync(e => e.username==username&&e.password==password);
        bool collectionBiggerThan0=false;
        if(loginData.ToList().Count>0){collectionBiggerThan0=true;}
        loginData=await hyperGamers.FindAsync(e => e.username==username);
        if(collectionBiggerThan0&&loginData.ToList()[0].username==username){
            SetRegisterMessage("You cant register an account that already exists");
        }else if(SaveSerial.instance.hyperGamerLoginData.registeredCount>=SaveSerial.instance.maxRegisteredHyperGamers){
            SetRegisterMessage("You cant register more than 3 accounts");
        }else{
            HyperGamer document=new HyperGamer { username=username, password=password, dateRegister=System.DateTime.Now, dateLastLogin=System.DateTime.Now };
            await hyperGamers.InsertOneAsync(document);
            SaveSerial.instance.SetLogin(username,password);SaveSerial.instance.SaveLogin();SaveSerial.instance.hyperGamerLoginData.registeredCount++;
        }
    }
    public async void LoginHyperGamer(string username, string password){
        var loginData=await hyperGamers.FindAsync(e => e.username==username&&e.password==password);
        if(loginData.ToList().Count>0){SaveSerial.instance.SetLogin(username,password);
            await hyperGamers.FindOneAndUpdateAsync(e => e.username==username, Builders<HyperGamer>.Update.Set(e => e.dateLastLogin, System.DateTime.Now));
        }else{SetLoginMessage("Login not found");}
    }


    
    public void SetRegisterMessage(string msg){StartCoroutine(SetRegisterMessageI(msg));}
    IEnumerator SetRegisterMessageI(string msg){DBAccess.instance.registerMessage=msg;yield return new WaitForSecondsRealtime(2);DBAccess.instance.registerMessage="";}
    public void SetLoginMessage(string msg){StartCoroutine(SetLoginMessageI(msg));}
    IEnumerator SetLoginMessageI(string msg){DBAccess.instance.loginMessage=msg;yield return new WaitForSecondsRealtime(2);DBAccess.instance.loginMessage="";}
    public void SetSubmitMessage(string msg){StartCoroutine(SetSubmitMessageI(msg));}
    IEnumerator SetSubmitMessageI(string msg){DBAccess.instance.submitMessage=msg;yield return new WaitForSecondsRealtime(2);DBAccess.instance.submitMessage="";}
}
// Model_User Sample
[System.Serializable]
public class Model_Score {
    public ObjectId _id { set; get; }
    
    //public int id { set; get; }
    public string name {  set; get; }
    public int score { set; get; }
    public string version { set; get; }
    public System.DateTime date { set; get; }
    
    //Possible Methods ...
        
}
[System.Serializable]
public class HyperGamer {
    public ObjectId _id { set; get; }
    
    public string username {  set; get; }
    public string password { set; get; }
    public System.DateTime dateLastLogin { set; get; }
    public System.DateTime dateRegister { set; get; }
}