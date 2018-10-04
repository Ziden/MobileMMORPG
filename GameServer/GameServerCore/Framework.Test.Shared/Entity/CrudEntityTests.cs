//-----------------------------------------------------------------------
// <copyright file="StoredProcedureEntityTests.cs" company="Genesys Source">
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
    public class StoredProcedureEntityTests
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
        /// Entity_StoredProcedureEntity
        /// </summary>
        [TestMethod()]
        public void Entity_StoredProcedureEntity_Create()
        {
            var writer = new StoredProcedureWriter<CustomerInfo>();
            var newCustomer = new CustomerInfo();
            var resultCustomer = new CustomerInfo();
            var dbCustomer = new CustomerInfo();            
            
            // Create should update original object, and pass back a fresh-from-db object
            newCustomer.Fill(testEntities[Arithmetic.Random(1, 5)]);
            resultCustomer = writer.Create(newCustomer);

            Assert.IsTrue(newCustomer.Key != TypeExtension.DefaultGuid);
            Assert.IsTrue(resultCustomer.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(resultCustomer.Key != TypeExtension.DefaultGuid);

            // Object in db should match in-memory objects
            dbCustomer = new EntityReader<CustomerInfo>().GetById(resultCustomer.Id);
            Assert.IsTrue(!dbCustomer.IsNew);
            Assert.IsTrue(dbCustomer.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(dbCustomer.Key != TypeExtension.DefaultGuid);
            Assert.IsTrue(dbCustomer.Id == resultCustomer.Id);
            Assert.IsTrue(dbCustomer.Key == resultCustomer.Key && resultCustomer.Key == newCustomer.Key);

            StoredProcedureEntityTests.RecycleBin.Add(newCustomer.Key);
        }

        /// <summary>
        /// Entity_StoredProcedureEntity
        /// </summary>
        [TestMethod()]
        public void Entity_StoredProcedureEntity_Update()
        {
            var writer = new StoredProcedureWriter<CustomerInfo>();
            var reader = new EntityReader<CustomerInfo>();
            var resultCustomer = new CustomerInfo();
            var dbCustomer = new CustomerInfo();
            var uniqueValue = Guid.NewGuid().ToString().Replace("-", "");
            var lastKey = TypeExtension.DefaultGuid;
            var originalId = TypeExtension.DefaultInteger;
            var originalKey = TypeExtension.DefaultGuid;

            Entity_StoredProcedureEntity_Create();
            lastKey = StoredProcedureEntityTests.RecycleBin.Last();

            dbCustomer = reader.GetByKey(lastKey);
            originalId = dbCustomer.Id;
            originalKey = dbCustomer.Key;
            Assert.IsTrue(!dbCustomer.IsNew);
            Assert.IsTrue(dbCustomer.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(dbCustomer.Key != TypeExtension.DefaultGuid);

            dbCustomer.FirstName = uniqueValue;
            resultCustomer = writer.Update(dbCustomer);
            Assert.IsTrue(!resultCustomer.IsNew);
            Assert.IsTrue(resultCustomer.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(resultCustomer.Key != TypeExtension.DefaultGuid);
            Assert.IsTrue(dbCustomer.Id == resultCustomer.Id && resultCustomer.Id == originalId);
            Assert.IsTrue(dbCustomer.Key == resultCustomer.Key && resultCustomer.Key == originalKey);

            dbCustomer = dbCustomer = reader.GetById(originalId);
            Assert.IsTrue(!dbCustomer.IsNew);
            Assert.IsTrue(dbCustomer.Id == resultCustomer.Id && resultCustomer.Id == originalId);
            Assert.IsTrue(dbCustomer.Key == resultCustomer.Key && resultCustomer.Key == originalKey);
            Assert.IsTrue(dbCustomer.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(dbCustomer.Key != TypeExtension.DefaultGuid);
        }

        /// <summary>
        /// Entity_StoredProcedureEntity
        /// </summary>
        [TestMethod()]
        public void Entity_StoredProcedureEntity_Delete()
        {
            var writer = new StoredProcedureWriter<CustomerInfo>();
            var reader = new EntityReader<CustomerInfo>();
            var dbCustomer = new CustomerInfo();
            var lastKey = TypeExtension.DefaultGuid;
            var originalId = TypeExtension.DefaultInteger;
            var originalKey = TypeExtension.DefaultGuid;

            Entity_StoredProcedureEntity_Create();
            lastKey = StoredProcedureEntityTests.RecycleBin.Last();

            dbCustomer = reader.GetByKey(lastKey);
            originalId = dbCustomer.Id;
            originalKey = dbCustomer.Key;
            Assert.IsTrue(!dbCustomer.IsNew);
            Assert.IsTrue(dbCustomer.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(dbCustomer.Key != TypeExtension.DefaultGuid);
            Assert.IsTrue(dbCustomer.CreatedDate.Date == DateTime.UtcNow.Date);

            dbCustomer = writer.Delete(dbCustomer);
            Assert.IsTrue(dbCustomer.IsNew);

            dbCustomer = reader.GetById(originalId);
            Assert.IsTrue(dbCustomer.IsNew);
            Assert.IsTrue(dbCustomer.Id == TypeExtension.DefaultInteger);
            Assert.IsTrue(dbCustomer.Key == TypeExtension.DefaultGuid);
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
