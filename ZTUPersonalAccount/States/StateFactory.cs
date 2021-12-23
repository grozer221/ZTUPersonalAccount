using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using ZTUPersonalAccount.States.LoginState;

namespace ZTUPersonalAccount.States
{
    public class StateFactory
    {
        private static Dictionary<long, string> States;
        private static Dictionary<long, Dictionary<string, string>> Data;

        private static readonly object _lockObject = new object();

        private readonly IServiceProvider _serviceProvider;

        public StateFactory(IServiceProvider serviceProvider)
        {
            States = new Dictionary<long, string>();
            Data = new Dictionary<long, Dictionary<string, string>>();
            _serviceProvider = serviceProvider;
        }

        public IState CreateState(string state)
        {
            return state switch
            {
                WriteUserNameState.Name => _serviceProvider.GetRequiredService<WriteUserNameState>(),
                WritePasswordState.Name => _serviceProvider.GetRequiredService<WritePasswordState>(),
                LoginSubmitState.Name => _serviceProvider.GetRequiredService<LoginSubmitState>(),
                _ => _serviceProvider.GetRequiredService<NotFoundState>()
            };
        }

        public void SetState(long chatId, string state)
        {
            lock (_lockObject)
            {
                if (States.ContainsKey(chatId))
                    States[chatId] = state;
                else
                    States.Add(chatId, state);

                if (!Data.ContainsKey(chatId))
                    Data[chatId] = new Dictionary<string, string>();
            }
        }

        public string GetState(long chatId)
        {
            lock (_lockObject)
            {
                if (States.ContainsKey(chatId))
                    return States[chatId];
                else
                    return null;
            }
        }

        public void RemoveStateAndData(long chatId)
        {
            lock (_lockObject)
            {
                if (States.ContainsKey(chatId))
                    States.Remove(chatId);
                
                if (Data.ContainsKey(chatId))
                    Data.Remove(chatId);
            }
        }

        public IState Next(long chatId)
        {
            lock (_lockObject)
            {
                if (!States.ContainsKey(chatId))
                    return null;

                IState nextState = States[chatId] switch
                {
                    WriteUserNameState.Name => _serviceProvider.GetRequiredService<WritePasswordState>(),
                    WritePasswordState.Name => _serviceProvider.GetRequiredService<LoginSubmitState>(),
                    LoginSubmitState.Name => null,
                    _ => _serviceProvider.GetRequiredService<NotFoundState>()
                };
                if (nextState == null)
                {
                    RemoveStateAndData(chatId);
                    return null;
                }
                else
                {
                    SetState(chatId, nextState.GetName());
                }
                return nextState;
            }
        }

        public void SetData(long chatId, string key, string value)
        {
            if (!Data.ContainsKey(chatId))
                Data[chatId] = new Dictionary<string, string>();

            if (Data[chatId].ContainsKey(key))
            {
                Data[chatId][key] = value;
            }
            else
            {
                Data[chatId].Add(key, value);
            }
        }
        
        
        public Dictionary<string, string> GetData(long chatId)
        {
            if (!Data.ContainsKey(chatId))
                return null;

            return Data[chatId];
        }
    }
}
