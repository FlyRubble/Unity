using System;
using System.Collections.Generic;
using UnityAsset;
using XLua;
using Framework;
using Framework.UI;

public static class LuaCode
{
    [LuaCallCSharp]
    public static List<Type> luaFuncList = new List<Type>()
    {
        typeof(Param),
        typeof(LuaCallCS),
    };
}
