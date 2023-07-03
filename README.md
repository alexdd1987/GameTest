- The time it took you to perform the exercise
  
	- I haven't exactly kept couunt of the hours, but I would estimate that this is the result of about 4/5 (6 tops) days of work overall. I worked on this mainly in the weekends and a few hours here and there during the week

- The parts that were difficult for you and why
- The parts you think you could do better and how
  
	- Overall, it wasn't particularly difficult. However I did spend some time fighting with Unity, especially when working on the UI and in particular on the Menu for the game. In fact, I had to resort to a different implementation than what I originally thought of.
	  
	  Besides that, the most challenging part was handling all the different combination of states for the Hero and the enemies. If I had to do this again properly, I would probably implement a FSM to handle it, because currently it's a bit messy and and easy to break, especially by adding more states on top of what's already implemented.
	  
	  I approached this project as a prototype, and implementing the core (Phase 1) was not complex enough to warrant a full FSM implementation. When moving to the next phases, I realized that the complexity would increase by quite a lot, but by then I didn't really have the time to do it. I should have planned it better, but it's not easy with a full time job + life happening :D
	  
	  Another thing that I would like to improve is resource management. I am continuously instantiating and destroying Gameobjects but of course the proper way to do this would be adding object pools to reuse Gameobjects, limit the amount of garbage generated at runtime and keep the memory footprint under control.
	  
	  One more thing that I would like to do, is implementing proper systems to handle inter/intra object communications. In the past, I have often used a custom message queue to handle this, but in this project I am heavily relying on events, which require me to always fetch the components that I am interested in listening to and subscribe to their events. It can easily get cumbersome, especially as the number of objects and components grow.
	  	
	  Finally, it would be nice to implement a proper scene loading system, to separate the game in different scenes to load additively. I tried doing this but I ran into some issues, so I had to cut my scope.

- What you would do if you could go a step further on this game
	- If I had to develop the game, besides the improvements I discussed above, I would start implementing different gameplay systems. 
	  For example:
		- Levelling system
		- Adding new enemies
		- Give the enemies different behaviors and proper pathfinding, in order to avoid other monsters and obstacles. This is something that I wanted to implement but I didn't have time to do it.
		- Progression system, defeat weaves of enemies and advance to the next level/combat scenario
		- New weapons, possibly even spells
		- Weapon levelling system
		- Weapon skin customization
		- Character customization
		- etc..
		  
		  I would also like to use a DI framework and implement different interfaces, in order to make everything more testable and extensible, when it makes sense, especially as the project grows in scope and complexity. In the past I have used [Zenject](https://github.com/modesttree/Zenject) but it would be interesting to explore alternatives.

- Any comment you may have
	- No further comments, I had fun doing this, but it would be nice to do it at a more relaxed pace :)
