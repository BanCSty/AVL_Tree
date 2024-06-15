using AVL_Tree.Entitys;

namespace AVL_Tree.AVL
{
    public class AVLTree
    {
        private AVLNode root;

        private int Height(AVLNode node)
        {
            return node?.Height ?? 0;
        }

        private int GetBalance(AVLNode node)
        {
            return node == null ? 0 : Height(node.Left) - Height(node.Right);
        }

        // Поворот вправо
        private AVLNode RightRotate(AVLNode y)
        {
            AVLNode x = y.Left;
            AVLNode T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;

            return x;
        }

        // Поворот влево
        private AVLNode LeftRotate(AVLNode x)
        {
            AVLNode y = x.Right;
            AVLNode T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
            y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;

            return y;
        }


        /*
            Использование стека: Мы используем стек (stack), чтобы отслеживать путь от узла, который мы вставляем или удаляем (key), 
        до корня дерева (root). Это позволяет нам после операции вставки или удаления последовательно 
        подниматься от измененного узла к корню для выполнения балансировки.
 
            Цикл балансировки: После того как мы нашли узел для вставки или удаления и заполнили стек, 
        мы начинаем цикл балансировки. В этом цикле мы извлекаем узлы из стека, обновляем их высоту и проверяем их баланс. 
        Если узел дисбалансирован, мы выполняем соответствующие повороты (RightRotate или LeftRotate), чтобы восстановить баланс.

            Восстановление связей: После каждого поворота мы восстанавливаем связи между узлами и их родителями. 
        Это важно для сохранения структуры AVL-дерева после балансировки.

            Возврат нового корня: По завершению цикла мы возвращаем новый корень поддерева (newRoot), 
        который может измениться в результате балансировки.
         */

        // Метод для балансировки AVL-дерева после вставки или удаления узла
        // root: Корень поддерева, в котором нужно выполнить балансировку
        // key: Ключ (идентификатор) узла, для которого производится балансировка
        private AVLNode BalanceTree(AVLNode root, Guid key)
        {
            Stack<AVLNode> stack = new Stack<AVLNode>();
            AVLNode current = root;

            // Поднимаемся вверх от узла до корня, добавляя все узлы в стек
            while (current != null)
            {
                stack.Push(current);

                if (key.CompareTo(current.Data.Id) < 0)
                    current = current.Left;
                else if (key.CompareTo(current.Data.Id) > 0)
                    current = current.Right;
                else
                    break; // Найден узел с искомым ключом, прекращаем поиск
            }

            AVLNode newRoot = root; // Новый корень поддерева после балансировки

            // Пока есть узлы в стеке, выполняем балансировку
            while (stack.Count > 0)
            {
                current = stack.Pop(); // Берем узел из стека для балансировки

                // Обновляем высоту текущего узла
                current.Height = Math.Max(Height(current.Left), Height(current.Right)) + 1;

                // Получаем баланс текущего узла после обновления высоты
                int balance = GetBalance(current);

                // Выполняем соответствующие повороты в зависимости от баланса
                if (balance > 1 && key.CompareTo(current.Left.Data.Id) < 0)
                {
                    // LL rotation
                    current = RightRotate(current);
                }
                else if (balance < -1 && key.CompareTo(current.Right.Data.Id) > 0)
                {
                    // RR rotation
                    current = LeftRotate(current);
                }
                else if (balance > 1 && key.CompareTo(current.Left.Data.Id) > 0)
                {
                    // LR rotation
                    current.Left = LeftRotate(current.Left);
                    current = RightRotate(current);
                }
                else if (balance < -1 && key.CompareTo(current.Right.Data.Id) < 0)
                {
                    // RL rotation
                    current.Right = RightRotate(current.Right);
                    current = LeftRotate(current);
                }

                // Восстанавливаем связи родителя и текущего узла после поворотов
                if (stack.Count > 0)
                {
                    AVLNode parent = stack.Peek(); // Получаем родителя текущего узла из стека

                    // Устанавливаем текущий узел как левый или правый ребенок родителя
                    if (parent.Left == current)
                        parent.Left = current;
                    else
                        parent.Right = current;
                }
                else
                {
                    newRoot = current; // Обновляем корень поддерева после завершения балансировки
                }
            }

            return newRoot; // Возвращаем новый корень поддерева
        }

        // Вставка узла
        public void Insert(User user)
        {
            if (root == null)
            {
                root = new AVLNode(user);
                return;
            }

            AVLNode current = root;
            AVLNode parent = null;

            while (current != null)
            {
                parent = current;

                if (user.Id.CompareTo(current.Data.Id) < 0)
                    current = current.Left;
                else if (user.Id.CompareTo(current.Data.Id) > 0)
                    current = current.Right;
                else
                    return; // Узел с таким ключом уже существует
            }

            AVLNode newNode = new AVLNode(user);

            if (user.Id.CompareTo(parent.Data.Id) < 0)
                parent.Left = newNode;
            else
                parent.Right = newNode;

            root = BalanceTree(root, user.Id);
        }

        // Поиск узла
        public User Search(Guid id)
        {
            AVLNode node = root;

            while (node != null)
            {
                if (id.CompareTo(node.Data.Id) == 0)
                {
                    return node.Data;
                }
                else if (id.CompareTo(node.Data.Id) < 0)
                {
                    node = node.Left;
                }
                else
                {
                    node = node.Right;
                }
            }

            return null;
        }

        // Удаление узла
        public void DeleteIterative(Guid id)
        {
            if (root == null)
                return;

            AVLNode parent = null;
            AVLNode current = root;

            // Поиск узла для удаления и его родителя
            while (current != null && current.Data.Id != id)
            {
                parent = current;
                if (id.CompareTo(current.Data.Id) < 0)
                    current = current.Left;
                else
                    current = current.Right;
            }

            // Если узел не найден
            if (current == null)
                return;

            // Если узел имеет одного или ноль потомков
            if (current.Left == null || current.Right == null)
            {
                AVLNode newCurrent = current.Left ?? current.Right;

                if (parent == null)
                {
                    root = newCurrent;
                    return;
                }

                if (current == parent.Left)
                    parent.Left = newCurrent;
                else
                    parent.Right = newCurrent;
            }
            else
            {
                // Узел имеет двух потомков
                AVLNode parentSuccessor = current;
                AVLNode successor = current.Right;

                while (successor.Left != null)
                {
                    parentSuccessor = successor;
                    successor = successor.Left;
                }

                if (parentSuccessor != current)
                    parentSuccessor.Left = successor.Right;
                else
                    parentSuccessor.Right = successor.Right;

                current.Data = successor.Data;
            }

            root = BalanceTree(root, id);
        }

        // Получение списка всех пользователей
        public List<User> GetAllValues()
        {
            List<User> result = new List<User>();
            if (root == null)
                return result;

            Stack<AVLNode> stack = new Stack<AVLNode>();
            AVLNode current = root;

            while (stack.Count > 0 || current != null)
            {
                while (current != null)
                {
                    stack.Push(current);
                    current = current.Left;
                }

                current = stack.Pop();
                result.Add(current.Data); // Добавляем данные текущего узла в список
                current = current.Right;
            }

            return result;
        }
    }
}
