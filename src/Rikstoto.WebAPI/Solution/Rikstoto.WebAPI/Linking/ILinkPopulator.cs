using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.Core.Internal;

namespace Rikstoto.WebAPI.Linking
{
    public interface ILinkPopulator
    {
        void PopulateLinks<T>(T model);
        void PopulateLinks(Type type, object model);
    }

    public class LinkPopulator : ILinkPopulator
    {
        private readonly IEnumerable<IGenerateLinksFor> _linkGenerators;

        public LinkPopulator(IEnumerable<IGenerateLinksFor> linkGenerators)
        {
            _linkGenerators = linkGenerators;
        }

        public void PopulateLinks<T>(T model)
        {
            PopulateLinks(typeof(T), model);
        }

        public void PopulateLinks(Type type, object model)
        {
            if (IsListOrArray(type))
            {
                PopulateList(type, model);
            }
            else
            {
                var linkGenerators = _linkGenerators.Where(d => d.GetType().GetInterfaces().Contains(typeof(IGenerateLinksFor<>).MakeGenericType(type)));
                if (!linkGenerators.Any())
                {
                    if(!type.IsSealed && model != null)
                        type.GetProperties().ForEach(propertyInfo => PopulateLinks(propertyInfo.PropertyType, propertyInfo.GetValue(model, null)));
                    return;
                }

                var linkGeneratorManager = Activator.CreateInstance(typeof(ManageLinkGeneratorsFor<>).MakeGenericType(type), linkGenerators, this);

                linkGeneratorManager.InvokeMethod("GenerateLinksFromGenerators", model);
            }
        }

        private void PopulateList(Type type, dynamic model)
        {
            if (model == null) return;

            if(type.BaseType != null && IsListOrArray(type.BaseType))
            {
                type = type.BaseType;
            }

            Type listType = type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0];
            foreach(object item in model)
            {
                PopulateLinks(listType, item);
            }
            

        }

        private bool IsListOrArray(Type type)
        {
            return (type.IsArray || (type.GetInterface(typeof (ICollection<>).FullName) != null));

        }
    }

    public static class ReflectionExtensions
    {
        public static object InvokeMethod(this object o, string methodName, params object[] arguments)
        {
            return o.GetType().InvokeMember(methodName, BindingFlags.InvokeMethod, null, o, arguments);
        }
    }

    public class ManageLinkGeneratorsFor<T>
    {
        private readonly IEnumerable<IGenerateLinksFor<T>> _linkGenerators;
        private readonly ILinkPopulator _linkPopulator;

        public ManageLinkGeneratorsFor(IEnumerable<IGenerateLinksFor<T>> linkGenerators, ILinkPopulator linkPopulator)
        {
            _linkGenerators = linkGenerators;
            _linkPopulator = linkPopulator;
        }

        public ManageLinkGeneratorsFor(IEnumerable<IGenerateLinksFor> linkGenerators, ILinkPopulator linkPopulator)
        {
            _linkGenerators = linkGenerators.OfType<IGenerateLinksFor<T>>();
            _linkPopulator = linkPopulator;
        }

        public void GenerateLinksFromGenerators(T model)
        {
            _linkGenerators.ForEach(generator => generator.Populate(model));
            PopulateLinksForChildren(model);
        }

        private void PopulateLinksForChildren(T model)
        {
            Type type = typeof(T);

            foreach (var property in type.GetProperties())
            {
                _linkPopulator.PopulateLinks(property.PropertyType, property.GetValue(model, null));
            }

        }
    }
}