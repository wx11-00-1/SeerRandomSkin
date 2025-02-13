# 蓝白Seer

页游赛尔号登录器，同时支持 Flash 和 H5，以 Flash 端的精灵随机换肤为特色功能，仅支持 Windows 操作系统

## 声明

- 项目全部开源，仅供学习使用，禁止用于任何商业和非法行为
- 登录器实现的所有功能，都是以改善玩家的游戏体验为出发点，无意损害游戏官方的正当权益

## 使用

下载 Releases 中的压缩包，解压，运行可执行程序

###### 异常情况处理

1. 安装缺少的系统组件
   - Service Pack 1
   - .net framework 4.8（[下载](https://dotnet.microsoft.com/zh-cn/download/dotnet-framework/net48)，Windows 7 可能需要额外添加证书，安装系统补丁）
   - C++（[下载](https://learn.microsoft.com/en-us/cpp/windows/latest-supported-vc-redist?view=msvc-170)）

2. 杀毒软件乱删东西了，自行调教
3. 其他未知问题，安装 Visual Studio，下载源码自行编译运行

## 缘起

我是偶然看到[一篇帖子](https://www.52pojie.cn/thread-1468888-1-1.html)，接触到了比较高级的登录器开发，他用的是 C# 自带的 IE 内核浏览器组件。

顺着这条线索，我在吾爱论坛上又下载到一份易语言版本的发包例子；因为之前我也研究过易语言变速，又没学过 C#，当然是从易语言开始了。
然后就碰上了如何实现 FD 功能的问题，试用过别人封装的易语言的 FD 模块，都很容易卡对战加载；看模块的介绍说是封装了 C# 的库，于是我也转向了 C#。
因为站在了巨人的肩膀上（直接抄核心代码)，封包功能方面少踩了一些坑。
变速功能就难多了，好在最后还是找到了能参考的项目；一个有趣的地方是，能够实现只加速精灵出招动画，不加速倒计时！（有个Rainbow登录器，应该也是用这个技巧）

后来又通过 TO 登录器认识了 CefSharp，开了这个新的巨坑。。。

从24年开始，游戏环境剧烈变动。无论是 PVP 还是 PVE，都让我感到枯燥乏味。
完全成为“机械族”，丧失了游戏的意义；想过彻底告别，亦是难以割舍，我还是喜欢好看的精灵，华丽的特效，偶然想到的随机换肤这个点子，也是我选择留下来的重要原因。

我在 b 站上有部分[开发过程视频](https://space.bilibili.com/435657414/channel/collectiondetail?sid=2842294&ctype=0)，权当纪念。有同好进行技术交流，解决难题，也是意外之喜。

## 功能

- Flash 端
  - 随机换肤
  - 关电池
  - 变速
  - 读取封包
  - **对战助手**，用 js 代码制作日常脚本
  - 活动收藏夹
  - 精灵跟随 DIY
- H5 端
  - 保存当前套装、称号、精灵背包，一键更换
  - 战斗结束后自动治疗
  - 屏蔽游戏公告
  - 巅峰记牌器
  - 更换加载背景

## 对比

- 使用谷歌浏览器内核，有着与官方微端相同的优势：
  - 自带 Flash 插件，无需另外安装。画质更加清晰，同时更加稳定，大幅减少与 Flash 相关问题的出现
  - 支持 H5 端
  - 开发者工具
  - 与网页通过 js 代码进行交互

- 劣势：
  - 多进程架构，部分功能的开发难度增加
  - 变速效果与以往的 IE 内核登录器有所不同

## API
WxFightHandler 对象提供的属性和方法，通过 js 代码调用，用于编写日常脚本（登录器自带了一些脚本例子，参照着文档看，更容易理解；可以直接在开发者工具的控制台测试）
### 1 对战过程
#### WxFightHandler.Utils.GetRound
无参数。获取本次对战经过的回合数
#### WxFightHandler.Utils.UseSkill
说明：

使用技能

参数：

- skillID：技能 ID

#### WxFightHandler.Utils.ChangePet
说明：

切换精灵

参数：

- petCatchTime：精灵获取时间，用于标识精灵

#### WxFightHandler.Utils.UsePetItem
说明：

在战斗中使用道具（药剂、胶囊等）

参数：

- itemID：道具 ID

#### WxFightHandler.Utils.UsePetItem10PP
说明：

无参数。先用赛尔豆买，再使用恢复 10 pp 的药剂
#### WxFightHandler.Utils.ItemBuy
说明：

购买道具

参数：

- itemID：道具 ID

#### WxFightHandler.Utils.GetFightingPetID
说明：

无参数。在战斗中，获取当前在场精灵的 ID
#### WxFightHandler.Utils.ChangePetByID
说明：

换上指定 ID 数组中的的精灵

参数：

- idArray：精灵 ID **数组**

#### WxFightHandler.Utils.DelayAsync
说明：

等待一段时间。有些日常关卡出招太快会掉线；调试时可以使用，方便观察脚本运行情况

参数：

- millisecond：毫秒（一毫秒 = 一千分之一秒）

#### WxFightHandler.Utils.GetFightingPets

说明：

无参数。获取本场战斗中，我方出战精灵的部分信息（id、catchTime、hp、skillArray 等）

### 2 发包函数

#### WxFightHandler.Utils.Send
说明：

发送封包

参数：

- commandID：命令号

- ...args：任意数量的整数参数

#### WxFightHandler.Utils.SendAsync
说明：

发包，并接收返回值

参数：

- commandID：命令号

- parameterArray：整数参数**数组**

### 3 反射

#### WxFightHandler.Reflection.Get

说明：

获取对象的静态属性

示例：

```js
WxFightHandler.Reflection.Get('com.robot.core.manager.MainManager','actorInfo.userID'); // 自己的米米号WxFightHandler.Reflection.Get('com.robot.core.manager.MainManager','actorInfo.nick'); // 昵称
```

#### WxFightHandler.Reflection.Set

说明：

设置对象的静态属性

#### WxFightHandler.Reflection.Action

说明：

调用对象的静态方法（无返回值）

参数：

- className：对象的全类名
- methodName：方法名
- ...args：任意数量的参数

#### WxFightHandler.Reflection.Func

说明：

调用对象的静态方法（有返回值）

### 4 其他

#### WxFightHandler.Utils.SetIsAutoCure

说明：

设置对战结束后是否自动治疗

参数：

true 或 false

#### WxFightHandler.Utils.CurePet20HP

说明：

无参数。为出战背包的所有精灵恢复20HP、10PP

#### WxFightHandler.Utils.CurePetAll

说明：

无参数。恢复整个背包所有精灵体力、PP

#### WxFightHandler.Utils.LowHP

说明：

无参数。发起与白虎完全体的对战

#### WxFightHandler.Utils.SimpleAlarm

说明：

在游戏中显示提示信息，维持一小段时间后，自动消失；一种使用场景：不算耗时，但又不是立刻能完成的任务，在完成后给用户提示

参数：

字符串
