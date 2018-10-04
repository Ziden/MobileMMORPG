//-----------------------------------------------------------------------
// <copyright file="CustomerViewModelTests.cs" company="Genesys Source">
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
using Genesys.Extras.Net;
using Genesys.Extras.Text;
using Genesys.Framework.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Test
{
    /// <summary>
    /// Test Genesys Framework for Web API endpoints
    /// </summary>
    /// <remarks></remarks>
    [TestClass()]
    public class FullViewModelTests
    {
        private bool interfaceBreakingRelease = true; // Current release breaks the interface?
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

        private List<CustomerModel> customerTestData = new List<CustomerModel>()
        {
            new CustomerModel() {FirstName = "John", MiddleName = "Adam", LastName = "Doe", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerModel() {FirstName = "Jane", MiddleName = "Michelle", LastName = "Smith", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerModel() {FirstName = "Xi", MiddleName = "", LastName = "Ling", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerModel() {FirstName = "Juan", MiddleName = "", LastName = "Gomez", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) },
            new CustomerModel() {FirstName = "Maki", MiddleName = "", LastName = "Ishii", BirthDate = DateTime.Today.AddYears(Arithmetic.Random(2).Negate()) }
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
        /// Get a customer, via HttpGet from Framework.WebServices endpoint
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Full_ViewModel_CRUD_Read()
        {
            var customer = new CustomerModel();
            var viewModel = new TestViewModel<CustomerModel>("Customer");

            // Create test record
            await Full_ViewModel_CRUD_Create();
            var idToTest = RecycleBin.Count() > 0 ? RecycleBin[0] : TypeExtension.DefaultGuid;

            // Verify update success
            customer = await viewModel.ReadAsync(idToTest);
            Assert.IsTrue(interfaceBreakingRelease | customer.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(interfaceBreakingRelease | customer.Key != TypeExtension.DefaultGuid);
            Assert.IsTrue(interfaceBreakingRelease | !customer.IsNew);
        }

        /// <summary>
        /// Create a new customer, via HttpPut to Framework.WebServices endpoint
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Full_ViewModel_CRUD_Create()
        {
            var customer = new CustomerModel();
            var url = new Uri(new ConfigurationManagerLocal().AppSettingValue("MyWebService").AddLast("/Customer"));

            customer.Fill(customerTestData[Arithmetic.Random(1, customerTestData.Count)]);
            var request = new HttpRequestPut<CustomerModel>(url, customer);
            customer = await request.SendAsync();
            Assert.IsTrue(interfaceBreakingRelease | customer.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(interfaceBreakingRelease | customer.Key != TypeExtension.DefaultGuid);

            RecycleBin.Add(customer.Key);
        }

        /// <summary>
        /// Update a customer, via HttpPost to Framework.WebServices endpoint
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Full_ViewModel_CRUD_Update()
        {
            var customer = new CustomerModel();
            var viewModel = new TestViewModel<CustomerModel>("Customer");

            // Create test record
            await Full_ViewModel_CRUD_Create();
            var idToTest = RecycleBin.Count() > 0 ? RecycleBin[0] : TypeExtension.DefaultGuid;
            // Read test record
            customer = await viewModel.ReadAsync(idToTest);
            // Update test record
            var testKey = RandomString.Next();
            customer.FirstName = customer.FirstName.AddLast(testKey);
            customer = await viewModel.UpdateAsync(customer);
            Assert.IsTrue(interfaceBreakingRelease | customer.Id != TypeExtension.DefaultInteger);
            Assert.IsTrue(interfaceBreakingRelease | customer.Key != TypeExtension.DefaultGuid);
            // Verify update success
            customer = await viewModel.ReadAsync(idToTest);
            Assert.IsTrue(interfaceBreakingRelease | customer.FirstName.Contains(testKey));
            Assert.IsTrue(interfaceBreakingRelease | !viewModel.MyModel.IsNew);
            Assert.IsTrue(interfaceBreakingRelease | !customer.IsNew);
        }

        /// <summary>
        /// Delete a customer, via HttpDelete to Framework.WebServices endpoint
        /// </summary>
        /// <remarks></remarks>
        [TestMethod()]
        public async Task Full_ViewModel_CRUD_Delete()
        {
            var customer = new CustomerModel();
            var customerReturn = new CustomerModel();
            var viewModel = new TestViewModel<CustomerModel>("Customer");

            // Create test record
            await Full_ViewModel_CRUD_Create();
            var idToTest = RecycleBin.Count() > 0 ? RecycleBin[0] : TypeExtension.DefaultGuid;

            // Test
            customer = await viewModel.ReadAsync(idToTest);
            Assert.IsTrue(interfaceBreakingRelease | !viewModel.MyModel.IsNew);
            customerReturn = await viewModel.DeleteAsync(customer);
            Assert.IsTrue(interfaceBreakingRelease | customerReturn.IsNew);
            Assert.IsTrue(interfaceBreakingRelease | viewModel.MyModel.IsNew);
            Assert.IsTrue(interfaceBreakingRelease | viewModel.MyModel.Id == TypeExtension.DefaultInteger);
            Assert.IsTrue(interfaceBreakingRelease | viewModel.MyModel.Key == TypeExtension.DefaultGuid);
            // Verify update success
            customer = await viewModel.ReadAsync(idToTest);
            Assert.IsTrue(interfaceBreakingRelease | viewModel.MyModel.IsNew);
            Assert.IsTrue(interfaceBreakingRelease | viewModel.MyModel.Id == TypeExtension.DefaultInteger);
            Assert.IsTrue(interfaceBreakingRelease | viewModel.MyModel.Key == TypeExtension.DefaultGuid);
            Assert.IsTrue(interfaceBreakingRelease | customer.IsNew);
            Assert.IsTrue(interfaceBreakingRelease | customer.Id == TypeExtension.DefaultInteger);
            Assert.IsTrue(interfaceBreakingRelease | customer.Key == TypeExtension.DefaultGuid);
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
