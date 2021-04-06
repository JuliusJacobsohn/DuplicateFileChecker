# DuplicateFileChecker
Takes a list of paths and searches for duplicate files by comparing filesize, fileextension and 64 bytes from the middle of the file.
This heuristic may not be useful to everyone but it is optimized to deal with large chunks of media files like images and videos, without checking each file against every other file.
Works pretty fast and reliable.

## Usage
0. Clone the project and open the .sln file in Visual Studio (not Visual Studio Code, but that might also work)
1. Start the program (CTRL+F5)
2. Enter all paths that you want to search for duplicate files. All entered paths are checked recursively.
3. Let the program run. It will output a list of all duplicates it found and save that list as a .dfc file.
4. Either press "y" to start deleting all duplicates that were found (they will NOT be moved to the recycle bin!), or press any other key to close the program.

## Alternative Usage
Instead of entering a file path, enter the name of any of the .dfc files that were generated. You will then be able to use the data to delete duplicates without having to re-do the search.
