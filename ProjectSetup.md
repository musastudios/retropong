# Atari Pong Project Setup

## Folder Structure
We'll follow the recommended structure from the SPECS.md file:

```
Assets/
  Sprites/
    Paddle.png
    Ball.png  
  Audio/
    Ping.wav
    Pong.wav
  Fonts/
    RetroFont.ttf
  Scripts/
    GameManager.cs
    PlayerController.cs
    BallController.cs
    ScoreManager.cs
    AIController.cs
    UI/
      MainMenu.cs
      PauseMenu.cs
      GameOver.cs
      HUD.cs
      SettingsMenu.cs
  Scenes/
    MainMenu.unity
    GameScene.unity
    SettingsScene.unity
    GameOverScene.unity
```

## Implementation Plan
1. Create basic GameObjects (paddles, ball)
2. Implement player controls and ball physics
3. Add scoring system and game logic
4. Create UI elements (score display, menus)
5. Implement AI for single-player mode
6. Add sound effects and polish

## Game Specifications
- Resolution: 800x600 pixels
- Frame Rate: 60 FPS
- Paddle size: 10px × 100px
- Ball size: 10px × 10px
- Player 1 controls: W (up), S (down)
- Player 2 controls: Up Arrow (up), Down Arrow (down)
- Score limit: 10 points 