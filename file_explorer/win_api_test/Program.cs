using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace win_api_test
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            driveinfo driveInfo = new driveinfo();
            DriveInfo[] allDrives = driveInfo.get_drive_info();
            */

            /*
            string dirPath = @"Z:\study\Encryption_study";
            Getfiles getFiles = new Getfiles();
            string[] dirSublist = getFiles.Getfiles_directory(dirPath);
            */
            
            string sourceDirectory = @"Z:\study\test";
            string destinationDirectory = @"C:\";
            Movefiledir moveFileDir = new Movefiledir();
            if(moveFileDir.Movedir(sourceDirectory, destinationDirectory))
            {
                Console.WriteLine("이동 성공");
            }
            else
            {
                Console.WriteLine("이동 실패");
            }
            

            /*
            string sourceFile = @"Z:\study\test";
            string destinationFile = @"C:\";
            Movefiledir moveFileDir = new Movefiledir();
            if (moveFileDir.Movefile(sourceFile, destinationFile))
            {
                Console.WriteLine("이동 성공");
            }
            else
            {
                Console.WriteLine("이동 실패");
            }
            */
        }
    }
}
