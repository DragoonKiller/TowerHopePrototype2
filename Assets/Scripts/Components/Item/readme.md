### Items 物品

游戏中的物品有三种存在方式:
1. [`WorldItem`] 放置在场景中, 可以拾取(与背包交互).
1. [`PickedItem`] 放在背包里, 可以使用或丢弃.
1. [`SceneItem`] 放在场景中, 作装饰, 或与角色发生交互, 但是不能拾取, 或者不经过标注的拾取流程.

所有的物品都是不可被主动破坏的; 只有它们自己的脚本能够控制它们是否被破坏.

如果需要给物品一个 HP 之类的东西, 或者给物品一个碰撞盒让它可以被攻击, 请使用 Role / RoleHealth 等角色相关脚本.

你可以给同一个 GameObject 挂载 Role 系列脚本和自定义的 WorldItem 脚本, 以支持拾取.

[`WorldItem`] 和 [`SceneItem`] 的碰撞响应由其它脚本完成. 接受该碰撞的物体通常是实体(RigidBody2D + (NonTrigger)Collider2D), 而我们也不希望物品之间相互碰撞, 所以通常会采用 Trigger Collider2D.
