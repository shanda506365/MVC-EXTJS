namespace USO.Infrastructure
{
    using System;
    using System.ComponentModel;
    using System.Data.Common;
    using System.Data.Metadata.Edm;
    using System.Data.Objects;
    using System.Linq;
    //using System.ServiceModel.DomainServices.Server;

    internal static class ObjectContextUtilities
    {
        public static StructuralType GetEdmType(MetadataWorkspace workspace, Type clrType)
        {
            if (workspace == null)
            {
                throw new ArgumentNullException("workspace");
            }
            if (clrType == null)
            {
                throw new ArgumentNullException("clrType");
            }
            if (clrType.IsPrimitive || clrType == typeof(object))
            {
                return null;
            }
            EdmType edmType = null;
            do
            {
                if (!workspace.TryGetType(clrType.Name, clrType.Namespace, DataSpace.OSpace, out edmType))
                {
                    workspace.LoadFromAssembly(clrType.Assembly);
                    workspace.TryGetType(clrType.Name, clrType.Namespace, DataSpace.OSpace, out edmType);
                }
            }
            while (edmType == null && (clrType = clrType.BaseType) != typeof(object) && clrType != null);
            StructuralType result = null;
            if (edmType != null && (edmType.BuiltInTypeKind == BuiltInTypeKind.EntityType || edmType.BuiltInTypeKind == BuiltInTypeKind.ComplexType))
            {
                workspace.TryGetEdmSpaceType((StructuralType)edmType, out result);
            }
            return result;
        }
        public static bool IsConcurrencyTimestamp(EdmMember member)
        {
            Facet facet = member.TypeUsage.Facets.FirstOrDefault((Facet p) => p.Name == "ConcurrencyMode");
            if (facet == null || facet.Value == null || (ConcurrencyMode)facet.Value != ConcurrencyMode.Fixed)
            {
                return false;
            }
            facet = member.TypeUsage.Facets.FirstOrDefault((Facet p) => p.Name == "FixedLength");
            if (facet == null || facet.Value == null || !(bool)facet.Value)
            {
                return false;
            }
            facet = member.TypeUsage.Facets.FirstOrDefault((Facet p) => p.Name == "MaxLength");
            if (facet == null || facet.Value == null || (int)facet.Value != 8)
            {
                return false;
            }
            MetadataProperty storeGeneratedPattern = ObjectContextUtilities.GetStoreGeneratedPattern(member);
            return storeGeneratedPattern != null && facet.Value != null && !((string)storeGeneratedPattern.Value != "Computed");
        }
        public static MetadataProperty GetStoreGeneratedPattern(EdmMember member)
        {
            MetadataProperty result;
            member.MetadataProperties.TryGetValue("http://schemas.microsoft.com/ado/2009/02/edm/annotation:StoreGeneratedPattern", true, out result);
            return result;
        }
        public static ObjectStateEntry AttachAsModifiedInternal<T>(T current, T original, ObjectContext objectContext)
        {
            ObjectStateEntry objectStateEntry = objectContext.ObjectStateManager.GetObjectStateEntry(current);
            objectStateEntry.ApplyOriginalValues(original);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            AttributeCollection attributes = TypeDescriptor.GetAttributes(typeof(T));
            //bool flag = attributes[typeof(RoundtripOriginalAttribute)] != null;
            foreach (FieldMetadata current2 in objectStateEntry.CurrentValues.DataRecordInfo.FieldMetadata)
            {
                string name = objectStateEntry.CurrentValues.GetName(current2.Ordinal);
                PropertyDescriptor propertyDescriptor = properties[name];
                if (propertyDescriptor != null/* && propertyDescriptor.Attributes[typeof(RoundtripOriginalAttribute)] == null &&
                    !flag && propertyDescriptor.Attributes[typeof(ExcludeAttribute)] == null*/)
                {
                    objectStateEntry.SetModifiedProperty(name);
                }
            }
            return objectStateEntry;
        }
    }
}
