using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Library.Utility
{
    public class DynamicUpdator
    {
        private readonly IModel _model;

        public DynamicUpdator(IModel model)
        {
            _model = model;
        }

        public TEntity Update<TEntity>(TEntity entity, TEntity updatEntity) where TEntity : class
        {
            var findEntityType = _model.FindEntityType(typeof(TEntity));

            foreach (var updatEntityProperty in updatEntity.GetType().GetProperties())
            {
                foreach (var entityProperty in entity.GetType().GetProperties())
                {
                    var attr = (IgnoreDataMemberAttribute[])entityProperty.GetCustomAttributes(typeof(IgnoreDataMemberAttribute), false);
                    if (attr.Length > 0)
                    {
                        continue;
                    }
                    // Use attr[0], you'll need foreach on attr if MultiUse is true
                    if (entityProperty.Name == updatEntityProperty.Name)
                            if (entityProperty.GetValue(entity) != updatEntityProperty.GetValue(updatEntity))
                            {
                                if (updatEntityProperty.GetValue(updatEntity) == null)
                                {
                                    if (findEntityType.GetProperty(entityProperty.Name).IsNullable)
                                        entityProperty.SetValue(entity, updatEntityProperty.GetValue(updatEntity));
                                }
                                else
                                {
                                    entityProperty.SetValue(entity, updatEntityProperty.GetValue(updatEntity));
                                }
                            }
                        
                    

                }
            }
            return entity;
        }
    }
}