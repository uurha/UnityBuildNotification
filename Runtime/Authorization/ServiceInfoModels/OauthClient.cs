﻿using System;
using Newtonsoft.Json;

namespace BuildNotification.Runtime.Authorization.ServiceInfoModels
{
    [Serializable]
    public class OauthClient
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_type")]
        public int ClientType { get; set; }
    }
}