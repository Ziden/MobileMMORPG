//-----------------------------------------------------------------------
// <copyright file="EntityReaderTests.cs" company="Genesys Source">
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
using Genesys.Framework.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Framework.Test
{
    [TestClass()]
    public class EntityReaderTests
    {
        /// <summary>
        /// Data_EntityReader_CountAny
        /// </summary>
        [TestMethod()]
        public void Data_EntityReader_CountAny()
        {
            var reader = new ValueReader<CustomerType>();

            // GetAll() count and any
            var resultsAll = reader.GetAll();
            Assert.IsTrue(resultsAll.Count() > 0);
            Assert.IsTrue(resultsAll.Any());

            // GetAll().Take(1) count and any
            var resultsTake = reader.GetAll().Take(1);
            Assert.IsTrue(resultsTake.Count() == 1);
            Assert.IsTrue(resultsTake.Any());

            // Get an Id to test
            var key = reader.GetAllExcludeDefault().FirstOrDefaultSafe().Key;
            Assert.IsTrue(key != TypeExtension.DefaultGuid);

            // GetAll().Where count and any
            var resultsWhere = reader.GetAll().Where(x => x.Key == key);
            Assert.IsTrue(resultsWhere.Count() > 0);
            Assert.IsTrue(resultsWhere.Any());
        }

        /// <summary>s
        /// Data_EntityReader_Select
        /// </summary>
        [TestMethod()]
        public void Data_EntityReader_GetAll()
        {
            var reader = new ValueReader<CustomerType>();
            var typeResults = reader.GetAll().Take(1);
            Assert.IsTrue(typeResults.Count() > 0);
        }

        /// <summary>
        /// Data_EntityReader_GetById
        /// </summary>
        [TestMethod()]
        public void Data_EntityReader_GetById()
        {
            var reader = new ValueReader<CustomerType>();
            var custEntity = new CustomerType();

            // ById Should return 1 record
            var existingKey = reader.GetAllExcludeDefault().FirstOrDefaultSafe().Key;
            var custWhereKey = reader.GetAll().Where(x => x.Key == existingKey);
            Assert.IsTrue(custWhereKey.Count() > 0);
            Assert.IsTrue(custWhereKey.Any());

            custEntity = custWhereKey.FirstOrDefaultSafe();
            Assert.IsTrue(custEntity.Key != TypeExtension.DefaultGuid);
        }

        /// <summary>
        /// Data_EntityReader_GetByKey
        /// </summary>
        [TestMethod()]
        public void Data_EntityReader_GetByKey()
        {
            var reader = new ValueReader<CustomerType>();

            // ByKey Should return 1 record
            var existingKey = reader.GetAll().FirstOrDefaultSafe().Key;
            var custWhereKey = reader.GetAll().Where(x => x.Key == existingKey);
            Assert.IsTrue(custWhereKey.Count() > 0);
        }

        /// <summary>
        /// Data_EntityReader_Insert
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Data_EntityReader_GetWhere()
        {
            // Plain EntityInfo object
            var reader = new ValueReader<CustomerType>();
            var testType = new CustomerType();
            var testKey = reader.GetAllExcludeDefault().FirstOrDefaultSafe().Key;
            testType = reader.GetAll().Where(x => x.Key == testKey).FirstOrDefaultSafe();
            Assert.IsTrue(testType.Key != TypeExtension.DefaultGuid);
        }

        /// <summary>
        /// EntityReader context and connection
        /// </summary>
        [TestMethod()]
        public void Data_EntityReader_Lists()
        {
            var emptyGuid = TypeExtension.DefaultGuid;

            // List Type
            var reader = new ValueReader<CustomerType>();
            var typeResults = reader.GetAllExcludeDefault();
            Assert.IsTrue(typeResults.Count() > 0);
            Assert.IsTrue(typeResults.Any(x => x.Key == emptyGuid) == false);
        }

        /// <summary>
        /// EntityReader context and connection
        /// </summary>
        [TestMethod()]
        public void Data_EntityReader_Singles()
        {
            var reader = new ValueReader<CustomerType>();
            var testItem = new CustomerType();
            var emptyGuid = TypeExtension.DefaultGuid;

            // By Key
            testItem = reader.GetByKey(reader.GetAllExcludeDefault().FirstOrDefaultSafe().Key);
            Assert.IsTrue(testItem.Key != TypeExtension.DefaultGuid);
        }
    }    
}
