# SeerRandomSkin

页游赛尔号登录器，同时支持 Flash 和 H5，以 Flash 端的精灵随机换肤为特色功能，仅支持 Windows 操作系统

## 声明

- 项目全部开源，仅供学习使用，禁止用于任何商业和非法行为
- 登录器实现的所有功能，都是以改善玩家的游戏体验为出发点，无意损害游戏官方的正当权益

## 使用

下载 Releases 中的压缩包，解压，运行 SeerRandomSkin.exe

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
因为站在了巨人的肩膀上（直接抄代码)，封包功能方面少踩了一些坑。
变速功能就难多了，好在最后还是找到了能参考的项目；一个有趣的地方是，能够实现只加速精灵出招动画，不加速倒计时！（有个叫彩虹的登录器，也用到了这个技巧）

后来又通过 TO 登录器认识了 CefSharp，开了个新的巨坑。。。

从24年开始，游戏环境剧烈变动。无论是 PVP 还是 PVE，都让我感到枯燥乏味。
完全成为“机械族”，丧失了游戏的意义；想过彻底告别，亦是难以割舍，我还是喜欢好看的精灵，华丽的特效，偶然想到的随机换肤这个点子，也是我选择留下来的重要原因。

我在 b 站上有部分[开发过程视频](https://space.bilibili.com/435657414/channel/collectiondetail?sid=2842294&ctype=0)，权当纪念。有同好进行技术交流，解决难题，也是意外之喜。

## 功能

- Flash 端
  - **小助手蒂朵**，在游戏内部提供借绿火和自动治疗等功能
  - 随机换肤
  - 关电池
  - 变速
  - 更换加载背景
  - 自动出招
  - 读取封包
  - 用 js 制作日常脚本

- H5 端
  - 保存当前套装、称号、精灵背包，一键更换
  - 战斗结束后自动治疗
  - 屏蔽游戏公告
  - 巅峰记牌器
  - 更换加载背景

## 对比

- 使用谷歌浏览器内核，有着与官方微端相同的优势：
  - 自带 Flash 插件，无需另外安装。画质更加清晰，同时更加稳定，大幅减少与 Flash 相关问题的出现
  - 支持 H5 端（性能似乎比 WebView2 内核更好）
  - 开发者工具
  - 与网页通过 js 代码进行交互

- 劣势：
  - 多进程架构，部分功能的开发难度增加
  - 变速效果与以往的 IE 内核登录器有所不同

## API
WxFightHandler 对象提供的属性和方法，通过 js 代码调用，用于编写日常脚本（登录器自带了一些脚本例子，参照着文档看，更容易理解；可以直接在开发者工具的控制台测试）
### 1 对战相关
#### 1.1 WxFightHandler.Utils.GetRound
无参数。获取本次对战经过的回合数
#### 1.2 WxFightHandler.Utils.UseSkill
##### 说明
使用技能
##### 参数
###### skillID
技能 ID
##### 示例
```js
WxFightHandler.Utils.UseSkill(19940); // 使用技能：违·永恒之寂
```
#### 1.3 WxFightHandler.Utils.ChangePet
##### 说明
切换精灵
##### 参数
###### petCatchTime
精灵获取时间，常用于标识精灵
##### 示例
```js
// 在一般出招回合触发
WxFightHandler.OnUseSkill = (mySkillInfo,enemySkillInfo) => {
  if (mySkillInfo.remainHP !== 0) {
    WxFightHandler.Utils.UseSkill(0); // 我方精灵存活，使用技能（0技能表示此回合弃权，不出招）
  }
  else {
    for(var pet of mySkillInfo.changehps) { // 遍历我方除在场精灵外的其他精灵，若存活则切换上场
      if(pet.hp>0) { WxFightHandler.Utils.ChangePet(pet.id); break; } // 注意此处的 pet.id，它的数值实际上就是精灵的获取时间，只是官方在这里混用了，大多数情况下 id 并不等于 catchTime
    }
  }
};
```
#### 1.4 WxFightHandler.Utils.UsePetItem
##### 说明
在战斗中使用道具（药剂、胶囊等）
##### 参数
###### itemID
道具 ID
##### 示例
```js
WxFightHandler.Utils.UsePetItem(300011); // 使用回 20 血的药剂
```
#### 1.5 WxFightHandler.Utils.UsePetItem10PP
##### 说明
无参数。使用回 10 pp 的药剂
#### 1.6 WxFightHandler.Utils.ItemBuy
##### 说明
购买道具
##### 参数
###### itemID
道具 ID
##### 示例
```js
WxFightHandler.Utils.ItemBuy(300017); // 购买 10 pp 药剂，可以在战斗中使用
```
#### 1.7 WxFightHandler.Utils.GetFightingPetID
##### 说明
无参数。在战斗中，获取当前在场精灵的 ID
##### 示例
```js
WxFightHandler.OnUseSkill = (mySkillInfo,enemySkillInfo) => {
  let petID = WxFightHandler.Utils.GetFightingPetID();
  // 当 3322 精灵（茉蕊儿）阵亡，4377 精灵跟着上场（获得500护盾）
  if (mySkillInfo.remainHP === 0) {
    if (3322 === petID) { WxFightHandler.Utils.ChangePetByID([4377]); }
  }
};
```
#### 1.8 WxFightHandler.Utils.ChangePetByID
##### 说明
换上指定 ID 数组中的的精灵
##### 参数
###### idArray
精灵 ID **数组**
##### 示例
```js
// 首回合触发
WxFightHandler.OnFirstRound = () => {
  WxFightHandler.KELUO = 2977;
  WxFightHandler.DIDUO = 4377;
  WxFightHandler.IsDIDUOFirstUp = true;
  WxFightHandler.Utils.UseSkill(0);
};
// 发生死亡切换时触发
WxFightHandler.OnChangePet = (petInfo) => {
  let petID = petInfo.petID;
  // 这一段的目的是：首发1级草王送死，给蒂朵500护盾；蒂朵第一次上场时立刻切换上1级神寂·克罗诺斯送死，锁伤250保护蒂朵第二次上场，顺利自爆
  // 完整的代码当然不止这点，这里主要演示 ChangePetByID 方法的使用
  if (WxFightHandler.DIDUO === petID) {
    if (WxFightHandler.IsDIDUOFirstUp) {
      WxFightHandler.Utils.ChangePetByID([WxFightHandler.KELUO]);
      WxFightHandler.IsDIDUOFirstUp = false;
    } else {
      WxFightHandler.Utils.UseSkill(35914);
    }
  }
};
```
#### 1.9 WxFightHandler.Utils.Delay
##### 说明
等待一段时间。有些日常关卡出招太快会掉线；调试时可以使用，方便观察脚本运行情况
##### 参数
###### millisecond
毫秒（一毫秒 = 一千分之一秒）
##### 示例
```js
WxFightHandler.OnFirstRound = async () => { // 函数要加 async 标识
  await WxFightHandler.Utils.Delay(1000);
  console.log('开始对战');
  WxFightHandler.Utils.UseSkill(0);
};
```
#### 1.10 WxFightHandler.Utils.GetFightingPets

##### 说明

无参数。获取本场战斗中，我方出战精灵的部分信息（id、catchTime、hp、skillArray 等）

#### 1.11 WxFightHandler.Utils.GetBag1

##### 说明

无参数。获取出招背包的精灵信息（GetBag2 是备战背包，即下面那一行）

#### 1.12 WxFightHandler.Utils.GetStoragePets

##### 说明

无参数。获取仓库中所有精灵

##### 示例

```js
let GetPetCatchtimeByID = async (id) => {
    let pets = await WxFightHandler.Utils.GetStoragePets();
    //pets = pets.filter(pet => pet.id===id).sort((a,b) => a.level-b.level); // 按精灵等级升序排列
    pets = pets.filter(pet => pet.id===id).sort((a,b) => b.level-a.level); // 降序
    return pets.length===0 ? 0 : pets[0].catchTime; // 找不到就返回 0；否则返回排在最前面的
};
```

#### 1.13 WxFightHandler.Utils.SetPetBag

##### 说明

更换背包中的精灵

##### 参数

###### bag1

出战背包精灵的 catchTime 数组

###### bag2

备注背包（默认值为空数组）

##### 示例

```js
await WxFightHandler.Utils.SetPetBag([1587000619],[1587000621]);
// 发起对战
```

#### 1.14 WxFightHandler.Utils.GetClothes

##### 说明

无参数。获取套装 id 数组

#### 1.15 WxFightHandler.Utils.ChangeCloth

##### 说明

更换套装

##### 参数

###### clothes

套装数组

##### 示例

```js
WxFightHandler.Utils.ChangeCloth([1300670, 0, 1300671, 0, 1300672, 0, 1300673, 0]); // 银翼骑士套装
```

#### 1.16 WxFightHandler.Utils.GetTitle

##### 说明

无参数。获取当前称号 id

#### 1.17 WxFightHandler.Utils.SetTitle

##### 说明

更换称号

##### 参数

###### title

称号 id

### 2 发包函数

#### 2.1 WxFightHandler.Utils.Send
##### 说明
发送封包
##### 参数
###### commandID
命令号
###### ...args
任意数量的整数参数
#### 2.2 WxFightHandler.Utils.SendAsync
##### 说明
发包 并接收返回值
##### 参数
###### commandID
命令号
###### parameterArray
整数参数**数组**
### 3 xml
#### 3.1 WxFightHandler.Utils.GetItemNameByID
##### 说明
根据 ID 获取道具名称
##### 参数
###### ID
#### 3.2 WxFightHandler.Utils.GetPetNameByID
##### 说明
根据 ID 获取精灵名称
##### 参数
###### ID
#### 3.3 WxFightHandler.Utils.GetSkillNameByID
##### 说明
根据 ID 获取技能名称
##### 参数
###### ID
### 4 其他

#### 4.1 WxFightHandler.Utils.GetAllCloth

##### 说明
无参数。获取游戏内所有套装部件的信息

#### 4.2 WxFightHandler.Utils.GetItemNumByID

##### 说明

根据 id 获取物品数量

##### 参数

###### ID

#### 4.3 WxFightHandler.Utils.AutoFight

##### 说明

自动向地图上的野生精灵发起对战

##### 参数

###### ID

野生精灵的 ID

##### 示例

```js
// 先去克洛斯星
WxFightHandler.OnFirstRound = () => {
    // 对战首回合，使用特殊胶囊捕捉
    WxFightHandler.Utils.Send(2409,300505);
    WxFightHandler.Utils.ItemBuy(300505);
};
WxFightHandler.Utils.AutoFight(164); // 闪光皮皮
```

