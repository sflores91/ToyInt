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
        string jsonPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"demoIntTest.json");
        ToyIntJsonParser jsonParse = new ToyIntJsonParser();
        jsonParse.ProcessFile(jsonPath);
        ToyIntFuncBank.RunFunctions();
    }
}


