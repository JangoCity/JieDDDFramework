﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JieDDDFramework.Core.Domain;
using JieDDDFramework.Core.Exceptions;
using JieDDDFramework.Core.Exceptions.Utilities;
using JieDDDFramework.Data.EntityFramework.DbContext;
using JieDDDFramework.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace JieDDDFramework.Data.EntityFramework.Repositories
{
    /// <summary>
    /// 基于EF实现的默认仓储
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepositoryBase<TEntity> where TEntity : class, IEntity
    {
        protected readonly DomainDbContext _domainDbContext;
        protected DbSet<TEntity> _entities;

        protected DbSet<TEntity> Entities => _entities ?? (_entities = _domainDbContext.Set<TEntity>());

        public IUnitOfWork UnitOfWork => _domainDbContext;

        public Repository(DomainDbContext context) => _domainDbContext = context ?? throw new ArgumentException(nameof(context));


        public virtual void Dispose()
        {
            //
        }

        public virtual void Insert(TEntity entity) => Entities.Add(entity);

        public virtual void Insert(params TEntity[] entities) => Entities.AddRange(entities);

        public virtual void Insert(IEnumerable<TEntity> entities) => Entities.AddRange(entities);

        public virtual Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken)) => Entities.AddAsync(entity, cancellationToken);

        public virtual Task InsertAsync(params TEntity[] entities) => Entities.AddRangeAsync(entities);

        public virtual Task InsertAsync(IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default(CancellationToken)) =>
            Entities.AddRangeAsync(entities, cancellationToken);

        public virtual void Update(TEntity entity) => Entities.Update(entity);

        public virtual void Update(params TEntity[] entities) => Entities.UpdateRange(entities);
        public virtual void Update(IEnumerable<TEntity> entities) => Entities.UpdateRange(entities);

        public virtual void Delete(object id)
        {
            var typeInfo = typeof(TEntity).GetTypeInfo();
            var key = _domainDbContext.Model.FindEntityType(typeInfo).FindPrimaryKey().Properties.FirstOrDefault();
            var property = typeInfo.GetProperty(key?.Name);
            if (property != null)
            {
                var entity = Activator.CreateInstance<TEntity>();
                property.SetValue(entity, id);
                _domainDbContext.Entry(entity).State = EntityState.Deleted;
            }
            else
            {
                var entity = Entities.Find(id);
                if (entity != null)
                {
                    Delete(entity);
                }
            }
        }

        public virtual void Delete(TEntity entity) => Entities.Remove(entity);

        public virtual void Delete(params TEntity[] entities) => Entities.RemoveRange(entities);

        public virtual void Delete(IEnumerable<TEntity> entities) => Entities.RemoveRange(entities);
        
        public TEntity FindEntity(object keyValue)
        {
            var entity = Entities.Find(keyValue);
            if (entity == null || entity is IAggregateRoot)
            {
                return entity;
            }
           throw new DomainException("Entity must implement IAggregateRoot");
        }

        public virtual TEntity FindEntity(Expression<Func<TEntity, bool>> criterion)
        {
            var entity = Entities.SingleOrDefault(criterion);
            if (entity == null || entity is IAggregateRoot)
            {
                return entity;
            }
            throw new DomainException("Entity must implement IAggregateRoot");
        }

        public virtual async Task<TEntity> FindEntityAsync(object keyValue)
        {
            var entity = await Entities.FindAsync(keyValue);
            if (entity ==null || entity is IAggregateRoot)
            {
                return entity;
            }
            throw new DomainException("Entity must implement IAggregateRoot");
        }

        public virtual async Task<TEntity> FindEntityAsync(Expression<Func<TEntity, bool>> criterion)
        {

            var entity = await Entities.SingleOrDefaultAsync(criterion);
            if (entity == null || entity is IAggregateRoot)
            {
                return entity;
            }
            throw new DomainException("Entity must implement IAggregateRoot");
        }

        public virtual IQueryable<TEntity> Tables() => Entities.AsNoTracking();

        public virtual IQueryable<TEntity> Tables(Expression<Func<TEntity, bool>> criterion) => Entities.AsNoTracking().Where(criterion);
    }
}
