### Items 物品

游戏中的物品有以下几种存在方式:
1. [`WorldItem`] 放置在场景中, 可以拾取(与背包交互).
1. [`PickedItem`] 放在背包里, 可以使用或丢弃.
1. [`SceneItem`] 放在场景中, 主动影响角色和环境.
1. [`InteractionItem`] 放在场景中, 可以被角色主动交互.

所有的物品都是**不可被其它脚本破坏**的; 只有它们自己的脚本能够控制它们是否被破坏.

如果需要给物品一个 HP 之类的东西, 或者给物品一个碰撞盒让它可以被攻击, 请使用 Role / RoleHealth 等角色相关脚本.

你可以给同一个 GameObject 挂载 Role 系列脚本和自定义的 WorldItem 脚本, 以支持拾取.
