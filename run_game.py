#!/usr/bin/env python3
"""
Atari Pong Game Launcher
A simple launcher script for the Atari Pong game.
"""
import subprocess
import sys
import os

def check_dependencies():
    """Check if required dependencies are installed."""
    try:
        import pygame
        print("Pygame is installed. Version:", pygame.version.ver)
        return True
    except ImportError:
        print("Pygame is not installed.")
        return False

def install_dependencies():
    """Install required dependencies."""
    print("Installing required dependencies...")
    try:
        subprocess.check_call([sys.executable, "-m", "pip", "install", "-r", "requirements.txt"])
        print("Dependencies installed successfully!")
        return True
    except subprocess.CalledProcessError:
        print("Failed to install dependencies.")
        return False

def run_game():
    """Run the Pong game."""
    print("Launching Atari Pong...")
    try:
        subprocess.check_call([sys.executable, "pong.py"])
    except FileNotFoundError:
        print("Error: pong.py not found!")
    except subprocess.CalledProcessError:
        print("Game terminated with an error.")

def main():
    """Main function."""
    print("=" * 50)
    print("Atari Pong Game Launcher")
    print("=" * 50)
    
    # Check if running in the correct directory
    if not os.path.exists("pong.py"):
        print("Error: pong.py not found in the current directory.")
        print("Please run this script from the main game directory.")
        return
    
    # Check dependencies
    if not check_dependencies():
        print("Would you like to install the required dependencies? (y/n)")
        choice = input("> ").strip().lower()
        if choice in ['y', 'yes']:
            if not install_dependencies():
                print("Unable to continue without required dependencies.")
                return
        else:
            print("Unable to continue without required dependencies.")
            return
    
    # Run the game
    run_game()
    
    print("Thanks for playing!")

if __name__ == "__main__":
    main() 