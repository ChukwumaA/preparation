using System.Collections.Generic;
using Managers;
using UnityEngine;

public class AI : MonoBehaviour
{
    public Transform target;

    private List<TileManager.Tile> tiles = new List<TileManager.Tile>();
    private TileManager manager;
    public GhostMove ghost;

    public TileManager.Tile nextTile = null;
    public TileManager.Tile targetTile;
    TileManager.Tile currentTile;

    void Awake()
    {
        manager = GameObject.Find("Game Manager").GetComponent<TileManager>();
        tiles = manager.tiles;

        if(ghost == null) Debug.Log("game object ghost not found");
        if(manager == null) Debug.Log("game object Game Manager not found");
    }

    public void AILogic()
    {
        //get current tile
        Vector3 currentPos = new Vector3(transform.position.x + 0.499f, transform.position.y + 0.499f);
        currentTile = tiles[TileManager.Index ((int)currentPos.x, (int)currentPos.y)];

        targetTile = GetTargetTilePerGhost();

        //get the next tile according to direction
        if(ghost.direction.x > 0) nextTile = tiles[TileManager.Index ((int)(currentPos.x+1), (int)currentPos.y)];
        if(ghost.direction.x < 0) nextTile = tiles[TileManager.Index ((int)(currentPos.x-1), (int)currentPos.y)];
        if(ghost.direction.y > 0) nextTile = tiles[TileManager.Index ((int)currentPos.x, (int)(currentPos.y+1))];
        if(ghost.direction.y < 0) nextTile = tiles[TileManager.Index ((int)currentPos.x, (int)(currentPos.y-1))];

        if(nextTile.Occupied || currentTile.IsIntersection)
        {
            //IF WE BUMP INTO A WALL
            if(nextTile.Occupied && !currentTile.IsIntersection)
            {
                //if ghost moves to right or left and there is wall on the next tile
                if(ghost.direction.x != 0)
                {
                    if(currentTile.down == null)
                        ghost.direction = Vector3.up;
                    else
                            ghost.direction = Vector3.down;
                }

                //If ghost moves to up or down and there is wall next to the tile
                else if(ghost.direction.y != 0)
                {
                    if(currentTile.left == null)
                        ghost.direction = Vector3.right;
                    else    
                        ghost.direction = Vector3.left;
                }
            }

//======================================================================================================================
            //IF WE ARE AT INTERSECTION
            //Calculate the distance to target from each available tile and choose the shortest path

            if(currentTile.IsIntersection)
            {
                float dist1, dist2, dist3, dist4;
                dist1 = dist2 = dist3 = dist4 = 999999f;
                if(currentTile.up != null && !currentTile.up.Occupied && !(ghost.direction.y < 0))
                        dist1 = TileManager.Distance(currentTile.up, targetTile);

                if(currentTile.down != null && !currentTile.down.Occupied && !(ghost.direction.y > 0))
                        dist2 = TileManager.Distance(currentTile.down, targetTile);

                if(currentTile.left != null && !currentTile.left.Occupied && !(ghost.direction.x > 0))
                        dist3 = TileManager.Distance(currentTile.left, targetTile);

                if(currentTile.right != null && !currentTile.right.Occupied && !(ghost.direction.x < 0))
                        dist4 = TileManager.Distance(currentTile.right, targetTile);

                float min = Mathf.Min(dist1, dist2, dist3, dist4 );
                if(min == dist1) ghost.direction = Vector3.up;
                if(min == dist2) ghost.direction = Vector3.down;
                if(min == dist3) ghost.direction = Vector3.left;
                if(min == dist4) ghost.direction = Vector3.right;
            }
        }

        //If there is no decision to be made, designate next waypoint for the ghost
        else
        {
            ghost.direction = ghost.direction; //setter updates the waypoint
        }

    }

