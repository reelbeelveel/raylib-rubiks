using Raylib_cs;
using static Raylib_cs.Raylib;

using static rbx.Keybinds;
using static rbx.RbxWindow;
using rbx.Colors;

using System;
using System.Numerics;
using System.Collections.Generic;



namespace rbx.Puzzle {
    public enum Mvmt {
        X, Xp, Y, Yp, Z, Zp,
        F, Fp, R, Rp, U, Up, D, Dp, B, Bp, L, Lp,
        M, Mp, E, Ep, S, Sp
    }
    public enum Side {
        X, Y, Z,
    }
    public class MoveObject {
        public MoveObject(Mvmt mvmt){
            this.mvmt = mvmt;
        }
        public static MoveObject operator!(MoveObject lhs) {
            return (Mvmt)(((int)lhs.mvmt % 2 != 0) ? lhs.mvmt - 1 : lhs.mvmt + 1);
        }
        public static bool IsOpposite(MoveObject lhs, MoveObject rhs) {
            return (lhs == !rhs);
        }
        public static implicit operator Mvmt(MoveObject m) => m.mvmt;
        public static implicit operator MoveObject(Mvmt m) => new MoveObject(m);
        private Mvmt mvmt;
    }
    public class RubixCube {
        private const double BEZEL_RATIO = 0.15;
        public RubixCube(uint size = 3) {
            Size = size;
            for(uint i = 0; i < 6; i++)
                Faces.Add(new Face(i, size, this));
            ConnectFaces();
        }
        public void Shuffle(uint moves = 1000) {
            for(uint i = 0; i < moves; i++) {
                MoveObject move = new MoveObject(RbxWindow.rng.GenMvmt());
                if(Moves.Count > 0 && MoveObject.IsOpposite(Moves[Moves.Count-1], move)) {
                    i--;
                    continue;
                }
                Move(move);
            }
        }

