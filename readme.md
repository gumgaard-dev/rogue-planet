# Capstone Readme
This document serves as a reference for anyone working on the project.

## 1. SOFTWARE VERSIONS
**Unity:** 2022.3.9f1


## 2. PROJECT ORGANIZATION + STYLE
### i) STRUCTURE
Stick to the existing project structure (Assets/AssetType/Subject)

eg. Assets/Art/Sprites/World/tex_stone.png

### ii) FILE NAMING

Don't include spaces in file or folder names! Unity has issues with this.

Scripts should be named in PascalCase.

Assets other than scripts should be written in camelCase, but prefixed according to the asset type.

#### _ASSET PREFIXES_:
- **tex_** --- texture/sprite
- **tex_bg** --- background texture
- **pfb_** --- prefabs
- **scn_** --- scene

#### _EXAMPLES_:
- tex_oreRed
- pfb_map
- scn_mainLevel

## 3. REPO CONTRIBUTION
### BRANCH NAMING
New branches should be formatted as **issue-type/INITIALS-description-of-issue**. 

#### _ISSUE TYPES_:
- **f/** --- feature

- **b/** --- bugfix

- **r/** --- refactor

- **c/** --- project or repo config (updating gitignore, readme, etc)

#### _EXAMPLES_:
- c/RHP-update-readme
- f/KA-implement-enemy-behavior
