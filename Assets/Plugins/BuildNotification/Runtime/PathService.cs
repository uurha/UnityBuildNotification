﻿using BuildNotification.Runtime.Authorization;
using UnityEngine;

namespace BuildNotification.Runtime
{
    public static class PathService
    {
        public static string ServiceInfoDataPath { get; }
        
        public const string DefaultExtension = "json";

        public const string JsonFileType = "application/json";

        static PathService()
        {
            ServiceInfoDataPath = Application.persistentDataPath + $"{nameof(ServiceInfoData)}.{DefaultExtension}";
        } 
    }
}