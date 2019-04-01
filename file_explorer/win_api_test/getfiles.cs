using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace win_api_test
{
    class Getfiles
    {
        
        public string[] Getfiles_directory(string targetDirectory)
        {
            //특정 폴더의 파일,폴더 리스트(이름,타입, 수정날짜, 크기 포함)출력
            string[] files = Directory.GetFiles(targetDirectory);
            string[] dirs = Directory.GetDirectories(targetDirectory);
            foreach (string fileName in files)
                Writefile(fileName);
            foreach (string dirsName in dirs)
                Writedir(dirsName);
            string[] dirSublist = new string[files.Length + dirs.Length];
            Array.Copy(files, dirSublist, files.Length);
            Array.Copy(dirs, 0, dirSublist, files.Length, dirs.Length);
            return dirSublist;
        }

        public static void Writefile(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            //Console.WriteLine("Processed file '{0}'.", path);
            Console.WriteLine("파일명 : {0} / 타입 :  {1} / 수정한 날짜 :  {2} / 크기 : {3}", fileInfo.Name, fileInfo.Extension,fileInfo.LastWriteTime, fileInfo.Length);
        }

        public static void Writedir(string path)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            //Console.WriteLine("Processed directory '{0}'.", path);
            Console.WriteLine("폴더명 : {0} / 타입 :  {1} / 수정한 날짜 :  {2}", dirInfo.Name, dirInfo.Extension, dirInfo.LastWriteTime);
        }
    }
}
