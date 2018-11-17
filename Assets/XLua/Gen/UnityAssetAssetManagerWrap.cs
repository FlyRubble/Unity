#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class UnityAssetAssetManagerWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UnityAsset.AssetManager);
			Utils.BeginObjectRegister(type, L, translator, 0, 8, 3, 4);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResourceLoad", _m_ResourceLoad);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResourceAsyncLoad", _m_ResourceAsyncLoad);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AssetBundleLoad", _m_AssetBundleLoad);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AssetBundleAsyncLoad", _m_AssetBundleAsyncLoad);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CompleteContains", _m_CompleteContains);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "LoadingContains", _m_LoadingContains);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "UnloadAssets", _m_UnloadAssets);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "isAllowload", _g_get_isAllowload);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "maxLoader", _g_get_maxLoader);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "url", _g_get_url);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "isAllowload", _s_set_isAllowload);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "maxLoader", _s_set_maxLoader);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "url", _s_set_url);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "setDependentAsset", _s_set_setDependentAsset);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					UnityAsset.AssetManager gen_ret = new UnityAsset.AssetManager();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UnityAsset.AssetManager constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResourceLoad(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)) 
                {
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        UnityEngine.Object gen_ret = gen_to_be_invoked.ResourceLoad( _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<System.Type>(L, 3)) 
                {
                    string _path = LuaAPI.lua_tostring(L, 2);
                    System.Type _type = (System.Type)translator.GetObject(L, 3, typeof(System.Type));
                    
                        UnityEngine.Object gen_ret = gen_to_be_invoked.ResourceLoad( _path, _type );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityAsset.AssetManager.ResourceLoad!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResourceAsyncLoad(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 2);
                    Framework.Event.Action<bool, UnityAsset.AsyncAsset> _action = translator.GetDelegate<Framework.Event.Action<bool, UnityAsset.AsyncAsset>>(L, 3);
                    
                        UnityAsset.AsyncAsset gen_ret = gen_to_be_invoked.ResourceAsyncLoad( _path, _action );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AssetBundleLoad(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        UnityAsset.AsyncAsset gen_ret = gen_to_be_invoked.AssetBundleLoad( _path );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AssetBundleAsyncLoad(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 4&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Framework.Event.Action<bool, UnityAsset.AsyncAsset>>(L, 3)&& translator.Assignable<System.Collections.Generic.Dictionary<string, UnityAsset.AsyncAsset>>(L, 4)) 
                {
                    string _path = LuaAPI.lua_tostring(L, 2);
                    Framework.Event.Action<bool, UnityAsset.AsyncAsset> _action = translator.GetDelegate<Framework.Event.Action<bool, UnityAsset.AsyncAsset>>(L, 3);
                    System.Collections.Generic.Dictionary<string, UnityAsset.AsyncAsset> _dic = (System.Collections.Generic.Dictionary<string, UnityAsset.AsyncAsset>)translator.GetObject(L, 4, typeof(System.Collections.Generic.Dictionary<string, UnityAsset.AsyncAsset>));
                    
                        UnityAsset.AsyncAsset gen_ret = gen_to_be_invoked.AssetBundleAsyncLoad( _path, _action, _dic );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                if(gen_param_count == 3&& (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING)&& translator.Assignable<Framework.Event.Action<bool, UnityAsset.AsyncAsset>>(L, 3)) 
                {
                    string _path = LuaAPI.lua_tostring(L, 2);
                    Framework.Event.Action<bool, UnityAsset.AsyncAsset> _action = translator.GetDelegate<Framework.Event.Action<bool, UnityAsset.AsyncAsset>>(L, 3);
                    
                        UnityAsset.AsyncAsset gen_ret = gen_to_be_invoked.AssetBundleAsyncLoad( _path, _action );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityAsset.AssetManager.AssetBundleAsyncLoad!");
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.Update(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CompleteContains(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.CompleteContains( _key );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_LoadingContains(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _key = LuaAPI.lua_tostring(L, 2);
                    
                        bool gen_ret = gen_to_be_invoked.LoadingContains( _key );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_UnloadAssets(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
            
            
			    int gen_param_count = LuaAPI.lua_gettop(L);
            
                if(gen_param_count == 2&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 2)) 
                {
                    bool _unloadAllLoadedObjects = LuaAPI.lua_toboolean(L, 2);
                    
                    gen_to_be_invoked.UnloadAssets( _unloadAllLoadedObjects );
                    
                    
                    
                    return 0;
                }
                if(gen_param_count == 3&& translator.Assignable<UnityAsset.AsyncAsset>(L, 2)&& LuaTypes.LUA_TBOOLEAN == LuaAPI.lua_type(L, 3)) 
                {
                    UnityAsset.AsyncAsset _asset = (UnityAsset.AsyncAsset)translator.GetObject(L, 2, typeof(UnityAsset.AsyncAsset));
                    bool _unloadAllLoadedObjects = LuaAPI.lua_toboolean(L, 3);
                    
                    gen_to_be_invoked.UnloadAssets( _asset, _unloadAllLoadedObjects );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
            return LuaAPI.luaL_error(L, "invalid arguments to UnityAsset.AssetManager.UnloadAssets!");
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_isAllowload(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.isAllowload);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_maxLoader(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, gen_to_be_invoked.maxLoader);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_url(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.url);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_isAllowload(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.isAllowload = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_maxLoader(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.maxLoader = LuaAPI.xlua_tointeger(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_url(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.url = LuaAPI.lua_tostring(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_setDependentAsset(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UnityAsset.AssetManager gen_to_be_invoked = (UnityAsset.AssetManager)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.setDependentAsset = translator.GetDelegate<UnityAsset.AssetManager.DependentAsset>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
