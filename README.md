# \# 🃏 Card Match Game (Unity Prototype)

# 

# A clean, polished, and scalable \*\*Card Match game prototype\*\* built in Unity.  

# Designed with SOLID principles, reusable architecture, and persistent game state saving.

# 

# ---

# 

# \## 📦 Features

# 

# \- 🎴 Flip-to-match card mechanic with smooth animations

# \- 🧠 Combo system (score multipliers for consecutive matches)

# \- ⏱️ Turn count and time tracking

# \- 💾 Save and resume full game session (including card layout, matched pairs, score, and more)

# \- 🎨 Dynamic grid generation (`rows × columns`)

# \- 🔊 Audio feedback for flip, match, and mismatch

# \- 📱 Clean UI for menu, in-game HUD, and game over screen

# \- 🔁 Auto-resume on launch

# \- 🧩 Built with scalability and separation of concerns

# 

# ---

# 

# \## 🧠 Architecture Overview

# 

# | Component       | Responsibility                           |

# |-----------------|-------------------------------------------|

# | `GameManager`   | Core game logic, grid generation, scoring |

# | `Card`          | Card flip, match, and UI animation        |

# | `ICard`         | Interface for card interaction            |

# | `CardGridGenerator` | Generates shuffled card pairs        |

# | `SaveSystem`    | Handles all saving/loading using `PlayerPrefs` |

# | `UIManager`     | Handles screen switching (menu, game, game over) |

# | `AudioManager`  | Plays match, mismatch, flip sounds        |

# | `GameOverPanel` | Displays stats and buttons after win      |

# 

# ---

# 

# \## 💾 Save System

# 

# The game persistently saves and loads:

# 

# \- ✅ Score

# \- ✅ Turns taken

# \- ✅ Time taken

# \- ✅ Combo count

# \- ✅ Rows × Columns grid layout

# \- ✅ Shuffled card ID layout

# \- ✅ Matched card IDs

# 

# \### 🧠 How it works

# 

# `SaveSystem` uses `PlayerPrefs` to store data. It serializes `List<int>` values as comma-separated strings.

# 

# ---

# 

# \## ▶️ How to Use

# 

# \### 🟢 On Game Start

# If saved data exists, game automatically resumes the previous session.

# 

# \### 🔁 Restarting Game

# Use "Restart" button to reset the game with same layout and start fresh.

# 

# ---

# 

# \## 🔧 Setup

# 

# 1\. Unity version: `2021.3 LTS` or higher

# 2\. Import this project or `.unitypackage`

# 3\. Connect your UI buttons to:

# &nbsp;  - `UIManager.ShowGame()`, `UIManager.ShowMainMenu()`

# &nbsp;  - `GameManager.GenerateGrid()` (if not resuming)

# 4\. Set card sprites and prefabs via Inspector

# 5\. Assign audio clips to `AudioManager`

# 

# ---

# 

# \## 📁 Project Structure (Key Scripts)

# 

\# 🃏 Card Match Game (Unity Prototype)



A clean, polished, and scalable \*\*Card Match game prototype\*\* built in Unity.  

Designed with SOLID principles, reusable architecture, and persistent game state saving.



---



\## 📦 Features



\- 🎴 Flip-to-match card mechanic with smooth animations

\- 🧠 Combo system (score multipliers for consecutive matches)

\- ⏱️ Turn count and time tracking

\- 💾 Save and resume full game session (including card layout, matched pairs, score, and more)

\- 🎨 Dynamic grid generation (`rows × columns`)

\- 🔊 Audio feedback for flip, match, and mismatch

\- 📱 Clean UI for menu, in-game HUD, and game over screen

\- 🔁 Auto-resume on launch

\- 🧩 Built with scalability and separation of concerns



---



\## 🧠 Architecture Overview



| Component       | Responsibility                           |

|-----------------|-------------------------------------------|

| `GameManager`   | Core game logic, grid generation, scoring |

| `Card`          | Card flip, match, and UI animation        |

| `ICard`         | Interface for card interaction            |

| `CardGridGenerator` | Generates shuffled card pairs        |

| `SaveSystem`    | Handles all saving/loading using `PlayerPrefs` |

| `UIManager`     | Handles screen switching (menu, game, game over) |

| `AudioManager`  | Plays match, mismatch, flip sounds        |

| `GameOverPanel` | Displays stats and buttons after win      |



---



\## 💾 Save System



The game persistently saves and loads:



\- ✅ Score

\- ✅ Turns taken

\- ✅ Time taken

\- ✅ Combo count

\- ✅ Rows × Columns grid layout

\- ✅ Shuffled card ID layout

\- ✅ Matched card IDs



\### 🧠 How it works



`SaveSystem` uses `PlayerPrefs` to store data. It serializes `List<int>` values as comma-separated strings.



---



\## ▶️ How to Use



\### 🟢 On Game Start

If saved data exists, game automatically resumes the previous session.



\### 🔁 Restarting Game

Use "Restart" button to reset the game with same layout and start fresh.



---



\## 🔧 Setup



1\. Unity version: `2021.3 LTS` or higher

2\. Import this project or `.unitypackage`

3\. Connect your UI buttons to:

&nbsp;  - `UIManager.ShowGame()`, `UIManager.ShowMainMenu()`

&nbsp;  - `GameManager.GenerateGrid()` (if not resuming)

4\. Set card sprites and prefabs via Inspector

5\. Assign audio clips to `AudioManager`



---



\## 📁 Project Structure (Key Scripts)



Assets/

├── Scripts/

│ ├── GameManager.cs

│ ├── Card.cs

│ ├── ICard.cs

│ ├── CardGridGenerator.cs

│ ├── SaveSystem.cs

│ ├── UIManager.cs

│ ├── GameOverPanel.cs

│ ├── AudioManager.cs





---



\## 🎮 Screens



\- Main Menu  

\- In-Game HUD  

\- Game Over Popup  

\- Dynamic card grid  

\- Combo feedback text



---



\## 🚀 Future Ideas



\- 👤 Multiple user profiles

\- ☁️ Cloud save with Firebase

\- 📱 Mobile build with touch support

\- 🔄 Pause/Resume system

\- 🎨 Theming and skins

\- 🧠 Difficulty levels (time limits, traps)



---



\## ✅ Built With



\- Unity 2021.3 LTS

\- C# (SOLID design principles)

\- UGUI

\- `PlayerPrefs` for persistence



---



\## 🧑‍💻 Author



\*\*Nilankar Deb\*\* — Game Developer  

🛠️ Built with a clean dev mindset and zero bullshit architecture.  

This ain’t no spaghetti 🍝



---





