# game-matches

Game Matches. 

Game starts with random matches count between 20 and 45. User and AI do moves by turns. First player can remove 1-3 matches. 
Next player removes n-1 - n+1 matches, where n is last count of removed matches. Who removes last matches won.


Features:
  - Cheater AI.
  
  Cheater AI knows best solutions for each situation. Solutions generated by "SolutionsGenerator.cs" script in Editor mode and saved in 
  "solutions.xml" file. In play mode AI choose number from list.
