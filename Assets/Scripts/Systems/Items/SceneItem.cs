using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Systems
{
    /// <summary>
    /// 放置在游戏场景中的, 以主动影响玩家的方式与玩家交互的物品.
    /// 一般来说, 这个种类的物品是不会被调用的, 因为是该物品主动与角色交互...
    /// 不过还是放一个接口在这里好了...
    /// </summary>
    public abstract class SceneItem : MonoBehaviour
    {
        
    }
}
