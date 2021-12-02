# Visual-Studio-Terminal
This is a windows .net console application which allows you to open visual studio projects from the command line. This will only work if you have 

## Instructions
- To use this you have to build this solution in visual studio or download the `studio.exe` file in the repo.
- After choosing one of the above options you will have to copy the file path for the folder containing studio.exe
  - If you downloaded the repo and built the solution the studio.exe file will be in the `VSTerminalSol/bin/debug`.
- After copying your file path:
  - go to `Start`
  - type `Edit System Environment Variables`
  - click `Environment Variables` on the bottom
  - *double click* on the `Path` in the area called `User variables`
  - click `New` and paste the file path
- After this you can go into any terminal and `cd` into the visual studio folder contining a `*.sln` file, then enter => `studio LAST_TWO_OF_YOUR_VS_VERSION` (Ex: studio 22 for visual studio 2022)

## Contributions
- Please let me of any bugs in the issues tab or additions in the Pull Request tab
- Appreciate your time looking at my repo and hope you like it enough to leave a star
