using Avvo.Core.Commons.Entities;

namespace Avvo.Core.Commons.Interfaces;

/// <summary>
/// Define métodos para processamento de eventos CRUD e clonagem de entidades.
/// </summary>
public interface ICrudEventService
{
    /// <summary>
    /// Executa um evento de operação CRUD para a entidade especificada.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade deve ser uma classe.</typeparam>
    /// <param name="entity">A entidade a ser processada.</param>
    /// <param name="operation">A operação CRUD a ser executada.</param>
    /// <returns>Uma tarefa assíncrona com um <see cref="Result"/> indicando sucesso ou falha.</returns>
    Task ExecuteAsync<TEntity>(TEntity entity, CrudEventOperationEnum operation) where TEntity : class;

    /// <summary>
    /// Limpa o estado de propagação de eventos para a entidade especificada.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <param name="entity">A entidade a ser limpa.</param>
    void CleanEventPropagation<TEntity>(TEntity entity);

    /// <summary>
    /// Cria uma cópia profunda da entidade especificada.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <param name="source">A entidade de origem a ser clonada.</param>
    /// <returns>Uma cópia profunda da entidade.</returns>
    TEntity DeepClone<TEntity>(TEntity source);
}
