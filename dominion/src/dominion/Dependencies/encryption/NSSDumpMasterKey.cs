using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using dominion.src.dominion.Dependencies.Hashing;
using dominion.src.dominion.Dependencies.Sql;

namespace dominion.src.dominion.Dependencies.Encryption
{
    public static class NSSDumpMasterKey
    {
        // Token: 0x06000304 RID: 772 RVA: 0x00017B64 File Offset: 0x00015D64
        public static byte[] Key4Database(string path)
        {
            Asn1Der asn1Der = new Asn1Der();
            SqLite sqLite = SqLite.ReadTable(path, "metaData");
            if (sqLite == null)
            {
                return null;
            }
            for (int i = 0; i < sqLite.GetRowCount(); i++)
            {
                if (!(sqLite.GetValue(i, 0) != "password"))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(sqLite.GetValue(i, 1));
                    byte[] bytes2 = Encoding.UTF8.GetBytes(sqLite.GetValue(i, 2));
                    if (bytes.Length >= 1 && bytes2.Length >= 1)
                    {
                        Asn1DerObject asn1DerObject = asn1Der.Parse(bytes2);
                        string text = asn1DerObject.ToString();
                        if (text != null)
                        {
                            if (text.Contains("2A864886F70D010C050103"))
                            {
                                Asn1DerObject asn1DerObject2 = asn1DerObject.Objects[0];
                                byte[] array;
                                if (asn1DerObject2 == null)
                                {
                                    array = null;
                                }
                                else
                                {
                                    Asn1DerObject asn1DerObject3 = asn1DerObject2.Objects[0];
                                    if (asn1DerObject3 == null)
                                    {
                                        array = null;
                                    }
                                    else
                                    {
                                        Asn1DerObject asn1DerObject4 = asn1DerObject3.Objects[1];
                                        if (asn1DerObject4 == null)
                                        {
                                            array = null;
                                        }
                                        else
                                        {
                                            Asn1DerObject asn1DerObject5 = asn1DerObject4.Objects[0];
                                            array = ((asn1DerObject5 != null) ? asn1DerObject5.Data : null);
                                        }
                                    }
                                }
                                byte[] array2 = array;
                                Asn1DerObject asn1DerObject6 = asn1DerObject.Objects[0];
                                byte[] array3;
                                if (asn1DerObject6 == null)
                                {
                                    array3 = null;
                                }
                                else
                                {
                                    Asn1DerObject asn1DerObject7 = asn1DerObject6.Objects[1];
                                    array3 = ((asn1DerObject7 != null) ? asn1DerObject7.Data : null);
                                }
                                byte[] array4 = array3;
                                if (array2 == null || array4 == null)
                                {
                                    goto IL_40F;
                                }
                                byte[] bytes3 = new TripleDes(array4, bytes, new byte[0], array2).Compute();
                                if (!Encoding.GetEncoding("ISO-8859-1").GetString(bytes3).StartsWith("password-check"))
                                {
                                    goto IL_40F;
                                }
                            }
                            else
                            {
                                if (!text.Contains("2A864886F70D01050D"))
                                {
                                    goto IL_40F;
                                }
                                Asn1DerObject asn1DerObject8 = asn1DerObject.Objects[0];
                                byte[] array5;
                                if (asn1DerObject8 == null)
                                {
                                    array5 = null;
                                }
                                else
                                {
                                    Asn1DerObject asn1DerObject9 = asn1DerObject8.Objects[0];
                                    if (asn1DerObject9 == null)
                                    {
                                        array5 = null;
                                    }
                                    else
                                    {
                                        Asn1DerObject asn1DerObject10 = asn1DerObject9.Objects[1];
                                        if (asn1DerObject10 == null)
                                        {
                                            array5 = null;
                                        }
                                        else
                                        {
                                            Asn1DerObject asn1DerObject11 = asn1DerObject10.Objects[0];
                                            if (asn1DerObject11 == null)
                                            {
                                                array5 = null;
                                            }
                                            else
                                            {
                                                Asn1DerObject asn1DerObject12 = asn1DerObject11.Objects[1];
                                                if (asn1DerObject12 == null)
                                                {
                                                    array5 = null;
                                                }
                                                else
                                                {
                                                    Asn1DerObject asn1DerObject13 = asn1DerObject12.Objects[0];
                                                    array5 = ((asn1DerObject13 != null) ? asn1DerObject13.Data : null);
                                                }
                                            }
                                        }
                                    }
                                }
                                byte[] array6 = array5;
                                Asn1DerObject asn1DerObject14 = asn1DerObject.Objects[0];
                                byte[] array7;
                                if (asn1DerObject14 == null)
                                {
                                    array7 = null;
                                }
                                else
                                {
                                    Asn1DerObject asn1DerObject15 = asn1DerObject14.Objects[0];
                                    if (asn1DerObject15 == null)
                                    {
                                        array7 = null;
                                    }
                                    else
                                    {
                                        Asn1DerObject asn1DerObject16 = asn1DerObject15.Objects[1];
                                        if (asn1DerObject16 == null)
                                        {
                                            array7 = null;
                                        }
                                        else
                                        {
                                            Asn1DerObject asn1DerObject17 = asn1DerObject16.Objects[2];
                                            if (asn1DerObject17 == null)
                                            {
                                                array7 = null;
                                            }
                                            else
                                            {
                                                Asn1DerObject asn1DerObject18 = asn1DerObject17.Objects[1];
                                                array7 = ((asn1DerObject18 != null) ? asn1DerObject18.Data : null);
                                            }
                                        }
                                    }
                                }
                                byte[] array8 = array7;
                                Asn1DerObject asn1DerObject19 = asn1DerObject.Objects[0];
                                byte[] array9;
                                if (asn1DerObject19 == null)
                                {
                                    array9 = null;
                                }
                                else
                                {
                                    Asn1DerObject asn1DerObject20 = asn1DerObject19.Objects[0];
                                    if (asn1DerObject20 == null)
                                    {
                                        array9 = null;
                                    }
                                    else
                                    {
                                        Asn1DerObject asn1DerObject21 = asn1DerObject20.Objects[1];
                                        if (asn1DerObject21 == null)
                                        {
                                            array9 = null;
                                        }
                                        else
                                        {
                                            Asn1DerObject asn1DerObject22 = asn1DerObject21.Objects[3];
                                            array9 = ((asn1DerObject22 != null) ? asn1DerObject22.Data : null);
                                        }
                                    }
                                }
                                byte[] array10 = array9;
                                if (array6 == null || array8 == null || array10 == null)
                                {
                                    goto IL_40F;
                                }
                                byte[] bytes4 = new PBE(array10, bytes, new byte[0], array6, array8).Compute();
                                if (!Encoding.GetEncoding("ISO-8859-1").GetString(bytes4).StartsWith("password-check"))
                                {
                                    goto IL_40F;
                                }
                            }
                            sqLite = SqLite.ReadTable(path, "nssPrivate");
                            if (sqLite != null)
                            {
                                int num = 0;
                                if (num < sqLite.GetRowCount())
                                {
                                    byte[] bytes5 = Encoding.UTF8.GetBytes(sqLite.GetValue(num, 6));
                                    Asn1DerObject asn1DerObject23 = asn1Der.Parse(bytes5);
                                    byte[] data = asn1DerObject23.Objects[0].Objects[0].Objects[1].Objects[0].Objects[1].Objects[0].Data;
                                    byte[] data2 = asn1DerObject23.Objects[0].Objects[0].Objects[1].Objects[2].Objects[1].Data;
                                    Array sourceArray = new PBE(asn1DerObject23.Objects[0].Objects[0].Objects[1].Objects[3].Data, bytes, new byte[0], data, data2).Compute();
                                    byte[] array11 = new byte[24];
                                    Array.Copy(sourceArray, array11, array11.Length);
                                    return array11;
                                }
                            }
                        }
                    }
                }
            IL_40F:;
            }
            return null;
        }

