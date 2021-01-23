# 概述
这是一套完整的 Unity 框架方案，提供 UI 管理、热更新、资源管理等、多线程、数据处理、定时器、解压缩等功能完备的工具集，目的是为了提高生产力，方便业务开发，这套工具集被命名为 **KSwordKit** 。

未来的预期中也将加入更多游戏技术，比如帧同步等。

# 目前框架刚开坑，欢迎大家指正
刚开始构思这个框架，主要是总结一下游戏开发技术，提高工作效率，所以在这里开坑造轮子了，希望大家给出宝贵的意见。

# **KSwordKit** 框架层次图
![**KSwordKit**](https://github.com/keenlovelife/KSwordKit/blob/master/Image/KSWordKit%E6%A1%86%E6%9E%B6%E8%AE%BE%E8%AE%A1.jpg?raw=true)

图中可以看出 **KSwordKit** 位于 Unity 之上，具体应用程序之下。 **KSwordKit** 试图为你的应用程序提供功能完备，接口丰富的快速开发工具包。

为实现这一目的， **KSwordKit** 内部的所有内容将被设计成`独立赖可插拔`的模块化系统，能够让开发者根据需要增删内容来精简自己的项目。

图中也可以看出 **KSwordKit** 内部分为两部分：`Core` 和 `Modules`

1. `Core` 内部提供大量丰富的工具类库，每个工具类库解决它对应的具体程序需求。 
2. `Modules` 是构建在 `Core` 之上的功能模块，每个模块针对具体业务场景提供完成解决方案。

而不管是 `Core` 还是 `Modules` ，它们内部的所有内容，都被 **KSwordKit** 看作独立模块。

# 模块遵循独立可插拔原则

一个独立模块是可插拔的，它可以依赖其他模块，它可以拥有自己的界面和代码。而模块间的引用依赖问题，则由 **KSwordKit** 管理。

**KSwordKit** 将会提供很多种模块，它们的设计模式、功能和依赖都会不尽相同。但这些模块都需要遵循同一个原则：**独立可插拔**

每个模块应当是独立的，以便可以直接单独复制到别的项目就可以直接使用的，这就是所谓`可插拔`。这样可以方便开发者根据需求自定义增减内容，框架更包容，不排他。

>模块：有的人叫做工具，有的人则叫做部件，有的被称为插件，在这里它们都被称为模块。

如有朋友设计并实现了某模块，欢迎给仓库提交PR，大家共同交流技术。

# 文档
**KSwordKit** 里面的每部分都会在 [Wiki](https://github.com/keenlovelife/KSwordKit/wiki) 里面撰写文档，大家使用中遇到问题可以去查阅。

# 当前框架内可用模块
1. [资源管理器](资源管理器)
2. [增强协程](增强协程)
