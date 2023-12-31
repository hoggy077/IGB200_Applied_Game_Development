# IGB200 - Applied Game Development
## Spellbinder
Spellbinder was the product of short term development assignment in 2021. The primary theme of the assignment was STEM & the idea of creating an educational game that met some highly specific conditions. To approach this, we centered the game around the idea of teaching programming by introducing a events console, but also by theming the spell building component around the layout of standard syntax.

As part of this project, my primary job was framework & gameplay programming, which quickly bled into UI related interactions. As such, I created the modular framework and managing systems that governed spells whilst the teams second programmer created and actual spells themselves. Eventually, we desired AI however, given the short time period remaining in the assignment, much of my new interest in game AI was short lived as majority of them were either scrapped or disabled due to last minute design changes that were unachievable.

The Spell manager retains a dictionary of all known spell fragments. These fragments are handled in two formats, the action & the effect, in which "Ray" is an action while "Push" is an effect.
Both of these are stored within a wrapper that is the spell. To pass these around without strictly defining the spell, each effect is stored as an *Action* which recieves a structure of relevant data from the related effect.

![Visuals](https://github.com/hoggy077/IGB200_Applied_Game_Development/blob/main/img/GameCap1.PNG?raw=true)
![Spell Crafting UI](https://github.com/hoggy077/IGB200_Applied_Game_Development/blob/main/img/GameCap2.PNG?raw=true)
