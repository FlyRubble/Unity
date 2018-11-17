using System;
using System.Collections.Generic;
using UnityAsset;
using XLua;

public static class LuaCode
{
    [LuaCallCSharp]
    public static List<Type> luaFuncList = new List<Type>()
    {
        typeof(App),
        typeof(AsyncAsset),
        typeof(AssetManager),
        typeof(LuaCallCS),
    };
}
