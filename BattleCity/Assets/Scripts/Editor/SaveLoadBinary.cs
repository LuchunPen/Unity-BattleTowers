/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 08/10/2016 10:14
*/

using System;
using System.IO;

public static class SaveLoadBinary
{
    public static readonly string exp = ".bcm";

    public static void Save(StageMap map, string path, string name)
    {
        if (map == null || map.SizeX % 2 != 0 || map.SizeY % 2 != 0) {
            throw new ArgumentException("Map is null or have incorrect size");
        }

        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(name)) {
            throw new ArgumentNullException("Directory or file name is null or empty");
        }

        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        string fullName = path + "/" + name + exp;
        Save(map, fullName);
    }

    public static void Save(StageMap map, string fullName)
    {
        try {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(fullName, FileMode.OpenOrCreate, FileAccess.Write))) {

                //write sector size (2x2)
                int mX = map.SizeX / 2;
                int mY = map.SizeY / 2;

                bw.Write(mX);
                bw.Write(mY);
                bw.Write(map.MapName);

                //write sectors
                MapObject[] mos = new MapObject[4];
                for (int x = 0; x < mX; x++) {
                    for (int y = 0; y < mY; y++) {

                        int sX = x * 2;
                        int sY = y * 2;

                        mos[0] = map[sX, sY].RegisterObject;
                        mos[1] = map[sX + 1, sY].RegisterObject;
                        mos[2] = map[sX, sY + 1].RegisterObject;
                        mos[3] = map[sX + 1, sY + 1].RegisterObject;

                        if (mos[0] == null && mos[1] == null && mos[2] == null && mos[3] == null) {
                            bw.Write(0); bw.Write(0);
                        }
                        else if (mos[0] != null && mos[0].MapSize == MapSize.x2) {
                            bw.Write((int)mos[0].MOType); bw.Write((int)mos[0].Direction);
                        }
                        else {
                            int tileset = 0; int mosType = 0;
                            if (mos[0] != null) { tileset += 1; mosType = (int)mos[0].MOType; }
                            if (mos[1] != null) { tileset += 2; mosType = (int)mos[1].MOType; }
                            if (mos[2] != null) { tileset += 4; mosType = (int)mos[2].MOType; }
                            if (mos[3] != null) { tileset += 8; mosType = (int)mos[3].MOType; }

                            if (mosType != 0 && tileset > 0) {
                                bw.Write(mosType); bw.Write(tileset);
                            }
                            else { bw.Write(0); bw.Write(0); }
                        }
                    }
                }
            }
        }
        catch (IOException ex) { return; }
    }

    public static SectorMap Load(string fullName)
    {
        if (string.IsNullOrEmpty(fullName)) { throw new ArgumentNullException("file name is null or empty"); }
        if (!File.Exists(fullName)) { throw new IOException("This file is not exist"); }

        try {
            using (BinaryReader br = new BinaryReader(File.OpenRead(fullName))) {
                int mx = br.ReadInt32();
                int my = br.ReadInt32();
                string mapName = br.ReadString();

                SectorMap map = new SectorMap(mx, my, mapName);
                for (int x = 0; x < mx; x++) {
                    for (int y = 0; y < my; y++) {
                        int type = br.ReadInt32();
                        int tileDirect = br.ReadInt32();

                        MapObjectSector mos = new MapObjectSector(type, tileDirect);
                        map[x, y] = mos;
                    }
                }
                return map;
            }
        }
        catch(IOException ex) { }
        return null;
    }
}
