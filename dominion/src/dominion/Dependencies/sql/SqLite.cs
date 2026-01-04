using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominion.src.dominion.Dependencies.Sql
{
    public class SqLite
    {
        // Token: 0x0600029B RID: 667 RVA: 0x00013EAC File Offset: 0x000120AC
        public SqLite(string fileName)
        {
            this._fileBytes = File.ReadAllBytes(fileName);
            this._pageSize = this.ConvertToULong(16, 2);
            this._dbEncoding = this.ConvertToULong(56, 4);
            this.ReadMasterTable(100L);
        }

        // Token: 0x0600029C RID: 668 RVA: 0x00013F0C File Offset: 0x0001210C
        public SqLite(byte[] basedata)
        {
            this._fileBytes = basedata;
            this._pageSize = this.ConvertToULong(16, 2);
            this._dbEncoding = this.ConvertToULong(56, 4);
            this.ReadMasterTable(100L);
        }

        // Token: 0x0600029D RID: 669 RVA: 0x00013F68 File Offset: 0x00012168
        public string GetValue(int rowNum, int field)
        {
            string result;
            try
            {
                if (rowNum >= this._tableEntries.Length)
                {
                    result = null;
                }
                else
                {
                    result = ((field >= this._tableEntries[rowNum].Content.Length) ? null : this._tableEntries[rowNum].Content[field]);
                }
            }
            catch
            {
                result = "";
            }
            return result;
        }

        // Token: 0x0600029E RID: 670 RVA: 0x00013FD0 File Offset: 0x000121D0
        public int GetRowCount()
        {
            return this._tableEntries.Length;
        }

        // Token: 0x0600029F RID: 671 RVA: 0x00013FDC File Offset: 0x000121DC
        private bool ReadTableFromOffset(ulong offset)
        {
            bool result;
            try
            {
                byte b = this._fileBytes[(int)(checked((IntPtr)offset))];
                if (b != 5)
                {
                    if (b == 13)
                    {
                        uint num = (uint)(this.ConvertToULong((int)offset + 3, 2) - 1UL);
                        int num2 = 0;
                        if (this._tableEntries != null)
                        {
                            num2 = this._tableEntries.Length;
                            Array.Resize<SqLite.TableEntry>(ref this._tableEntries, this._tableEntries.Length + (int)num + 1);
                        }
                        else
                        {
                            this._tableEntries = new SqLite.TableEntry[num + 1U];
                        }
                        for (uint num3 = 0U; num3 <= num; num3 += 1U)
                        {
                            ulong num4 = this.ConvertToULong((int)offset + 8 + (int)(num3 * 2U), 2);
                            if (offset != 100UL)
                            {
                                num4 += offset;
                            }
                            int num5 = this.Gvl((int)num4);
                            this.Cvl((int)num4, num5);
                            int num6 = this.Gvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL));
                            this.Cvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL), num6);
                            ulong num7 = num4 + (ulong)((long)num6 - (long)num4 + 1L);
                            int num8 = this.Gvl((int)num7);
                            int num9 = num8;
                            long num10 = this.Cvl((int)num7, num8);
                            SqLite.RecordHeaderField[] array = null;
                            long num11 = (long)(num7 - (ulong)((long)num8) + 1UL);
                            int num12 = 0;
                            while (num11 < num10)
                            {
                                Array.Resize<SqLite.RecordHeaderField>(ref array, num12 + 1);
                                int num13 = num9 + 1;
                                num9 = this.Gvl(num13);
                                array[num12].Type = this.Cvl(num13, num9);
                                array[num12].Size = (long)((array[num12].Type <= 9L) ? ((ulong)this._sqlDataTypeSize[(int)(checked((IntPtr)array[num12].Type))]) : ((ulong)((!SqLite.IsOdd(array[num12].Type)) ? ((array[num12].Type - 12L) / 2L) : ((array[num12].Type - 13L) / 2L))));
                                num11 = num11 + (long)(num9 - num13) + 1L;
                                num12++;
                            }
                            if (array != null)
                            {
                                this._tableEntries[num2 + (int)num3].Content = new string[array.Length];
                                int num14 = 0;
                                for (int i = 0; i <= array.Length - 1; i++)
                                {
                                    if (array[i].Type > 9L)
                                    {
                                        if (!SqLite.IsOdd(array[i].Type))
                                        {
                                            long num15 = (long)(this._dbEncoding - 1UL);
                                            if (num15 <= 2L)
                                            {
                                                long num16 = num15;
                                                if (num16 <= 2L)
                                                {
                                                    switch ((uint)num16)
                                                    {
                                                        case 0U:
                                                            this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
                                                            break;
                                                        case 1U:
                                                            this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
                                                            break;
                                                        case 2U:
                                                            this._tableEntries[num2 + (int)num3].Content[i] = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
                                        }
                                    }
                                    else
                                    {
                                        this._tableEntries[num2 + (int)num3].Content[i] = Convert.ToString(this.ConvertToULong((int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size));
                                    }
                                    num14 += (int)array[i].Size;
                                }
                            }
                        }
                    }
                }
                else
                {
                    uint num17 = (uint)(this.ConvertToULong((int)(offset + 3UL), 2) - 1UL);
                    for (uint num18 = 0U; num18 <= num17; num18 += 1U)
                    {
                        uint num19 = (uint)this.ConvertToULong((int)offset + 12 + (int)(num18 * 2U), 2);
                        this.ReadTableFromOffset((this.ConvertToULong((int)(offset + (ulong)num19), 4) - 1UL) * this._pageSize);
                    }
                    this.ReadTableFromOffset((this.ConvertToULong((int)(offset + 8UL), 4) - 1UL) * this._pageSize);
                }
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        // Token: 0x060002A0 RID: 672 RVA: 0x00014464 File Offset: 0x00012664
        private void ReadMasterTable(long offset)
        {
            byte b;
            for (; ; )
            {
                b = this._fileBytes[(int)(checked((IntPtr)offset))];
                if (b != 5)
                {
                    break;
                }
                uint num = (uint)(this.ConvertToULong((int)offset + 3, 2) - 1UL);
                for (int i = 0; i <= (int)num; i++)
                {
                    uint num2 = (uint)this.ConvertToULong((int)offset + 12 + i * 2, 2);
                    if (offset == 100L)
                    {
                        this.ReadMasterTable((long)((this.ConvertToULong((int)num2, 4) - 1UL) * this._pageSize));
                    }
                    else
                    {
                        this.ReadMasterTable((long)((this.ConvertToULong((int)(offset + (long)((ulong)num2)), 4) - 1UL) * this._pageSize));
                    }
                }
                offset = (long)((this.ConvertToULong((int)offset + 8, 4) - 1UL) * this._pageSize);
            }
            if (b != 13)
            {
                return;
            }
            ulong num3 = this.ConvertToULong((int)offset + 3, 2) - 1UL;
            int num4 = 0;
            if (this._masterTableEntries != null)
            {
                num4 = this._masterTableEntries.Length;
                Array.Resize<SqLite.SqliteMasterEntry>(ref this._masterTableEntries, this._masterTableEntries.Length + (int)num3 + 1);
            }
            else
            {
                this._masterTableEntries = new SqLite.SqliteMasterEntry[num3 + 1UL];
            }
            for (ulong num5 = 0UL; num5 <= num3; num5 += 1UL)
            {
                ulong num6 = this.ConvertToULong((int)offset + 8 + (int)num5 * 2, 2);
                if (offset != 100L)
                {
                    num6 += (ulong)offset;
                }
                int num7 = this.Gvl((int)num6);
                this.Cvl((int)num6, num7);
                int num8 = this.Gvl((int)(num6 + (ulong)((long)num7 - (long)num6) + 1UL));
                this.Cvl((int)(num6 + (ulong)((long)num7 - (long)num6) + 1UL), num8);
                ulong num9 = num6 + (ulong)((long)num8 - (long)num6 + 1L);
                int num10 = this.Gvl((int)num9);
                int num11 = num10;
                long num12 = this.Cvl((int)num9, num10);
                long[] array = new long[5];
                for (int j = 0; j <= 4; j++)
                {
                    int startIdx = num11 + 1;
                    num11 = this.Gvl(startIdx);
                    array[j] = this.Cvl(startIdx, num11);
                    array[j] = (long)((array[j] <= 9L) ? ((ulong)this._sqlDataTypeSize[(int)(checked((IntPtr)array[j]))]) : ((ulong)((!SqLite.IsOdd(array[j])) ? ((array[j] - 12L) / 2L) : ((array[j] - 13L) / 2L))));
                }
                if (this._dbEncoding == 1UL || this._dbEncoding == 2UL)
                {
                    long num13 = (long)(this._dbEncoding - 1UL);
                    if (num13 <= 2L)
                    {
                        long num14 = num13;
                        if (num14 <= 2L)
                        {
                            switch ((uint)num14)
                            {
                                case 0U:
                                    this._masterTableEntries[num4 + (int)num5].ItemName = Encoding.Default.GetString(this._fileBytes, (int)(num9 + (ulong)num12 + (ulong)array[0]), (int)array[1]);
                                    break;
                                case 1U:
                                    this._masterTableEntries[num4 + (int)num5].ItemName = Encoding.Unicode.GetString(this._fileBytes, (int)(num9 + (ulong)num12 + (ulong)array[0]), (int)array[1]);
                                    break;
                                case 2U:
                                    this._masterTableEntries[num4 + (int)num5].ItemName = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num9 + (ulong)num12 + (ulong)array[0]), (int)array[1]);
                                    break;
                            }
                        }
                    }
                }
                this._masterTableEntries[num4 + (int)num5].RootNum = (long)this.ConvertToULong((int)(num9 + (ulong)num12 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2]), (int)array[3]);
                long num15 = (long)(this._dbEncoding - 1UL);
                if (num15 <= 2L)
                {
                    long num16 = num15;
                    if (num16 <= 2L)
                    {
                        switch ((uint)num16)
                        {
                            case 0U:
                                this._masterTableEntries[num4 + (int)num5].SqlStatement = Encoding.Default.GetString(this._fileBytes, (int)(num9 + (ulong)num12 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
                                break;
                            case 1U:
                                this._masterTableEntries[num4 + (int)num5].SqlStatement = Encoding.Unicode.GetString(this._fileBytes, (int)(num9 + (ulong)num12 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
                                break;
                            case 2U:
                                this._masterTableEntries[num4 + (int)num5].SqlStatement = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num9 + (ulong)num12 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
                                break;
                        }
                    }
                }
            }
        }

        // Token: 0x060002A1 RID: 673 RVA: 0x000148BC File Offset: 0x00012ABC
        public bool ReadTable(string tableName)
        {
            int num = -1;
            for (int i = 0; i <= this._masterTableEntries.Length; i++)
            {
                if (string.Compare(this._masterTableEntries[i].ItemName.ToLower(), tableName.ToLower(), StringComparison.Ordinal) == 0)
                {
                    num = i;
                    break;
                }
            }
            if (num == -1)
            {
                return false;
            }
            string[] array = this._masterTableEntries[num].SqlStatement.Substring(this._masterTableEntries[num].SqlStatement.IndexOf("(", StringComparison.Ordinal) + 1).Split(new char[]
            {
                ','
            });
            for (int j = 0; j <= array.Length - 1; j++)
            {
                array[j] = array[j].TrimStart(Array.Empty<char>());
                int num2 = array[j].IndexOf(' ');
                if (num2 > 0)
                {
                    array[j] = array[j].Substring(0, num2);
                }
                if (array[j].IndexOf("UNIQUE", StringComparison.Ordinal) != 0)
                {
                    Array.Resize<string>(ref this._fieldNames, j + 1);
                    this._fieldNames[j] = array[j];
                }
            }
            return this.ReadTableFromOffset((ulong)((this._masterTableEntries[num].RootNum - 1L) * (long)this._pageSize));
        }

        // Token: 0x060002A2 RID: 674 RVA: 0x000149DC File Offset: 0x00012BDC
        private ulong ConvertToULong(int startIndex, int size)
        {
            ulong result;
            try
            {
                if (size > 8 || size == 0)
                {
                    result = 0UL;
                }
                else
                {
                    ulong num = 0UL;
                    for (int i = 0; i <= size - 1; i++)
                    {
                        num = (num << 8 | (ulong)this._fileBytes[startIndex + i]);
                    }
                    result = num;
                }
            }
            catch
            {
                result = 0UL;
            }
            return result;
        }

        // Token: 0x060002A3 RID: 675 RVA: 0x00014A34 File Offset: 0x00012C34
        private int Gvl(int startIdx)
        {
            int result;
            try
            {
                if (startIdx > this._fileBytes.Length)
                {
                    result = 0;
                }
                else
                {
                    for (int i = startIdx; i <= startIdx + 8; i++)
                    {
                        if (i > this._fileBytes.Length - 1)
                        {
                            return 0;
                        }
                        if ((this._fileBytes[i] & 128) != 128)
                        {
                            return i;
                        }
                    }
                    result = startIdx + 8;
                }
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        // Token: 0x060002A4 RID: 676 RVA: 0x00014AA4 File Offset: 0x00012CA4
        private long Cvl(int startIdx, int endIdx)
        {
            long result;
            try
            {
                endIdx++;
                byte[] array = new byte[8];
                int num = endIdx - startIdx;
                bool flag = false;
                if (num == 0 || num > 9)
                {
                    result = 0L;
                }
                else if (num != 1)
                {
                    if (num == 9)
                    {
                        flag = true;
                    }
                    int num2 = 1;
                    int num3 = 7;
                    int num4 = 0;
                    if (flag)
                    {
                        array[0] = this._fileBytes[endIdx - 1];
                        endIdx--;
                        num4 = 1;
                    }
                    for (int i = endIdx - 1; i >= startIdx; i += -1)
                    {
                        if (i - 1 >= startIdx)
                        {
                            array[num4] = (byte)((this._fileBytes[i] >> num2 - 1 & 255 >> num2) | (int)this._fileBytes[i - 1] << num3);
                            num2++;
                            num4++;
                            num3--;
                        }
                        else if (!flag)
                        {
                            array[num4] = (byte)(this._fileBytes[i] >> num2 - 1 & 255 >> num2);
                        }
                    }
                    result = BitConverter.ToInt64(array, 0);
                }
                else
                {
                    array[0] = (byte)(this._fileBytes[startIdx] & 127);
                    result = BitConverter.ToInt64(array, 0);
                }
            }
            catch
            {
                result = 0L;
            }
            return result;
        }

        // Token: 0x060002A5 RID: 677 RVA: 0x00014BC4 File Offset: 0x00012DC4
        private static bool IsOdd(long value)
        {
            return (value & 1L) == 1L;
        }

        // Token: 0x060002A6 RID: 678 RVA: 0x00014BD0 File Offset: 0x00012DD0
        public static SqLite ReadTable(string database, string table)
        {
            SqLite result;
            try
            {
                string text = Path.GetTempFileName() + ".tmpdb";
                File.Copy(database, text);
                SqLite sqLite = new SqLite(text);
                sqLite.ReadTable(table);
                File.Delete(text);
                result = ((sqLite.GetRowCount() == 65536) ? null : sqLite);
            }
            catch
            {
                result = null;
            }
            return result;
        }

        // Token: 0x04000194 RID: 404
        private readonly ulong _dbEncoding;

        // Token: 0x04000195 RID: 405
        private readonly byte[] _fileBytes;

        // Token: 0x04000196 RID: 406
        private readonly ulong _pageSize;

        // Token: 0x04000197 RID: 407
        private readonly byte[] _sqlDataTypeSize = new byte[]
        {
            0,
            1,
            2,
            3,
            4,
            6,
            8,
            8,
            0,
            0
        };

        // Token: 0x04000198 RID: 408
        private string[] _fieldNames;

        // Token: 0x04000199 RID: 409
        private SqLite.SqliteMasterEntry[] _masterTableEntries;

        // Token: 0x0400019A RID: 410
        private SqLite.TableEntry[] _tableEntries;

        // Token: 0x020000CB RID: 203
        private struct RecordHeaderField
        {
            // Token: 0x0400019B RID: 411
            public long Size;

            // Token: 0x0400019C RID: 412
            public long Type;
        }

        // Token: 0x020000CC RID: 204
        private struct TableEntry
        {
            // Token: 0x0400019D RID: 413
            public string[] Content;
        }

        // Token: 0x020000CD RID: 205
        private struct SqliteMasterEntry
        {
            // Token: 0x0400019E RID: 414
            public string ItemName;

            // Token: 0x0400019F RID: 415
            public long RootNum;

            // Token: 0x040001A0 RID: 416
            public string SqlStatement;
        }
    }
}
