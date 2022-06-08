using Microsoft.AspNetCore.Mvc;
using MircrolisR.Logging;
using System.Text.Json;

namespace MicrolisR.Api;

[ApiController]
[Route("[controller]")]
public abstract class CrudController<TId, TEntity> : ControllerBase, ICrudController<TId, TEntity>
    where TEntity : class, IEntity<TId>
    where TId : struct
{
    private readonly ILogger? _logger;
    private readonly IRepository<TId, TEntity>? _repository;

    protected ILogger? Logger => _logger;

    protected CrudController(ILogger? logger = default, IRepository<TId, TEntity>? repository = default)
    {
        _repository = repository;
        _logger = logger;
    }

    private const string ErrorMessage = "An unexpected error happend";
    private const string ErrorMessageTemplate = ErrorMessage + ". {0}";

    [HttpGet]
    public virtual async Task<ActionResult<IEnumerable<TEntity>>> GetAsync()
    {
        try
        {
            var list = await (_repository?.GetAsync() ?? throw new NotImplementedException());

            _logger?.Debug.LogDebug("get list of results with success. number of records: {0}", list.Count());
            return this.Ok(list);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ErrorMessageTemplate, ex.ToString());
            return this.BadRequest(ErrorMessage);
        }
    }

    [HttpGet("{id}")]
    public virtual async Task<ActionResult<TEntity?>> GetAsync([FromRoute] TId id)
    {
        if (id.Equals(default(TId)))
            return this.BadRequest();

        try
        {
            var entity = await  (_repository?.GetAsync(id) ?? throw new NotImplementedException());

            _logger?.Debug.LogDebug("get of result with success. Result is: {0}", JsonSerializer.Serialize(entity));
            return this.Ok(entity);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ErrorMessageTemplate, ex.ToString());
            return this.BadRequest(ErrorMessage);
        }
    }

    [HttpDelete("{id}")]
    public virtual async Task<IActionResult> DeleteAsync([FromRoute] TId id)
    {
        if (id.Equals(default(TId)))
            return this.BadRequest();

        try
        {
            await (_repository?.DeleteAsync(id) ?? throw new NotImplementedException());

            _logger?.Debug.LogDebug("deleted entity with success");
            return this.Ok();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ErrorMessageTemplate, ex.ToString());
            return this.BadRequest(ErrorMessage);
        }
    }

    [HttpPut("{id}")]
    public virtual async Task<IActionResult> UpdateAsync([FromRoute] TId id, [FromBody] TEntity entity)
    {
        if (id.Equals(default(TId)) || entity == default(TEntity))
            return this.BadRequest();

        try
        {
            await (_repository?.UpdateAsync(id, entity) ?? throw new NotImplementedException());

            _logger?.Debug.LogDebug("updated entity with success. Updated entity is: {0}", JsonSerializer.Serialize(entity));
            return this.Ok();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ErrorMessageTemplate, ex.ToString());
            return this.BadRequest(ErrorMessage);
        }
    }

    [HttpPost]
    public virtual async Task<IActionResult> CreateAsync([FromBody] TEntity entity)
    {
        if (entity == default(TEntity))
            return this.BadRequest();

        try
        {
            await (_repository?.CreateAsync(entity) ?? throw new NotImplementedException());

            _logger?.Debug.LogDebug("created entity with success. New entity is: {0}", JsonSerializer.Serialize(entity));
            return this.Ok();
        }
        catch (Exception ex)
        {
            _logger?.LogError(ErrorMessageTemplate, ex.ToString());
            return this.BadRequest(ErrorMessage);
        }
    }
}

//[AttributeUsage(AttributeTargets.Class)]
//public class CrudControllerAttribute<TRepository, TId, TEntity> : RouteAttribute
//    where TRepository : IRepository<TId, TEntity>
//{
//    public CrudControllerAttribute(string groupeName = "[controller]") : base(groupeName)
//    {

//    }
//}

