#!/usr/bin/env python3
"""
Atari Pong Game
A classic 2D table tennis game originally developed by Atari in 1972.
"""
import pygame
import sys
import random
import math
from enum import Enum

# Initialize pygame
pygame.init()
pygame.font.init()
pygame.mixer.init()

# Game constants
WINDOW_WIDTH = 800
WINDOW_HEIGHT = 600
FPS = 60
PADDLE_WIDTH = 10
PADDLE_HEIGHT = 100
BALL_SIZE = 10
PADDLE_SPEED = 5
INITIAL_BALL_SPEED = 4
BALL_SPEED_INCREMENT = 0.5
MAX_BALL_SPEED = 10
SCORE_TO_WIN = 10

# Colors
BLACK = (0, 0, 0)
WHITE = (255, 255, 255)

# Game state
class GameState(Enum):
    MAIN_MENU = 0
    PLAYING = 1
    PAUSED = 2
    GAME_OVER = 3

# Create the game window
screen = pygame.display.set_mode((WINDOW_WIDTH, WINDOW_HEIGHT))
pygame.display.set_caption("Atari Pong")
clock = pygame.time.Clock()

# Load fonts
try:
    font = pygame.font.Font("assets/fonts/retro.ttf", 36)
    small_font = pygame.font.Font("assets/fonts/retro.ttf", 24)
except:
    # Fallback if font not found
    font = pygame.font.SysFont("monospace", 36)
    small_font = pygame.font.SysFont("monospace", 24)

# Load sounds
try:
    ping_sound = pygame.mixer.Sound("assets/sounds/ping.wav")
    pong_sound = pygame.mixer.Sound("assets/sounds/pong.wav")
    score_sound = pygame.mixer.Sound("assets/sounds/score.wav")
except:
    # Create empty sounds if files not found
    ping_sound = pygame.mixer.Sound(buffer=bytearray())
    pong_sound = pygame.mixer.Sound(buffer=bytearray())
    score_sound = pygame.mixer.Sound(buffer=bytearray())

