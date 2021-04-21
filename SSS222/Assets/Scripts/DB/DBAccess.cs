using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;


public class DBAccess : MonoBehaviour{
    const string MONGO_URI = "mongodb+srv://sss222db:ElBpPfSw8tHOEYi2@cluster0.9rrjl.mongodb.net/Cluster0";
    const string DATABASE_NAME_SSS222 = "sss222";
    MongoClient client_SSS222;
    IMongoDatabase db_SSS222;
    const string DATABASE_NAME_hyperGamers = "hyperGamerLogin";
    MongoClient client_hyperGamers;
    IMongoDatabase db_hyperGamers;
    
    IMongoCollection<Model_Score> scores_arcade;
    IMongoCollection<HyperGamer> hyperGamerLogin;
    void Start(){
        client_SSS222=new MongoClient(MONGO_URI);
        db_SSS222=client_SSS222.GetDatabase(DATABASE_NAME_SSS222);
        scores_arcade=db_SSS222.GetCollection<Model_Score>("scores_arcade");

        
        client_hyperGamers=new MongoClient(MONGO_URI);
        db_hyperGamers=client_hyperGamers.GetDatabase(DATABASE_NAME_hyperGamers);
        hyperGamerLogin=db_hyperGamers.GetCollection<HyperGamer>("userdata");

        //SaveScoreToDB("testname",100);
        //GetScoresFromDB();
    }

    public async void SaveScoreToDB(string name, int score){
        ObjectId id;
        var sameIDscore=await scores_arcade.FindAsync(e => e.name==name);
        id=sameIDscore.ToList()[0]._id;
        Model_Score document=new Model_Score { _id=id, name=name, score=score, version=GameSession.instance.GetGameVersion(), date=System.DateTime.Now };
        await scores_arcade.FindOneAndReplaceAsync(e => e.name==document.name, document);
    }
    public async Task<List<Model_Score>> GetScoresFromDB(){
        var allScoresTask=scores_arcade.FindAsync<Model_Score>(FilterDefinition<Model_Score>.Empty);
        var scoresAwaited=await allScoresTask;
        List<Model_Score> highscores=new List<Model_Score>();
        foreach(var score in scoresAwaited.ToList()){
            Debug.Log(score.ToString());
            highscores.Add(score);
            //highscores.Add(Deserialize(score.ToString()));
        }
        Debug.Log("Highscores: "+highscores.ToString());
        return highscores;
    }
    private Model_Score Deserialize(string rawJson){
        var highScore=new Model_Score();
        /*var stringWithoutID=rawJson.Substring(rawJson.IndexOf("),")+4);
        var username=stringWithoutID.Substring(0,stringWithoutID.IndexOf(":")-2);
        var score=stringWithoutID.Substring(stringWithoutID.IndexOf(":")+2);
        highScore.name=username;
        highScore.score=Convert.ToInt32(score);
        Debug.Log("Deserialized: "+highScore);*/
        return highScore;
    }



    public async void RegisterHyperGamer(string username, string password){
        HyperGamer document=new HyperGamer { username=username, password=password, dateRegister=System.DateTime.Now };
        await hyperGamerLogin.InsertOneAsync(document);
    }
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