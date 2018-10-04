//-----------------------------------------------------------------------
// <copyright file="CustomerSearchModelTests.cs" company="Genesys Source">
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
using Framework.Customer;
using Genesys.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Test
{
    [TestClass()]
    public class CustomerSearchModelTests
    {
        /// <summary>
        /// Entity_CustomerInfo
        /// </summary>
        [TestMethod()]
        public void Model_CustomerSearch_GetBySearchFields()
        {
            var searchChar = "i";
            var searchModel = new CustomerSearchModel() { FirstName = searchChar, LastName = searchChar };
            var results = CustomerInfo.GetByAny(searchModel);
            Assert.IsTrue(results.Count() > 0);
            searchModel.Results.FillRange(results);
            Assert.IsTrue(searchModel.FirstName == searchChar && searchModel.LastName == searchChar);
            Assert.IsTrue(searchModel.Results.Count() > 0);
        }
    }
}
