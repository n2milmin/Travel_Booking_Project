﻿namespace GBC_Travel_Group_136.Services
{
    public interface ISessionService
    {
        void SetSessionData<T>(string key, T value);
        T GetSessionData<T>(string key);
    }
}
