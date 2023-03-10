namespace ToyInterpereter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToyInterpereter;

class ToyIntFunc
{
    private string funcName;
    private List<ToyIntCmd> commandBank = new List<ToyIntCmd>();
    public ToyIntFunc(string fName)
    {
        funcName = fName;
    }
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
    public ToyIntCmd GetCommandFromFunction(string customCommand)
    {
        var index = commandBank.Find(x => x.cmdType == customCommand);
        return index;
    }
    public void DoCommands()
    {
        //ToyIntFunc funcInst = (ToyIntFunc)this.MemberwiseClone();    
        foreach (ToyIntCmd cmd in commandBank)
        {
            if (cmd.cmdType.StartsWith("#"))
            {
                ToyIntFunc rootFunc = ToyIntFuncBank.GetFuncFromBank(cmd.cmdType.Replace("#", ""));
                ToyIntFunc funcInst = (ToyIntFunc)rootFunc.MemberwiseClone();
                funcInst.DoCommands(funcInst, cmd.parentFunction, cmd.cmdType);
            }
            cmd.DoCommand();
        }
    }
    public void DoCommands(ToyIntFunc func, string caller, string callingFunc)
    {
        //ToyIntFunc funcInst = (ToyIntFunc)this.MemberwiseClone();
        foreach (ToyIntCmd cmd in func.commandBank)
        {
            if (cmd.cmdType.StartsWith("#"))
            {
                ToyIntFunc rootFunc = ToyIntFuncBank.GetFuncFromBank(cmd.cmdType.Replace("#", ""));
                ToyIntFunc funcInst = (ToyIntFunc)rootFunc.MemberwiseClone();
                funcInst.DoCommands(funcInst, cmd.parentFunction, cmd.cmdType);
            }
            cmd.DoCommand(caller, callingFunc);
        }
    }
}

