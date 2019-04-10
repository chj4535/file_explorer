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

            
            string dirPath = @"Z:\study\";
            Getfiles getFiles = new Getfiles();
            string[] dirSublist = getFiles.Getfiles_directory(dirPath);
            

            /*
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
            */

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

            /*
            Parent check = new Parent();
            Child cd = new Child(20);
            cd.DisplayValue();
            Child cd2 = new Child(20);
            cd2.DisplayValue();
            check.setnum(30);
            check.setnum(30);
            cd2.DisplayValue();
            cd.handler*/
        }
    }

    class Parent2
    {
        static int num2;
        static Parent2()
        {
            Console.WriteLine("부모 클래스의 생성자가 호출되었습니다.");
        }
        public void setnum2(int num)
        {
            Parent2.num2 = num;
        }
        public int getnum2()
        {
            return Parent2.num2;
        }
    }
    class Parent
    {
        public static Handler handler = new Handler();
        public static int num3;
        static int num;
        static Parent()
        {
            Console.WriteLine("부모 클래스의 생성자가 호출되었습니다.");
        }
        public void setnum(int num)
        {
            Parent.num = num;
        }
        public int getnum()
        {
            return Parent.num;
        }
    }
    class Child : Parent
    {
        public Child(int num)
        {
            this.setnum(num);
            Console.WriteLine("자식 클래스의 생성자가 호출되었습니다.");
        }
        public void DisplayValue()
        {
            Console.WriteLine("num의 값은 {0} 입니다.", getnum());
        }
    }
    class Handler
    {
        public void hello()
        {
            Console.WriteLine("hello");
        }
    }
}
