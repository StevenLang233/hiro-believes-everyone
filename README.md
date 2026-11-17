# 希罗宝宝拯救器 | 让希罗宝宝相信大家

**作者：StevenLang** · **版本：0.1.0** · **GPT-6 Luna vibe-coding 项目**

> 此mod修复某些情况下（鬼知道啥情况）希罗宝宝无法相信大家（不能点罢了）的bug。

这是一个针对《魔法少女的魔女审判》Windows PC 版的 BepInEx 6 IL2CPP 插件，并附带 ManosabaLoader 模式菜单元数据。

## 框架与致谢

- [ManosabaMod / ManosabaLoader](https://github.com/IrisuM/ManosabaMod)，作者 IrisuM；本项目使用其 MOD 框架与元数据格式，LGPL-2.1。感谢原作者及贡献者。
- [BepInEx](https://github.com/BepInEx/BepInEx)，BepInEx 团队及贡献者，LGPL-2.1；[Il2CppInterop](https://github.com/BepInEx/Il2CppInterop)，knah、BepInEx 团队及贡献者，LGPL-3.0。
- FuyumiM 对游戏启动崩溃的分析及[源码修复](https://github.com/FuyumiM/Il2CppInterop_for_manosaba/commit/519be46765e454da5df5979a7b34c1775adf7cbb)，以及 IrisuM 分享的运行库二进制修补，见[上游问题](https://github.com/BepInEx/Il2CppInterop/issues/225)。
- 本插件的 Trial 6-12 状态恢复逻辑由本项目开发，项目代码及其许可证详见源码。框架和第三方组件按各自许可证发行。

当前快速安装包**需要预装框架**；含框架的单文件一键安装包正在准备，尚未发布。
## 修复内容

在游戏原生剧本 `Act02_Chapter01/Act02_Chapter01_Trial12` 的最后门槛处，检查原条件 `Skip6_12_3flag1=True&&Skip6_12_4flag1=True&&Skip6_12_5flag1=True`。仅当执行到该精确剧本和条件、且某个条件变量仍为 false 时，插件才将缺失标记恢复为 true。原生剧本继续负责生成可点击的伪证链接、显示答案并进入原有后续分支；插件不伪造按钮、不跳结局、不覆盖游戏剧本，也不使用 Harmony detour。

Steam 社区曾报告同一段对峙无法继续，官方在 1.0.6 更新说明中称修复了一个简中审判台词无法反驳的进度阻塞问题。该说明没有点名具体场景；本补丁的定位是恢复在 1.1.2 版本和现有存档中实际复现的门槛状态，不声称发现了全新的游戏缺陷。

## 双击安装包（Windows）

[下载双击安装包](dist/HiroBelievesEveryone-quick-v0.1.0.zip)。需要先安装 BepInEx 6 Unity IL2CPP 和 [ManosabaLoader v2.0.4](https://github.com/IrisuM/ManosabaMod/releases/tag/v2.0.4)；双击包不包含、不自动安装这些框架。关闭游戏，解压后将整个 `HiroBelievesEveryone-quick` 文件夹放到 `manosaba.exe` 所在目录，双击其中的 `Install.cmd`。已有存档在标题菜单读取。

卸载时关闭游戏并双击 `Uninstall.cmd`。安装遇到不同内容的同名文件会停止，卸载只删除内容一致的补丁文件。
## 安装

需要已安装 BepInEx 6 Unity IL2CPP 和 ManosabaLoader（通常位于 `BepInEx/plugins`）。解压后将以下文件按目录复制到游戏根目录：

- `BepInEx/plugins/ManosabaTrial12Fix.dll`
- `ManosabaMod/HiroBelievesEveryone/info.json`

也可以在 PowerShell 中运行 `install.ps1 -GameRoot "游戏根目录"`。脚本会验证依赖路径；目标文件若已存在且内容不同会拒绝覆盖。

已有进度请在标题菜单选择“读取游戏”并加载原存档。模式列表里的本项目入口是给新游戏使用的，会从原版第一章开始，不会自动加载你的存档。

## 卸载

运行 `uninstall.ps1 -GameRoot "游戏根目录"`，或手动删除上面两个文件。卸载脚本只删除哈希与发行包一致的文件，不会删除存档、BepInEx、ManosabaLoader 或其他 MOD。

## 验证记录

- 环境：Windows PC，游戏 1.1.2，Unity 6000.0.48f1，IL2CPP x64，BepInEx 6。
- 从现有自动存档 A4 读取到二周目第一场审判末段。
- 日志确认加载目标剧本，在原生门槛恢复 `Skip6_12_3flag1`、`Skip6_12_4flag1`、`Skip6_12_5flag1`。
- “令人信服”的伪证链接变为红色可点击；选择“我相信大家”后进入后续剧情。用户随后确认修复成功。
- 没有重启后再读档验证；为了保留正在继续的进度，没有自动退出游戏。
- 作用范围由精确剧本路径和完整条件表达式限定；未逐一游玩其他审判场景。

## 从源码构建

安装 .NET SDK，并设置 `GameRoot` 为本机游戏目录（构建引用本机 BepInEx、Il2CppInterop 和 Naninovel interop 文件；它们不随项目分发）：

```powershell
dotnet build .\src\Trial12Fix.csproj -c Release -p:GameRoot="D:\SteamLibrary\steamapps\common\manosaba_game"
```

项目代码使用 MIT 许可证，见 `LICENSE`。第三方工具和依赖说明见 `THIRD_PARTY_NOTICES.md`。




