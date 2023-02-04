namespace ToyInterpereter;
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime;
using System.Windows.Markup;

class ToyIntCmd
{
    public string parentFunction;
    public string callingFunction;
    public string cmdType;
    public Dictionary<string, string> cmdArgs = new Dictionary<string, string>();
    public Dictionary<string, string> cmdParams = new Dictionary<string, string>();
    public string cmdOutput = "!";
    public ToyIntCmd(string cType, string parFunc, Dictionary<string, string> cArgs)
    {
        cmdType = cType;
        parentFunction = parFunc;
        cmdArgs = cArgs;
    }
    public ToyIntCmd(string cType, string parFunc, string output, Dictionary<string, string> cArgs)
    {
        cmdType = cType;
        parentFunction = parFunc;
        cmdArgs = cArgs;
        cmdOutput = output;
    }
    //public void SetCommandPropertiesFromRoot(string cName, string parFunc)
    //{
    //    string rootName = parFunc.Replace("#", "");
    //    //ToyIntFunc parentFunction = ToyIntFuncBank.GetFuncFromBank(rootName);
    //    string callerCommand = rootName;
    //    //ToyIntCmd parentCommand = parentFunction.GetCommandFromFunction(callerCommand);

    //    //cmdType = parentCommand.cmdType; 
    //    //Dictionary<string, string> pCmdArgs = parentCommand.cmdArgs;
    //    Dictionary<string, string> tCmdArgs = new Dictionary<string, string>();
    //    //string pCmdOutput = parentCommand.cmdOutput;

    //    foreach(KeyValuePair<string,string> p in pCmdArgs)
    //    {
    //        if (cmdArgs.ContainsKey(p.Value.Replace("$", ""))){
    //            tCmdArgs.Add(p.Key, cmdArgs[p.Value.Replace("$", "")]);
    //        }
    //    }

    //cmdArgs = tCmdArgs;

    //}
    //public void DoCommand(string parentCaller)
    //{
    ////parentFunction = parentCaller;
    ////check if nested function
    //if (cmdType.StartsWith("#"))
    //{
    //    //new ToyIntCmd();
    //SetCommandPropertiesFromRoot(parentCaller, cmdType);
    public void DoCommand(string caller, string parent)
    {
        callingFunction = caller;
        parentFunction = parent;
        DoCommand();
    }
    public void DoCommand()
    {
        switch (cmdType)
        {
            case "create":
                CreateVar();
                break;
            case "delete":
                DeleteVar();
                break;
            case "update":
                UpdateVar();
                break;
            case "add" or "subtract" or "multiply" or "divide":
                DoMath();
                break;
            case "print":
                PrintVar();
                break;
        }
    }
    //create
    public void CreateVar()
    {
        Dictionary<string, string> args = GetVar();

        ToyIntVar.AddVar(cmdOutput, args["value"]);
    }
    //delete
    public void DeleteVar()
    {
        Dictionary<string, string> args = GetVar();

        ToyIntVar.RemoveVar(cmdOutput);
    }
    //update
    public void UpdateVar()
    {
        Dictionary<string, string> args = GetVar();

        ToyIntVar.UpdateVar(cmdOutput, args["value"]);
    }
    //print
    public void PrintVar()
    {

        Dictionary<string, string> vars = GetVar();

        foreach (var v in vars)
        {
            Console.WriteLine(v.Value);
        }
    }
    //add, subtract, multiply, divide
    public void DoMath()
    {
        Dictionary<string, string> vars = GetVar();
        decimal ret = 0;
        switch (cmdType)
        {
            case "add":
                foreach (var v in vars)
                {
                    Decimal.TryParse(v.Value, out decimal val);
                    ret += val;
                }
                break;
            case "subtract":
                foreach (var v in vars)
                {
                    Decimal.TryParse(v.Value, out decimal val);
                    ret -= val;
                }
                break;
            case "multiply":
                foreach (var v in vars)
                {
                    Decimal.TryParse(v.Value, out decimal val);
                    ret *= val;
                }
                break;
            case "divide":
                foreach (var v in vars)
                {
                    Decimal.TryParse(v.Value, out decimal val);
                    ret /= val;
                }
                break;
        }
        ToyIntVar.AddVar(cmdOutput, ret.ToString());
    }
    public Dictionary<string, string> GetVar()
    {
        Dictionary<string, string> vars = new Dictionary<string, string>();
        if (cmdOutput.StartsWith("$"))
        {
            ToyIntCmd callingFunc = ToyIntFuncBank.GetCommandFromBank(callingFunction, parentFunction);
            cmdOutput = callingFunc.cmdOutput;
        }
        foreach (var arg in cmdArgs)
        {
            if (arg.Value.StartsWith("$"))
            {
                string s = arg.Value.Replace("$", "");
                ToyIntCmd callingFunc = ToyIntFuncBank.GetCommandFromBank(callingFunction, parentFunction);
                cmdArgs[arg.Key] = callingFunc.cmdArgs[s];
            }

        }
        foreach (var arg in cmdArgs)
        {
            switch (arg.Value.Substring(0, 1))
            {
                case "#":
                    //if its a declared var fetch from bank
                    string var = ToyIntVar.ReturnVar(arg.Value);
                    //be value here because the value is technically the input param
                    vars.Add(arg.Value, var);
                    break;
                case "$":
                    //if the declared var to fetch from the bank is input arg, need to get the input param from parent func
                    //do stuff
                    ToyIntFunc callingFunc = ToyIntFuncBank.GetFuncFromBank(parentFunction);
                    break;
                default:
                    //normal constant arg assigninment
                    vars.Add(arg.Key, arg.Value);
                    break;
            }
        }
        return vars;
    }
}




