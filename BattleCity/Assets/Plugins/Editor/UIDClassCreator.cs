using UnityEngine;
using UnityEditor;
using System;
using System.Runtime.Serialization;
using System.Globalization;

public class UIDClassCreator: UnityEditor.AssetModificationProcessor
{
    #region UIDgenerator
    private struct UIDgen
    {
        private static object newLocker = new object();
        private static System.Random random = new System.Random();
        private static byte[] firstChar = { 160, 176, 192, 208, 224, 240 };
        private static uint tick;
        public static readonly UIDgen EmptyUid = 0;

        public long Data;

        #region ----- Date Time -----

        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;
        private const long TicksPerMinute = TicksPerSecond * 60;
        private const long TicksPerHour = TicksPerMinute * 60;
        private const long TicksPerDay = TicksPerHour * 24;
        private const int DaysPerYear = 365;
        private const int DaysPer4Years = DaysPerYear * 4 + 1;       // 1461
        private const int DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524
        private const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097    
        private static readonly int[] DaysToMonth365 = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334, 365 };
        private static readonly int[] DaysToMonth366 = { 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335, 366 };

        #endregion

        public static UIDgen NewUid
        {
            get
            {
                lock (newLocker)
                {
                    CALC:
                    long ticks = DateTime.Now.Ticks;
                    tick++; // накопительный тик

                    int n = (int)(ticks / TicksPerDay);
                    int y400 = n / DaysPer400Years;
                    n -= y400 * DaysPer400Years;
                    int y100 = n / DaysPer100Years;
                    if (y100 == 4) y100 = 3;
                    n -= y100 * DaysPer100Years;
                    int y4 = n / DaysPer4Years;
                    n -= y4 * DaysPer4Years;
                    int y1 = n / DaysPerYear;
                    if (y1 == 4) y1 = 3;
                    int year = y400 * 400 + y100 * 100 + y4 * 4 + y1 + 1;
                    n -= y1 * DaysPerYear;
                    bool leapYear = y1 == 3 && (y4 != 24 || y100 == 3);
                    int m = n >> 5 + 1;
                    if (leapYear) { while (n >= DaysToMonth366[m]) m++; }
                    else { while (n >= DaysToMonth365[m]) m++; }

                    uint l = (uint)((year - 2010) * 31536000 + m * 2592000 + (n + 1) * 86400 + (int)((ticks / TicksPerHour) % 24) * 3600 + (int)((ticks / TicksPerMinute) % 60) * 60 + (int)((ticks / TicksPerSecond) % 60));
                    long data = (long)tick;
                    if (tick < 256)
                    {
                        data |= (long)random.Next(256) << 8;
                        data |= (long)random.Next(256) << 16;
                        data |= (long)random.Next(256) << 24;
                    }
                    else if (tick < 65536)
                    {
                        data |= (long)random.Next(256) << 16;
                        data |= (long)random.Next(256) << 24;
                    }
                    else if (tick < 16777216)
                    {
                        data |= (long)random.Next(256) << 24;
                    }
                    byte b = (byte)(l >> 24);
                    b = (byte)((b & 15) | firstChar[random.Next(6)]);
                    data |= (long)(16777215 & l) << 32 | (long)b << 56;

                    if (data == 0) { goto CALC; }

                    return new UIDgen(data);
                }
            }
        }

        public UIDgen(SerializationInfo si, StreamingContext ctx) { Data = (long)si.GetValue("uid", typeof(long)); }
        public UIDgen(long n) { Data = n; }

        public override int GetHashCode() { return ((int)Data) ^ (int)(Data >> 32); }
        public override bool Equals(object obj) { if (obj is UIDgen) return Data == ((UIDgen)obj).Data; return false; }
        public bool Equals(UIDgen uid) { return Data == uid.Data; }

        public static implicit operator string (UIDgen value) { return value.Data.ToString("X").ToUpper(); }
        public static implicit operator UIDgen(string value) { return LoadFromString(value); }
        public static implicit operator UIDgen(long value) { return new UIDgen(value); }
        public static bool operator !=(UIDgen uid1, UIDgen uid2) { return uid1.Data != uid2.Data; }
        public static bool operator ==(UIDgen uid1, UIDgen uid2) { return uid1.Data == uid2.Data; }

        public static UIDgen LoadFromString(string s)
        {
            UIDgen u = EmptyUid;
            if (string.IsNullOrEmpty(s) || s == "0" || s.Length < 16) return EmptyUid;
            if (!System.Text.RegularExpressions.Regex.IsMatch(s, @"\A\b[0-9a-fA-F]+\b\Z")) return EmptyUid;
            if (s.Length == 16) long.TryParse(s, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out u.Data);
            return u;
        }

        public override string ToString()
        {
            return Data.ToString("X").ToUpper();
        }
    }
    #endregion UIDgenerator

    private static readonly string componentstring =
    @"private static readonly Uid64 UNIQ = " + "\""+ UIDgen.NewUid.ToString() +"\""+ @";
    private static readonly long uniqueID = UID.LoadFromString(stringUID).Data;
    public virtual long UniqueID { get {return uniqueID; } }";

    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");
        if (index == -1) return;
        string file = path.Substring(index);
        if (file != ".cs" && file != ".js" && file != ".boo") return;
        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        file = System.IO.File.ReadAllText(path);

        file = file.Replace("#CREATIONDATE#", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
        file = file.Replace("#UIDGEN#", "public static readonly Uid64 UNIQ = " + "\"" + UIDgen.NewUid.ToString() + "\";");
        file = file.Replace("#UCOMPONENT#", componentstring);

        System.IO.File.WriteAllText(path, file);
        Debug.Log("File created " + path);
        AssetDatabase.Refresh();
    }
}