        private void ConnectFaces() {
            Faces[0].Connect(new Face[]
                    {Faces[1], Faces[3], Faces[2], Faces[4], Faces[5]});
            Faces[1].Connect(new Face[]
                    {Faces[5], Faces[0], Faces[2], Faces[4], Faces[3]});
            Faces[2].Connect(new Face[]
                    {Faces[1], Faces[3], Faces[5], Faces[0], Faces[4]});
            Faces[3].Connect(new Face[]
                    {Faces[0], Faces[5], Faces[2], Faces[4], Faces[1]});
            Faces[4].Connect(new Face[]
                    {Faces[1], Faces[3], Faces[0], Faces[5], Faces[2]});
            Faces[5].Connect(new Face[]
                    {Faces[1], Faces[3], Faces[4], Faces[2], Faces[0]});

            this.X = Faces[0];
            this.Y = Faces[2];
            this.Z = Faces[1];
        }
        public void DrawMiniMap() {
            // TODO figure out anti aliasing
            float size = RbxWindow.MiniMapUnitSize(Size);
            float BezlSize = (float)(size * BEZEL_RATIO);
            float TileSize = BezlSize + size;
            float FaceSize = (float)((size * Size) + (BezlSize * (Size + 1)));
            float TLxcoord = (float)RbxWindow.MiniMapCenter().X - (FaceSize/2);
            float TLycoord = (float)RbxWindow.MiniMapCenter().Y - (FaceSize/2);
            float TMxcoord = TLxcoord;
            float TMycoord = TLycoord;
            Rectangle bkdrop = new Rectangle(TMxcoord, TMycoord, FaceSize, FaceSize);
            Raylib.DrawRectangleRec(bkdrop, SystemPalette.cubeBg);
            //Raylib.DrawText($"X {this.X.Id}", (int)TMxcoord, (int)TMycoord, 10, SystemPalette.fg);
            for(int row = 0; row < Size; row++) {
                for(int column = 0; column < Size; column++) {
                    Rectangle tileRect = new Rectangle(
                            TMxcoord+BezlSize+(column * TileSize),
                            TMycoord+BezlSize+(row * TileSize),
                            size,size);
                    Raylib.DrawRectangleRec(tileRect, SystemPalette.SideColor[this.X.Tiles[row,column]]);
                }
            }

            TMxcoord = TLxcoord - FaceSize;
            TMycoord = TLycoord;
            bkdrop = new Rectangle(TMxcoord, TMycoord, FaceSize, FaceSize);
            Raylib.DrawRectangleRec(bkdrop, SystemPalette.cubeBg);
            //Raylib.DrawText($"Y {this.Y.Id}", (int)TMxcoord, (int)TMycoord, 10, SystemPalette.fg);
            for(int row = 0; row < Size; row++) {
                for(int column = 0; column < Size; column++) {
                    Rectangle tileRect = new Rectangle(
                            TMxcoord+BezlSize+(column * TileSize),
                            TMycoord+BezlSize+(row * TileSize),
                            size,size);
                    Raylib.DrawRectangleRec(tileRect, SystemPalette.SideColor[this.Y.Tiles[row,column]]);
                }
            }

            TMxcoord = TLxcoord;
            TMycoord = TLycoord - FaceSize;
            bkdrop = new Rectangle(TMxcoord, TMycoord, FaceSize, FaceSize);
            Raylib.DrawRectangleRec(bkdrop, SystemPalette.cubeBg);
            //Raylib.DrawText($"Z {this.Z.Id}", (int)TMxcoord, (int)TMycoord, 10, SystemPalette.fg);
            for(int row = 0; row < Size; row++) {
                for(int column = 0; column < Size; column++) {
                    Rectangle tileRect = new Rectangle(
                            TMxcoord+BezlSize+(column * TileSize),
                            TMycoord+BezlSize+(row * TileSize),
                            size,size);
                    Raylib.DrawRectangleRec(tileRect, SystemPalette.SideColor[this.Z.Tiles[row,column]]);
                }
            }
            TMxcoord = TLxcoord;
            TMycoord = TLycoord + FaceSize;
            bkdrop = new Rectangle(TMxcoord, TMycoord, FaceSize, FaceSize);
            Raylib.DrawRectangleRec(bkdrop, SystemPalette.cubeBg);
            //Raylib.DrawText($"ZOPP {this.ZOpp().Id}", (int)TMxcoord, (int)TMycoord, 10, SystemPalette.fg);
            for(int row = 0; row < Size; row++) {
                for(int column = 0; column < Size; column++) {
                    Rectangle tileRect = new Rectangle(
                            TMxcoord+BezlSize+(column * TileSize),
                            TMycoord+BezlSize+(row * TileSize),
                            size,size);
                    Raylib.DrawRectangleRec(tileRect, SystemPalette.SideColor[this.ZOpp().Tiles[row,column]]);
                }
            }
            TMxcoord = TLxcoord + FaceSize;
            TMycoord = TLycoord;
            bkdrop = new Rectangle(TMxcoord, TMycoord, FaceSize, FaceSize);
            Raylib.DrawRectangleRec(bkdrop, SystemPalette.cubeBg);
            //Raylib.DrawText($"YOPP {this.YOpp().Id}", (int)TMxcoord, (int)TMycoord, 10, SystemPalette.fg);
            for(int row = 0; row < Size; row++) {
                for(int column = 0; column < Size; column++) {
                    Rectangle tileRect = new Rectangle(
                            TMxcoord+BezlSize+(column * TileSize),
                            TMycoord+BezlSize+(row * TileSize),
                            size,size);
                    Raylib.DrawRectangleRec(tileRect, SystemPalette.SideColor[this.YOpp().Tiles[row,column]]);
                }
            }
            TMxcoord = TLxcoord + (2*FaceSize);
            TMycoord = TLycoord;
            //Raylib.DrawText($"XOPP {this.XOpp().Id}", (int)TMxcoord, (int)TMycoord, 10, SystemPalette.fg);
            bkdrop = new Rectangle(TMxcoord, TMycoord, FaceSize, FaceSize);
            Raylib.DrawRectangleRec(bkdrop, SystemPalette.cubeBg);
            for(int row = 0; row < Size; row++) {
                for(int column = 0; column < Size; column++) {
                    Rectangle tileRect = new Rectangle(
                            TMxcoord+BezlSize+(column * TileSize),
                            TMycoord+BezlSize+(row * TileSize),
                            size,size);
                    Raylib.DrawRectangleRec(tileRect, SystemPalette.SideColor[this.XOpp().Tiles[row,column]]);
                }
            }


            Raylib.DrawText($"Viewing face {this.X.Id}", 20, 20, 50, SystemPalette.fg);
            if(Solved())
                Raylib.DrawText("SOLVED", 20, 80, 50, Color.GREEN);
            else
                Raylib.DrawText("NOT SOLVED", 20, 80, 50, Color.RED);
        }

        public bool Solved() {
            bool solved = true;
            foreach(Face f in Faces)
                solved &= f.Solved();
            return solved;
        }

        public void Move(Mvmt? mvmt) {
            if(mvmt == null) return;
            Mvmt m = (Mvmt)mvmt;
            Moves.Add(m);
            InternalMove(m);
        }

        public void Undo() {
            if(Moves.Count > 0) {
                Mvmt m = Moves[Moves.Count-1];
                InternalMove(!new MoveObject(m));
                Moves.RemoveAt(Moves.Count-1);
            }
        }

