using LoginAPI.Interface;
using LoginAPI.Models;

namespace LoginAPI.Repos
{
    public class UnitofWorkRepo : IUnitofWork
    {
        public iToDoItemRepository todoitemRepository { get; private set; }
        //declare private var that are used for dapper like _db
        // then this._db = _db; inside the func and add IDbConnection _db as arg
        private readonly UserDBContext UserDBContext;

        public UnitofWorkRepo(UserDBContext UserDBContext)
        {
            this.UserDBContext = UserDBContext;
            todoitemRepository = new TodoItemRepos(UserDBContext);
        }

        public async Task CompleteAsync()
        {
            await this.UserDBContext.SaveChangesAsync();
        }
    }
}
