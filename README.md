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
  - 套装幻化增强版
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
WxSc 对象提供的属性和方法，通过 js 代码调用，用于编写日常脚本（登录器自带了一些脚本例子，参照着文档看，更容易理解；可以直接在开发者工具的控制台测试）
### 1 对战过程
#### WxSc.Util.GetRound
无参数。获取本次对战经过的回合数
#### WxSc.Util.UseSkill
说明：

使用技能

参数：

- skillID：技能 ID

#### WxSc.Util.ChangePet
说明：

切换精灵

参数：

- petCatchTime：精灵获取时间，用于标识精灵

#### WxSc.Util.UsePetItem
说明：

在战斗中使用道具（药剂、胶囊等）

参数：

- itemID：道具 ID

#### WxSc.Util.UsePetItem10PP
说明：

无参数。先用赛尔豆买，再使用恢复 10 pp 的药剂
#### WxSc.Util.ItemBuy
说明：

购买道具

参数：

- itemID：道具 ID

#### WxSc.Util.GetFightingPetID
说明：

无参数。在战斗中，获取当前在场精灵的 ID
#### WxSc.Util.ChangePetByID
说明：

换上指定 ID 数组中的的精灵

参数：

- idArray：精灵 ID **数组**

#### WxSc.Util.DelayAsync
说明：

等待一段时间。有些日常关卡出招太快会掉线；调试时可以使用，方便观察脚本运行情况

参数：

- millisecond：毫秒（一毫秒 = 一千分之一秒）

#### WxSc.Util.GetFightingPets

说明：

无参数。获取本场战斗中，我方出战精灵的部分信息（id、catchTime、hp、skillArray 等）

### 2 发包函数

#### WxSc.Util.Send
说明：

发送封包

参数：

- commandID：命令号

- ...args：任意数量的整数参数

#### WxSc.Util.SendAsync
说明：

发包，并接收返回值

参数：

- commandID：命令号

- parameterArray：整数参数**数组**

### 3 *对象暂存区*

#### 使用场景

##### 与第 4 点——反射配合使用

1. 反射可以调用游戏内部的函数
2. 函数的参数可能是基本类型，也可能是游戏特有类型的对象：
   - 基本类型：数字、字符串等类型，是通用的
   - 其他：需要在游戏内部的对象暂存区创建对象，再作为参数传入

##### 保存函数的返回值

有时需要获取游戏内部函数的返回值，这个返回值又是特殊类型，不能直接返回到 js 层，可以先放到暂存区，再读取其中一部分允许读取的属性

#### 使用示例

参考对战助手中的示例脚本：杂项-刻印仓库脚本、杂项-野生精灵

#### WxSc.Dict.Add

说明：

创建暂存区对象

参数：

- key：字符串类型，作为标识
- name：完整类名
- ...args：构造函数需要的参数

示例：

假设 as3（ActionScript 3，flash 游戏专用编程语言）层有一个 Test 类：

```java
package com.robot.app {
    public class Test {
        private var _a:int;
        public function Test(param1:int) {
            _a = param1;
        }
    }
}
```

在暂存区创建 Test 类型对象，可以这样写：

```js
WxSc.Dict.Add('te','com.robot.app.Test',false,1);
```

这样就创建出了一个 Test 对象，它的私有属性 _a 在构造时被赋值为 1。

不知道你有没有注意到，这里还多传了一个 false 值（表示这个参数是不是要从暂存区里面取）。

这种设计是考虑到构造函数的参数可能是其他特殊类型对象的情况，比如这个 Test2 对象：

```java
package com.robot.app2 {
    import com.robot.app.Test;
    
    public class Test2 {
        private var _b:String;
        private var _t:Test
        public function Test(param1:String, param2:Test) {
            _b = param1;
            _t = param2;
        }
    }
}
```

那么创建 Test2 对象，可以用到之前放在暂存区的对象 te：

```js
WxSc.Dict.Add('te2','com.robot.app2.Test2',false,'1',true,'te');
```

之后的接口也是同样的情况，就不一一解释这些“多余的”布尔值啦

#### WxSc.Dict.Set

