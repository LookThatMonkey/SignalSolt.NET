using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SignalSolt.NET
{
    public class SignalSoltItem
    {
        private static volatile Dictionary<string, PropertyInfo> _propertyInfos = new Dictionary<string, PropertyInfo>();
        private static volatile Dictionary<string, FieldInfo> _fieldInfos = new Dictionary<string, FieldInfo>();
        private static volatile Dictionary<string, MethodInfo> _methodInfos = new Dictionary<string, MethodInfo>();

        public string ActionKey { get; internal set; }

        public string Key { get; set; }

        public object Sender { get; set; }

        public object NewValue { get; set; }

        public object OldValue { get; set; }

        public object Tag { get; set; }

        public OptionType OptionType { get; set; } = OptionType.PropertyChanged;

        public bool CanGo = true;

        public bool SignalContinue = true;

        public T GetSenderPropertyValue<T>(string propertyName)
        {
            if (!_propertyInfos.ContainsKey($"{Sender.GetType().FullName}.{propertyName}"))
            {
                BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                PropertyInfo propertyInfo = Sender.GetType().GetProperty(propertyName, bindingFlags);
                if (propertyInfo != null)
                {
                    _propertyInfos.Add($"{Sender.GetType().FullName}.{propertyName}", propertyInfo);
                }
            }

            if (_propertyInfos.ContainsKey($"{Sender.GetType().FullName}.{propertyName}"))
                return (T)_propertyInfos[$"{Sender.GetType().FullName}.{propertyName}"].GetValue(Sender);

            return default(T);
        }

        public T GetSenderFieldValue<T>(string fieldName)
        {
            if (!_fieldInfos.ContainsKey($"{Sender.GetType().FullName}.{fieldName}"))
            {
                BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                FieldInfo fieldInfo = Sender.GetType().GetField(fieldName, bindingFlags);
                if(fieldInfo != null)
                    _fieldInfos.Add($"{Sender.GetType().FullName}.{fieldName}", fieldInfo);
            }

            if (_fieldInfos.ContainsKey($"{Sender.GetType().FullName}.{fieldName}"))
                return (T)_fieldInfos[$"{Sender.GetType().FullName}.{fieldName}"].GetValue(Sender);

            return default(T);
        }

        public T InvokeSenderMethod<T>(string methodName, params object[] args)
        {
            Type[] types = args.Select(s => s.GetType()).ToArray();
            string key = $"{Sender.GetType().FullName}.{methodName}[{string.Join(",", types.Select(s => s.FullName))}]";
            if (!_methodInfos.ContainsKey(key))
            {
                BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                MethodInfo methodInfo = Sender.GetType().GetMethod(methodName, bindingFlags, null, types, null);
                if (methodInfo != null)
                    _methodInfos.Add(key, methodInfo);
            }

            if (_methodInfos.ContainsKey(key))
                return (T)_methodInfos[key].Invoke(Sender, args);

            return default(T);
        }

        public void InvokeSenderMethod(string methodName, params object[] args)
        {
            Type[] types = args.Select(s => s.GetType()).ToArray();
            string key = $"{Sender.GetType().FullName}.{methodName}[{string.Join(",", types.Select(s => s.FullName))}]";
            if (!_methodInfos.ContainsKey(key))
            {
                BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                MethodInfo methodInfo = Sender.GetType().GetMethod(methodName, bindingFlags, null, types, null);
                if (methodInfo != null)
                    _methodInfos.Add(key, methodInfo);
            }

            if (_methodInfos.ContainsKey(key))
            {
                _methodInfos[key].Invoke(Sender, args);
            }
        }
    }

    public enum OptionType
    {
        Create,
        Remove,
        Clear,
        PropertyChanged
    }
}