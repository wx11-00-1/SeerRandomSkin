# SeerRandomSkin

页游赛尔号登录器，同时支持 Flash 和 H5，以 Flash 端的精灵随机换肤为特色功能，仅支持 Windows 操作系统

## 声明

- 项目全部开源，仅供学习使用，禁止用于任何商业和非法行为

- 登录器实现的所有功能，都是以改善玩家的游戏体验为出发点，无意损害游戏官方的正当权益

## 立项原因

随着游戏的发展，无论是 PVP 还是 PVE，都让我感到枯燥乏味。

完全成为“机械族”，丧失了游戏的意义；想过彻底告别，亦是难以割舍，我还是喜欢好看的精灵，华丽的特效。

偶然想到随机换肤这个点子，我兴奋得辗转难眠。如果读者看过我在 b 站上的[开发过程视频](https://space.bilibili.com/435657414/channel/collectiondetail?sid=2842294&ctype=0)，应该可以感受到我的孜孜不倦。

## 功能

- Flash 端
  - 随机换肤
  - 关电池
  - 变速
  - 更换加载背景
  - **小助手蒂朵**，提供借绿火和自动治疗功能

- H5 端
  - 保存当前套装、称号、精灵背包，一键更换
  - 战斗结束后自动治疗
  - 屏蔽游戏公告
  - 巅峰记牌器
  - 更换加载背景

## 对比

- 使用谷歌浏览器内核，有着与官方微端相同的优势：
  - 自带 Flash 插件，无需另外安装。画质更加清晰，同时更加稳定，大幅减少与 Flash 相关问题的出现
  - 支持 H5 端（表现似乎比 WebView2 内核更好）
  - 开发者工具

- 劣势：
  - 多进程架构，部分功能的开发难度增加
