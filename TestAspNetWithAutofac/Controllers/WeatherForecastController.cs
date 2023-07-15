using System;
using Microsoft.AspNetCore.Mvc;


namespace dotnet_articles_api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController: ControllerBase
    {
        private IRepository _repository;

       // private readonly LoggerProxy _logger;

        public ArticlesController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            var data=_repository.Get(id);
            if(data is null)
            {
                return NotFound("Article not found.");
            }
            return Ok(data); 
        }
        
        
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if(!_repository.Delete(id))
            {
                return NotFound("Article not found.");
            }
            return Ok(); 
        }

        [HttpPost]
        public IActionResult Create([FromBody]ArticleDTO input)
        {
            try
            {
                var articleInput = new Article()
                {
                    Title = input.Title,
                    Text = input.Text
                };
                var id=_repository.Create(articleInput);
                articleInput.Id = id;
                return Created(Url.RouteUrl(id),articleInput);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
          
        }
        
        [HttpPut("{id}")]
        public IActionResult update(Guid id ,[FromBody]ArticleDTO input)
        {
            try
            {
                var articleInput = new Article()
                {
                    Id = id,
                };
                if (!string.IsNullOrWhiteSpace(input.Text))
                {
                    articleInput.Text = input.Text;
                }
                if (!string.IsNullOrWhiteSpace(input.Title))
                {
                    articleInput.Title = input.Title;
                }
                if (!_repository.Update(articleInput))
                {
                    return NotFound("Article not found.");
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest();
            }
           
        }
    }

    public class ArticleDTO {
         public string Title { get; set; } //maybe init here
        public string Text { get; set; }

       public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                return false;
            }

            return true;
        }
    }
}


public interface IRepository
{
    // Returns a found article or null.
    Article Get(Guid id);

    // Creates a new article and returns its identifier.
    // Throws an exception if a article is null.
    // Throws an exception if a title is null or empty.
    Guid Create(Article article);

    // Returns true if an article was deleted or false if it was not possible to find it.
    bool Delete(Guid id);
    
    // Returns true if an article was updated or false if it was not possible to find it.
    // Throws an exception if an articleToUpdate is null.
    // Throws an exception or if a title is null or empty.
    bool Update(Article articleToUpdate);
}


public class Repository:IRepository
{
    private List<Article> _data = new List<Article>(); 
    // Returns a found article or null.
    public Article Get(Guid id)
    {
        return _data.FirstOrDefault(a => a.Id == id);
    }

    // Creates a new article and returns its identifier.
    // Throws an exception if a article is null.
    // Throws an exception if a title is null or empty.
    public Guid Create(Article article)
    {
        article.Id=Guid.NewGuid();
        _data.Add(article);
        return article.Id;
    }

    // Returns true if an article was deleted or false if it was not possible to find it.
    public bool Delete(Guid id)
    {
        var current= _data.FirstOrDefault(a => a.Id == id);
        if (current is not null)
        {
            _data.Remove(current);
            return true;
        }

        return false;
    }

    // Returns true if an article was updated or false if it was not possible to find it.
    // Throws an exception if an articleToUpdate is null.
    // Throws an exception or if a title is null or empty.
    public bool Update(Article articleToUpdate)
    {
        var current= _data.FirstOrDefault(a => a.Id == articleToUpdate.Id);
        if (current is not null)
        {
            current.Text = articleToUpdate.Text;
            current.Title = articleToUpdate.Title;
            return true;
        }

        return false;
    }
}


public class Article
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
}

