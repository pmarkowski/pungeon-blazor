# Reflecting on the Blazor Prototype
After speaking to Nav, looking at some existing pre-made dungeons, and looking at competitor's software, I took some time to re-evaluate the current state of Pungeon, what worked, what didn't work.

## Summarizing
Based on the musings below, I decided on the following courses of action:
* Try to keep Blazor for now.
* Drop auto Connections, instead focusing on making it easier to hollow out spaces.
* Consider dropping the unbounded grid in favour of a 2D Array.
* The Grid is a View into the underlying Dungeon Model.
* The various Tools should each have a Controller that handles interaction with the underlying Dungeon.
* Drop or hide the JSON view of the Dungeon.

## Product
### Successes
#### Web Based Tool.
Being able to send someone a tool online is already paying off for quick user feedback and being able to share current state of progress with others instead of asking them to download and install some unknown application.
#### Click and Drag Rooms.
Blazor/DOM rendering issues not withstanding this feature and functionality worked pretty well and is a natural way to block out/define how a dungeon looks.
#### Selector.
Similar to above, although it doesn't perform great being able to select an entity and modify it is key to achieving the primary goal of an easily editable/manipulable dungeon design and what separates Pungeon from something like mipui which is otherwise a pretty good map maker.
### Failures
#### Connections.
While being able to mark "doors" differently and explicitly might be good for clarity and differentiating between say an open passage way vs one with a gate in front of it, the auto-hallway stuff is falling flat for me right now and should be cut.

*Bad performance.* Although more optimization can happen around it the A* implementation was the worst performing thing in the application and really hindered draw updates and rendering more expansive dungeons.

*Pathfinding Complications.* A* was too optimal and resulted in unnatural looking hallways. The pathfinding scoring algorithm can be tweaked to result in more natural hallways. Despite this however I found that there were undesirable pathfinding complications, for example hallways "blocking" other hallways with little recourse offered to the user about what went wrong or how to fix it.

All that said "Auto-hallways" might still be a worthwhile feature to consider, but it's far down the backlog for now.

#### DungeonScript
Some of the impetus of the project itself turned out to be a bit of a failure. I found pretty quickly that I wanted to be able to use the mouse and GUI more. This might be due to the expressiveness of DungeonScript, but either way I think it makes sense to shelve/lower the text input idea in priority.

## Code
Distinct from Blazor, these are observations about the burgeoning architecture.

### Successes
#### Starting Simple and Iterating.
Slowly building up a model, rendering to text, and then rendering via DOM was generally a successful approach. Though there was a set back in terms of connectors and some UI approaches being unperformant.
### Failures
#### Need a clearer presentation/model separation.
The dungeon right now is freely being modified by the various tools, causing scalability problems and making it harder to figure out when the underlying model has changed and a dungeon re-render needs to be done.
#### Boundless Grid.
I liked this in theory but it doesn't seem all that useful, and it seems like you need to set at least a minimum size anyway. This might as well just be a fixed size two dimensional grid.
#### Lack of Equality implementations
RelativePosition, Size, etc all should have their basic equality support. Note: C# has a built in Vector2 in System.Numerics that can probably be used instead of the RelativePosition class...

## Blazor
### Successes
Leveraging HTML DOM for UI prototyping.
Being able to leverage the built in DOM events for clicking, mouse hovering, Blazor's render trees made it really easy and quick to perform model changes and view them in real time.
#### Coding in C#.
Another promise of Blazor that paid off was being able to code in C#. It saved a lot of time and mental overhead on my part to not require to "think" in another language. Although some time was spent understanding the framework, it was beneficial to be able to rely on the tools I'm most familiar with to solve the problems I encountered. (i.e., performance profiling in Visual Studio)
### Failures
#### Performance.
I found Blazor's performance in a heavy, interactive heavy application to be a little lacking, although I think in theory I can spend some extra time optimizing this. That said, it is time spent optimizing something that potentially wouldn't be an issue if I was just running in something more native or pre-compiled to WASM. I'm not sure it's time to move off Blazor just yet given the above two successes but it might be less useful outside the prototyping stage.