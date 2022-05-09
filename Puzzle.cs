using Raylib_cs;
using static Raylib_cs.Raylib;

using rbx.Generator;

using static rbx.Keybinds;
using rbx.Colors;

using System;
using System.Numerics;
using System.Collections.Generic;

namespace rbx {
    public enum Mvmt {
        X, Xp, Y, Yp, Z, Zp,
        F, Fp, R, Rp, U, Up, D, Dp, B, Bp, L, Lp,
        M, Mp, E, Ep, S, Sp
    }
    public enum Side {
        X, Y, Z,
    }
    public class RubixCube {
        public RubixCube(uint size = 3) {
            for(uint i = 0; i < 6; i++)
                Faces.Add(new Face(i, size, this));
            ConnectFaces();
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
            this.MainFace = this.X;
        }

        public void Move(Mvmt mvmt, Side? face = null) {
            Face f = face.HasValue
                ? ((face == Side.X)
                    ? this.X
                    : ((face == Side.Y)
                      ? this.Y
                      : this.Z)
                  )
                : MainFace;
            InternalMove(mvmt, f);
        }

        private void InternalMove(Mvmt mvmt, Face face, bool prime = false) {
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

                case Mvmt.F: face.FMove(prime); break;

                case Mvmt.R: face.Rgt.FMove(prime); break;
                case Mvmt.U: face.Top.FMove(prime); break;
                case Mvmt.D: face.Bot.FMove(prime); break;
                case Mvmt.B: face.Opp.FMove(prime); break;
                case Mvmt.L: face.Lft.FMove(prime); break;

                case Mvmt.M: face.MMove(prime); break;
                case Mvmt.E: face.EMove(prime); break;
                case Mvmt.S: face.SMove(prime); break;
                default: // Prime Moves
                    InternalMove(mvmt - 1, face, true);
                    break;
            }
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

            public void Connect(Face[] faces) {
                this.Top = faces[0];
                this.Bot = faces[1];
                this.Lft = faces[2];
                this.Rgt = faces[3];
                this.Opp = faces[4];
            }

            public void FMove(bool prime = false) {
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
                if(prime) {
                    Top.ISide(Id, Rgt.ISide(Id, Bot.ISide(Id, Lft.ISide(Id, Top.ISide(Id)))));

                } else {
                    Top.ISide(Id, Lft.ISide(Id, Bot.ISide(Id, Rgt.ISide(Id, Top.ISide(Id)))));
                }
            }
            public void MMove(bool prime = false) {
            }
            
            public void EMove(bool prime = false) {
            }

            public void SMove(bool prime = false) {
            }

            protected uint[] ISide(uint id, uint[] assign = null) {
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
                        uint k = (j > 0) ? Size - (uint)i - 1 : (uint)i;
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

            private RubixCube Cube;
            private uint[,] Tiles;
            private uint Size;
            public Face? Top;
            public Face? Bot;
            public Face? Lft;
            public Face? Rgt;
            public Face? Opp;
            public uint Id;
        }

        private List<Face> Faces = new List<Face>();
        private Face X;
        private Face Y;
        private Face Z;
        private Face XOpp () => this.X.Opp;
        private Face YOpp () => this.Y.Opp;
        private Face ZOpp () => this.Z.Opp;
        private Face MainFace;
    }
}
