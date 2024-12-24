# A* 寻路算法 Unity 实现 🚀

[![Unity](https://img.shields.io/badge/Unity-2023.x-blue.svg)](https://unity.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![GitHub Stars](https://img.shields.io/github/stars/3034337688/Csahrp_demo_study?style=social)](https://github.com/3034337688/Csahrp_demo_study)

这个仓库包含了一个简单但功能强大的 A*（A-star）寻路算法在 Unity 中的实现。该项目旨在帮助你理解和可视化 A* 算法在 2D 网格环境中的工作原理。

## 🌟 功能亮点

- **A* 寻路算法**：高效地在网格中找到两点之间的最短路径。
- **动态网格初始化**：网格大小和障碍物位置可以动态生成。
- **可视化路径**：通过颜色区分可行走区域、障碍物和寻路路径。
- **交互式操作**：用户可以通过鼠标点击选择起点和终点，实时查看寻路结果。

## 📂 项目结构

- **AStarManager.cs**：A* 管理器，负责初始化地图、执行寻路算法。
- **AStarNode.cs**：A* 节点类，包含节点的坐标、类型（可行走或障碍）、以及寻路相关的消耗值（F, G, H）。
- **TestAStar.cs**：测试脚本，负责生成网格、处理用户输入、并可视化寻路结果。

## 🚀 如何使用

1. **克隆仓库**：
   ```bash
   git clone https://github.com/3034337688/Csahrp_demo_study.git
   ```

2. **打开 Unity 项目**：
   - 将项目导入 Unity 编辑器。
   - 在场景中运行项目，确保 `TestAStar` 脚本挂载在主摄像机或其他合适的 GameObject 上。

3. **交互操作**：
   - 点击鼠标左键选择起点。
   - 再次点击鼠标左键选择终点。
   - 系统将自动计算并显示从起点到终点的最短路径。

## 🎨 可视化说明

- **绿色**：表示寻路路径。
- **黄色**：表示起点和终点。
- **红色**：表示障碍物。
- **默认颜色**：表示可行走区域。

## 📸 示例

![示例图片](https://via.placeholder.com/600x300.png?text=A*%20Pathfinding%20Demo)

## 🤝 贡献

欢迎任何形式的贡献！如果你有任何改进建议或发现了 bug，请提交 issue 或 pull request。

## 📜 许可证

本项目采用 MIT 许可证，详情请参阅 [LICENSE](LICENSE) 文件。

---

**Enjoy coding and exploring the A* algorithm!** 🚀
