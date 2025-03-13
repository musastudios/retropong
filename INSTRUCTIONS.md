# Atari Pong - Game Instructions

## Table of Contents
- [Installation](#installation)
- [Game Overview](#game-overview)
- [Controls](#controls)
- [Game Modes](#game-modes)
- [Gameplay Rules](#gameplay-rules)
- [Scoring](#scoring)
- [Tips & Strategies](#tips--strategies)
- [Troubleshooting](#troubleshooting)
- [Customization](#customization)

## Installation

### Prerequisites
- Python 3.6 or higher
- Pygame library

### Setup Instructions
1. **Install Python**:
   - Download and install Python from [python.org](https://python.org)
   - Make sure to check "Add Python to PATH" during installation

2. **Install Pygame**:
   ```bash
   # Using pip (standard method)
   pip install pygame
   
   # If you encounter build errors on macOS, try:
   pip install --no-cache-dir --pre -i https://pypi.org/simple/ pygame --prefer-binary
   ```

3. **Run the Game**:
   ```bash
   # Using the launcher script (recommended)
   python run_game.py
   
   # Or run directly
   python pong.py
   ```

## Game Overview

Atari Pong is a classic 2D table tennis game originally developed by Atari in 1972. This recreation features the same simple yet addictive gameplay with some modern enhancements.

The game features:
- Classic black and white retro aesthetic
- Single-player mode with AI opponent
- Two-player local multiplayer
- Increasing ball speed for escalating difficulty
- Score tracking and win conditions

## Controls

### Menu Navigation
- **1**: Start Single Player game
- **2**: Start Two Player game
- **Q**: Quit game
- **ESC**: Return to main menu (when paused or at game over)
- **SPACE**: Resume game (when paused)
- **R**: Restart game (at game over screen)

### In-Game Controls
- **Player 1**:
  - **W**: Move paddle up
  - **S**: Move paddle down

- **Player 2**:
  - **↑** (Up Arrow): Move paddle up
  - **↓** (Down Arrow): Move paddle down

- **ESC**: Pause game

## Game Modes

### Single Player
- Play against an AI-controlled opponent
- The AI adapts to the ball's movement and tries to predict its trajectory
- AI difficulty is set to medium by default (can be customized in the code)

### Two Player
- Play against a friend on the same computer
- Player 1 uses W/S keys
- Player 2 uses Up/Down arrow keys

## Gameplay Rules

1. The ball starts in the center of the screen, moving in a random direction
2. Players control paddles on opposite sides of the screen
3. The objective is to hit the ball with your paddle to send it back to your opponent
4. If a player misses the ball, the opponent scores a point
5. The ball speed increases slightly with each paddle hit, making the game progressively more challenging
6. First player to reach 10 points wins the game

## Scoring

- Score a point when your opponent misses the ball
- The current score is displayed at the top of the screen
- First player to reach 10 points wins the match
- After a point is scored, the ball resets to the center with a brief pause before launching again

## Tips & Strategies

### For Beginners
- Focus on tracking the ball's movement rather than trying to predict it
- Stay near the center of your side when the ball is far away
- Move early rather than waiting until the last moment

### Advanced Techniques
- Hit the ball with the edge of your paddle to create steeper angles
- The ball's bounce angle depends on where it hits your paddle:
  - Center hits produce straight returns
  - Edge hits create sharper angles
- Try to anticipate your opponent's movements to gain an advantage

## Troubleshooting

### Common Issues

**Game doesn't start**:
- Ensure Python and Pygame are properly installed
- Try running with the launcher script: `python run_game.py`
- Check console for error messages

**No sound effects**:
- The game uses placeholder sound files
- To add sound, place WAV files in the `assets/sounds/` directory:
  - `ping.wav`: For wall collisions
  - `pong.wav`: For paddle collisions
  - `score.wav`: For when a point is scored

**Game performance issues**:
- If the game runs too fast or slow, adjust the `FPS` constant in `pong.py`
- Close other applications to free up system resources

## Customization

You can customize various aspects of the game by editing the constants at the top of the `pong.py` file:

```python
# Game constants
WINDOW_WIDTH = 800      # Change window width
WINDOW_HEIGHT = 600     # Change window height
FPS = 60                # Adjust frame rate
PADDLE_WIDTH = 10       # Change paddle width
PADDLE_HEIGHT = 100     # Change paddle height
BALL_SIZE = 10          # Change ball size
PADDLE_SPEED = 5        # Adjust paddle movement speed
INITIAL_BALL_SPEED = 4  # Set starting ball speed
BALL_SPEED_INCREMENT = 0.5  # How much speed increases per hit
MAX_BALL_SPEED = 10     # Maximum ball speed
SCORE_TO_WIN = 10       # Points needed to win
```

### Adding Custom Fonts
1. Place your font file (e.g., `retro.ttf`) in the `assets/fonts/` directory
2. The game will automatically use it if available

### Adding Custom Sounds
1. Place your sound files in the `assets/sounds/` directory:
   - `ping.wav`: Wall collision sound
   - `pong.wav`: Paddle collision sound
   - `score.wav`: Scoring sound
2. The game will automatically use them if available

---

Enjoy playing Atari Pong! This recreation aims to capture the simple yet addictive gameplay of the original 1972 classic while adding some modern features for enhanced enjoyment. 