## 脚本文件夹
命名为 Scripts, 存储所有C#脚本内容.

各个子文件夹的含义如下:

* Components : 能够挂在 Unity GameObject 的东西. 通常是最终采用的脚本.
* Systems : 存储游戏的数据和计算系统的相关内容. 各种可以在其它地方复用的, 或者因为组件的需求需要以多态的方式实现的内容, 其接口, 基类, 都放在这里.
* Utils : 各种工具函数, 工具类等可能在底层, 中层和上层的任何地方使用的内容. 
* Editor : 存储 Unity 编辑器扩展的代码. 其目录结构应当与 Scripts 保持一致, 比如 Scripts/Systems/UnitTest 相关的编辑器扩展, 应当在放 Scripts/Editor/Systems/UnitTest 当中.

除 Editor 外, 调用关系自上而下, 下层不能调用上层, 上层可以跨级调用下层.

除了 Impls 使用 Tower 作为 namespace 以外, 每个子文件夹中内容的 namespace 应当和它的文件夹名称一致. 再往下的子文件夹不作此要求.
