using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyInterpereter;

namespace ToyInterpereter;

class ToyIntFunc
{
    private string funcName;
    private List<ToyIntCmd> commandBank = new List<ToyIntCmd>();
    public ToyIntFunc(string fName)
    {
        funcName = fName;
    }

    //public static void ToyIntFunc()
    //{ }

    public void CommandAdd(string cmd, string parentFunc, Dictionary<string, string> args)
    {
        ToyIntCmd cm = new ToyIntCmd(cmd, parentFunc, args);
        commandBank.Add(cm);
    }

    public void CommandAdd(string cmd, string parentFunc, string output, Dictionary<string, string> args)
    {
        ToyIntCmd cm = new ToyIntCmd(cmd, parentFunc, output, args);
        commandBank.Add(cm);
    }
    //public void CommandCustomAdd(string cmdName, string parentFunc, int index)
    //{
    //    if (cmdName.StartsWith("#")) { return; }
    //    ToyIntCmd newCmd = new ToyIntCmd(cmdName, parentFunc);

    //}

    public ToyIntCmd GetCommandFromFunction(string customCommand)
    {
        var index = commandBank.Find(x => x.cmdType == customCommand);
        return index;
    }

    public void DoCommands()
    {
        foreach (ToyIntCmd cmd in commandBank)
        {
            if (cmd.cmdType.StartsWith("#")){
                ToyIntFunc rootFunc = ToyIntFuncBank.GetFuncFromBank(cmd.cmdType.Replace("#",""));
                rootFunc.DoCommands(rootFunc, cmd.parentFunction,cmd.cmdType);  

            }
            cmd.DoCommand();
        }
    }
    public void DoCommands(ToyIntFunc func, string caller, string callingFunc)
    {
        foreach(ToyIntCmd cmd in func.commandBank)
        {
            if (cmd.cmdType.StartsWith("#"))
            {
                ToyIntFunc rootFunc = ToyIntFuncBank.GetFuncFromBank(cmd.cmdType.Replace("#", ""));
                rootFunc.DoCommands(rootFunc, cmd.parentFunction, cmd.cmdType);

            }
            cmd.DoCommand(caller, callingFunc);
        }
    }
}
    
