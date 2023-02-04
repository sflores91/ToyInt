
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ToyInterpereter
{
    internal class ToyIntJsonParser
    {
        string jsonPath;


        public void ProcessFile(string filePath)
        {
            jsonPath = filePath;
            string jsonFile = File.ReadAllText(jsonPath);
            using var doc = JsonDocument.Parse(jsonFile);
            JsonElement root = doc.RootElement;
            var test = root.EnumerateObject();
            var props = test.GetEnumerator();
            foreach (var prop in test.GetEnumerator())
            {
                //Console.WriteLine(prop.Value.ValueKind.ToString());
                switch (prop.Value.ValueKind)
                {
                    case JsonValueKind.Number:
                        //prop.Value;
                        ToyIntVar.AddVar(prop.Name, prop.Value.ToString());
                        break;
                    case JsonValueKind.Array:
                        //ToyIntFuncBank.functionBankAdd()
                        ToyIntFunc func = new ToyIntFunc(prop.Name);
                        // Console.WriteLine(prop.Name);
                        var cmds = prop.Value;
                        var cm = cmds.EnumerateArray();
                        while (cm.MoveNext())
                        {
                            var c = cm.Current;
                            var a = c.EnumerateObject();
                            Dictionary<string, string> cArgs = new Dictionary<string, string>();
                            string cType = null; //clear command for next loop
                            string cOutput = null; // clear output var for next loop                       
                            while (a.MoveNext())
                            {
                                switch (a.Current.Name)
                                {
                                    case "cmd": //if command add to command var

                                        cType = a.Current.Value.ToString();
                                        break;
                                    case "id": //if output var add to id var
                                        cOutput = a.Current.Value.ToString();
                                        break;
                                    default: //if nethier its arguments for FSL lang add to args dictionary
                                        cArgs.Add(a.Current.Name.ToString(), a.Current.Value.ToString());
                                        break;

                                }
                                //debug
                                //Console.Write(a.Current.Name);
                                //Console.WriteLine(" : " + a.Current.Value);
                            }
                            //if output is null use correct ToyIntCmd init
                            if (cOutput is null)
                            {
                                // if(prop.Name.StartsWith("#"))
                                func.CommandAdd(cType, prop.Name, cArgs);
                            }
                            else
                            {
                                func.CommandAdd(cType, prop.Name, cOutput, cArgs);
                            }
                        }
                        //finally add the ToyIntFunc to the bank of functions
                        ToyIntFuncBank.FunctionBankAdd(prop.Name, func);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
