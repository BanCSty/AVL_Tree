using AVL_Tree.Entitys;

namespace AVL_Tree.AVL
{
    public class AVLNode
    {
        public User Data { get; set; }
        public AVLNode Left { get; set; }
        public AVLNode Right { get; set; }
        public int Height { get; set; }

        public AVLNode(User data)
        {
            Data = data;
            Height = 1;
        }
    }
}
