# Async Pack 1 — Monetisation Models & Ethics

**Game:** Forward  
**Module:** Mobile Game Development (A12581)

## Task 1 — Economy Loop

Players earn coins by collecting items while running along the track.

Coins can be spent on upgrades, and players can also obtain simulated coin packs through the current Fake IAP shop.

Purchasing a speed upgrade increases the player's movement speed by 10%, while the coin cost increases with each upgrade level.

## Task 2 — Monetisation Model

If Forward were published commercially, I would consider a free-to-play model with optional consumable in-app purchases.

Players could purchase coin packs to obtain upgrades more quickly, but all upgrades would remain accessible through normal gameplay. This would allow players to enjoy the game without spending money.

The current project uses a Fake IAP provider for testing and does not process real payments. A commercial version would require a proper billing system, transparent real-money prices and purchase confirmation.

## Task 3 — Dark Patterns and Guardrails

- **Excessive grinding:** Upgrade costs could become too high and pressure players into buying coins. I would balance coin rewards and upgrade prices so that progression remains achievable through gameplay.
    
- **Purchase pressure:** Repeated shop prompts after a failed run could encourage impulsive purchases. I would keep the shop accessible through the Menu and avoid automatic purchase pop-ups.
    
- **Artificial scarcity:** Limited-time coin offers could create unnecessary urgency. I would avoid misleading countdown timers and false scarcity claims.
    
- **Paid random rewards:** Loot boxes could encourage spending without guaranteeing a useful reward. I would avoid paid random rewards and keep coin-pack contents clearly defined.
    
- **Pay-to-skip:** Artificial waiting periods could encourage players to pay to continue playing. I would not introduce energy restrictions, paid cooldown skips or mandatory waiting timers.
    

## Reflection

This activity helped me understand how monetisation decisions can affect both game balance and player experience. Forward's economy should reward normal gameplay rather than pressure players into spending money. If monetisation were introduced in a published version, it should remain optional, transparent and fair.