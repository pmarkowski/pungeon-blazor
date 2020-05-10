# Walls; Redux, PixiJS Experiments; Social Sharing Brainstorming

## Walls Update
Did some dev work today to remove the "Tile" walls in the Blazor prototype. Instead, when adding a space into a dungeon 4 `WallSegment`s are automatically added as well surrounding the space. This ends up looking a lot more consistent if you add walls via the Wall tool since you're dealing with all the same type of entity.

You can still make 1 tile thick "walls" no problem, really, just separate WallSegments with that much space.

Possible next step is a "clear wall segment" tool that goes through all the `WallSegment`s in the Dungeon and finds ones that overlap with what you entered, and makes gaps in those areas. This way you can use the *New Space* tool to quickly block out rooms, make your hallways, and then use the *Clear Wall* tool to delete all the overlapping walls that were created by quickly making spaces.

There are two things to consider after that:
* Splitting a wallsegment would result in two wallsegments, i.e. a big ID change. What happens to things referring the old wallsegment? Is one arbitrarily chosen, or would any such relationship get cleared out?
* One thing that was nice about the "tile" walls was they were automatically created by the Grid when rendering, so changing a Space automatically meant adjusting the position and size of the surrounding walls. How does this work now? Do walls and spaces have some reference to each other? How do you know to resize them, and in what way, especially if they have been split as in above or are unrooted in the middle of the space?

## Redux, PixiJS Experiments
Took some time to learn about PixiJS and Redux today.

Redux in isolation makes a lot of sense and brings a lot of nice functionality and features that would probably be useful for Pungeon. An example in particular is it would make something like [undo/redo really easy to implement](https://redux.js.org/recipes/implementing-undo-history).

Started looking into PixiJS as well. PixiJS by default has a `ticker` method that gets called on a set interval that can be used to update the sprites in the PixiJS scene graph and re-render them. I was initially disappointed in PixiJS because it seemed like it's strongly intentioned to be very stateful and mutable framework where you'd have objects and references and whatnot, and update them directly so that they get rendered in a new location. I want to keep the underlying dungeon model fairly "pure" and just render whatever its current status is in a set interval or potentially even just on change. Looking into PixiJS more and given some assurances that it's really lightweight and flexible, I think it might end up working actually pretty well.

You can extend sprites and objects in JavaScript so I can either make a custom Sprite class or Container that represents a Dungeon object, iterate through those, and sync them up with the Redux state.

There's also an example of PixiJS interacting with the Redux state by dispatching actions on PixiJS interaction events which can be used to sync up things like selecting objects on click.

All-in-all, a loose architecture/data flow is becoming evident here.

There is a `DungeonState` that is the backing state in a Redux application. Modifications to the dungeon happen via `DungeonReducer` that handles all the interactions.

PixiJS interactions dispatch Redux actions and passes them off to the `DungeonReducer`. One thing to note here though is that it might make sense to split out `DungeonState` vs `ApplicationState`. Things like the currently selected tool, currently selected Dungeon object, likely make much more sense in an `ApplicationState`. This keeps the `DungeonState` a purer representation of the dungeon contents and probably better helps enable the history and undo/redo I want to use Redux to take advantage of.

Ideally, this can all be wrapped in a React application for the UI side of things and use React-Redux to glue the UI components that I want to use for the user interaction with the Dungeon and have that modify the underlying DungeonState so it can just get picked up by a Pixi instance and draw and give the user whatever feedback necessary.

Crude ASCII diagram of structure here:
```
React Components          PIXI Canvas
                \        /
           Redux Application State
```

Next immediate steps are probably:
* learn more about React
* before diving in deeper into React-Redux and 
* research if it plays well with having a Canvas and PixiJS instance
* create a POC React app that contains all these elements working together

Long term tasks:
* I still need to get the underlying dungeon model and presentation down. I can continue to use Blazor for that since I think it's working well enough in terms of nailing things down conceptually.
* Figure out when the right time to shift to prototyping/developing the pure JS web app.

## Social Sharing Brainstorming
Switching gears to product/feature, it might be interesting if there's a "social" hub where you can opt-in to have your dungeons/work be visible, or follow another map maker. Then on the front page there can be a gallery of recent creations, top rated dungeons, etc.
