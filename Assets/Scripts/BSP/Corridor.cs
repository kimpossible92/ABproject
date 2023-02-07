using UnityEngine;

namespace SnakeMaze.BSP
{
    public class Corridor
    {
        private float _internalWitdh;
        private Vector2 _center;
        private Vector2 _endPoint;
        private Vector2 _startPoint;
        private bool _horizontalCorridor;

        public bool HorizontalCorridor
        {
            get => _horizontalCorridor;
            set => _horizontalCorridor = value;
        }

        public GameObject CorridorGO { get; set; }

        public Corridor(Vector2 start, Vector2 end, float width, GameObject corridorGO)
        {
            _startPoint = start;
            _endPoint = end;
            _internalWitdh = width;
            CorridorGO = corridorGO;
        }
        public Corridor(Vector2 start, Vector2 end, float width)
        {
            _startPoint = start;
            _endPoint = end;
            _internalWitdh = width;
        }

        public Vector2 Center
        {
            get
            {
                return (_startPoint + _endPoint) / 2;
                /*                if (startPoint.x == endPoint.x){
                                    _center.x = startPoint.x;
                                    _center.y = (startPoint.y<endPoint.y)?(endPoint.y-startPoint.y)/2:(startPoint.y+endPoint.y)/2;
                                }
                                else if (startPoint.y == endPoint.y){
                                    _center.x = (startPoint.x<endPoint.x)?(endPoint.x-startPoint.x)/2:(startPoint.x-endPoint.x)/2;
                                    _center.y = startPoint.y;
                                }
                                return _center;
                */
            }
        }

        public float Height
        {
            get
            {
                if (_startPoint.y == _endPoint.y)
                    return _internalWitdh;
                else
                    return Vector2.Distance(_startPoint, _endPoint);
            }
        }

        public float Width
        {
            get
            {
                if (_startPoint.y == _endPoint.y)
                    return Vector2.Distance(_startPoint, _endPoint);
                else
                    return _internalWitdh;
            }
        }

        public override string ToString()
        {
            return $"<{_startPoint}>,<{_endPoint}>";
        }
    }
}