        private void InternalMove(Mvmt mvmt, bool prime = false) {
            switch(mvmt) {
                case Mvmt.X:
                    {
                        Face tmp = this.Y;
                        this.Y = this.ZOpp();
                        this.Z = tmp;
                    } break;

                case Mvmt.Xp:
                    {
                        Face tmp = this.Z;
                        this.Z = this.YOpp();
                        this.Y = tmp;
                    } break;

                case Mvmt.Y:
                    {
                        Face tmp = this.Z;
                        this.Z = this.XOpp();
                        this.X = tmp;
                    } break;

                case Mvmt.Yp:
                    {
                        Face tmp = this.X;
                        this.X = this.ZOpp();
                        this.Z = tmp;
                    } break;

                case Mvmt.Z:
                    {
                        Face tmp = this.X;
                        this.X = this.YOpp();
                        this.Y = tmp;
                    } break;

                case Mvmt.Zp:
                    {
                        Face tmp = this.Y;
                        this.Y = this.XOpp();
                        this.X = tmp;
                    } break;

                case Mvmt.F: this.X.FMove(prime); break;

                case Mvmt.R: this.X.Rgt.FMove(prime); break;
                case Mvmt.U: this.X.Top.FMove(prime); break;
                case Mvmt.D: this.X.Bot.FMove(prime); break;
                case Mvmt.B: this.X.Opp.FMove(prime); break;
                case Mvmt.L: this.X.Lft.FMove(prime); break;

                case Mvmt.M: this.X.MMove(prime); break;
                case Mvmt.E: this.X.EMove(prime); break;
                case Mvmt.S: this.X.SMove(prime); break;
                default: // Prime Moves
                             InternalMove(mvmt - 1, true);
                             return;
            }
            Console.WriteLine($"Made move {mvmt}{(prime ? 'p' : ' ')}");
            if(mvmt <= Mvmt.Zp)
                foreach(var Face in Faces)
                    Face.Orient();
        }

        protected class Face {
            public Face(uint id, uint size, RubixCube cube) {
                Id = id;
                Size = size;
                Cube = cube;
                Tiles = new uint[size, size];
                for(uint i = 0; i < size; i++)
                    for(uint j = 0; j < size; j++)
                        Tiles[i, j] = Id;
            }

            public static bool operator==(Face lhs, Face rhs) {
                return (lhs.Id == rhs.Id);
            }
            public static bool operator!=(Face lhs, Face rhs) {
                return (lhs.Id != rhs.Id);
            }
            public bool Solved(){
                bool solved = true;
                uint id = Tiles[0,0];
                foreach(uint tile in Tiles)
                    solved &= (tile == id);
                return solved;
            }

            public void Connect(Face[] faces) {
                this.Top = faces[0];
                this.Bot = faces[1];
                this.Lft = faces[2];
                this.Rgt = faces[3];
                this.Opp = faces[4];
            }

            public void Orient() {
                uint wasTop = this.Top.Id;
                if(this == Cube.Z) {
                    this.Top = Cube.XOpp();
                    this.Bot = Cube.X;
                    this.Lft = Cube.Y;
                    this.Rgt = Cube.YOpp();
                } else if(this == Cube.ZOpp()) {
                    this.Top = Cube.X;
                    this.Bot = Cube.XOpp();
                    this.Lft = Cube.Y;
                    this.Rgt = Cube.YOpp();
                } else {
                    this.Top = Cube.Z;
                    this.Bot = Cube.ZOpp();
                    if(this == Cube.X) {
                        this.Lft = Cube.Y;
                        this.Rgt = Cube.YOpp();
                    } else if(this == Cube.Y) {
                        this.Lft = Cube.XOpp();
                        this.Rgt = Cube.X;
                    } else if(this == Cube.XOpp()) {
                        this.Lft = Cube.YOpp();
                        this.Rgt = Cube.Y;
                    } else {
                        this.Lft = Cube.X;
                        this.Rgt = Cube.XOpp();
                    }
                }

                if(wasTop == this.Top.Id) { return; }
                if(wasTop == this.Rgt.Id) { SilentFMove(); return; }
                if(wasTop == this.Bot.Id) { SilentFMove(); SilentFMove(); return; }
                SilentFMove(true);
            }

            protected void SilentFMove(bool prime = false) {
                uint[,] RtoL = new uint[Size, Size];
                uint[,] UtoD = new uint[Size, Size];
                uint[,] Clockwise = new uint[Size, Size];
                uint[,] AntiClockwise = new uint[Size, Size];
                for(uint i = 0; i < Size; i++) {
                    for(uint j = 0; j < Size; j++) {
                        RtoL[i, j] = Tiles[i, Size-j-1];
                        UtoD[i, j] = Tiles[Size-i-1, j];
                    }
                }
                for(uint i = 0; i < Size; i++) {
                    for(uint j = 0; j < Size; j++) {
                        Clockwise[i,j] = UtoD[j, i];
                        AntiClockwise[i,j] = RtoL[j,i];
                    }
                }
                Tiles = prime ? AntiClockwise : Clockwise;
            }

