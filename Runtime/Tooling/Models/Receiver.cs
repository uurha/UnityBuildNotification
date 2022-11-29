﻿using System;

namespace Better.BuildNotification.Runtime.Tooling.Models
{
    [Serializable]
    public class Receiver
    {
        public Receiver(string token)
        {
            Token = token;
        }

        public string Token { get; }
    }
}