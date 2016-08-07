
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    public class RandomCode
    {
       private static Random rand = new Random();

        /// <summary>
        /// 生成注册手机验证码
        /// </summary>
        /// <returns></returns>
       public static string CreatPhoneRegCode()
       {
           return rand.Next(100000).ToString();
       }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>9位兑换码格式为：898iyj009</returns>
       public string CreateCode()
       {
           return Number(3) + GenerateRandom(3) + Number(3);
       }
        /// <summary>
        /// 抽奖码
        /// </summary>
        /// <returns></returns>
       public string CreateLottoryCode()
       {
           return Number(3) + Number(3) + GenerateRandom(2);
       }
        /// <summary>
        /// 随机N位数字
        /// </summary>
        /// <returns></returns>
       public string Number(int n)
        {
            string max = "9";
            string min = "0";
            for (int i = 1; i < n; i++)
            {
                max += "9";
                min += "0";
            }
            return rand.Next(0, Convert.ToInt32(max)).ToString(min);
        }

        private string GenerateRandom(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(49);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(49)]);
            }
            return newRandom.ToString();
        }
        private static char[] constant =   
      {   
        'a','b','c','d','e','f','g','h','i','j','k','m','n','o','p','q','r','s','t','u','v','w','x','y','z',   
        'A','B','C','D','E','F','G','H','J','K','L','M','N','P','Q','R','S','T','U','V','W','X','Y','Z'   
      };


        // 随机串组成数据
        private static char[] upperConstants =   
        {   
            '2','3','4','5','6','7','8','9',  
            'A','B','C','D','E','F','G','H','J','K','L','M','N','P','Q','R','S','T','U','V','W','X','Y','Z'   
        };
        
        /// <summary>
        /// 生成最大32位字符串
        /// </summary>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string GenerateRandomStr(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(32);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(upperConstants[rd.Next(32)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// N位随机
        /// </summary>
        /// <param name="CodeCount"></param>
        /// <returns></returns>
        public static string GetRandomCode(int CodeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,M,N,P,Q,R,S,T,U,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;

            //Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(33);

                while (temp == t)
                {
                    t = rand.Next(33);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }

            return RandomCode;
        }
    }
}
