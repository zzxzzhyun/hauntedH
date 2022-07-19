using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Databaseaccess : MonoBehaviour
{
    MongoClient client = new MongoClient("mongodb+srv://jakekang28:FqUkciooEUNPa5gZ@cluster1.zyaxdsi.mongodb.net/?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> collection;

    private void Start()
    {
        database = client.GetDatabase("test");
        collection = database.GetCollection<BsonDocument>("playerdata");

        GetScoresFromDataBase();
    }

    public async void SaveScoreToDataBase(string userName, int score)
    {
        var document = new BsonDocument { { userName, score } };
        await collection.InsertOneAsync(document);
    }

    public async Task<List<HighScore>> GetScoresFromDataBase()
    {
        var allScoresTask = collection.FindAsync(new BsonDocument());
        var scoresAwaited = await allScoresTask;

        List<HighScore> highscores = new List<HighScore>();
        foreach(var score in scoresAwaited.ToList())
        {
            highscores.Add(Deserialize(score.ToString()));
        }
        return highscores;
    }
    private HighScore Deserialize(string rawJson)
    {
        var highScore = new HighScore();

        var stringWithoutID = rawJson.Substring(rawJson.IndexOf("),") + 4);
        var username = stringWithoutID.Substring(0, stringWithoutID.IndexOf(":") - 2);
        var score = stringWithoutID.Substring(stringWithoutID.IndexOf(":") + 2, stringWithoutID.IndexOf("}") - stringWithoutID.IndexOf(":") - 3);
        highScore.UserName = username;
        highScore.Score = Convert.ToInt32(score);
        return highScore;
    }
}

public class HighScore
{
    public string UserName { get; set; }
    public int Score { get; set; }
}
