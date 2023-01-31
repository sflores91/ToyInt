using System;
using ToyInterpereter;

 static class ToyIntFuncBank
{
    static public Dictionary<string, ToyIntFunc> funcBank = new Dictionary<string, ToyIntFunc>();

    static public void FunctionBankAdd(string key, ToyIntFunc newFunc)
	{
		if (funcBank.ContainsKey(key)) { funcBank.Remove(key); }
		funcBank.Add(key, newFunc);
	}

    /*public  void RunInit() { 
		var init = funcBank.Where(x => x.Key == "init").First().Value;
		init.DoCommands(funcBank);
    }*/

    static public void RunFunctions() {
		ToyIntFunc init = funcBank["init"];
		init.DoCommands();
	}

    static public ToyIntFunc GetFuncFromBank (string key) { return funcBank[key]; }

	static public ToyIntCmd GetCommandFromBank(string func, string command) {
		ToyIntFunc vFunc = funcBank[func];
		ToyIntCmd vCmd = vFunc.GetCommandFromFunction(command);
		return vCmd;
	}
}

