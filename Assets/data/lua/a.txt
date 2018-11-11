a = {}    -- 全局的变量，模块名称

function a.new(r, i) return {r = r, i = i} end

-- 定义一个常量i
a.i = a.new(0, 1)
a.go = nil
function a.add(c1, c2)
    return a.new(c1.r + c2.r, c1.i + c2.i)
end

function a.sub(c1, c2)
    return a.new(c1.r - c2.r, c1.i - c2.i)
end

function a.print()
    go = CS.UnityEngine.GameObject.Find('Launch')
    if (nil ~= go) then
        go.transform.localScale = CS.UnityEngine.Vector3.one * 2
    end
    return "12344646545665798"
end

function a.update(time)
    if (nil ~= go) then
        print(time)
        go.transform.localScale = CS.UnityEngine.Vector3.one * time * 0.11
    end
end

return a  -- 返回模块的table