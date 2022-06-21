using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using Steamworks.Data;
using System.Threading.Tasks;

public class SteamManager : MonoBehaviour{  public static SteamManager instance;
    const int appID=playtestID;
    const int mainAppID=2000190;
    const int playtestID=2000200;
    void Awake(){
        if(SteamManager.instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}
    }
    void Start(){//IEnumerator Start(){
        //yield return new WaitForSeconds(0.1f);
        if(Application.platform==RuntimePlatform.WindowsPlayer||Application.platform==RuntimePlatform.WindowsEditor){
        if(GameSession.instance!=null){if(GameSession.instance.isSteam){
            InitSteam();
            SteamUserStats.RequestCurrentStats();
        }}
        }else{if(GameSession.instance!=null){GameSession.instance.isSteam=false;}}
    }
    void Update(){
        //SteamClient.RunCallbacks();
    }
    void InitSteam(){
        try{
            SteamClient.Init(appID,true);
            Debug.Log("Steam initialized for appID: " + appID);
        }
        catch(System.Exception e){
            Debug.LogError(e);
            GameSession.instance.steamAchievsStatsLeaderboards=false;
            // Something went wrong - it's one of these:
            //
            //     Steam is closed?
            //     Can't find steam_api dll?
            //     Don't have permission to play app?
            //
        }
    }
    /*[Sirenix.OdinInspector.Button("Shutdown Steam")]*/void OnApplicationQuit(){SteamClient.Shutdown();}
    public async void SubmitScore(string name,int score){
        Steamworks.Data.Leaderboard? leaderboard = await SteamUserStats.FindLeaderboardAsync(name);
        if(leaderboard.HasValue){
            Steamworks.Data.Leaderboard lb=(Steamworks.Data.Leaderboard)leaderboard;
            var result = await lb.SubmitScoreAsync(score);
        }
    }
    public async Task<Texture2D> GetAvatarCurrent(SteamId steamId){return await GetAvatar(SteamClient.SteamId);}
    public async Task<Texture2D> GetAvatar(SteamId steamId){
        // Get the task
        var avatar=await GetAvatarAsync(steamId);

        // Use Task.WhenAll, to cache multiple items at the same time
        //await Task.WhenAll(avatar);

        // Cache Items
        //Cache.Avatar=avatar.Result?.ConvertSteamImg();

        return ConvertSteamImg((Image)avatar);
    }
    async Task<Image?> GetAvatarAsync(SteamId steamId){
        try{
            // Get Avatar using await
            return await SteamFriends.GetLargeAvatarAsync(steamId);
        }
        catch (Exception e){
            // If something goes wrong, log it
            Debug.LogError(e);
            return null;
        }
    }
    public static Texture2D ConvertSteamImg(Image image){
        // Create a new Texture2D
        var avatar = new Texture2D( (int)image.Width, (int)image.Height, TextureFormat.ARGB32, false );
        
        // Set filter type, or else its really blury
        avatar.filterMode = FilterMode.Trilinear;

        // Flip image
        for ( int x = 0; x < image.Width; x++ )
        {
            for ( int y = 0; y < image.Height; y++ )
            {
                var p = image.GetPixel( x, y );
                avatar.SetPixel( x, (int)image.Height - y, new UnityEngine.Color( p.r / 255.0f, p.g / 255.0f, p.b / 255.0f, p.a / 255.0f ) );
            }
        }
        
        avatar.Apply();
        return avatar;
    }
}
