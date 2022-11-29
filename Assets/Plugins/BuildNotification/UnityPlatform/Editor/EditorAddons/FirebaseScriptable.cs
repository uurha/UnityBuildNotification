﻿using System.Collections.Generic;
using Better.BuildNotification.Runtime.Tooling.Authorization;
using Better.BuildNotification.Runtime.Tooling.FirebaseImplementation;
using Better.BuildNotification.Runtime.Tooling.Models;
using UnityEngine;

namespace Better.BuildNotification.UnityPlatform.EditorAddons
{
    public class FirebaseScriptable : ScriptableObject
    {
        [SerializeField] private FirebaseData data;

        public List<Receiver> Receivers => data.cloudMessagingData.Receivers;
        public bool IsValid => data.serviceAccountData.IsValid;

        public FirebaseData Data => data;

        internal void SetFirebaseAdminSDk(FirebaseAdminSDKData adminSDKData)
        {
            data.serviceAccountData = adminSDKData;
            data.cloudMessagingData.SetProject(adminSDKData.ProjectID);
            data.realtimeDatabaseData.SetProject(adminSDKData.ProjectID);
        }
    }
}