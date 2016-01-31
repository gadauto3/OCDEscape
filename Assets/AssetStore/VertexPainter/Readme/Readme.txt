Contributors		: Martijn Dekker 
Contac 				: martijn.pixelstudio@gmail.com	
Tested from version	: Unity 4.3
Stable tag			: 2.0


A simple tool to create vertex colors on your custom models.
Due to some thingies in unity a new mesh is create when you apply the colors (press stop button)
This way everything can be saved and is not reset on the next load.

All shaders are localed under "VertexPaint"

Installation
Just import the package into you project and you are good to go!

Frequently Asked Questions (FAQ)
1) The vertex painter is located under \"Window\" 
2) When painting does not work, close the vertexpaint window and reopen
3) All is lost when you reimport your model (or press the revert button)

ChangeLog

Version 2.0
- Updated to Unity 4.3
- Added several shaders for texture blending (blend 2,3 or 4 textures)
- Added op for calculating Vertex based AO (including example shaders)

Version 1:0 First release
- Basic Undo support  
- paint vertex colors  
- reset colors.  
- save data