class Paddle:
    def __init__(self, x, y, width, height, speed, up_key, down_key):
        self.rect = pygame.Rect(x, y, width, height)
        self.speed = speed
        self.up_key = up_key
        self.down_key = down_key
    
    def move(self, keys):
        if keys[self.up_key] and self.rect.top > 0:
            self.rect.y -= self.speed
        if keys[self.down_key] and self.rect.bottom < WINDOW_HEIGHT:
            self.rect.y += self.speed
    
    def ai_move(self, ball, difficulty=1.0):
        # Only move if ball is coming toward AI
        if ball.velocity[0] > 0:
            # AI will try to predict where the ball will be
            # But with some prediction error based on difficulty
            target_y = ball.rect.centery
            prediction_error = random.uniform(-30 * (1-difficulty), 30 * (1-difficulty))
            target_y += prediction_error
            
            # Move toward the predicted position
            if self.rect.centery < target_y and self.rect.bottom < WINDOW_HEIGHT:
                self.rect.y += self.speed * difficulty
            elif self.rect.centery > target_y and self.rect.top > 0:
                self.rect.y -= self.speed * difficulty
        else:
            # If ball is moving away, return to center with slight delay
            if abs(self.rect.centery - WINDOW_HEIGHT // 2) > self.speed:
                if self.rect.centery > WINDOW_HEIGHT // 2:
                    self.rect.y -= self.speed * 0.5
                else:
                    self.rect.y += self.speed * 0.5
    
    def draw(self, screen):
        pygame.draw.rect(screen, WHITE, self.rect)

class Ball:
    def __init__(self, x, y, size, initial_speed):
        self.rect = pygame.Rect(x, y, size, size)
        self.initial_speed = initial_speed
        self.speed = initial_speed
        self.velocity = [0, 0]
        self.active = False
    
    def launch(self):
        # Random direction (left or right)
        direction_x = random.choice([-1, 1])
        # Random angle (not too steep)
        direction_y = random.uniform(-0.5, 0.5)
        
        # Normalize and set velocity
        magnitude = (direction_x**2 + direction_y**2)**0.5
        self.velocity = [direction_x/magnitude * self.speed, 
                         direction_y/magnitude * self.speed]
        self.active = True
    
    def update(self):
        if not self.active:
            return
        
        # Move the ball
        self.rect.x += self.velocity[0]
        self.rect.y += self.velocity[1]
        
        # Wall collision (top/bottom)
        if self.rect.top <= 0 or self.rect.bottom >= WINDOW_HEIGHT:
            self.velocity[1] = -self.velocity[1]
            ping_sound.play()
            # If hitting top or bottom wall
            if self.rect.top <= 0:
                self.rect.top = 1
            else:
                self.rect.bottom = WINDOW_HEIGHT - 1
    
    def check_paddle_collision(self, paddle1, paddle2):
        if self.rect.colliderect(paddle1.rect):
            # Calculate new angle based on where ball hit the paddle
            relative_intersect_y = (paddle1.rect.centery - self.rect.centery) / (paddle1.rect.height / 2)
            bounce_angle = relative_intersect_y * (5 * math.pi / 12)  # Max 75-degree bounce
            
            # Set new velocity with increased speed
            self.speed = min(self.speed + BALL_SPEED_INCREMENT, MAX_BALL_SPEED)
            self.velocity[0] = math.cos(bounce_angle) * self.speed
            self.velocity[1] = -math.sin(bounce_angle) * self.speed
            
            # Move ball outside paddle to prevent getting stuck
            self.rect.left = paddle1.rect.right + 1
            
            # Play sound
            pong_sound.play()
            
        elif self.rect.colliderect(paddle2.rect):
            # Calculate new angle based on where ball hit the paddle
            relative_intersect_y = (paddle2.rect.centery - self.rect.centery) / (paddle2.rect.height / 2)
            bounce_angle = relative_intersect_y * (5 * math.pi / 12)  # Max 75-degree bounce
            
            # Set new velocity with increased speed
            self.speed = min(self.speed + BALL_SPEED_INCREMENT, MAX_BALL_SPEED)
            self.velocity[0] = -math.cos(bounce_angle) * self.speed
            self.velocity[1] = -math.sin(bounce_angle) * self.speed
            
            # Move ball outside paddle to prevent getting stuck
            self.rect.right = paddle2.rect.left - 1
            
            # Play sound
            pong_sound.play()
    
    def check_score(self):
        # Check if ball passed paddles
        if self.rect.left <= 0:
            # Player 2 scored
            score_sound.play()
            self.reset()
            return 2
        elif self.rect.right >= WINDOW_WIDTH:
            # Player 1 scored
            score_sound.play()
            self.reset()
            return 1
        return 0
    
    def reset(self):
        # Reset ball position and state
        self.rect.center = (WINDOW_WIDTH // 2, WINDOW_HEIGHT // 2)
        self.speed = self.initial_speed
        self.velocity = [0, 0]
        self.active = False
    
    def draw(self, screen):
        pygame.draw.rect(screen, WHITE, self.rect)

class Game:
    def __init__(self):
        self.state = GameState.MAIN_MENU
        self.single_player = True
        self.player1_score = 0
        self.player2_score = 0
        self.difficulty = 0.8  # 0.0-1.0, default medium difficulty
        
        # Create game objects
        self.paddle1 = Paddle(
            50, WINDOW_HEIGHT // 2 - PADDLE_HEIGHT // 2,
            PADDLE_WIDTH, PADDLE_HEIGHT, PADDLE_SPEED,
            pygame.K_w, pygame.K_s
        )
        
        self.paddle2 = Paddle(
            WINDOW_WIDTH - 50 - PADDLE_WIDTH, WINDOW_HEIGHT // 2 - PADDLE_HEIGHT // 2,
            PADDLE_WIDTH, PADDLE_HEIGHT, PADDLE_SPEED,
            pygame.K_UP, pygame.K_DOWN
        )
        
        self.ball = Ball(
            WINDOW_WIDTH // 2 - BALL_SIZE // 2,
            WINDOW_HEIGHT // 2 - BALL_SIZE // 2,
            BALL_SIZE, INITIAL_BALL_SPEED
        )
    
    def start_game(self):
        # Reset scores
        self.player1_score = 0
        self.player2_score = 0
        
        # Reset ball
        self.ball.reset()
        
        # Start the game
        self.state = GameState.PLAYING
        
        # Launch ball after a short delay
        pygame.time.set_timer(pygame.USEREVENT, 1000)  # 1 second delay
    
    def update(self):
        if self.state == GameState.PLAYING:
            keys = pygame.key.get_pressed()
            
            # Move paddles
            self.paddle1.move(keys)
            
            if self.single_player:
                # AI controls paddle2
                self.paddle2.ai_move(self.ball, self.difficulty)
            else:
                # Player 2 controls paddle2
                self.paddle2.move(keys)
            
            # Update ball
            self.ball.update()
            
            # Check for collisions
            self.ball.check_paddle_collision(self.paddle1, self.paddle2)
            
            # Check for scoring
            scorer = self.ball.check_score()
            if scorer == 1:
                self.player1_score += 1
                if self.player1_score >= SCORE_TO_WIN:
                    self.state = GameState.GAME_OVER
                else:
                    # Launch ball after a short delay
                    pygame.time.set_timer(pygame.USEREVENT, 1000)  # 1 second delay
            elif scorer == 2:
                self.player2_score += 1
                if self.player2_score >= SCORE_TO_WIN:
                    self.state = GameState.GAME_OVER
                else:
                    # Launch ball after a short delay
                    pygame.time.set_timer(pygame.USEREVENT, 1000)  # 1 second delay
    
    def draw(self):
        # Clear screen
        screen.fill(BLACK)
        
        if self.state == GameState.MAIN_MENU:
            self.draw_main_menu()
        elif self.state == GameState.PLAYING or self.state == GameState.PAUSED:
            self.draw_game()
            if self.state == GameState.PAUSED:
                self.draw_pause_menu()
        elif self.state == GameState.GAME_OVER:
            self.draw_game()
            self.draw_game_over()
        
        # Update the display
        pygame.display.flip()
    
    def draw_main_menu(self):
        # Title
        title_text = font.render("ATARI PONG", True, WHITE)
        screen.blit(title_text, (WINDOW_WIDTH // 2 - title_text.get_width() // 2, 100))
        
        # Menu options
        single_player_text = small_font.render("1. Single Player", True, WHITE)
        two_player_text = small_font.render("2. Two Players", True, WHITE)
        quit_text = small_font.render("Q. Quit", True, WHITE)
        
        screen.blit(single_player_text, (WINDOW_WIDTH // 2 - single_player_text.get_width() // 2, 250))
        screen.blit(two_player_text, (WINDOW_WIDTH // 2 - two_player_text.get_width() // 2, 300))
        screen.blit(quit_text, (WINDOW_WIDTH // 2 - quit_text.get_width() // 2, 350))
        
        # Instructions
        instructions_text = small_font.render("Press key to select", True, WHITE)
        screen.blit(instructions_text, (WINDOW_WIDTH // 2 - instructions_text.get_width() // 2, 450))
    
    def draw_game(self):
        # Draw center line (dashed)
        for i in range(0, WINDOW_HEIGHT, 20):
            pygame.draw.rect(screen, WHITE, (WINDOW_WIDTH // 2 - 1, i, 2, 10))
        
        # Draw paddles
        self.paddle1.draw(screen)
        self.paddle2.draw(screen)
        
        # Draw ball
        self.ball.draw(screen)
        
        # Draw scores
        score1_text = font.render(str(self.player1_score), True, WHITE)
        score2_text = font.render(str(self.player2_score), True, WHITE)
        screen.blit(score1_text, (WINDOW_WIDTH // 4 - score1_text.get_width() // 2, 20))
        screen.blit(score2_text, (3 * WINDOW_WIDTH // 4 - score2_text.get_width() // 2, 20))
    
    def draw_pause_menu(self):
        # Transparent overlay
        overlay = pygame.Surface((WINDOW_WIDTH, WINDOW_HEIGHT), pygame.SRCALPHA)
        overlay.fill((0, 0, 0, 128))
        screen.blit(overlay, (0, 0))
        
        # Pause menu
        pause_text = font.render("GAME PAUSED", True, WHITE)
        continue_text = small_font.render("Press SPACE to continue", True, WHITE)
        menu_text = small_font.render("Press ESC to return to menu", True, WHITE)
        
        screen.blit(pause_text, (WINDOW_WIDTH // 2 - pause_text.get_width() // 2, 200))
        screen.blit(continue_text, (WINDOW_WIDTH // 2 - continue_text.get_width() // 2, 300))
        screen.blit(menu_text, (WINDOW_WIDTH // 2 - menu_text.get_width() // 2, 350))
    
    def draw_game_over(self):
        # Transparent overlay
        overlay = pygame.Surface((WINDOW_WIDTH, WINDOW_HEIGHT), pygame.SRCALPHA)
        overlay.fill((0, 0, 0, 128))
        screen.blit(overlay, (0, 0))
        
        # Game over screen
        game_over_text = font.render("GAME OVER", True, WHITE)
        
        if self.player1_score > self.player2_score:
            winner_text = font.render("Player 1 Wins!", True, WHITE)
        else:
            if self.single_player:
                winner_text = font.render("Computer Wins!", True, WHITE)
            else:
                winner_text = font.render("Player 2 Wins!", True, WHITE)
        
        replay_text = small_font.render("Press R to play again", True, WHITE)
        menu_text = small_font.render("Press ESC to return to menu", True, WHITE)
        
        screen.blit(game_over_text, (WINDOW_WIDTH // 2 - game_over_text.get_width() // 2, 150))
        screen.blit(winner_text, (WINDOW_WIDTH // 2 - winner_text.get_width() // 2, 250))
        screen.blit(replay_text, (WINDOW_WIDTH // 2 - replay_text.get_width() // 2, 350))
        screen.blit(menu_text, (WINDOW_WIDTH // 2 - menu_text.get_width() // 2, 400))
    
    def handle_event(self, event):
        if event.type == pygame.QUIT:
            return False
        
        if event.type == pygame.KEYDOWN:
            if self.state == GameState.MAIN_MENU:
                if event.key == pygame.K_1:
                    self.single_player = True
                    self.start_game()
                elif event.key == pygame.K_2:
                    self.single_player = False
                    self.start_game()
                elif event.key == pygame.K_q:
                    return False
            
            elif self.state == GameState.PLAYING:
                if event.key == pygame.K_ESCAPE:
                    self.state = GameState.PAUSED
            
            elif self.state == GameState.PAUSED:
                if event.key == pygame.K_SPACE:
                    self.state = GameState.PLAYING
                elif event.key == pygame.K_ESCAPE:
                    self.state = GameState.MAIN_MENU
            
            elif self.state == GameState.GAME_OVER:
                if event.key == pygame.K_r:
                    self.start_game()
                elif event.key == pygame.K_ESCAPE:
                    self.state = GameState.MAIN_MENU
        
        # Handle ball launch timer
        if event.type == pygame.USEREVENT:
            if self.state == GameState.PLAYING and not self.ball.active:
                self.ball.launch()
                pygame.time.set_timer(pygame.USEREVENT, 0)  # Cancel the timer
        
        return True

def main():
    game = Game()
    running = True
    
    while running:
        # Handle events
        for event in pygame.event.get():
            if not game.handle_event(event):
                running = False
        
        # Update game state
        game.update()
        
        # Draw everything
        game.draw()
        
        # Cap the frame rate
        clock.tick(FPS)
    
    pygame.quit()
    sys.exit()

if __name__ == "__main__":
    main() 