﻿
namespace USO.Mvc.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using USO.Core;

    public interface ICookie : IDependency
    {
        T GetValue<T>(string name);

        T GetValue<T>(string name, bool expireOnceRead);

        void SetValue<T>(string name, T value);

        void SetValue<T>(string name, T value, float expireDurationInMinutes);

        void SetValue<T>(string name, T value, bool httpOnly);

        void SetValue<T>(string name, T value, float expireDurationInMinutes, bool httpOnly);
    }
}
