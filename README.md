# Atari Pong

A classic retro-style Pong game implementation using Python and Pygame, following the original 1972 Atari game design with some modern enhancements.

## Features

- Classic black and white retro aesthetic
- Single-player mode with AI opponent
- Two-player local multiplayer
- Increasing ball speed for escalating difficulty
- Score tracking and win conditions
- Pause functionality

## Game Controls

- **Player 1**: W (up), S (down)
- **Player 2**: Up Arrow (up), Down Arrow (down)
- **Pause**: Escape key
- **Menu Navigation**: Follow on-screen prompts

## Setup Instructions

### Prerequisites
- Python 3.6+ installed
- Pygame library

### Installation
1. Clone this repository or download the ZIP file
2. Install the required dependencies:
```bash
pip install -r requirements.txt
```
3. Run the game:
```bash
python pong.py
```

## Game Rules

- The ball starts in the center, moving toward a random player
- Players move paddles to hit the ball back and forth
- If the ball passes a paddle, the opposing player scores a point
- First player to reach 10 points wins
- Ball speed increases with each hit, making the game progressively more challenging

## Project Structure

```
Atari Pong/
  assets/
    images/
    sounds/
      ping.wav
      pong.wav
      score.wav
    fonts/
  pong.py
  requirements.txt
  README.md
```

## Development Notes

This project follows the original Pong game specifications, implementing a classic Pong game with:
- 800x600 pixel resolution
- 60 FPS target
- Paddle size: 10px × 100px
- Ball size: 10px × 10px
- Initial ball speed: 4px per frame
- Speed increase: +0.5px per hit (up to max 10px)

## Customization

You can customize various game parameters by modifying the constants at the top of the `pong.py` file:
- Game resolution
- Paddle and ball size
- Speed settings
- Score limit

## License

This project is open source and available for educational purposes. 