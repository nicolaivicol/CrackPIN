# CrackPIN
Algorithm to find the PIN in the game: https://meduza.io/games/bud-hakerom-igra-meduzy   

## How to use the console app  
The console application (*CrackPIN.exe*) will give you the best PIN guess to submit. After you submit it in the game on the site, the site will give you a response with 1) number of digits exactly matching positions and 2) number of digits existing but not matching positions in the real PIN. You'll be asked to introduce these responses in the console app. Then the app takes into account the new information and gives a new best guess. The cycle is repeated until the real PIN is found. It takes up to 7 moves, on average 4-6 moves (based on several played games, Monte Carlo or algorithm analysis not performed yet).    

## The algorithm
The game wants a PIN of size 4 with non-repeating digits, meaning there are initially 5040 possible PINs (4-permutations of 10). The algorithm pursues a greedy strategy of submitting the most probable PIN (permutation) given all provided information (responses to submits). Each new response to submit is intersected with previous information so that impossible PINs are fitered out and the set of remaining possible PINs becomes smaller.   
A buest guess is created by finding first the most probable/frequent (in the set of possible PINs) digit at one of the 4 positions. Then it finds the second digit on one of the 3 positions left, most probable given the first digit selected. This is done untill the PIN of 4 digits is formed.   
  
