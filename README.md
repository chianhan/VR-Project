VR Experience

A Unity-based educational virtual reality project designed for children and families, developed as an undergraduate capstone project.

The project explores how interactive digital environments can support learning through exploration and engagement. It was exhibited at Vision Get Wild 2025 and our university's graduation exhibition.

Project Overview

Project Type: Educational VR Experience

Development Platform: Unity

Programming Language: C#

VR Platform: Meta Quest 2

Team Size: 5 members

My Role: Team Leader & Lead Programmer

Technical Implementation

1. VR Interaction System — PlayerWeapon.cs

This script handles VR controller input and character interactions.

Detects the positions of the left and right hand controllers.

Uses hand height to control an animation parameter.

Connects controller input with the elephant character's Animator.

Includes trigger-based projectile functionality.

2. Player Movement — PlayerMovement.cs

This script manages player movement using VR controllers.

Reads thumbstick input from the VR controllers.

Supports movement and rotation.

Updates the player model's position relative to the VR camera.

3. Game State Management — GameData.cs

This script manages several core game systems.

Controls game states, including menu, gameplay, pause, game over, and game clear.

Tracks collected items.

Manages a countdown timer.

Handles scene transitions and records previously used doors.

Unity Implementation Screenshots

VR Interaction Setup



Unity Inspector configuration showing references to the VR hand controllers, camera, Animator, and projectile components.

Animator State Transitions



The Animator uses the NoseUp Boolean parameter to control transitions between the elephant character's animation states.

Repository Contents

Scripts/ — Selected C# scripts from the Unity project.

Screenshots/ — Unity Inspector and Animator configuration screenshots.

Notes

This repository contains selected scripts and technical implementation screenshots from an undergraduate team project. It is not a complete or independently runnable Unity project.

The code has been prepared for portfolio presentation, including English comment translations and minor readability cleanup, while preserving the original implementation logic.