            public void FMove(bool prime = false) {
                SilentFMove(prime);
                if(prime)
                    Top.FSide(Id,
                            Rgt.FSide(Id,
                                Bot.FSide(Id,
                                    Lft.FSide(Id,
                                        Top.FSide(Id)))));
                else
                    Top.FSide(Id,
                            Lft.FSide(Id,
                                Bot.FSide(Id,
                                    Rgt.FSide(Id,
                                        Top.FSide(Id)))));
            }

            public void MMove(bool prime = false) {
                if(Size < 3) return;
                if(prime)
                    this.MSide(
                            Bot.MSide(
                                Opp.MSide(
                                    Top.MSide(
                                        MSide(), true),
                                    true)));
                else
                    this.MSide(
                            Top.MSide(
                                Opp.MSide(
                                    Bot.MSide(MSide(), true), true)));
            }

            public void EMove(bool prime = false) {
                if(Size < 3) return;
                if(prime)
                    this.ESide(
                            Rgt.ESide(
                                Opp.ESide(
                                    Lft.ESide(
                                        this.ESide()))));
                else this.ESide(
                        Lft.ESide(
                            Opp.ESide(
                                Rgt.ESide(
                                    this.ESide()))));
            }

            public void SMove(bool prime = false) {
                if(Size < 3) return;
                if(prime)
                    Top.ESide(
                            Rgt.MSide(
                                Bot.ESide(
                                    Lft.MSide(
                                        Top.ESide(null, true)
                                        ),
                                    true)
                                ),
                            true);
                else
                    Top.ESide(
                            Lft.MSide(
                                Bot.ESide(
                                    Rgt.MSide(
                                        Top.ESide(),
                                        true)
                                    ),
                                true)
                            );
            }

            protected uint[] FSide(uint id, uint[] assign = null) {
                uint[] row = new uint[Size];
                uint? i = null;
                uint? j = null;
                if(Top.Id == id) i = 0;
                if(Bot.Id == id) i = Size - 1;
                if(Lft.Id == id) j = 0;
                if(Rgt.Id == id) j = Size - 1;
                if((i == null && j == null) || (i != null && j != null))
                    throw new Exception("Face not connected to cube properly.");

                if(i == null) {
                    for(i = 0; i < Size; i++) {
                        uint k = (j == 0) ? Size - (uint)i - 1 : (uint)i;
                        row[(uint)i] = Tiles[k, (uint)j];
                        if(assign != null)
                            Tiles[k, (uint)j] = assign[(uint)i];
                    }
                } else {
                    for(j = 0; j < Size; j++) {
                        uint k = (i > 0) ? Size - (uint)j - 1 : (uint)j;
                        row[(uint)j] = Tiles[(uint)i, k];
                        if(assign != null)
                            Tiles[(uint)i, k] = (uint)assign[(uint)j];
                    }
                }
                return row;
            }
            protected uint[] MSide(uint[] assign=null, bool prime=false) {
                uint[] row = new uint[Size];
                uint pos = (Size-1)/2; // TODO fix for other sizes
                for(uint i = 0; i < Size; i++) {
                    uint k = (prime) ? (Size - i) - 1 : i;
                    row[k] = Tiles[i,pos];
                    if(assign != null)
                        Tiles[i,pos] = (uint)assign[i];
                }
                return row;
            }

            protected uint[] ESide(uint[] assign=null, bool prime=false) {
                uint[] row = new uint[Size];
                uint pos = (Size-1)/2; // TODO fix for other sizes
                for(uint i = 0; i < Size; i++) {
                    uint k = (prime) ? (Size - i) - 1 : i;
                    row[k] = Tiles[pos,i];
                    if(assign != null)
                        Tiles[pos,i] = (uint)assign[i];
                }
                return row;
            }

            public RubixCube Cube;
            public uint[,] Tiles;
            public uint Size;
            public Face? Top;
            public Face? Bot;
            public Face? Lft;
            public Face? Rgt;
            public Face? Opp;
            public uint Id;
        }

        private List<Face> Faces = new List<Face>();
        private List<MoveObject> Moves = new List<MoveObject>();
        private Face X;
        private Face Y;
        private Face Z;
        private Face XOpp () => this.X.Opp;
        private Face YOpp () => this.Y.Opp;
        private Face ZOpp () => this.Z.Opp;
        private Face MainFace () => this.X;
        private uint Size;
    }
}
