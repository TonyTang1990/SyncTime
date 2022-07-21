# 网络对时

## 工具

1. Unity版本(2020 3.34f1c2)
2. Visual Studio 2019

## 最终目标

游戏开发过程中经常会设计到时间的运算判定，比如开服时间，触发时间，倒计时时间等，所有的这些时间都要基于和服务器或网络对时后的网络时间为基准来计算。单纯采用本地客户端时间的话会导致通过修改本地客户端时间就能达到加速和调时间的目的。本章节通过深入学习网络对时，实现一套没有服务器也能正确网络对时的对时方案(**如果有后端的话以后端时间对时为准即可,本人是出于不懂服务器开发所以才采用网络对时的方案**)。

## 网络对时

### UTC时间

**当我们开发多个国家的游戏的时候，为了确保时间的统一，这个时候我们就可以采用UTC时间作为时间标准，每个国家的时间戳计算标准都是统一的(不需要考虑时区问题)。**

了解了UTC时间后，这里本人**使用NTP服务来实现没有专属服务器时的网络对时。**

### NTP(网络对时服务)

NTP(Network Time Protocol)(网络对时服务)**

[通过在网络报文上打「时间戳」的方式，然后配合计算网络延迟，从而修正本机的时间。](https://www.zhihu.com/question/21045190)

![网络对时请求](/img/SyncTime/NTPTimeCaculation.jpg)

[网络延时 = (t4 - t1) - (t3 - t2)](https://www.zhihu.com/question/21045190)

[网络延时 = (t4 - t1) - (t3 - t2)](https://www.zhihu.com/question/21045190)

[NTP报文格式](/img/SyncTime/NTPDataContent.png)

**可以看到通过包含发送，接收，返回和返回接收时间戳的方式，我们可以计算出网络延迟从而做到正确的同步的网络时间**

**网路延时 = (客户端接收时间 - 客户端发送时间) - (服务器返回消息时间 - 服务器接受消息时间)**
 **时间差 = 服务器接受消息时间 - 客户端发送时间 - 网络延时 / 2 = ((服务器接受消息时间 - 客户端发送时间) + (服务器返回消息时间 - 客户端接收时间)) / 2**
**当前同步服务器时间 = 客户端接收时间 + 时间差**

### 实战

网络对时方案:

1. **使用阿里云提供的NTP网址作为NTP服务器**
2. **Socket短连接方式获取NTP报文数据**
3. **当前同步网络时间 = 网络对时 + (本地真实运行时长 - 对时时本地真实运行时长**

**这里我把本地时间同步关闭，并修改时间从2022/07/22 00:06到2022/07/24/03:06时间**

![同步时间前](/img/SyncTime/BeforeSyncTime.PNG)

![同步时间后](/img/SyncTime/AfterSyncTime.PNG)

从上面可以看到通过NTP的对时后，我们成功获取到了网络的UTC时间。

**当前同步网络时间 = 网络对时 + (本地运行时长 - 对时时本地运行时长)**

![当前同步网络UTC时间](/img/Sync/AfterSyncAddRealtime.PNG)

**可以看到我成功的通过网络对时和记录本地真实运行时间等信息成功计算出了当前网络对时时间。**


## 重点知识

1. **同步网络对网络要求不高，可以采用UDP**
2. **NTP报文里有服务器相关获得请求时间和服务求返回请求时间，通过这些数据的解析我们能计算出真实的网络时间**
3. **本地最新网络时间需要通过记录对时时的运行时间来计算最新的当前网络时间**
4. **网络时间同步没必要一直同步，可以采用短连接在必要的时候同步一次**

## 博客

[网络对时](http://tonytang1990.github.io/2022/07/16/%E6%97%B6%E9%97%B4%E5%90%8C%E6%AD%A5/)