using System;
using System.Collections.Generic;
using System.Linq;

namespace TetrisGame.Domain
{
    /// <summary>
    /// Represents a tetromino piece with its shape and position
    /// </summary>
    public class Tetromino
    {
        private readonly List<Position> _relativePositions;
        private Position _origin;
        private readonly TetrominoType _type;
        private int _rotationState;
        private readonly ConsoleColor _color;

        public TetrominoType Type => _type;
        public ConsoleColor Color => _color;

        private static readonly Dictionary<TetrominoType, List<List<Position>>> ShapeDefinitions = new Dictionary<TetrominoType, List<List<Position>>>
        {
            // Each shape has 4 rotation states defined by relative positions from the origin
            {
                TetrominoType.I, new List<List<Position>> {
                    new List<Position> { new Position(0, -1), new Position(0, 0), new Position(0, 1), new Position(0, 2) },
                    new List<Position> { new Position(-1, 0), new Position(0, 0), new Position(1, 0), new Position(2, 0) },
                    new List<Position> { new Position(0, -1), new Position(0, 0), new Position(0, 1), new Position(0, 2) },
                    new List<Position> { new Position(-1, 0), new Position(0, 0), new Position(1, 0), new Position(2, 0) }
                }
            },
            {
                TetrominoType.O, new List<List<Position>> {
                    new List<Position> { new Position(0, 0), new Position(0, 1), new Position(1, 0), new Position(1, 1) },
                    new List<Position> { new Position(0, 0), new Position(0, 1), new Position(1, 0), new Position(1, 1) },
                    new List<Position> { new Position(0, 0), new Position(0, 1), new Position(1, 0), new Position(1, 1) },
                    new List<Position> { new Position(0, 0), new Position(0, 1), new Position(1, 0), new Position(1, 1) }
                }
            },
            {
                TetrominoType.T, new List<List<Position>> {
                    new List<Position> { new Position(0, -1), new Position(0, 0), new Position(0, 1), new Position(1, 0) },
                    new List<Position> { new Position(-1, 0), new Position(0, 0), new Position(1, 0), new Position(0, 1) },
                    new List<Position> { new Position(-1, 0), new Position(0, -1), new Position(0, 0), new Position(0, 1) },
                    new List<Position> { new Position(-1, 0), new Position(0, 0), new Position(1, 0), new Position(0, -1) }
                }
            },
            {
                TetrominoType.S, new List<List<Position>> {
                    new List<Position> { new Position(0, 0), new Position(0, 1), new Position(1, -1), new Position(1, 0) },
                    new List<Position> { new Position(-1, 0), new Position(0, 0), new Position(0, 1), new Position(1, 1) },
                    new List<Position> { new Position(0, 0), new Position(0, 1), new Position(1, -1), new Position(1, 0) },
                    new List<Position> { new Position(-1, 0), new Position(0, 0), new Position(0, 1), new Position(1, 1) }
                }
            },
            {
                TetrominoType.Z, new List<List<Position>> {
                    new List<Position> { new Position(0, -1), new Position(0, 0), new Position(1, 0), new Position(1, 1) },
                    new List<Position> { new Position(-1, 1), new Position(0, 0), new Position(0, 1), new Position(1, 0) },
                    new List<Position> { new Position(0, -1), new Position(0, 0), new Position(1, 0), new Position(1, 1) },
                    new List<Position> { new Position(-1, 1), new Position(0, 0), new Position(0, 1), new Position(1, 0) }
                }
            },
            {
                TetrominoType.J, new List<List<Position>> {
                    new List<Position> { new Position(0, -1), new Position(0, 0), new Position(0, 1), new Position(1, 1) },
                    new List<Position> { new Position(-1, 0), new Position(0, 0), new Position(1, 0), new Position(-1, 1) },
                    new List<Position> { new Position(-1, -1), new Position(0, -1), new Position(0, 0), new Position(0, 1) },
                    new List<Position> { new Position(-1, 0), new Position(0, 0), new Position(1, 0), new Position(1, -1) }
                }
            },
            {
                TetrominoType.L, new List<List<Position>> {
                    new List<Position> { new Position(0, -1), new Position(0, 0), new Position(0, 1), new Position(1, -1) },
                    new List<Position> { new Position(-1, 0), new Position(0, 0), new Position(1, 0), new Position(1, 1) },
                    new List<Position> { new Position(-1, 1), new Position(0, -1), new Position(0, 0), new Position(0, 1) },
                    new List<Position> { new Position(-1, -1), new Position(-1, 0), new Position(0, 0), new Position(1, 0) }
                }
            }
        };

        private static readonly Dictionary<TetrominoType, ConsoleColor> ColorMap = new Dictionary<TetrominoType, ConsoleColor>
        {
            { TetrominoType.I, ConsoleColor.Cyan },
            { TetrominoType.O, ConsoleColor.Yellow },
            { TetrominoType.T, ConsoleColor.Magenta },
            { TetrominoType.S, ConsoleColor.Green },
            { TetrominoType.Z, ConsoleColor.Red },
            { TetrominoType.J, ConsoleColor.Blue },
            { TetrominoType.L, ConsoleColor.DarkYellow }
        };

        public Tetromino(TetrominoType type, Position origin)
        {
            _type = type;
            _origin = origin;
            _rotationState = 0;
            _color = ColorMap[type];
            _relativePositions = ShapeDefinitions[type][_rotationState];
        }

        public List<Position> GetAbsolutePositions()
        {
            return _relativePositions.Select(rp => new Position(rp.Row + _origin.Row, rp.Column + _origin.Column)).ToList();
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    _origin = new Position(_origin.Row, _origin.Column - 1);
                    break;
                case Direction.Right:
                    _origin = new Position(_origin.Row, _origin.Column + 1);
                    break;
                case Direction.Down:
                    _origin = new Position(_origin.Row + 1, _origin.Column);
                    break;
            }
        }

        public void Rotate()
        {
            _rotationState = (_rotationState + 1) % 4;
            _relativePositions.Clear();
            foreach (var position in ShapeDefinitions[_type][_rotationState])
            {
                _relativePositions.Add(position);
            }
        }

        public Tetromino GetCopy()
        {
            return new Tetromino(_type, new Position(_origin.Row, _origin.Column));
        }

        public void SetOrigin(Position newOrigin)
        {
            _origin = newOrigin;
        }

        public Position GetOrigin()
        {
            return _origin;
        }
    }
}