namespace back.Data.Entities
{
    public class File
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public Guid Id { get; set; }
        public Guid ToDoItemId { get; set; }
        public ToDoItem ToDoItem { get; set; }

    }
}
