BatchProcessor.cs for UNITY
=============

An Editor-script that steps in whenever simple multi-object-editing is not enough. It's still early, speak up if you have specific needs/ideas!
And just drop me a line if you want to be a collaborator on the main Repository!


Features
-------

This is what the script can do so far:

* Find/Filter objects by name
* Find/Filter objects by components
* Batch-modify components
* Spawn Prefabs, Clone GameObjects, Spawn components


Example
-------

If you want to see what the script can do, follow these steps:

Open the project in Unity
Open the scene TEST
Window > BatchProcessor
Enter "Cube" into the "Name must contain" field and Click "Find"
Select "Small Cube 1" and drag the SelfDestructInSec component (click and drag the bold title in the inspector) into the "Has Component" field - click "Filter"

You now have a selection of all GameObjects that have "Cube" in their name and hold a SelfDestructInSec component.

Drag the SelfDestructInSec component into the "Modify Component" field in the "DO" section, check the "sec" checkbox and set it to 3
Drag and drop the Rotate script from the Project pane into the "AddThis" field.
Click "Process"

A list of all the modified objects appears in the "DONE" section. All gameObjects that have Cube in their name and held a SelfDestructInSec component had a Rotate component added and the sec value of the SelfDestructInSec component has been changed to 3.

