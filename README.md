# KurechiiAssessment
Improvements:
•	Originally, the inventory system is implemented on the player. As the inventory system needed to be accessed by the player and the UI, the inventory is later separated into an individual game object with a public script. 
•	The hover description was shown instantly when hovered, a hover timer is added so that the description will only appear after it hovered over a specific time.
•	The description is despawned on click and while dragging.
•	Instead of making the drop function to be achieved by clicking a button, it can be achieved by tossing the item outside of the box to be more interactive.
•	Drag function is added to support drop and swap functions.
•	To avoid the dragging icon being covered by other icons due to sibling index, an instant of the dragging icon is created to guarantee overlay on all icons.
•	Drag countdown is added to avoid weird animation on click.
