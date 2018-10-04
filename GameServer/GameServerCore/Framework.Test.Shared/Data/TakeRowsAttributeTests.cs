//-----------------------------------------------------------------------
// <copyright file="TakeRowsTests.cs" company="Genesys Source">
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
using Genesys.Extensions;
using Genesys.Extras.Data;
using Genesys.Framework.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Test
{
    [TestClass()]
    public class TakeRowsTests
    {
        private const int testValue = 25;
        private const int testValueNotFound = 100;
        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Data_TakeRowsAttribute()
        {
            var testItem = new ClassWithTakeRows();
            int result = testItem.GetAttributeValue<TakeRows, int>(testValueNotFound);
            Assert.IsTrue(result != testValueNotFound);
            Assert.IsTrue(result == testValue);
        }

        /// <summary>
        /// Tests attributes        
        /// </summary>
        [TakeRows(testValue)]
        internal class ClassWithTakeRows
        {            
        }
    }
}
