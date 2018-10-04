//-----------------------------------------------------------------------
// <copyright file="DatabaseWriterTests.cs" company="Genesys Source">
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
using Genesys.Extras.Configuration;
using Genesys.Extras.Mathematics;
using Genesys.Framework.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Framework.Test
{
    [TestClass()]
    public class DatabaseWriterTests
    {
        private static readonly object LockObject = new object();
        private static volatile List<Guid> _recycleBin = null;
        /// <summary>
        /// Singleton for recycle bin
        /// </summary>
        private static List<Guid> RecycleBin
        {
            get
            {
                if (_recycleBin != null) return _recycleBin;
                lock (LockObject)
                {
                    if (_recycleBin == null)
                    {
                        _recycleBin = new List<Guid>();
                    }
                }
                return _recycleBin;
            }
        }
        List<CustomerInfo> testEntities = new List<CustomerInfo>()
        {
            new CustomerInfo() {FirstName = "John", MiddleName = "Adam", LastName = "Doe", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerInfo() {FirstName = "Jane", MiddleName = "Michelle", LastName = "Smith", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerInfo() {FirstName = "Xi", MiddleName = "", LastName = "Ling", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerInfo() {FirstName = "Juan", MiddleName = "", LastName = "Gomez", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerInfo() {FirstName = "Maki", MiddleName = "", LastName = "Ishii", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) }
        };

        /// <summary>
        /// Initializes class before tests are ran
        /// </summary>
        [ClassInitialize()]
        public static void ClassInit(TestContext context)
        {
            // Database is required for these tests
            var databaseAccess = false;
            var configuration = new ConfigurationManagerLocal();
            using (var connection = new SqlConnection(configuration.ConnectionStringValue("DefaultConnection")))
            {
                databaseAccess = connection.CanOpen();
            }
            Assert.IsTrue(databaseAccess);
        }

        /// <summary>
        /// Data_DatabaseWriter_Insert
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Data_DatabaseWriter_Insert()
        {
            var writer = new StoredProcedureWriter<CustomerInfo>();
            var testEntity = new CustomerInfo();
            var resultEntity = new CustomerInfo();
            var oldId = TypeExtension.DefaultInteger;
            var oldKey = TypeExtension.DefaultGuid;
            var newId = TypeExtension.DefaultInteger;
            var newKey = TypeExtension.DefaultGuid;

            // Create and insert record
            testEntity.Fill(testEntities[Arithmetic.Random(1, 5)]);
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Key == TypeExtension.DefaultGuid);

            // Do Insert and check passed entity and returned entity            
            resultEntity = writer.Create(testEntity);
            Assert.IsTrue(testEntity.Key != TypeExtension.DefaultGuid);
            Assert.IsTrue(resultEntity.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(resultEntity.Key != TypeExtension.DefaultGuid);
        
            // Pull from DB and retest
            testEntity = new EntityReader<CustomerInfo>().GetById(resultEntity.Id);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key != oldKey);
            Assert.IsTrue(testEntity.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(testEntity.Key != TypeExtension.DefaultGuid);

            // Cleanup
            DatabaseWriterTests.RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Data_DatabaseWriter_Update
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Data_DatabaseWriter_Update()
        {
            var testEntity = new CustomerInfo();
            var writer = new StoredProcedureWriter<CustomerInfo>();
            var oldFirstName = TypeExtension.DefaultString;
            var newFirstName = DateTime.UtcNow.Ticks.ToString();
            int entityId = TypeExtension.DefaultInteger;
            var entityKey = TypeExtension.DefaultGuid;

            // Create and capture original data
            this.Data_DatabaseWriter_Insert();
            testEntity = new EntityReader<CustomerInfo>().GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefaultSafe();
            oldFirstName = testEntity.FirstName;
            entityId = testEntity.Id;
            entityKey = testEntity.Key;
            testEntity.FirstName = newFirstName;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(testEntity.Key != TypeExtension.DefaultGuid);

            // Do Update
            writer.Save(testEntity);

            // Pull from DB and retest
            testEntity = new EntityReader<CustomerInfo>().GetById(entityId);
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id == entityId);
            Assert.IsTrue(testEntity.Key == entityKey);
            Assert.IsTrue(testEntity.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(testEntity.Key != TypeExtension.DefaultGuid);
        }

        /// <summary>
        /// Data_DatabaseWriter_Delete
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public void Data_DatabaseWriter_Delete()
        {
            var writer = new StoredProcedureWriter<CustomerInfo>();
            var testEntity = new CustomerInfo();
            var oldId = TypeExtension.DefaultInteger;
            var oldKey = TypeExtension.DefaultGuid;

            // Insert and baseline test
            this.Data_DatabaseWriter_Insert();
            testEntity = new EntityReader<CustomerInfo>().GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefaultSafe();
            oldId = testEntity.Id;
            oldKey = testEntity.Key;
            Assert.IsTrue(testEntity.IsNew == false);
            Assert.IsTrue(testEntity.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(testEntity.Key != TypeExtension.DefaultGuid);

            // Do delete
            writer.Delete(testEntity);

            // Pull from DB and retest
            testEntity = new EntityReader<CustomerInfo>().GetById(oldId);
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Id != oldId);
            Assert.IsTrue(testEntity.Key != oldKey);
            Assert.IsTrue(testEntity.IsNew);
            Assert.IsTrue(testEntity.Key == TypeExtension.DefaultGuid);

            // Add to recycle bin for cleanup
            DatabaseWriterTests.RecycleBin.Add(testEntity.Key);
        }

        /// <summary>
        /// Cleanup all data
        /// </summary>
        [ClassCleanup()]
        public static void Cleanup()
        {
            var writer = new EntityWriter<CustomerInfo>();
            var reader = new EntityReader<CustomerInfo>();
            foreach (Guid item in RecycleBin)
            {
                writer.Delete(reader.GetByKey(item));
            }
        }
    }    
}
