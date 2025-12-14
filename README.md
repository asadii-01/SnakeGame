# 🐍 Classic Snake Game (C# Windows Forms)

A modern, feature-rich iteration of the classic Snake arcade game built using **C#** and **Windows Forms (.NET Framework)**.

This project goes beyond the basics by introducing a risk-reward food system, progressive difficulty scaling, directional sprite rendering, and a polished UI.

![StartView](https://github.com/asadii-01/SnakeGame/blob/main/ScreenShots/StartView.png)

## ✨ Features

* **Dynamic Food System**: It's not just about eating apples anymore.
    * 🍎 **Normal Food**: Standard growth (+10 pts).
    * 🍊 **Fast Food**: Speed boost (+10 pts, increases game speed).
    * 🍇 **Slow Food**: Slows down time (+10 pts, decreases game speed).
    * 🍋 **Bonus Food**: Massive score boost (+50 pts).
* **Progressive Difficulty**: The game gets harder as you play. Every **50 points**, the speed increases and new obstacles (rocks) spawn on the map.
* **Sprite-Based Rendering**: Full directional support for the snake's head and tail, plus context-aware body segments (horizontal vs. vertical) for a natural look.
* **Persistent High Scores**: Your best runs are saved locally.
* **Polished UI**: Includes a dedicated Start Screen with instructions, a real-time HUD for score/effects, and background music.

## 🎮 Gameplay

![NormalView](https://github.com/asadii-01/SnakeGame/blob/main/ScreenShots/NormalView.png)
![PausedView](https://github.com/asadii-01/SnakeGame/blob/main/ScreenShots/PausedView.png)
![GameOverView](https://github.com/asadii-01/SnakeGame/blob/main/ScreenShots/GameOverView.png)

### Controls

| Key | Action |
| :--- | :--- |
| **Arrow Keys** | Move Snake (Up, Down, Left, Right) |
| **P** or **Space** | Pause / Resume Game |
| **Enter** | Start Game / Retry after Game Over |

## 🛠️ Built With

* **Language**: C#
* **Framework**: .NET Framework (Windows Forms)
* **IDE**: Visual Studio

## 🚀 Getting Started

### Prerequisites

* Visual Studio 2019 or later.
* .NET Framework 4.7.2 or later installed.

### Installation

1.  **Clone the repository**
    ```bash
    git clone [https://github.com/asadii-01/SnakeGame](https://github.com/asadii-01/SnakeGame)
    ```
2.  **Open the Project**
    * Navigate to the folder and open `SnakeGame.sln` in Visual Studio.
3.  **Asset Setup (Crucial Step)**
    * Ensure all image (`.png`) and sound (`.wav`) files are present in the project directory.
    * **Important:** When building, ensure these assets are copied to the output directory (`bin/Debug` or `bin/Release`). You can do this by selecting the files in Solution Explorer, going to Properties, and setting **"Copy to Output Directory"** to **"Copy if newer"**.
4.  **Build and Run**
    * Press `F5` or click **Start** in Visual Studio.

## 📂 Project Structure

* `Form1.cs`: Contains the main game loop, rendering logic, and input handling.
* `Circle.cs`: Helper class defining grid coordinates for snake segments and objects.
* `Settings.cs`: Static class managing global game state (Speed, Score, Points).
* `Input.cs`: Helper class for optimizing keyboard state detection.

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👏 Acknowledgments

* Original concept based on the classic arcade game.
* Icons and sprites were sourced from [OpenGameArt/Custom Sources].
