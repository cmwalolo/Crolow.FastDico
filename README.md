# A .NET implementation of a DAWG and GADDAG dictionary. 
## Ultra compact on disk and in memory.
## Ultra fast tree navigation. 

The implementations includes : 
- Dawg and GadDag Compiler
- Dawg and GadDag Word Search : 
    - Word search
    - Prefix search
    - Suffix search 
    - Wildcard search (seemless multiple ? or *)
    - Anagram search 
    - Anagram & smaller search.

The code is provided as is and far less complex then other implementations. But feel free to make pull requests.

That part of code has been written from scratch coauthored by ChatGpt under my lead. 
Results from IA are not that effective. But it was a good training for both. 
Finally ChatGpt could totally not write the compiler, without eating minimum 100MB of memory, and could totally not optimize on disk, and in memory. But he did more or less well the Search functions, after some debugging and small fixes. 

Next is a basic implementation of a board wordgame... To be continued or not ... or make it as for now :
    - Configuration of The Grid, The bag, and play configurations are implemented in JSON files
    - The Game handle the bag and the rack.
    - The Game plays the first move and update the history and the board.
    - Reject from the rack letters played
    - Next Round : 
        - The game pickups new letters, and reject the rack following the play config (related to vowels and consumns)
        - It creates a double grid of playable letters at each pivot square (one for horizontal moves another for vertical moves)


For any information and credits : llequenne at hotmail.com
