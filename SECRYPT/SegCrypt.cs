using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace SECRYPT
{
    class FunctionLoader
    {
        [DllImport("Kernel32.dll")]
        private static extern IntPtr LoadLibrary(string path);

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        public static Delegate LoadFunction<T>(string dllPath, string functionName)
        {
            var hModule = LoadLibrary(dllPath);
            var functionAddress = GetProcAddress(hModule, functionName);
            return Marshal.GetDelegateForFunctionPointer(functionAddress, typeof(T));
        }
    }
    public class SegCryptV2
    {
        public SegCryptV2(string _semilla)
        {
            if (string.IsNullOrEmpty(_semilla))
                _semilla = "SegCrypt64";
            string file = $"C:\\Windows\\SysWOW64\\{_semilla}.dll";
            if (!File.Exists($"C:\\Windows\\SysWOW64\\{_semilla}.dll"))
                throw new Exception($"LA SEMILLA {_semilla} NO EXISTE EN EL SERVIDOR.");
            EncryptDecryptReader = (EncryptDecryptDelegate)
                FunctionLoader.LoadFunction<EncryptDecryptDelegate>(
                    file, "EncryptDecrypt");
        }
        private delegate bool EncryptDecryptDelegate([MarshalAs(UnmanagedType.Bool)] bool fEncrypt, string lpszInBuffer, StringBuilder sOut, [MarshalAs(UnmanagedType.I4)] ref int dsize);
        static private EncryptDecryptDelegate? EncryptDecryptReader;

        public string FunctionEncryptDecrypt(bool encrypt, string text)
        {
            string res = string.Empty;
            if (string.IsNullOrWhiteSpace(text))
                return res;
            try
            {
                int nSize = text.Length * 2 + 1;
                bool bRet;
                StringBuilder outString = new StringBuilder(nSize);
                bRet = EncryptDecryptReader(encrypt, text, outString, ref nSize);
                res = outString.ToString();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en EncryptDecrypt, " + ex.Message);
            }
        }
    }

    [SuppressUnmanagedCodeSecurity]
    internal static class SegCrypt
    {
        const string str = @"SegCryptBancSeg.dll";
        [DllImport(str,
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Ansi,
            ExactSpelling = true,
            SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EncryptDecrypt([MarshalAs(UnmanagedType.Bool)] bool fEncrypt, string lpszInBuffer, StringBuilder sOut, [MarshalAs(UnmanagedType.I4)] ref int dsize);

        public static string EncryptDecrypt(bool encrypt, string text)
        {
            string process = Environment.Is64BitProcess ? "64Bits" : "32Bits";
            string res = string.Empty;
            if (string.IsNullOrWhiteSpace(text))
                return res;
            try
            {
                int nSize = text.Length * 2 + 1;
                bool bRet;
                StringBuilder outString = new StringBuilder(nSize);
                bRet = EncryptDecrypt(encrypt, text, outString, ref nSize);
                res = outString.ToString();
                return res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en EncryptDecrypt, " + ex.Message + " Proceso: " + process);
            }
        }

    }
}
