using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public class Tile
    {
        public int x { get; set; }
        public int y { get; set; }
        public bool occupied { get; set; }
        public int adjacentCount { get; set; }
        public bool isIntersection { get; set; }

        public Tile left, right, up, down;

        public Tile(int x_in, int y_in)
        {
            x = x_in; y = y_in;
            occupied = false;
            left = right = up == down = null;
        }


    };

    public List<Tile> tiles = new List<Tile>();

    //Call this Scipt when the game launches
    void Start()
    {
        ReadTiles();
    }

    //Update is called once per frame
    void Update()
    {
        DrawNeighbours();
    }

//=====================================================================================================
    //hardcoded tile data: 1 = free tile, 0 = wall

    void ReadTiles()
    {
        //hardwired data instead of reading from file (not feasible on web player)

        string data = @"0000000000000000000000000000
0111111111111001111111111110
0100001000001001000001000010
0100001000001111000001000010
0100001000001001000001000010
0111111111111001111111111110
0100001001000000001001000010
0100001001000000001001000010
0111111001111001111001111110
0001001000001001000001001000
0001001000001001000001001000
0111001111111111111111001110
0100001001000000001001000010
0100001001000000001001000010
0111111001000000001001111110
0100001001000000001001000010
0100001001000000001001000010
0111001001111111111001001110
0001001001000000001001001000
0001001001000000001001001000
0111111111111111111111111110
0100001000001001000001000010
0100001000001001000001000010
0111001111111001111111001110
0001001001000000001001001000
0001001001000000001001001000
0111111001111001111001111110
0100001000001001000001000010
0100001000001001000001000010
0111111111111111111111111110
0000000000000000000000000000";

        int X = 1, Y = 31;
        using(StringReader reader = new StringReader(data))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                X = 1; //for every line
                for (int i = 0; i < line.Length; ++i)
                {
                    Tile newTile = new Tile(X,Y);

                    //if the tile we read is a valid tile (movable)
                    if (line[i] == "i")
                    {
                        //check for left-right neighbour
                        if (i != 0 && line[i - 1] == "1")
                        {
                            //assign each tile to the corresponding side of other tile
                            newTile.left = tiles[tiles.Count - 1];
                            tiles[tiles.Count - 1].right = newTile;

                            //adjust adjacent tile counts of each tile
                            newTile.adjacentCount++;
                            tiles[tiles.Count - 1].adjacentCount++;
                        }
                    }

                    //if the current tile is not movable
                    else newTile.occupied = true;

                    //check for up-down neighbour, starting from second row (Y < 30)
                    int upNeighbour = tiles.Count - line.Length; //up neighbour index
                    if (Y < 30 && !newTile.occupied && !tiles[upNeighbour].occupied)
                    {
                        tiles[upNeighbour].down = newTile;
                        newTile.up = tiles[upNeighbour];

                        //adjust adjacent tile counts of each tile
                        newTile.adjacentCount++;
                        tiles[upNeighbour].adjacentCount++;
                    }

                    tiles.Add(newTile);
                    X++;
                }

                Y--;
            }
        }

        //after reading all tiles, determine the intersection tiles
        foreach (Tile tile in tiles)
        {
            if (tile.adjacentCount > 2)
                tile.isIntersection = true;
        }
    }

//===============================================================================================================
    //Draw lines between neighbour tiles (debug)
    void Drawneighbours()
    {
        foreach(Tile tile in tiles)
        {
            Vector3 pos = new Vector3(tile.x, tile.y, 0);
            Vector3 up = new Vector3(tile.x+0.1f, tile.y+1, 0);
            Vector3 down = new Vector3(tile.x-0.1f, tile.y-1, 0);
            Vector3 left = new Vector3(tile.x-1, tile.y+0.1f, 0);
            Vector3 right = new Vector3(tile.x+1, tile.y-0.1f, 0);

            if(tile.up != null) Debug.DrawLine(pos, up);
            if(tile.down != null) Debug.DrawLine(pos, down);
            if(tile.left != null) Debug.DrawLine(pos, left);
            if(tile.right != null) Debug.DrawLine(pos, right);
        }
    }

//===========================================================================================================
    //Returns the index in the tiles list of a given tile's coordinates
    public int Index(int X, int Y)
    {
        //if the requested index is in bounds
        //The code below was commented out
        Debug.Log("Index called for X: " + X + ", Y: "+ Y);
        if(X>=1 && X<=28 && Y<= 31 && Y>=1)
            return (31 -Y)*28 + X - 1;

        //else, if the requested index is out of bounds
        //return closet in-bounds tile's index
        if(X < 1)   X =1;
        if(X > 28)  X = 28;
        if(Y < 1)   Y = 1;
        if(Y > 31)  Y = 31;

        return (31 - Y)*28 + X -1;

    }

    public int Index(Tile tile)
    {
        return (31 - tile.y)* 28 + tile.x-1;
    }

//=======================================================================================================
    //Returns the distance between 2 tiles
    public float distance (Tile tile1, Tile tile2)
    {
        return Mathf.Sqrt( Mathf.Pow(tile1.x - tile2.x, 2) + Mathf.Pow(tile1.y - tile2.y, 2));
    }
}
