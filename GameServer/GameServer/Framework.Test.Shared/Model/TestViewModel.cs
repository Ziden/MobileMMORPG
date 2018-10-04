//-----------------------------------------------------------------------
// <copyright file="TestViewModel.cs" company="Genesys Source">
//      Copyright (c) 2017-2018 Genesys Source. All rights reserved.
//      Licensed to the Apache Software Foundation (ASF) under one or more 
//      contributor license agreements.  See the NOTICE file distributed with 
//      this work for additional information regarding copyright ownership.
//      The ASF licenses this file to You under the Apache License, Version 2.0 
//      (the 'License'); you may not use this file except in compliance with 
//      the License.  You may obtain a copy of the License at 
//       
//        http://www.apache.org/licenses/LICENSE-2.0 
//       
//       Unless required by applicable law or agreed to in writing, software  
//       distributed under the License is distributed on an 'AS IS' BASIS, 
//       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.  
//       See the License for the specific language governing permissions and  
//       limitations under the License. 
// </copyright>
//-----------------------------------------------------------------------
using Genesys.Framework.Application;
using Genesys.Framework.Data;
using Genesys.Framework.Operation;
using System;

namespace Framework.Test
{
    /// <summary>
    /// ViewModel holds model and is responsible for server calls, navigation, etc.
    /// </summary>
    public class TestViewModel<TModel> : CrudViewModel<TModel>, ICrudOperationAsync<TModel> where TModel : EntityModel<TModel>, new()
    {
        /// <summary>
        /// Currently running application
        /// </summary>
        public override IApplication MyApplication { get; protected set; } = new TestApplication();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="myApplication"></param>
        public TestViewModel(string webServiceControllerName)
            : base(webServiceControllerName)
        {
        }        
    }
}
