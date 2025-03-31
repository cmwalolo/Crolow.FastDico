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


There is a basic implementation of a board wordgame... Hyper fast. 200ms to 1000 ms to generate a full game

A Maui/Blazor application... To be continued

The current implementation is straight forward. 
There will be a lot of refactoring to enable a better integration with an app.


For any information and credits : llequenne at hotmail.com
