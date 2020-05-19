﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace UserDataPump.Framework
{
    public static class DirectoryService
    {
        public static void MoveJsonToDoneDirectory(string filename, string sourcePath, string destinationPath)
        {
            if(!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            File.Move(Path.Combine(sourcePath, filename), Path.Combine(destinationPath, filename), true);
        }
    }
}