说明：

设置暂存区对象的属性

参数：

- key：标识
- attribute：属性名
- usePool：是否使用暂存区的对象
- value：通用类型变量，或者暂存区对象的标识字符串

#### WxSc.Dict.Get

说明：

读取暂存区对象的属性

参数：

- key：标识
- attribute：属性名（可选）

#### WxSc.Dict.Func

说明：

执行暂存区对象的公开（public）函数，可以获取返回值

参数：

- key：标识
- method：函数名
- ...args：参数

#### WxSc.Dict.Tmp

执行暂存区对象的公开（public）函数，把函数的返回值也放到暂存区

参数：

- key：标识
- method：函数名
- key2：返回值的标识
- ...args：参数

#### WxSc.Dict.TmpAttrib

获取暂存区对象的属性，保存为另一个暂存区对象

参数：

- key：原暂存区对象的标识
- attrib：属性名称
- key2：新暂存区对象的标识

#### WxSc.Dict.Del

临时使用完对象后，从暂存区删除

参数：

- key：标识

#### WxSc.Dict.AddCall

添加回调函数

参数：

- key：标识
- result：返回值标识
- func：无参回调函数

### 4 *反射*

#### WxSc.Refl.Get

说明：

获取对象的属性

参数：

- name: 完整类名
- attribute: 属性名

示例：

```js
WxSc.Refl.Get('com.robot.core.manager.MainManager','actorInfo.userID'); // 自己的米米号
WxSc.Refl.Get('com.robot.core.manager.MainManager','actorInfo.nick'); // 昵称
```

#### WxSc.Refl.Set

说明：

设置对象的属性

参数：

- name: 完整类名
- attribute: 属性名
- usePool：是否使用暂存区的对象
- value：通用类型变量，或者暂存区对象的标识字符串

#### WxSc.Refl.Func

说明：

调用函数

示例：

```js
// 进入编号为 1 的地图
WxSc.Refl.Func('com.robot.core.manager.MapManager','changeMap',false,1);
```

#### WxSc.Refl.Tmp

说明：

调用函数，并将返回值放入暂存区

- key：标识
- method：函数名
- key2：返回值的标识
- ...args：参数

#### 关卡脚本常用函数

使用示例：【关卡-噬梦魔灵-第二关】【关卡-暗黑托鲁克】

##### WxSc.KTool.getMultiValueAsync

46046 包，通常用于获取关卡进度

##### WxSc.KTool.subByte

接收包的处理

##### WxSc.ItemManager.updateItemsAsync

42399 包，获取拥有道具的数量

### 5 其他

#### WxSc.Util.SetIsAutoCure

说明：

设置对战结束后是否自动治疗

参数：

true 或 false

#### WxSc.Util.CurePet20HP

说明：

无参数。为出战背包的所有精灵恢复20HP、10PP

#### WxSc.Util.CurePetAll

说明：

无参数。恢复整个背包所有精灵体力、PP

#### WxSc.Util.LowHP

说明：

无参数。发起与白虎完全体的对战

#### WxSc.Util.SimpleAlarm

说明：

在游戏中显示提示信息，维持一小段时间后，自动消失；一种使用场景：不算耗时，但又不是立刻能完成的任务，在完成后给用户提示

参数：

字符串

#### WxSc.Util.ShowAppModule

打开活动页面

示例：

```js
WxSc.Util.ShowAppModule('AssessorPanel'); // https://seer.61.com/module/com/robot/module/app/AssessorPanel.swf
```

#### WxSc.Util.ChangeCloth

更换套装

参数：

- 套装数组
- 更换模式（可选，默认是 true）

#### WxSc.Util.GetDownloadedFileNames

获取已下载的文件名称（可以在配置窗口修改下载路径，默认放在程序所在目录下的downloads文件夹）

#### WxSc.Util.DowloadSwfFirstFrame

将 swf 的第一帧转成图片，保存到设置的下载文件夹

参数：

- 链接
- 文件名
- swf 主体内容缩放
- swf 舞台缩放（可以先尝试与主体内容缩放一致，如果显示不完整，再增加）
