using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace USO.Domain.Extensions
{
    public static class MapperExtension
    {
        /// <summary>
        /// 复制实例，产生新的实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <returns></returns>
        public static T MapperClone<T>(this T original) where T : class
        {
            return original.MapperClone<T, T>();
        }

        /// <summary>
        /// 目标对象将产生一个新的实例
        /// </summary>
        /// <typeparam name="TSourceType"></typeparam>
        /// <typeparam name="TDestinationType"></typeparam>
        /// <param name="sourceEntity"></param>
        /// <returns></returns>
        public static TDestinationType MapperClone<TSourceType, TDestinationType>(this TSourceType sourceEntity)
            where TSourceType : class
        {
            RegisterMapper<TSourceType, TDestinationType>();

            return Mapper.Map<TDestinationType>(sourceEntity);
        }

        /// <summary>
        /// 复制属性到目标实例
        /// </summary>
        /// <typeparam name="TSourceType"></typeparam>
        /// <typeparam name="TDestinationType"></typeparam>
        /// <param name="sourceEntity"></param>
        /// <param name="destinationEntity"></param>
        public static void MapperCopy<TSourceType, TDestinationType>(this TSourceType sourceEntity,
                                                                     TDestinationType destinationEntity)
            where TSourceType : class
            where TDestinationType : class
        {
            RegisterMapper<TSourceType, TDestinationType>();
            Mapper.DynamicMap(sourceEntity, destinationEntity);
        }

        public static void MapperCopy(this object sourceObject, object destinationObject
            ,Type sourceType, Type destinationType
                                      )
        {
            RegisterMapper(sourceType, destinationType);
            Mapper.DynamicMap(sourceObject, destinationObject, sourceType, destinationType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSourceType"></typeparam>
        /// <typeparam name="TDestinationType"></typeparam>
        /// <param name="sourceList"></param>
        /// <returns></returns>
        public static IList<TDestinationType> MapperList<TSourceType, TDestinationType>(
            this IList<TSourceType> sourceList)
        {
            RegisterMapper<TSourceType, TDestinationType>();

            var destinationList = new List<TDestinationType>();
            foreach (TSourceType item in sourceList)
            {
                destinationList.Add(Mapper.Map<TDestinationType>(item));
            }
            return destinationList;
        }
        
        private static void RegisterMapper<TSourceType, TDestinationType>()
        {
            var sourceType = typeof (TSourceType);
            var destinationType = typeof (TDestinationType);

            RegisterMapper(sourceType,destinationType);
        }

        private static void RegisterMapper(Type sourceType, Type desctinationType)
        {
            if (Mapper.FindTypeMapFor(sourceType, desctinationType) == null)
            {
                Mapper.CreateMap(sourceType, desctinationType);
            }
        }

    }
}