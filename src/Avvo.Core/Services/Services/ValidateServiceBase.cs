using System.Threading.Tasks;
using FluentValidation;
using Avvo.Core.Services.Interfaces;

namespace Avvo.Core.Services.Services
{
    public abstract class ValidateServiceBase<TEntity> : AbstractValidator<TEntity>, IValidateService<TEntity>
        where TEntity : class
    {
        public ValidateServiceBase()
        {
            ConfigureValidations();
        }

        protected virtual void ConfigureValidations() { }

        public virtual Task ExecuteAsync(TEntity entity)
        {
            if (entity == null)
                throw new ValidationException("Entity is null!");

            return this.ValidateAndThrowAsync(entity);
        }
    }
}
