using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

public class Test : MonoBehaviour {
    Action luaAwake;
    
    Action<object> t;
    // Use this for initialization
    void Start () {
        Application.targetFrameRate = 180;
        LuaEnv luaEnv = new LuaEnv();
        LuaTable scriptEnv = luaEnv.NewTable();

        // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        scriptEnv.Set("self", this);

        TextAsset ta = Resources.Load("lua") as TextAsset;

        luaEnv.DoString("require('lua')", "Test", scriptEnv);
        luaAwake = scriptEnv.Get<Action>("awake");
        luaAwake();


        t = scriptEnv.Get<Action<object>>("update");
    }
	
	// Update is called once per frame
	void Update () {
        if (t != null)
        {
            //t(Time.realtimeSinceStartup);
        }
    }

    float tt = 0;
    int iii = 0;
    private void OnGUI()
    {
        if (tt + 1 < Time.realtimeSinceStartup)
        {
            iii = ((int)(1f / Time.deltaTime));
            tt = Time.realtimeSinceStartup;
        }
        GUILayout.Label(iii.ToString());
    }
}
