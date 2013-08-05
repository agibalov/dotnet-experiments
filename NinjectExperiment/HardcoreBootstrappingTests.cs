using System.IO;
using NUnit.Framework;
using Ninject;
using Ninject.Activation;

namespace NinjectExperiment
{
    public class HardcoreBootstrappingTests
    {
        [Test]
        public void CanHaveMultistageBootstrapping()
        {
            var stage1Kernel = new StandardKernel();
            stage1Kernel.Bind<TemplateLoader>()
                .ToSelf()
                .WithConstructorArgument("shouldThrow", false);

            var stage2Kernel = new StandardKernel();
            stage2Kernel.Bind<Template>().ToProvider(stage1Kernel.Get<TemplateLoader>());
            stage2Kernel.Get<Service>();
        }

        [Test]
        public void WhatItLooksLikeWhenItFails()
        {
            var stage1Kernel = new StandardKernel();
            stage1Kernel.Bind<TemplateLoader>()
                .ToSelf()
                .WithConstructorArgument("shouldThrow", true);

            var stage2Kernel = new StandardKernel();
            stage2Kernel.Bind<Template>().ToProvider(stage1Kernel.Get<TemplateLoader>());
            try
            {
                stage2Kernel.Get<Service>();
                Assert.Fail();
            }
            catch (FileNotFoundException)
            {
            }
        }

        class Service
        {
            public Service(Template template)
            {
            }
        }

        class TemplateLoader : Provider<Template>
        {
            private readonly bool _shouldThrow;

            public TemplateLoader(bool shouldThrow)
            {
                _shouldThrow = shouldThrow;
            }

            protected override Template CreateInstance(IContext context)
            {
                if (_shouldThrow)
                {
                    throw new FileNotFoundException();
                }

                return new Template();
            }
        }

        class Template
        {
        }
    }
}