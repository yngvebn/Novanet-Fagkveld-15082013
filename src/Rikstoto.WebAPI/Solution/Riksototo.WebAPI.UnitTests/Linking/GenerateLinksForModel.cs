using System.Collections.Generic;
using NUnit.Framework;
using Rikstoto.WebAPI.Linking;

namespace Rikstoto.WebAPI.UnitTests.Linking
{
    [TestFixture]
    public class GenerateLinksForModel
    {
        private List<IGenerateLinksFor> linkGenerators = new List<IGenerateLinksFor>();

        public GenerateLinksForModel()
        {
            linkGenerators.Add(new IGenerateLinksForSimpleModel());
            linkGenerators.Add(new IGenerateLinksForComplexModel());
        }
         

        [Test]
        public void CanGenerateLinksForSimpleClass()
        {
            LinkPopulator linkPopulator = new LinkPopulator(linkGenerators);
            var simpleModel = new SimpleModel();
            linkPopulator.PopulateLinks(simpleModel);

            Assert.That(simpleModel.Links.Length, Is.EqualTo(2));
        }

        [Test]
        public void CanGenerateLinksForClassWithProperty()
        {
            LinkPopulator linkPopulator = new LinkPopulator(linkGenerators);
            var complexModel = new ComplexModel()
                {
                    SimpleModel = new SimpleModel()
                };
            linkPopulator.PopulateLinks(complexModel);

            Assert.That(complexModel.Links.Length, Is.EqualTo(2));
            Assert.That(complexModel.SimpleModel.Links.Length, Is.EqualTo(2));
        }

        [Test]
        public void CanGenerateLinksForSubPropertiesWithNoGeneratorForParent()
        {
            LinkPopulator linkPopulator = new LinkPopulator(linkGenerators);
            var complexModel = new ComplexModelWithNoGenerator()
            {
                SimpleModel = new SimpleModel()
            };
            linkPopulator.PopulateLinks(complexModel);

            Assert.That(complexModel.SimpleModel.Links.Length, Is.EqualTo(2));
        }

        [Test]
        public void CanGenerateLinksForClassWithListProperty()
        {
            LinkPopulator linkPopulator = new LinkPopulator(linkGenerators);
            var complexModel = new ComplexModelWithList()
            {
                SimpleModel = new SimpleModel[]{ new SimpleModel() }
            };
            linkPopulator.PopulateLinks(complexModel);

            Assert.That(complexModel.Links.Length, Is.EqualTo(2));
            Assert.That(complexModel.SimpleModel[0].Links.Length, Is.EqualTo(2));
        }
    }

    public class ComplexModel
    {
        public string[] Links { get; set; }
        public SimpleModel SimpleModel { get; set; }
    }

    public class ComplexModelWithNoGenerator
    {
        public string[] Links { get; set; }
        public SimpleModel SimpleModel { get; set; }
    }


    public class ComplexModelWithList
    {
        public string[] Links { get; set; }
        public SimpleModel[] SimpleModel { get; set; }
    }

    public class SimpleModel
    {
        public string[] Links { get; set; }
    }


    public class IGenerateLinksForComplexModel : IGenerateLinksFor<ComplexModel>,
        IGenerateLinksFor<ComplexModelWithList>
    {
        public void Populate(ComplexModel model)
        {
            model.Links = new string[]
                {
                    "Link1", "Link2"
                };
        }

        public void Populate(ComplexModelWithList model)
        {
            model.Links = new string[]
                {
                    "Link1", "Link2"
                };
        }
    }

    public class IGenerateLinksForSimpleModel: IGenerateLinksFor<SimpleModel>
    {
        public void Populate(SimpleModel model)
        {
            model.Links = new string[]
                {
                    "Link1", "Link2"
                };
        }
    }
}