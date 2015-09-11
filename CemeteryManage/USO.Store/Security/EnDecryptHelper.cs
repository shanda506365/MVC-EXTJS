using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace USO.Store.Security
{
    public  class EnDecryptHelper
    {
        /*
         * DES使用例子
         *  byte[] dd = new byte[] { 10,127,1,77,2,110,6,99};
                string ddt = DESEncrypt(this.textBox1.Text, dd);
                this.textBox2.Text = ddt;
                 this.textBox3.Text =DESDecrypt(ddt, dd);
         */
        //// <summary>
        /// 进行DES加密。
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串。</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>以Base64格式返回的加密字符串。</returns>
        public static string DESEncrypt(string pToEncrypt, byte[] sKey)
        {
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
                des.Key = sKey;
                des.IV = sKey;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Convert.ToBase64String(ms.ToArray());
                ms.Close();
                return str;
            }
        }

        /**/
        /// <summary>
        /// 进行DES解密。
        /// </summary>
        /// <param name="pToDecrypt">要解密的以Base64</param>
        /// <param name="sKey">密钥，且必须为8位。</param>
        /// <returns>已解密的字符串。</returns>
        public static string DESDecrypt(string pToDecrypt, byte[] sKey)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = sKey;
                des.IV = sKey;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }


        /*
         MD5使用例子
         * MD5Encrypt(this.textBox1.Text);
         */
        /// <summary>
        /// 进行MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                pwd = pwd + s[i].ToString("x2");

            }
            return pwd;
        }

        /*
         * AES使用例子
         this.textBox2.Text = AESEncrypt(this.textBox1.Text);
            this.textBox3.Text = AESDecrypt(this.textBox2.Text);
         */
        /// <summary>
        /// 256位AES加密
        /// </summary>
        /// <param name="toEncrypt"></param>
        /// <returns></returns>
        public static string AESEncrypt(string toEncrypt)
        {
            // 256-AES key    
            string keyArray = "12345678901234567890123456789012";
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = UTF8Encoding.UTF8.GetBytes(EnDecryptHelper.MD5Encrypt(keyArray));
            rDel.IV = UTF8Encoding.UTF8.GetBytes(EnDecryptHelper.MD5Encrypt(keyArray).Substring(8, 16));
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 256位AES解密
        /// </summary>
        /// <param name="toDecrypt"></param>
        /// <returns></returns>
        public static string AESDecrypt(string toDecrypt)
        {
            // 256-AES key    
            string keyArray = "12345678901234567890123456789012";
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = UTF8Encoding.UTF8.GetBytes(EnDecryptHelper.MD5Encrypt(keyArray));
            rDel.IV = UTF8Encoding.UTF8.GetBytes(EnDecryptHelper.MD5Encrypt(keyArray).Substring(8, 16));
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <returns></returns>
        private  string GenerateRandomCode()
        {
            int number;
            char code;
            string checkCode = String.Empty;

            Random random = new Random();
            int length = random.Next(32, 32);

            for (int i = 0; i < length; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));

                checkCode += code.ToString();
            }

            return checkCode;
        }

        #region RSA
        /*
         RSA使用例子
         *  BigInteger ee = new BigInteger("17", 10);
            BigInteger dd = new BigInteger("5067044795589020437124571818135797313", 10);
            BigInteger nn = new BigInteger("17227952305002669504378944624689494731", 10);

            this.textBox2.Text = EncryptProcess(this.textBox1.Text, dd,nn);
            this.textBox3.Text = DecryptProcess(this.textBox2.Text,ee,nn);
         */
        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="source"></param>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private string EncryptString(string source, BigInteger d, BigInteger n)
        {
            int len = source.Length;
            int len1 = 0;
            int blockLen = 0;
            if ((len % 128) == 0)
                len1 = len / 128;
            else
                len1 = len / 128 + 1;
            string block = "";
            string temp = "";
            for (int i = 0; i < len1; i++)
            {
                if (len >= 128)
                    blockLen = 128;
                else
                    blockLen = len;
                block = source.Substring(i * 128, blockLen);
                byte[] oText = System.Text.Encoding.Default.GetBytes(block);
                BigInteger biText = new BigInteger(oText);
                BigInteger biEnText = biText.modPow(d, n);
                string temp1 = biEnText.ToHexString();
                temp += temp1;
                len -= blockLen;
            }
            return temp;
        }

        private string EncryptStringNew(string source, BigInteger d, BigInteger n)
        {
            int len = source.Length;
            int len1 = 0;
            int blockLen = 0;
            if ((len % 128) == 0)
                len1 = len / 128;
            else
                len1 = len / 128 + 1;
            string block = "";
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < len1; i++)
            {
                if (len >= 128)
                    blockLen = 128;
                else
                    blockLen = len;
                block = source.Substring(i * 128, blockLen);
                byte[] oText = System.Text.Encoding.Default.GetBytes(block);
                BigInteger biText = new BigInteger(oText);
                BigInteger biEnText = biText.modPow(d, n);
                string temp = biEnText.ToHexString();
                result.Append(temp).Append("@");
                len -= blockLen;
            }
            return result.ToString().TrimEnd('@');
        }
        /*  
       功能：用指定的公钥(n,e)
       RSA解密指定字符串
       source        */
        private string DecryptString(string source, BigInteger e, BigInteger n)
        {
            int len = source.Length;
            int len1 = 0;
            int blockLen = 0;
            if ((len % 256) == 0)
                len1 = len / 256;
            else
                len1 = len / 256 + 1;
            string block = "";
            string temp = "";
            for (int i = 0; i < len1; i++)
            {
                if (len >= 256)
                    blockLen = 256;
                else
                    blockLen = len;
                block = source.Substring(i * 256, blockLen);
                BigInteger biText = new BigInteger(block, 16);
                BigInteger biEnText = biText.modPow(e, n);
                string temp1 = System.Text.Encoding.Default.GetString(biEnText.getBytes());
                temp += temp1;
                len -= blockLen;
            }
            return temp;
        }

        private string DecryptStringNew(string source, BigInteger e, BigInteger n)
        {
            StringBuilder result = new StringBuilder();
            string[] strarr1 = source.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strarr1.Length; i++)
            {
                string block = strarr1[i];
                BigInteger biText = new BigInteger(block, 16);
                BigInteger biEnText = biText.modPow(e, n);
                string temp = System.Text.Encoding.Default.GetString(biEnText.getBytes());
                result.Append(temp);
            }
            return result.ToString();
        }


        /* 
         * RSA加密
            加密过程,其中d、n是
            RSACryptoServiceProvider生成的D、Modulus    
         */
        private string EncryptProcess(string source, string d, string n)
        {
            byte[] N = Convert.FromBase64String(n);
            byte[] D = Convert.FromBase64String(d);
            BigInteger biN = new BigInteger(N);
            BigInteger biD = new BigInteger(D);
            return EncryptString(source, biD, biN);
        }
        private string EncryptProcess(string source, BigInteger d, BigInteger n)
        {
            return EncryptStringNew(source, d, n);
        }
        /*  
         * RSA解密
            解密过程,其中e、n是
            RSACryptoServiceProvider生成的Exponent、Modulus   */
        private string DecryptProcess(string source, string e, string n)
        {
            byte[] N = Convert.FromBase64String(n);
            byte[] E = Convert.FromBase64String(e);
            BigInteger biN = new BigInteger(N);
            BigInteger biE = new BigInteger(E);
            return DecryptString(source, biE, biN);
        }

        private string DecryptProcess(string source, BigInteger e, BigInteger n)
        {
            return DecryptStringNew(source, e, n);
        }
        #endregion
    }
}