using AutoMapper;

namespace LoginAPI.Models
{
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItem, ToDoItemRespDTO>();
        }
    }
}
