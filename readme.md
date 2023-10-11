# Capstone Readme
This document serves as a reference for anyone working on the project.

## 1 \- SOFTWARE VERSIONS
**Unity:** 2022.3.9f1


## 2 \- PROJECT ORGANIZATION + STYLE
### 2.1 \- STRUCTURE
Stick to the existing project structure (Assets/AssetType/Subject)

eg. Assets/Art/Sprites/World/\[asset name\]

### 2.2 \- FILE NAMING

Don't include spaces in file or folder names! Unity has issues with this.

Scripts and Prefabs should be named in PascalCase.

Assets other than scripts and prefabs should be written in camelCase, but prefixed according to the asset type.

#### _ASSET PREFIXES_:
- **tex_** --- texture/sprite other than backgrounds
- **bg_** --- background textures
- **ani_** --- animation
- **sfx_** --- sound effect
- **mus_** --- music
- **scn_** --- scene

#### _EXAMPLES_:
- tex_oreRed
- scn_mainLevel

## 3 \- REPO CONTRIBUTION
### BRANCH NAMING
New branches should be formatted as **issue-type/INITIALS-description-of-issue**. 

#### _ISSUE TYPES_:
- **f/** --- feature
- **b/** --- bugfix
- **r/** --- refactor
- **c/** --- project or repo config (updating gitignore, readme, etc)
- **u/** --- weekly feature update

#### _EXAMPLES_:
- c/RHP-update-readme
- f/KA-implement-enemy-behavior
