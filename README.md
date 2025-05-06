# WPF MVVM INotifyPropertyChanged 示例

这是一个基于.NET Core 8.0的WPF应用程序，展示了如何使用MVVM（Model-View-ViewModel）设计模式实现INotifyPropertyChanged接口，以实现UI与数据的双向绑定。

## 项目结构

项目采用完全分离的MVVM架构，包含以下目录结构：

- **Models**：数据模型层
  - `Person.cs`：人员数据模型类
  - `Product.cs`：产品数据模型类

- **ViewModels**：视图模型层
  - `MainViewModel.cs`：主视图模型类
  - `PersonViewModel.cs`：人员视图模型类
  - `ProductViewModel.cs`：产品视图模型类（演示高级功能）

- **Views**：视图层
  - `PersonView.xaml`：人员信息视图
  - `ProductView.xaml`：产品信息视图（演示高级功能）
  - `AdvancedDemoWindow.xaml`：高级功能演示窗口

- **Common**：公共组件
  - `NotifyPropertyBase.cs`：实现INotifyPropertyChanged接口的基类
  - `AdvancedNotifyPropertyBase.cs`：实现高级INotifyPropertyChanged功能的基类
  - `RelayCommand.cs`：实现ICommand接口的命令类

## 技术要点

### 基础 INotifyPropertyChanged 功能
1. **基本实现**：
   - 通过基类实现属性变更通知机制
   - 使用CallerMemberName特性简化属性通知
   - SetProperty辅助方法减少重复代码

2. **命令模式**：
   - 使用RelayCommand实现ICommand接口
   - 支持命令的可执行状态判断

3. **数据绑定**：
   - 实现双向绑定（TwoWay）
   - 实现只读绑定（OneWay）
   - 使用UpdateSourceTrigger控制绑定更新时机

### 高级 INotifyPropertyChanged 功能
1. **INotifyPropertyChanging 接口**：
   - 实现属性变更前通知
   - 允许在属性变化前执行操作

2. **批量更新模式**：
   - BeginUpdate/EndUpdate 方法
   - 支持多个属性同时更新但只发送一次通知
   - UsingBatchMode 辅助方法

3. **延迟通知**：
   - 使用 DispatcherTimer 实现延迟通知
   - 支持高频率属性变化时合并通知
   - 提高高频更新场景下的性能

4. **属性依赖关系处理**：
   - SetPropertyWithDependencies 方法
   - 自动通知依赖属性的变化

5. **条件编译与调试支持**：
   - 在调试模式下提供额外验证
   - 提高开发时的错误检测能力

6. **资源管理**：
   - 实现 IDisposable 接口
   - 正确清理延迟通知的计时器等资源

## 运行环境

- .NET Core 8.0 或更高版本
- 支持WPF的操作系统（Windows）
- Visual Studio 2022 或更高版本

## 如何运行

1. 使用Visual Studio打开`WPF_INotifyPropertyChanged.sln`解决方案文件
2. 按F5或点击"开始调试"按钮运行项目
3. 应用程序启动后，可以在两个选项卡之间切换：
   - **基本属性通知**：展示基本的INotifyPropertyChanged功能
   - **高级属性通知**：展示高级INotifyPropertyChanged功能

## 高级功能演示说明

高级属性通知选项卡中提供了以下功能演示：

1. **批量更新**：同时修改多个属性，但只触发一次通知，减少UI重绘次数
2. **延迟通知**：在快速连续修改同一属性时（如输入框快速输入），只在最后一次修改触发通知
3. **属性依赖**：当一个属性变化时（如价格或数量），自动更新相关联的属性（如总价值）
4. **通知所有属性**：使用空字符串参数通知所有属性变化的技术

## 开发建议

1. 对于简单项目，使用基本的NotifyPropertyBase类已足够
2. 对于性能要求高、属性更新频繁或有复杂依赖关系的场景，考虑使用AdvancedNotifyPropertyBase类
3. 在UI线程频繁更新的场景中，批量更新和延迟通知可以显著提高性能
4. 使用属性依赖机制可以简化代码，减少手动更新相关属性的需要

## 授权

本项目采用MIT许可证开源。 