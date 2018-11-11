using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Framework.Event;
using Framework;

public class Util
{
    /// <summary>
    /// 写入字节到文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="bytes"></param>
    public static void WriteAllBytes(string path, byte[] bytes)
    {
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        File.WriteAllBytes(path, bytes);
    }

    /// <summary>
    /// 查找对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static T Find<T>(Transform parent, string name) where T : Component
    {
        if (null == parent)
        {
            Debugger.LogError("The parent is null!");
            return null;
        }

        Transform child = parent.Find(name);
        if (null != child)
        {
            T t = child.GetComponent<T>();
            if (null == t)
            {
                Debugger.LogErrorFormat("Get component failed! type = {0}", typeof(T));
            }
            return t;
        }
        else
        {
            Debugger.LogErrorFormat("Find child failed! name = {0}", name);
        }

        return null;
    }

    /// <summary>
    /// 设置文本
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="text"></param>
    public static void SetText(Transform transform, string text)
    {
        if (null == transform)
        {
            Debugger.LogError("The transform is null!");
        }
        if (null == text)
        {
            Debugger.LogError("The text is null!");
        }

        Text t = transform.GetComponent<Text>();
        if (null == t)
        {
            Debugger.LogErrorFormat("Get component failed! type = {0}", typeof(Text));
        }
        else
        {
            t.text = text;
        }
    }

    /// <summary>
    /// 设置文本
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="text"></param>
    public static void SetText(GameObject gameObject, string text)
    {
        if (null == gameObject)
        {
            Debugger.LogError("The gameObject is null!");
        }

        SetText(gameObject.transform, text);
    }

    /// <summary>
    /// 设置文本
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <param name="text"></param>
    public static void SetText(Transform parent, string name, string text)
    {
        if (null == parent)
        {
            Debugger.LogError("The parent is null!");
        }
        if (null == text)
        {
            Debugger.LogError("The text is null!");
        }

        Transform child = parent.Find(name);
        if (null != child)
        {
            Text t = child.GetComponent<Text>();
            if (null == t)
            {
                Debugger.LogErrorFormat("Get component failed! type = {0}", typeof(Text));
            }
            else
            {
                t.text = text;
            }
        }
        else
        {
            Debugger.LogErrorFormat("Find child failed! name = {0}", name);
        }
    }

    /// <summary>
    /// 设置文本
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="name"></param>
    /// <param name="text"></param>
    public static void SetText(GameObject gameObject, string name, string text)
    {
        if (null == gameObject)
        {
            Debugger.LogError("The gameObject is null!");
        }

        SetText(gameObject.transform, name, text);
    }
    
    /// <summary>
    /// 设置激活状态
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="active"></param>
    public static void SetActive(Transform transform, bool active)
    {
        if (null == transform)
        {
            Debugger.LogError("The transform is null!");
        }
        
        transform.gameObject.SetActive(active);
    }

    /// <summary>
    /// 设置激活状态
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="active"></param>
    public static void SetActive(GameObject gameObject, bool active)
    {
        if (null == gameObject)
        {
            Debugger.LogError("The gameObject is null!");
        }

        gameObject.SetActive(active);
    }

    /// <summary>
    /// 设置激活状态
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <param name="active"></param>
    public static void SetActive(Transform parent, string name, bool active)
    {
        if (null == parent)
        {
            Debugger.LogError("The parent is null!");
        }

        Transform child = parent.Find(name);
        if (null != child)
        {
            child.gameObject.SetActive(active);
        }
        else
        {
            Debugger.LogErrorFormat("Find child failed! name = {0}", name);
        }
    }

    /// <summary>
    /// 设置激活状态
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="name"></param>
    /// <param name="active"></param>
    public static void SetActive(GameObject gameObject, string name, bool active)
    {
        if (null == gameObject)
        {
            Debugger.LogError("The gameObject is null!");
        }

        SetActive(gameObject.transform, name, active);
    }

    /// <summary>
    /// 设置按钮事件
    /// </summary>
    /// <param name="button"></param>
    /// <param name="action"></param>
    /// <param name="removeAllListeners"></param>
    public static void SetButton(Button button, Action action, bool removeAllListeners = true)
    {
        if (null == button)
        {
            Debugger.LogError("The button is null!");
        }
        if (null == action)
        {
            Debugger.LogError("The action is null!");
        }

        if (removeAllListeners)
        {
            button.onClick.RemoveAllListeners();
        }
        button.onClick.AddListener(()=> { action(); });
    }

    /// <summary>
    /// 设置按钮事件
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="name"></param>
    /// <param name="action"></param>
    /// <param name="removeAllListeners"></param>
    public static void SetButton(Transform parent, string name, Action action, bool removeAllListeners = true)
    {
        if (null == parent)
        {
            Debugger.LogError("The parent is null!");
        }

        Transform child = parent.Find(name);
        if (null != child)
        {
            Button t = child.GetComponent<Button>();
            SetButton(t, action, removeAllListeners);
        }
        else
        {
            Debugger.LogErrorFormat("Find child failed! name = {0}", name);
        }
    }

    /// <summary>
    /// 设置按钮事件
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="name"></param>
    /// <param name="action"></param>
    /// <param name="removeAllListeners"></param>
    public static void SetButton(GameObject gameObject, string name, Action action, bool removeAllListeners = true)
    {
        if (null == gameObject)
        {
            Debugger.LogError("The gameObject is null!");
        }

        SetButton(gameObject.transform, name, action, removeAllListeners);
    }

    /// <summary>
    /// 设置按钮事件
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="action"></param>
    /// <param name="removeAllListeners"></param>
    public static void SetButton(Transform transform, Action action, bool removeAllListeners = true)
    {
        if (null == transform)
        {
            Debugger.LogError("The transform is null!");
        }
        
        Button t = transform.GetComponent<Button>();
        SetButton(t, action, removeAllListeners);
    }

    /// <summary>
    /// 设置按钮事件
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="action"></param>
    /// <param name="removeAllListeners"></param>
    public static void SetButton(GameObject gameObject, Action action, bool removeAllListeners = true)
    {
        if (null == gameObject)
        {
            Debugger.LogError("The gameObject is null!");
        }

        Button t = gameObject.GetComponent<Button>();
        SetButton(t, action, removeAllListeners);
    }

    /// <summary>
    /// 得到多语言文本
    /// </summary>
    /// <param name="id"></param>
    /// <param name="defaultText"></param>
    /// <returns></returns>
    public static string GetLanguage(string id, string defaultText)
    {
        return defaultText;
    }

    /// <summary>
    /// 得到多语言文本
    /// </summary>
    /// <param name="id"></param>
    /// <param name="defaultText"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static string GetLanguageFormat(string id, string defaultText, params object[] args)
    {
        return string.Format(GetLanguage(id, defaultText), args);
    }














    public static void Msg(string msg, bool quit = false)
    {
        Debug.LogError(msg);
        if (quit)
        {
            Application.Quit();
        }
    }
}
