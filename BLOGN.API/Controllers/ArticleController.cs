using AutoMapper;
using BLOGN.Data.Services.IService;
using BLOGN.Models;
using BLOGN.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BLOGN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        public ArticleController(IArticleService articleService,IMapper mapper)
        {
            _articleService = articleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articles = await _articleService.GetAll();
            var articlesDto = _mapper.Map<IEnumerable<ArticleDto>>(articles);//CategoryDto'yu döndürttürdük.
            return Ok(articlesDto);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            var article = await _articleService.Get(Id);
            var articleDto = _mapper.Map<ArticleDto>(article);
            return Ok(articleDto);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ArticleDto articleDto)
        {
            var article = _mapper.Map<Article>(articleDto);
            var newArticle = await _articleService.Add(article);
            return Created(String.Empty, null); /*_mapper.Map<CategoryDto>(newCategory));*/
        }

        [HttpPatch]
        public async Task<IActionResult> Patch(int Id, ArticleDto articleDto)
        {
            if (Id == 0)
            {
                return BadRequest();
            }
            var article = _mapper.Map<Article>(articleDto);
            _articleService.Update(article);
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var entity = _articleService.Delete(Id);
            return NoContent();
        }

    }
}
