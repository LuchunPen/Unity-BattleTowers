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
        try {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(fullName, FileMode.OpenOrCreate, FileAccess.Write))) {

                //write sector size (2x2)
                int sX = map.SizeX / 2;
                int sY = map.SizeY / 2;

                bw.Write(sX);
                bw.Write(sY);
                
                //write sectors
                
                for (int x = 0; x < sX; x++) {
                    for (int y = 0; y < sY; y++) {

                        

                    }
                }
            }
        }
        catch(IOException ex) {
            return;
        }
    }

    public static StageMap Load(string path)
    {
        return null;
    }
}