    public void RunLogic()
    {
        //get current tile
        Vector3 currentPos = new Vector3(transform.position.x + 0.499f, transform.position.y + 0.499f);
        currentTile = tiles[TileManager.Index ((int)currentPos.x, (int)currentPos.y)];

        //get the next tile according to direction
        if(ghost.direction.x > 0) nextTile = tiles[TileManager.Index ((int)(currentPos.x+1), (int)currentPos.y)];
        if(ghost.direction.x < 0) nextTile = tiles[TileManager.Index ((int)(currentPos.x-1), (int)currentPos.y)];
        if(ghost.direction.y > 0) nextTile = tiles[TileManager.Index ((int)currentPos.x, (int)(currentPos.y+1))];
        if(ghost.direction.y < 0) nextTile = tiles[TileManager.Index ((int)currentPos.x, (int)(currentPos.y-1))];

        //The two debug.log lines of code everywhere is commented out
        Debug.Log(ghost.direction.x + " " + ghost.direction.y);
        Debug.Log(ghost.name + ": Next Tile (" + nextTile.X + ", " + nextTile.Y + ")" );

        if(nextTile.Occupied || currentTile.IsIntersection)
        {
//=======================================================================================================================================
            //IF WE BUMP INTO A WALL
                if(nextTile.Occupied && !currentTile.IsIntersection)
                {
                    //if ghost moves to right or left and there is wall on next tile
                    if(ghost.direction.x != 0)
                    {
                        if(currentTile.down == null)
                            ghost.direction = Vector3.up;
                        else 
                            ghost.direction = Vector3.down;
                    }

                    //If ghost moves to up or down and there is wall on next tile
                    else if(ghost.direction.y != 0)
                    {
                        if(currentTile.left == null)
                            ghost.direction = Vector3.right;
                        else
                            ghost.direction = Vector3.left;
                    }
                }

//======================================================================================================================================================
            //IF WE ARE AT INTERSECTION
            //Choose one available option at random
            if(currentTile.IsIntersection)
            {
                List<TileManager.Tile> availableTiles = new List<TileManager.Tile>();
                TileManager.Tile chosenTile;
                if(currentTile.up != null && !currentTile.up.Occupied && !(ghost.direction.y < 0))
                    availableTiles.Add (currentTile.up);
                if(currentTile.down != null && !currentTile.down.Occupied && !(ghost.direction.y > 0))
                    availableTiles.Add (currentTile.down);
                if(currentTile.left != null && !currentTile.left.Occupied && !(ghost.direction.x > 0))
                    availableTiles.Add (currentTile.left);
                if(currentTile.right != null && !currentTile.right.Occupied && !(ghost.direction.x < 0))
                    availableTiles.Add(currentTile.right);

                int rand = Random.Range(0, availableTiles.Count);
                chosenTile = availableTiles[rand];
                ghost.direction = Vector3.Normalize(new Vector3(chosenTile.X - currentTile.X, chosenTile.Y - currentTile.Y, 0));
                Debug.Log(ghost.name + ": Chosen Tile (" + chosenTile.X + ", " + chosenTile.Y + ")" );
                //Debug statement was commented out.
            }
        }

        //if there is no decision to be made, designate next waypoint for the ghost
        else
        {
            ghost.direction = ghost.direction; //setter updates the waypoint
        }
    }

    TileManager.Tile GetTargetTilePerGhost()
    {
        Vector3 targetPos;
        TileManager.Tile targetTile;
        Vector3 dir;

        //Get the target tile position (round it down to int so we can reach with Index() function)
        switch(name)
        {
            case "blinky": //target = pacman
                targetPos = new Vector3 (target.position.x+0.4999f, target.position.y + 0.499f);
                targetTile = tiles[TileManager.Index((int)targetPos.x, (int)targetPos.y)];
                break;

            case "pinky": //target = pacman + 4*pacman's direction (4 steps ahead of pacman)
                dir = target.GetComponent<PacmanMove>().GetDir();
                targetPos = new Vector3 (target.position.x + 0.499f, target.position.y+0.499f) + 4*dir;

                //if pacmans going up, not 4 ahead but 4 up and 4 left is the target
                //so, we subtract 4 from X co-ordinates from target position
                if(dir == Vector3.up)   targetPos -= new Vector3(4, 0, 0);

                targetTile = tiles[TileManager.Index((int)targetPos.x, (int)targetPos.y)];
                break;
            
            case "inky":    //target = ambushVector(pacman+2 - blinky)  added to pacman+2
                dir = target.GetComponent<PacmanMove>().GetDir();
                Vector3 blinkyPos = GameObject.Find("blinky").transform.position;
                Vector3 ambushVector = target.position + 2*dir - blinkyPos;
                targetPos = new Vector3 (target.position.x+0.499f, target.position.y+0.499f) + 2*dir + ambushVector;
                targetTile = tiles[TileManager.Index((int)targetPos.x, (int)targetPos.y)];
                break;

            case "clyde":
                targetPos = new Vector3 (target.position.x+0.499f, target.position.y + 0.499f);
                targetTile = tiles[TileManager.Index((int)targetPos.x, (int)targetPos.y)];
                if(TileManager.Distance(targetTile, currentTile) < 9)
                    targetTile = tiles[TileManager.Index (0, 2)];
                break;
            default: 
                targetTile = null;
                Debug.Log("TARGET TILE NOT ASSIGNED");
                break;
        }
        return targetTile;
    }
}
