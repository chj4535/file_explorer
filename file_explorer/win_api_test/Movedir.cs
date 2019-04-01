using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace win_api_test
{
    class Movefiledir //파일 및 폴더를 이동합니다.
    {
        public bool Movedir(string sourceDir, string destinationDir)
        {
            try
            {
                Directory.Move(sourceDir, destinationDir);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool Movefile(string sourceFile, string destinationFile)
        {
            try
            {
                File.Move(sourceFile, destinationFile);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }
    }
}
