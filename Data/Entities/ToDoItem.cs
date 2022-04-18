namespace back.Data.Entities
{
    public class ToDoItem
    {
            public string Task { get; set; }
            public bool IsDone { get; set; }
            public DateTime MyDateTime { get; set; }
            public  ICollection<File> Files { get; set; }
            public Guid Id { get; set; }
    }
}
