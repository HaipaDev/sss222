using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Threading.Tasks;


public class DBAccess : MonoBehaviour{
    const string MONGO_URI = "mongodb+srv://sss222db:ElBpPfSw8tHOEYi2@cluster0.9rrjl.mongodb.net/Cluster0";
    const string DATABASE_NAME = "sss222";
    MongoClient client;
    IMongoDatabase db;
    
    IMongoCollection<Model_Score> scores_arcade;
    void Start(){
        client=new MongoClient(MONGO_URI);
        db=client.GetDatabase(DATABASE_NAME);
        scores_arcade = db.GetCollection<Model_Score>("scores_arcade");

        //SaveScoreToDB("testname",100);
        //GetScoresFromDB();
    }

    public async void SaveScoreToDB(string name, int score){
        Model_Score document=new Model_Score { name=name, score=score, version="0.5t3", date=System.DateTime.Now };
        await scores_arcade.InsertOneAsync(document);    
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