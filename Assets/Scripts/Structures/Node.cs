namespace SnakeMaze.Structures
{
    public class Node<T>
    {
        public Node() { }

        public Node(T data) => this.Value = data;

        public T Value { get; set; }

        public override string ToString()
        {
            string datastring = (Value == null) ? "null" : Value.ToString();
            return string.Format("{0}", datastring);
        }
    }
}