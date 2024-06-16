using AVL_Tree.AVL;
using AVL_Tree.Entitys;

AVLTree tree = new AVLTree();

User user1 = new User(Guid.NewGuid(), "Alice", 21);
User user2 = new User(Guid.NewGuid(), "Bob", 31);
User user3 = new User(Guid.NewGuid(), "Charlie", 21);

tree.Insert(user1);
tree.Insert(user2);
tree.Insert(user3);

User searchResult = tree.Search(user2.Id);
//tree.DeleteIterative(user2.Id);

var users = tree.GetAllUsers();

var minAgeUsers = tree.GetUsersWithMinAge();

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

Console.WriteLine("\nShow users with minumal age");
foreach (var user in minAgeUsers)
{
    Console.WriteLine($"Users Age: {user.Age}, Name :{user.Name}");
}
