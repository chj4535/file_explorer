using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace socket_server
{
    class MoveFilesAndDirs
    {
        public byte[] MvoeItems(string targetPath, string dragStaticpath, string itemsLength,string item)
        {
            string[] items = item.Split('/');
            int count = 0;
            int successCount=0;
            int failCount = 0;
            string succesResult = "";
            string failResult = "";
            try
            {
                for(int i=0;i< Int32.Parse(itemsLength); i++)
                {
                    string itemType = items[count++];
                    string itemName = items[count++];
                    string movetargetPath = targetPath + '\\' + itemName;
                    string moveSourcePath = dragStaticpath + '\\' + itemName;
                    if (itemType.Equals("file"))
                    {
                        if(!Movefile(moveSourcePath, movetargetPath))
                        {
                            failCount++;
                            failResult += itemType + "/" + itemName + "/";
                        }
                        else
                        {
                            successCount++;
                            succesResult += itemType + "/" + itemName +"/";
                        }
                    }
                    else
                    {
                        if (!Movedir(moveSourcePath, movetargetPath))
                        {
                            failCount++;
                            failResult += itemType + "/" + itemName + "/";
                        }
                        else
                        {
                            successCount++;
                            succesResult += itemType + "/" + itemName + "/";
                        }
                    }
                }
                return Encoding.UTF8.GetBytes(successCount+"|"+succesResult+"|"+failCount+"|"+failResult+"|");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Encoding.UTF8.GetBytes("error|" + e.Message + "|");
            }
        }
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
