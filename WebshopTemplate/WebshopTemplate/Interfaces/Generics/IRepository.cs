namespace WebshopTemplate.Interfaces.Generics;

/// <summary>
/// Represents a generic repository interface.
/// </summary>
/// <typeparam name="T">The type of entity.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Adds a new entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>The added entity.</returns>
    Task<T?> Add(T entity);

    /// <summary>
    /// Gets an entity from the repository by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The retrieved entity.</returns>
    Task<T?> Get(string id);

    /// <summary>
    /// Gets all entities from the repository.
    /// </summary>
    /// <returns>A list of all entities.</returns>
    Task<List<T>?> GetAllAsync();

    /// <summary>
    /// Updates an entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>The updated entity.</returns>
    Task<T?> UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity from the repository by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>The deleted entity.</returns>
    Task<T?> DeleteAsync(string id);
}