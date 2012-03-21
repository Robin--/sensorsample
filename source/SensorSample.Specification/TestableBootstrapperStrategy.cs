//-------------------------------------------------------------------------------
// <copyright file="TestableBootstrapperStrategy.cs" company="Appccelerate">
//   Copyright (c) 2008-2012
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace SensorSample.Specification
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Reflection;

    using Appccelerate.AsyncModule;
    using Appccelerate.AsyncModule.Events;
    using Appccelerate.Bootstrapper;
    using Appccelerate.Bootstrapper.Configuration;
    using Appccelerate.EventBroker;
    using Appccelerate.EventBroker.Factories;
    using Appccelerate.StateMachine;

    using FakeItEasy;

    using SensorSample.Bootstrapping;
    using SensorSample.Sensors;
    using SensorSample.Sirius;

    public class TestableBootstrapperStrategy : BootstrapperStrategy
    {
        public TestableBootstrapperStrategy()
        {
            this.Door = A.Fake<IVhptDoor>();
            this.BlackHoleSubOrbitDetectionEngine = A.Fake<IVhptBlackHoleSubOrbitDetectionEngine>();
            this.TravelCoordinator = A.Fake<IVhptTravelCoordinator>();
            this.FileLogger = A.Fake<IVhptFileLogger>();
            this.Configuration = new Dictionary<string, IDictionary<string, string>>();
        }

        public IVhptDoor Door { get; private set; }

        public IVhptBlackHoleSubOrbitDetectionEngine BlackHoleSubOrbitDetectionEngine { get; private set; }

        public IVhptTravelCoordinator TravelCoordinator { get; private set; }

        public IVhptFileLogger FileLogger { get; private set; }

        public IDictionary<string, IDictionary<string, string>> Configuration { get; set; }

        protected override IVhptDoor CreateDoor()
        {
            return this.Door;
        }

        protected override IVhptBlackHoleSubOrbitDetectionEngine CreateBlackHoleSubOrbitDetectionEngine()
        {
            return this.BlackHoleSubOrbitDetectionEngine;
        }

        protected override IVhptFileLogger CreateFileLogger()
        {
            return this.FileLogger;
        }

        protected override ModuleController CreateModuleController()
        {
            var moduleController = base.CreateModuleController();

            moduleController.BeforeEnqueueMessage += this.ModuleControllerOnBeforeEnqueueMessage;

            return moduleController;
        }

        protected override EventBroker CreateGlobalEventBroker()
        {
            return new EventBroker(new UnitTestFactory());
        }

        protected override IStateMachine<States, Events> CreateStateMachine()
        {
            return new PassiveStateMachine<States, Events>();
        }

        private void ModuleControllerOnBeforeEnqueueMessage(object sender, EnqueueMessageEventArgs enqueueMessageEventArgs)
        {
            enqueueMessageEventArgs.Cancel = true;

            MethodInfo method = null;

            foreach (MethodInfo methodInfo in enqueueMessageEventArgs.Module.GetType().GetMethods())
            {
                if (Attribute.IsDefined(methodInfo, typeof(MessageConsumerAttribute), true))
                {
                    method = methodInfo;
                }
            }

            method.Invoke(enqueueMessageEventArgs.Module, new[] { enqueueMessageEventArgs.Message });
        }

        private class TestableExtensionConfigurationSectionBehaviorFactory : DefaultExtensionConfigurationSectionBehaviorFactory, ILoadConfigurationSection
        {
            private readonly IDictionary<string, IDictionary<string, string>> configuration;

            public TestableExtensionConfigurationSectionBehaviorFactory(IDictionary<string, IDictionary<string, string>> configuration)
            {
                this.configuration = configuration;
            }

            public override ILoadConfigurationSection CreateLoadConfigurationSection(IExtension extension)
            {
                return this;
            }

            public ConfigurationSection GetSection(string sectionName)
            {
                IDictionary<string, string> configurationSection;

                return this.configuration.TryGetValue(sectionName, out configurationSection) ? ExtensionConfigurationSectionHelper.CreateSection(configurationSection) : null;
            }
        }
    }
}