using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.IO;

namespace socket_server
{
    class GetDirSubItems
    {
        public byte[] GetFilesDirs(string targetDirectory)
        {
            try
            {
                string[] files = Directory.GetFiles(targetDirectory);
                string[] dirs = Directory.GetDirectories(targetDirectory);
                string filesInfo = GetFeilsInfo(files);
                string dirsInfo = GetDirsInfo(dirs);
                return Encoding.UTF8.GetBytes(filesInfo + dirsInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Encoding.UTF8.GetBytes("error|"+e.Message+"|");
            }
        }
        public static string GetFeilsInfo(string[] files)
        {
            /*
            List<int> list = new List<int>();
            list.AddRange(x);
            list.AddRange(y);
            int[] z = list.ToArray();
            */
            byte[] filesInfo=null;
            string allFilesinfotoString = files.Length.ToString()+"/";
            foreach (string filePath in files)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                //Icon iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(fileInfo.FullName);

                //allFilesinfotoString += "/" + fileInfo.Name + "/";
                allFilesinfotoString += fileInfo.FullName + "/";
                allFilesinfotoString += fileInfo.Name + "/";
                allFilesinfotoString += fileInfo.Extension + "/";
                allFilesinfotoString += fileInfo.LastWriteTime.ToString() + "/";
                allFilesinfotoString += fileInfo.Length.ToString() + "/";

                //byte[] iconTobyte = GetBytes(iconForFile);
                //byte[] fileInfotobyte = Encoding.UTF8.GetBytes(allFilesinfotoString);
               // filesInfo = ByteArrayConcat(filesInfo, iconTobyte);
                //filesInfo = ByteArrayConcat(filesInfo, fileInfotobyte);
            }
            //filesInfo = ByteArrayConcat(filesInfo, Encoding.UTF8.GetBytes("|"));
            //return filesInfo;
            return allFilesinfotoString + "|";
        }

        public static string GetDirsInfo(string[] dirs)
        {
            string alldirsinfotoString = dirs.Length.ToString()+"/";
            foreach (string dirPath in dirs)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                alldirsinfotoString += dirInfo.FullName + "/";
                alldirsinfotoString += dirInfo.Name + "/";
                alldirsinfotoString += dirInfo.Attributes + "/";
                alldirsinfotoString += dirInfo.LastWriteTime.ToString() + "/";
            }
            Console.WriteLine(alldirsinfotoString);
            return alldirsinfotoString + "|";
        }
        public static byte[] GetBytes(Icon icon)
        {
            MemoryStream ms = new MemoryStream();
            icon.Save(ms);
            return ms.ToArray();
        }

        public static byte[] ByteArrayConcat(byte[] x, byte[] y)
        {
            int xLength = 0;
            int yLength = 0;
            if (x != null) { xLength = x.Length; }
            if (y != null) { yLength = y.Length; }
            byte[] concatArray = new byte[xLength + yLength];
            if (x != null) { x.CopyTo(concatArray, 0); }
            if (y != null) { y.CopyTo(concatArray, xLength); }
            return concatArray;
        }
    }
}
