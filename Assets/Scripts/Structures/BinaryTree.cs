namespace SnakeMaze.Structures
{
    public class BinaryTree<T>
    {
        private BinaryTree<T> _left;
        private BinaryTree<T> _right;
        private Node<T> _root;

        public BinaryTree() { }

        public BinaryTree(T data)
        {
            _root = new Node<T>(data);
            this._left = null;
            this._right = null;
        }

        public BinaryTree(T data, BinaryTree<T> left, BinaryTree<T> right)
        {
            _root = new Node<T>(data);
            this._left = left;
            this._right = right;
        }

        public T Root
        {
            get
            {
                return _root.Value;
            }
            set
            {
                _root.Value = value;
            }
        }

        public BinaryTree<T> Left
        {
            get
            {
                return _left;
            }
            set
            {
                _left = value;
            }
        }

        public BinaryTree<T> Right
        {
            get
            {
                return _right;
            }
            set
            {
                _right = value;
            }
        }

        public bool IsALeaf()
        {
            return (this.Left == null) && (this.Right == null);
        }

        public bool HasTwoChilds()
        {
            return (this.Left != null) && (this.Right != null);
        }

        public override string ToString()
        {   //It shows the tree in order
            string datastring = "";
            if (this != null)
            {
                if (this._left != null)
                    datastring += "<Left>" + this._left.ToString() + "</Left>";

                datastring += "<Root(" + this._root.ToString() + ">";

                if (this._right != null)
                    datastring += "<Right>" + this._right.ToString() + "</Right>";
            }

            return string.Format("{0}", datastring);
        }
    }
}