﻿namespace Library.Interfaces
{
    public interface IRedisConfigurationModel
    {
        public string Redis { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public int ExpirySecondtime { get; set; }
    }
}