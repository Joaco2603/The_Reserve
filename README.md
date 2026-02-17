# The Reserve - Multiplayer 3D Game

**The Reserve** is a multiplayer 3D video game focused on environmental restoration through teamwork. Players work together to collect garbage, recycle materials, plant trees, and solve puzzles to rejuvenate the game world.

<p align="center">
  <!-- Add a screenshot here later -->
</p>

## 🎮 Game Overview

In *The Reserve*, you join other players in a shared world that needs cleaning. The gameplay revolves around cooperative mechanics where communication and coordination are key to success.

### Key Features

*   **Multiplayer Co-op:** Built with Unity Netcode for GameObjects and Unity Relay for seamless online play. Join lobbies or host your own sessions.
*   **Garbage Collection:** Roam the environment to find and collect various types of trash (plastic, etc.).
*   **Recycling Mechanics:** Process collected waste to earn resources or points.
*   **Environmental Restoration:** Use resources to plant trees and restore the natural beauty of the reserve.
*   **Dynamic Day/Night Cycle:** Experience the game world changing as time passes.
*   **Puzzles:** Solve environmental challenges to unlock new areas or bonuses.

## 🛠️ Tecnologías Usadas

*   **Engine:** Unity 2020.1.6f1
*   **Networking:**
    *   Unity Netcode for GameObjects (NGO)
    *   Unity Relay
    *   Unity Lobby Service
*   **Graphics:**
    *   Universal Render Pipeline (URP) to ensure performance across devices.
    *   TextMeshPro for crisp UI text.
*   **Assets:** Uses standard Unity Starter Assets for character controllers and various low-poly asset packs for the environment.

## 🚀 Getting Started

To get a local copy up and running follow these simple steps.

### Prerequisites

*   Unity Hub
*   Unity Editor version **2020.1.6f1**

### Installation

1.  Clone the repository:
    ```sh
    git clone https://github.com/yourusername/The_Reserve.git
    ```
2.  Open Unity Hub and add the project folder.
3.  Open the project with Unity 2020.1.6f1.
4.  Wait for Unity to import assets and resolve packages.

### Playing the Game

1.  Navigate to `Assets/Scenes/Menus` in the Project window.
2.  Open the **MenuInicio** scene.
3.  Press **Play** in the Unity Editor.
4.  Use the UI to Host or Join a game.

## 📂 Project Structure

*   `Assets/Scripts`: Contains all the game logic C# scripts.
    *   `Core`: Core game systems.
    *   `Lobbys` & `RelayManager.cs`: Networking logic.
    *   `Trash` & `Recicle`: Garbage collection mechanics.
    *   `TreeMechanics`: Planting system.
*   `Assets/Scenes`: Game levels and menus.
*   `Assets/Prefabs`: Reusable game objects (Players, UI, Trash items).

## 🤝 Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

1.  Fork the Project
2.  Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3.  Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4.  Push to the Branch (`git push origin feature/AmazingFeature`)
5.  Open a Pull Request

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.

## 📞 Contact

Project Link: [https://github.com/yourusername/The_Reserve](https://github.com/yourusername/The_Reserve)
