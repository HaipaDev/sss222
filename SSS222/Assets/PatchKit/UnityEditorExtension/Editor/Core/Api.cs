using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using PatchKit.Api;
using PatchKit.Api.Models.Main;
using PatchKit.UnityEditorExtension.Connection;
using UnityEngine.Assertions;

namespace PatchKit.UnityEditorExtension.Core
{
public static class Api
{
    private class CachedApps
    {
        public CachedApps(ApiKey apiKey)
        {
            ApiKey = apiKey;
        }

        [NotNull]
        public readonly List<App> List = new List<App>();

        public readonly ApiKey ApiKey;

        public DateTime? LastFullUpdateDateTime;
    }

    [NotNull]
    private static MainApiConnection ApiConnection
    {
        get
        {
            return new MainApiConnection(Config.ApiConnectionSettings)
            {
                HttpClient = new UnityHttpClient()
            };
        }
    }

    private static ApiKey GetApiKey()
    {
        ApiKey? savedApiKey = Config.GetLinkedAccountApiKey();

        Assert.IsTrue(savedApiKey.HasValue);

        return savedApiKey.Value;
    }

    private static CachedApps _cachedApps;

    [NotNull]
    public static App[] GetApps()
    {
        ApiKey apiKey = GetApiKey();

        App[] result = ApiConnection.ListsUserApplications(apiKey.Value);
        Assert.IsNotNull(result);

        _cachedApps = new CachedApps(apiKey);
        _cachedApps.List.AddRange(result);
        _cachedApps.LastFullUpdateDateTime = DateTime.Now;

        return result;
    }

    private static bool ShouldGetAppsUseCache()
    {
        ApiKey apiKey = GetApiKey();

        if (_cachedApps == null)
        {
            return false;
        }

        if (!_cachedApps.ApiKey.Equals(apiKey))
        {
            return false;
        }

        if (!_cachedApps.LastFullUpdateDateTime.HasValue)
        {
            return false;
        }

        TimeSpan timeSinceLastFullUpdate =
            DateTime.Now - _cachedApps.LastFullUpdateDateTime.Value;

        return timeSinceLastFullUpdate.TotalMinutes < 1.0;
    }

    [NotNull]
    public static App[] GetAppsCached()
    {
        if (ShouldGetAppsUseCache())
        {
            Assert.IsNotNull(_cachedApps);
            return _cachedApps.List.ToArray();
        }

        return GetApps();
    }

    public static App GetAppInfo(AppSecret secret)
    {
        if (!secret.IsValid)
        {
            throw new InvalidArgumentException("secret");
        }

        ApiKey apiKey = GetApiKey();

        App app = ApiConnection.GetApplicationInfo(secret.Value);

        if (_cachedApps == null || !_cachedApps.ApiKey.Equals(apiKey))
        {
            _cachedApps = new CachedApps(apiKey);
        }

        _cachedApps.List.RemoveAll(x => x.Secret == app.Secret);
        _cachedApps.List.Add(app);

        return app;
    }

    public static App GetAppInfoCached(AppSecret secret)
    {
        if (!secret.IsValid)
        {
            throw new InvalidArgumentException("secret");
        }

        ApiKey apiKey = GetApiKey();

        if (_cachedApps != null && _cachedApps.ApiKey.Equals(apiKey))
        {
            int i = _cachedApps.List.FindIndex(a => a.Secret == secret.Value);

            if (i != -1)
            {
                return _cachedApps.List[i];
            }
        }

        return GetAppInfo(secret);
    }

    public static App CreateNewApp(AppName name, AppPlatform platform)
    {
        if (!name.IsValid)
        {
            throw new InvalidArgumentException("name");
        }

        ApiKey apiKey = GetApiKey();

        App app = ApiConnection.PostUserApplication(
            apiKey.Value,
            name.Value,
            platform.ToApiString());

        if (_cachedApps == null || !_cachedApps.ApiKey.Equals(apiKey))
        {
            _cachedApps = new CachedApps(apiKey);
        }

        _cachedApps.List.RemoveAll(x => x.Secret == app.Secret);
        _cachedApps.List.Add(app);

        return app;
    }
}
}