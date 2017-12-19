# NNUI (Neural Networks User Interface)

NNUI is a visual neural networks editor. 
It's convenient and efficient and it aims to the beginners and the researchers.

## Requirements

Windows 10 (version 10586 or higher version)

## How to deploy it?

NNUI is divided into the client and the server.

- Client
    1. Install NNUI on Microsoft Store (unavailable)
    2. Compile the source code using Visual Studio 2017
- Server
    1. Install Python 2.7 / 3.6 or higher version
    2. Install the third-package:
        ```
            pip install keras tensorflow flask
        ```
    3. Enter the directory `server`, run the command `python server.py`
    
## How to use it?
    
    When the server is running, open the program NNUI. 
    
    In the model page,
    
    You can click the layer for adding it into the model.
    
    Click 'Modify' on the right side to modify the parameters.
    
    Set the Server IP, and click `Compile` button to train the model.
    
    And in the training page,
    
    Click `Get loss info` and see the loss graph.