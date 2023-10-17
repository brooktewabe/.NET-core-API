using Microsoft.EntityFrameworkCore;
using LoginAPI.Interface;
using LoginAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;
using AutoMapper;

namespace LoginAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemController : ControllerBase
    {
        private readonly IUnitofWork unitofWork;
        private readonly IMapper _mapper;

        public ToDoItemController(IMapper mapper, IUnitofWork work)
        {
            this.unitofWork = work;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entities = await this.unitofWork.todoitemRepository.GetAllAsync();
            return Ok(entities);
        }
        [HttpGet("GetDTO")]
        public async Task<IActionResult> GetAllDTO()
        {
            var entities = await this.unitofWork.todoitemRepository.GetAllAsync();
            var toDoItemsdto = _mapper.Map<IEnumerable<ToDoItemRespDTO>>(entities);
            return Ok(toDoItemsdto);
        }

        [HttpGet("GetPaginated")]
        public async Task<IActionResult> GetAllPaginated(int page = 1, int pageSize = 4)
        {
            var entities = await this.unitofWork.todoitemRepository.GetAllAsync();

            var totalItems = entities.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var paginatedEntities = entities
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var paginationMetadata = new
            {
                totalCount = totalItems,
                pageSize,
                currentPage = page,
                totalPages
            };

            Response.Headers.Add("Total-Count", totalItems.ToString());
            Response.Headers.Add("Page-Count", totalPages.ToString());
            Response.Headers.Add("Page-Number", page.ToString());
            Response.Headers.Add("Page-Size", pageSize.ToString());

            return Ok(paginatedEntities);
        }
        [HttpGet("Getbycode/{id}")]
        public async Task<IActionResult> Getbycode(int id)
        {
            var entities = await this.unitofWork.todoitemRepository.GetAsync(id);
            return Ok(entities);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TodoItem todo)
        {
            var _data = await this.unitofWork.todoitemRepository.AddEntity(todo);
            await this.unitofWork.CompleteAsync();
            return Ok(_data);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(TodoItem todo)
        {
            var _data = await this.unitofWork.todoitemRepository.UpdateEntity(todo);
            await this.unitofWork.CompleteAsync();
            return Ok(_data);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var _data = await this.unitofWork.todoitemRepository.DeleteEntity(id);
            await this.unitofWork.CompleteAsync();
            return Ok(_data);
        }
    }
}
