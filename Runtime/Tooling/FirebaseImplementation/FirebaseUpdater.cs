﻿using System;
using System.Threading.Tasks;
using Better.BuildNotification.Runtime.Tooling.Authorization;
using UnityEngine;

namespace Better.BuildNotification.Runtime.Tooling.FirebaseImplementation
{
    public static class FirebaseUpdater
    {
        public static async Task<bool> RefreshToken(FirebaseData fcmScriptable, DateTimeOffset now)
        {
            var firebaseAdminSDKData = fcmScriptable.ServiceAccountData;
            var (messageToken, messageTokenError) =
                await FirebaseCustomToken.MakeTokenRequest(firebaseAdminSDKData, fcmScriptable.MessagingData.Scopes, now);

            if (messageTokenError != null)
            {
                Debug.LogError(
                    $"Error: {messageTokenError.Error} Error Description: {messageTokenError.ErrorDescription}");
            
                fcmScriptable.LastRequestSuccessful = false;
                return false;
            }
            
            var (databaseToken, databaseTokenError) =
                await FirebaseCustomToken.MakeTokenRequest(firebaseAdminSDKData, fcmScriptable.DatabaseData.Scopes, now);

            if (databaseTokenError != null)
            {
                Debug.LogError(
                    $"Error: {databaseTokenError.Error} Error Description: {databaseTokenError.ErrorDescription}");

                fcmScriptable.LastRequestSuccessful = false;
                return false;
            }

            fcmScriptable.MessagingData.SetToken(messageToken.AccessToken);
            fcmScriptable.DatabaseData.SetToken(databaseToken.AccessToken);
            fcmScriptable.ExpirationTime = FirebaseCustomToken.GetExpirationTime(now);
            fcmScriptable.LastRequestSuccessful = true;
            return true;
        }

        public static bool ValidateLastRequest(FirebaseData fcmScriptable, DateTimeOffset now)
        {
            return now < FirebaseCustomToken.FromSecond(fcmScriptable.ExpirationTime) &&
                   fcmScriptable.LastRequestSuccessful;
        }
    }
}