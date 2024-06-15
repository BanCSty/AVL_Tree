using AVL_Tree.AVL;
using AVL_Tree.Entitys;

AVLTree tree = new AVLTree();

User user1 = new User(Guid.NewGuid(), "Alice");
User user2 = new User(Guid.NewGuid(), "Bob");
User user3 = new User(Guid.NewGuid(), "Charlie");

tree.Insert(user1);
tree.Insert(user2);
tree.Insert(user3);

User searchResult = tree.Search(user2.Id);
tree.DeleteIterative(user2.Id);

var users = tree.GetAllValues();

if (searchResult != null)
{
    Console.WriteLine($"User found: {searchResult.Name}\n");
}
else
{
    Console.WriteLine("User not found.\n");
}

foreach (var user in users)
{
    Console.WriteLine($"User ID: {user.Id}, Name: {user.Name}");
}
