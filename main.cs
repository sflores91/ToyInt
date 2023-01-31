namespace ToyInterpereter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

class ToyInterpereterMain
{


    //public Dictionary<string, object> funcBank = new Dictionary<string, object>();



    static void Main()
    {

        
        //Dictionary<string, ToyIntFunc> funcBank = new Dictionary<string, ToyIntFunc>();

        string json = File.ReadAllText(@"./demoIntTest.json");


        //var genderObj = JObject.Parse(_configurationHelper.GetValue(ConfigurationKeyConstants.AcceptedGenders) ?? ConfigurationKeyConstants.AcceptedGendersDefaults);
        //var genderList = genderObj.ToObject<Dictionary<string, string>>();

        // using var doc = JsonDocument.Parse(test);
        using var doc = JsonDocument.Parse(json);
        JsonElement root = doc.RootElement;
       // var dynamicObject = JsonConvert.DeserializeObject<dynamic>(jsonString)!;

        //root.EnumerateObject()
        //var dictionary = root.EnumerateObject().ToDictionary<string, object>(x =>
        //{
        
        //});
        
        var test = root.EnumerateObject();
        //var user = test.Current;
        // System.Console.WriteLine(user);

        var props = test.GetEnumerator();

        //ToyIntFuncBank functionBank = new ToyIntFuncBank();
        foreach(var prop in test.GetEnumerator())
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
                            func.CommandAdd(cType,prop.Name, cArgs);
                        }
                        else
                        {
                            func.CommandAdd(cType,prop.Name, cOutput, cArgs);
                        }

                    }
                    //finally add the ToyIntFunc to the bank of functions
                    ToyIntFuncBank.FunctionBankAdd(prop.Name, func);
                    break;
                default:
                    break;
            }


        }


        ToyIntFuncBank.RunFunctions();




    }
}


