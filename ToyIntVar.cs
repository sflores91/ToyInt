namespace ToyInterpereter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

static class ToyIntVar
{
    static Dictionary<string, string> varBank = new Dictionary<string, string>();

    public static void AddVar(string varName, string varValue) //Create and update
    {
        if (varBank.ContainsKey(varName))
        {
            UpdateVar(varName, varValue);
            return;
        }
        varBank.Add(varName, varValue);
    }
    public static string ReturnVar(string varName) //Get from other class
    {
        string var = varName.Replace("#", "");
        string ret;
        return varBank.TryGetValue(var, out ret) ? ret : "undefined";
    }
    public static void RemoveVar(string varName) //delete
    {
        varBank.Remove(varName);
    }
    public static void UpdateVar(string varName, string varVal)
    {
        varBank[varName] = varVal;
    }
}

