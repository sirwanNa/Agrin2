using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Agrin2.Entities
{
    public class EntityHelper
    {
        public static void LoadEntities(Assembly asm, ModelBuilder modelBuilder, string nameSpace)
        {
            var entityTypes = asm.GetTypes()
                .Where(type => type.BaseType != null &&
                               type.BaseType != Type.GetType("System.Enum") &&
                               type.Name != "BaseEntity" &&
                               !type.IsAbstract &&
                               (type.IsSubclassOf(typeof(BaseEntity)) &&
                               type.Namespace != null &&
                               type.Namespace.Contains(nameSpace)))
                               .ToList();

            // entityTypes.ForEach(modelBuilder.RegisterEntityType);
            entityTypes.ForEach(m => modelBuilder.Model.AddEntityType(m));


        }
    }
}
