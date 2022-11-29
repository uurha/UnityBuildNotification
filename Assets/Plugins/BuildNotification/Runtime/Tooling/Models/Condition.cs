﻿using System;

namespace Better.BuildNotification.Runtime.Tooling.Models
{
    [Serializable]
    public class Condition
    {
        public Condition(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}