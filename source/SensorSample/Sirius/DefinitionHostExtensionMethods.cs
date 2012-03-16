﻿//-------------------------------------------------------------------------------
// <copyright file="DefinitionHostExtensionMethods.cs" company="Appccelerate">
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

namespace SensorSample.Sirius
{
    using Appccelerate.EvaluationEngine;
    using Appccelerate.EvaluationEngine.Syntax;

    using SensorSample.Evaluation;

    public static class DefinitionHostExtensionMethods
    {
         public static IDefinitionSyntax<WhereDoesThePassangerWantToTravelTo, int, bool, int?> 
             SolveWhereDoesThePassengerWantToTravelTo(this ISolutionDefinitionHost host)
         {
             return host.SolveWithResultMapping<WhereDoesThePassangerWantToTravelTo, int, bool, int?>();
         }
    }
}