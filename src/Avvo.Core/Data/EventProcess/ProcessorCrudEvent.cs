using System.Diagnostics;
using System.Reflection;
using Avvo.Core.Commons.Entities;
using Avvo.Core.Commons.Interfaces;
using Avvo.Core.Messaging.Publisher;
using Microsoft.Extensions.Logging;

namespace Avvo.Core.Data.EntityFramework.EventProcess
{
    public class ProcessorCrudEvent : ICrudEventService
    {
        private readonly IUserIdentity _userIdentity;
        private readonly ILogger<ProcessorCrudEvent> _logger;
        private readonly ActivitySource _activitySource;
        private readonly ISqsPublisher<CrudEventMessage> _publisher;

        public ProcessorCrudEvent(IUserIdentity userIdentity, ILogger<ProcessorCrudEvent> logger, ActivitySource activitySource, ISqsPublisher<CrudEventMessage> publisher)
        {
            _userIdentity = userIdentity;
            _logger = logger;
            _activitySource = activitySource;
            _publisher = publisher;
        }

        public object? GetPropertyValue(object obj, string propertyName)
        {
            if (obj == null) return null;

            var property = obj.GetType().GetProperties()
                             .FirstOrDefault(pi => pi.Name == propertyName);

            if (property == null)
            {
                _logger.LogWarning("Property '{PropertyName}' not found on type '{TypeName}'.", propertyName, obj.GetType().Name);
                return null;
            }

            return property.GetValue(obj);
        }

        public async Task ExecuteAsync<TEntity>(TEntity entity, CrudEventOperationEnum operation) where TEntity : class
        {
            using var activity = _activitySource.StartActivity($"CrudEventProcess_{nameof(ExecuteAsync)}", ActivityKind.Internal)!;

            try
            {
                if (entity is ICrudEventProcessor eventEntity)
                {
                    if (eventEntity.PropagateEvent && eventEntity.PropagateDestination != null && eventEntity.PropagateDestination.Any())
                    {
                        Guid entityId = Guid.Empty;

                        if (entity is IBaseEntity baseEntity)
                        {
                            entityId = baseEntity.Id;
                        }

                        Guid tenantId = Guid.Empty;

                        if (entity is ITenantEntity tenantEntity)
                        {
                            tenantId = tenantEntity.TenantId;
                        }

                        var crudEventMessage = CrudEventMessage.Create
                        (
                            entityId,
                            tenantId,
                            operation,
                            string.IsNullOrEmpty(eventEntity.OverrideEntityName) ? entity.GetType().Name : eventEntity.OverrideEntityName,
                            new
                            {
                                Id = entityId,
                                BusinessId = GetPropertyValue(eventEntity, "BusinessId"),
                            },
                            CrudEventAuthentication.Create(_userIdentity),
                            eventEntity.PropagateDestination,
                            eventEntity.EventCustomData
                        );

                        await _publisher.PublishAsync(crudEventMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"CrudEventProcess_{nameof(ExecuteAsync)} Couldn't send message: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                activity.SetStatus(ActivityStatusCode.Error, errorMessage);
            }
        }
        public void CleanEventPropagation<TEntity>(TEntity entity)
        {
            if (entity is ICrudEventProcessor eventEntity)
            {
                eventEntity.PropagateEvent = false;
                eventEntity.PropagateDestination = null;
            }
        }
        public TEntity DeepClone<TEntity>(TEntity source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return (TEntity)DeepCloneInternal(source, new Dictionary<object, object>());
        }

        private object DeepCloneInternal(object source, Dictionary<object, object> clones)
        {
            if (source == null)
                return null;

            Type type = source.GetType();

            // If the object has already been cloned, return the existing clone
            if (clones.ContainsKey(source))
                return clones[source];

            // Handle primitive types and string
            if (type.IsPrimitive || type == typeof(string))
                return source;

            // If the object implements ICrudEventProcessor
            if (type.GetInterfaces().Contains(typeof(ICrudEventProcessor)))
            {
                // Create a new instance of the same type
                object clone = Activator.CreateInstance(type);

                // Store the clone before cloning its properties to handle circular references
                clones[source] = clone;

                // Get all properties of the type
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                                .Where(p => !p.GetGetMethod().IsVirtual || !IsProxyType(p.PropertyType))
                                                .ToArray();
                // PropertyInfo[] properties2 = type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.CanRead && property.CanWrite)
                    {
                        // Clone the property value recursively
                        object value = property.GetValue(source);
                        object clonedValue = DeepCloneInternal(value, clones);
                        property.SetValue(clone, clonedValue);
                    }
                }

                return clone;
            }

            return source;
        }
        private bool IsProxyType(Type type)
        {
            // Check if the type is generated by a proxy generator (e.g., DynamicProxy2)
            return type.Namespace.Contains("Castle.Proxies") || type.Namespace.Contains("System.Data.Entity.DynamicProxies") ||
            type.Namespace.Contains("Castle") ||
           type == typeof(Microsoft.EntityFrameworkCore.Infrastructure.ILazyLoader) ||
            type.GetInterfaces().Contains(typeof(Microsoft.EntityFrameworkCore.Infrastructure.ILazyLoader));
        }
    }
}
