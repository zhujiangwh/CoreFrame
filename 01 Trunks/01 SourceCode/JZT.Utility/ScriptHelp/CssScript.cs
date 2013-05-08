using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace JZT.Utility.ScriptHelp
{
    public class CssScript
    {
        public static Assembly GetAssembly(string code, string[] refAssembly)
        {
            System.Reflection.Assembly ass = CSScriptLibrary.CSScript.LoadCode(code, refAssembly);
            return ass;
        }
        public static T GetInstance<T>(string code, string[] refAssembly,string className)
        {
            Assembly ass = GetAssembly(code, refAssembly);
            return (T)ass.CreateInstance(className);
        }
        public static string Compile(string code, string[] refAssembly)
        {
            string errorMessage = string.Empty;
            try
            {
                CSScriptLibrary.CSScript.CompileCode(code, refAssembly);
            }
            catch(csscript.CompilerException ex)
            {
                errorMessage = ex.Message;
            }
            return errorMessage;
        }
    }
}