        // Token: 0x06000305 RID: 773 RVA: 0x00017F94 File Offset: 0x00016194
        public static byte[] Key3Database(string path)
        {
            byte[] array = File.ReadAllBytes(path);
            if (array == null)
            {
                return null;
            }
            Asn1Der asn1Der = new Asn1Der();
            BerkeleyDB berkeleyDB = new BerkeleyDB(array);
            string text = berkeleyDB.Keys.Where(delegate (KeyValuePair<string, string> p)
            {
                KeyValuePair<string, string> keyValuePair = p;
                return keyValuePair.Key.Equals("password-check");
            }).Select(delegate (KeyValuePair<string, string> p)
            {
                KeyValuePair<string, string> keyValuePair = p;
                return keyValuePair.Value;
            }).FirstOrDefault<string>();
            if (text == null)
            {
                return null;
            }
            text = text.Replace("-", null);
            int num = int.Parse(text.Substring(2, 2), NumberStyles.HexNumber) * 2;
            string hexString = text.Substring(6, num);
            int num2 = text.Length - (6 + num + 36);
            string hexString2 = text.Substring(6 + num + 4 + num2);
            string text2 = berkeleyDB.Keys.Where(delegate (KeyValuePair<string, string> p)
            {
                KeyValuePair<string, string> keyValuePair = p;
                return keyValuePair.Key.Equals("global-salt");
            }).Select(delegate (KeyValuePair<string, string> p)
            {
                KeyValuePair<string, string> keyValuePair = p;
                return keyValuePair.Value;
            }).FirstOrDefault<string>();
            if (text2 == null)
            {
                return null;
            }
            text2 = text2.Replace("-", null);
            TripleDes tripleDes = new TripleDes(NSSDumpMasterKey.HexToBytes(text2), Encoding.ASCII.GetBytes(""), NSSDumpMasterKey.HexToBytes(hexString));
            tripleDes.ComputeVoid();
            if (!TripleDes.DecryptStringDesCbc(tripleDes.Key, tripleDes.Vector, NSSDumpMasterKey.HexToBytes(hexString2)).StartsWith("password-check"))
            {
                return null;
            }
            string text3 = berkeleyDB.Keys.Where(delegate (KeyValuePair<string, string> p)
            {
                KeyValuePair<string, string> keyValuePair = p;
                if (!keyValuePair.Key.Equals("global-salt"))
                {
                    keyValuePair = p;
                    if (!keyValuePair.Key.Equals("Version"))
                    {
                        keyValuePair = p;
                        return !keyValuePair.Key.Equals("password-check");
                    }
                }
                return false;
            }).Select(delegate (KeyValuePair<string, string> p)
            {
                KeyValuePair<string, string> keyValuePair = p;
                return keyValuePair.Value;
            }).FirstOrDefault<string>();
            if (text3 == null)
            {
                return null;
            }
            text3 = text3.Replace("-", "");
            Asn1DerObject asn1DerObject = asn1Der.Parse(NSSDumpMasterKey.HexToBytes(text3));
            TripleDes tripleDes2 = new TripleDes(NSSDumpMasterKey.HexToBytes(text2), Encoding.ASCII.GetBytes(""), asn1DerObject.Objects[0].Objects[0].Objects[1].Objects[0].Data);
            tripleDes2.ComputeVoid();
            byte[] toParse = TripleDes.DecryptByteDesCbc(tripleDes2.Key, tripleDes2.Vector, asn1DerObject.Objects[0].Objects[1].Data);
            Asn1DerObject asn1DerObject2 = asn1Der.Parse(toParse);
            Asn1DerObject asn1DerObject3 = asn1Der.Parse(asn1DerObject2.Objects[0].Objects[2].Data);
            byte[] array2 = new byte[24];
            if (asn1DerObject3.Objects[0].Objects[3].Data.Length > 24)
            {
                Array.Copy(asn1DerObject3.Objects[0].Objects[3].Data, asn1DerObject3.Objects[0].Objects[3].Data.Length - 24, array2, 0, 24);
            }
            else
            {
                array2 = asn1DerObject3.Objects[0].Objects[3].Data;
            }
            return array2;
        }

        // Token: 0x06000306 RID: 774 RVA: 0x000182EC File Offset: 0x000164EC
        public static byte[] HexToBytes(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                return null;
            }
            byte[] array = new byte[hexString.Length / 2];
            for (int i = 0; i < array.Length; i++)
            {
                string s = hexString.Substring(i * 2, 2);
                array[i] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return array;
        }
    }
}
