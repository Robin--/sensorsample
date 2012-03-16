﻿//-------------------------------------------------------------------------------
// <copyright file="DoorSpecifications.cs" company="Appccelerate">
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
    using FakeItEasy;

    using Machine.Specifications;

    [Subject(Subjects.Door)]
    public class When_a_door_opens : InitializedApplicationSpecification
    {
        Because of = () =>
            {
                RunApplication();
                bootstrapperStrategy.Door.Opened += Raise.WithEmpty().Now;
            };

        It should_write_message_to_file_logger = () =>
            A.CallTo(() => bootstrapperStrategy.FileLogger.Log("door is open!")).MustHaveHappened();
    }

    [Subject(Subjects.Door)]
    public class When_a_door_closes : InitializedApplicationSpecification
    {
        Establish context = () =>
        {
            RunApplication();
            bootstrapperStrategy.Door.Opened += Raise.WithEmpty().Now;  // we have to open the door first, otherwise it cannot be closed.    
        };


        Because of = () =>
        {
            bootstrapperStrategy.Door.Closed += Raise.WithEmpty().Now;
        };

        It should_write_message_to_file_logger = () =>
            A.CallTo(() => bootstrapperStrategy.FileLogger.Log("door is closed!")).MustHaveHappened();

        It should_tell_travel_coordinator_to_go_to_level_specified_by_oracle = () =>
            A.CallTo(() => bootstrapperStrategy.TravelCoordinator.TravelTo(42)).MustHaveHappened();
    }
}