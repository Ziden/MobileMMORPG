//-----------------------------------------------------------------------
// <copyright file="SerializerTests.cs" company="Genesys Source">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Genesys.Extensions;
using Genesys.Extras.Serialization;
using Genesys.Extras.Text;

namespace Framework.Test
{
    [TestClass()]
    public class SerializerTests
    {
        [TestMethod()]
        public void Serialization_Serializer_ValueTypes()
        {
            // Immutable string class
            var data1 = TypeExtension.DefaultString;
            var TestData1 = "TestDataHere";
            ISerializer<object> serialzer1 = new JsonSerializer<object>();
            data1 = serialzer1.Serialize(TestData1);
            Assert.IsTrue(serialzer1.Deserialize(data1).ToString() == TestData1);

            
            var data = TypeExtension.DefaultString;
            StringMutable testData = "TestDataHere";
            var Serialzer = new JsonSerializer<StringMutable>();
            data = Serialzer.Serialize(testData);
            Assert.IsTrue(Serialzer.Deserialize(data).ToString() == testData.ToString());
        }

        [TestMethod()]
        public void Serialization_Serializer_ReferenceTypes()
        {
            // Collections, etc
            var ItemL = new List<int> { 1, 2, 3 };
            var Serializer = new JsonSerializer<List<int>>();
            var SerializedDataL = Serializer.Serialize(ItemL);
            Assert.IsTrue(ItemL.Count == Serializer.Deserialize(SerializedDataL).Count);
        }
    }
}