using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace SignalSolt.NET
{
    public static class SignalSoltRegister
    {
        private static volatile ConcurrentDictionary<string, ConcurrentDictionary<string, Action<string, SignalSoltItem>>> Solts =
            new ConcurrentDictionary<string, ConcurrentDictionary<string, Action<string, SignalSoltItem>>>();

        public static string AddSolt(string key, Action<string, SignalSoltItem> action, string actionKey = default)
        {
            if (!Solts.ContainsKey(key))
            {
                Solts.TryAdd(key, new ConcurrentDictionary<string, Action<string, SignalSoltItem>>());
            }

            string keyFunc = "";
            if (!string.IsNullOrEmpty(actionKey))
            {
                keyFunc = actionKey;
            }
            else
            {
                IntPtr intPtr = action.Method.MethodHandle.Value;
                keyFunc = intPtr.ToString("X16");
            }

            if (!Solts[key].ContainsKey(keyFunc))
            {
                Solts[key].TryAdd(keyFunc, action);
            }
            else
            {
                Solts[key][keyFunc] = action;
            }

            return keyFunc;
        }

        public static bool Signal(string key, SignalSoltItem data)
        {
            if (Solts.ContainsKey(key))
            {
                if (!Solts.TryGetValue(key, out ConcurrentDictionary<string, Action<string, SignalSoltItem>> actions))
                {
                    return false;
                }

                if (actions == null || actions.Count <= 0)
                {
                    return true;
                }

                foreach (KeyValuePair<string, Action<string, SignalSoltItem>> actioin in actions)
                {
                    if (data == null)
                    {
                        actioin.Value(key, data);
                    }
                    else if (data != null && data.SignalContinue)
                    {
                        data.ActionKey = actioin.Key;
                        actioin.Value(key, data);
                    }
                    else
                    {
                        break;
                    }
                }

                return true;
            }

            return false;
        }

        public static bool BringToFront(string key, string actionKey)
        {
            if (!Solts.TryGetValue(key, out ConcurrentDictionary<string, Action<string, SignalSoltItem>> actions))
            {
                return false;
            }

            if (actions.TryRemove(actionKey, out Action<string, SignalSoltItem> action))
            {
                return actions.TryAdd(actionKey, action);
            }

            return false;
        }

        public static bool SendToBack(string key, string actionKey)
        {
            return false;
        }

        public static bool SendToIndexBack(string key, string actionKey)
        {
            return false;
        }

        public static bool RemoveSoltByKey(string key, string actionKey = default)
        {
            if (actionKey == default)
            {
                //if (Solts.ContainsKey(key))
                //{
                //    Solts.TryRemove(key, out _);
                //}
                //else
                //{
                //    ConcurrentDictionary<string, Action<string, SignalSoltItem>> obj = Solts.Values.FirstOrDefault(f => f.ContainsKey(key));
                //    if (obj != null)
                //    {
                //        obj.TryRemove(key, out _);
                //    }
                //}
                return !Solts.TryRemove(key, out _);
            }
            else
            {
                if (!Solts.TryGetValue(key, out ConcurrentDictionary<string, Action<string, SignalSoltItem>> actions))
                {
                    return false;
                }

                return actions.TryRemove(actionKey, out _);
            }
        }

        public static void ClearSoltAll()
        {
            Solts.Clear();
        }
    }
}
