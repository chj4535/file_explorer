using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace socket_server
{
    class Driveinfo
    {
        public byte[] GetDriveInfo()
        {
            //논리 드라이브 정보를 가져옵니다.
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            string allDriveInfotoString = allDrives.Length.ToString()+"/";
            foreach (DriveInfo d in allDrives)
            {
                allDriveInfotoString += d.Name+"/"; //드라이브 이름
                allDriveInfotoString += d.VolumeLabel + "/"; //드라이브 라벨
                allDriveInfotoString += d.DriveType + "/"; //드라이브 타입
                allDriveInfotoString += d.TotalSize.ToString() + "/";//드라이브 전체 크기
                allDriveInfotoString += d.TotalFreeSpace.ToString() + "/";//드라이브 여분 크기
            }
            Console.WriteLine(allDriveInfotoString);
            return Encoding.UTF8.GetBytes(allDriveInfotoString+'|');
            /*
            foreach (DriveInfo d in allDrives)
            {
                Console.WriteLine("Drive {0}", d.Name);
                Console.WriteLine("  Drive type: {0}", d.DriveType);
                if (d.IsReady == true)
                {
                    Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                    Console.WriteLine("  File system: {0}", d.DriveFormat);
                    Console.WriteLine(
                        "  Available space to current user:{0, 15} bytes",
                        d.AvailableFreeSpace);

                    Console.WriteLine(
                        "  Total available space:          {0, 15} bytes",
                        d.TotalFreeSpace);

                    Console.WriteLine(
                        "  Total size of drive:            {0, 15} bytes ",
                        d.TotalSize);
                }
            }
            */
        }
    }
}
