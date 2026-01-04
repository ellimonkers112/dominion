## Dominion Infostealer - Red Team Tool

Disclaimer: This project is intended strictly for authorized red team engagements, penetration testing, and educational purposes only. Use in any unauthorized or illegal manner is prohibited. The developers assume no responsibility for misuse. Always obtain explicit permission before deploying on any target system.

## Overview

Dominion is a modular C# information stealer designed for red team operations to simulate advanced credential theft and data exfiltration scenarios. It targets common user data sources on Windows systems, including browsers, applications, crypto wallets, and more. The tool emphasizes stealth, modularity, and ease of configuration for realistic adversary emulation.
Key goals in red teaming:

Demonstrate post-compromise credential access (T1555).
Exfiltrate session tokens, passwords, and sensitive files.
Test detection capabilities for common infostealer behaviors (e.g., browser data parsing, clipboard monitoring).

## Features

Modular Design: Separate namespaces/folders for different theft modules.
 - Browser Stealing: Extracts passwords, cookies, history, and autofill from Chrome, Edge, Firefox, and others.
 - Application-Specific:
 - Messengers (Discord, Telegram, etc.)
 - VPN clients
 - Crypto wallets
 - Gaming platforms
 - General apps

 * System Data Collection: Clipboard, screenshots, system info, WiFi passwords.
 * Encryption & Hashing: Secure handling of stolen data (AES encryption, hashing for integrity).
 * Exfiltration: Configurable C2 (e.g., HTTP POST, Discord webhook, Telegram bot) with data compression and encryption.
 * Anti-Analysis (Basic): Simple checks for VM/sandbox, delays, and persistence options.
 * Stealth: Runs in memory where possible, minimal disk footprint.
