using System;
using System.Collections.Generic;
using UnityAsset;
using XLua;
using Framework;
using Framework.UI;

public static class LuaCode
{
    //lua中要使用到C#库的配置，比如C#标准库，或者Unity API，第三方库等。
    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>()
    {
        typeof(Param),
        typeof(Dictionary<string, Framework.Event.Action<string>>),
        typeof(LuaCallCS),
    };

    //C#静态调用Lua的配置（包括事件的原型），仅可以配delegate，interface
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>()
    {
        typeof(Framework.Event.Action),
        typeof(Framework.Event.Action<string>),
        typeof(Lua.ConfigList),
    };
}
