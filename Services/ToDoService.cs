using back.Data;
using back.Data.Entities;
using back.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace back.Services
{
    public class ToDoService
    {
        private readonly EfDbContex dbContex;
        private readonly FileService fileService;

        public ToDoService(EfDbContex dbContex, FileService fileService)
        {
            this.dbContex = dbContex;
            this.fileService = fileService;
        }
        public async Task<ToDoItemViewModel> AddNewTask(ToDoItemViewModel data)
        {
            using (dbContex)
            {
                var item = new Data.Entities.ToDoItem()
                {
                    IsDone = data.IsDone,
                    MyDateTime = data.MyDateTime,
                    Task = data.Task
                };
                dbContex.ToDoItems.Add(item);
                await fileService.RangeFiles(data.MyRes, item.Id);
                dbContex.SaveChanges();
            }
            return data;
        }
        public async Task<List<ToDoItemViewModel>> GetNewTasks()
        {
            var tasks = await dbContex.ToDoItems.AsNoTracking().Include(x=>x.Files).ToListAsync();
            return tasks.Select(x => new ToDoItemViewModel()
            {
                Id = x.Id,
                IsDone = x.IsDone,
                MyDateTime = x.MyDateTime,
                Task = x.Task,
                FileNames = x.Files.Select(x=>x.Name).ToList()
            }).ToList();
        }
        public async Task<ToDoItemViewModel> GetNewTask(Guid id)
        {
            var task = await dbContex.ToDoItems.AsNoTracking().Include(x => x.Files).FirstOrDefaultAsync(x=>x.Id==id);
            return new ToDoItemViewModel()
            {
                Id = task.Id,
                IsDone = task.IsDone,
                MyDateTime = task.MyDateTime,
                Task = task.Task,
                FileNames = task.Files.Select(x => x.Name).ToList()
            };
        }
        public async Task UpdateStatus (Guid id)
        {
            var item =  await dbContex.ToDoItems.FindAsync(id);
            if(item is not null)
            item.IsDone = !item.IsDone;
            dbContex.Update(item);
            dbContex.SaveChanges();
        }
        public async Task<List<ToDoItemViewModel>> GetNewTaskByPredicate(Expression<Func<ToDoItem,bool>> expression)
        {
            var tasks = await dbContex.ToDoItems.AsNoTracking().Where(expression).Include(x=>x.Files).ToListAsync();
            return tasks.Select(x => new ToDoItemViewModel()
            {
                Id = x.Id,
                IsDone = x.IsDone,
                MyDateTime = x.MyDateTime,
                Task = x.Task,
                FileNames = x.Files.Select(x => x.Name).ToList()
            }).ToList();
        }
        public async Task Delete(Guid id)
        {
            var item = await dbContex.ToDoItems.FindAsync(id);
            if (item is not null)
                item.IsDone = !item.IsDone;
            dbContex.ToDoItems.Remove(item);
            dbContex.SaveChanges();
        }
    }
}
