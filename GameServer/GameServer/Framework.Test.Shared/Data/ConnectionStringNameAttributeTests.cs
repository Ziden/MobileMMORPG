//-----------------------------------------------------------------------
// <copyright file="ConnectionStringNameTests.cs" company="Genesys Source">
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
using Genesys.Extras.Configuration;
using Genesys.Extras.Data;
using Genesys.Framework.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Test
{
    [TestClass()]
    public class ConnectionStringNameTests
    {
        private const string testValue = "DefaultConnection";
        private const string testValueNotFound = "NoConnection";
        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Data_ConnectionStringAttribute()
        {
            var testItem = new ClassWithConnectString();
            string result = testItem.GetAttributeValue<ConnectionStringName>(testValueNotFound);
            Assert.IsTrue(result != testValueNotFound);
            Assert.IsTrue(result == testValue);
        }

        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Data_ConnectionStringFromConfig()
        {
            var result = TypeExtension.DefaultString;
            var configManager = new ConfigurationManagerLocal();
            var configConnectString = new ConnectionStringSafe();
            configConnectString = configManager.ConnectionString(this.GetAttributeValue<ConnectionStringName>(ConnectionStringName.DefaultConnectionName));
            result = configConnectString.ToEF(typeof(ClassWithConnectString));
            Assert.IsTrue(result != TypeExtension.DefaultString);
            Assert.IsTrue(configConnectString.IsValid);
            Assert.IsTrue(configConnectString.IsEF || configConnectString.IsADO);
            Assert.IsTrue(configConnectString.ConnectionStringType != ConnectionStringSafe.ConnectionStringTypes.Empty
                && configConnectString.ConnectionStringType != ConnectionStringSafe.ConnectionStringTypes.Invalid);
        }

        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Data_ConnectionStringEntity()
        {
            var result = TypeExtension.DefaultString;
            var configManager = new ConfigurationManagerLocal();
            var configConnectString = new ConnectionStringSafe();
            configConnectString = configManager.ConnectionString(this.GetAttributeValue<ConnectionStringName>(ConnectionStringName.DefaultConnectionName));
            result = configConnectString.ToEF(typeof(EntityWithConnectString));
            Assert.IsTrue(result != TypeExtension.DefaultString);
            Assert.IsTrue(configConnectString.IsValid);
            Assert.IsTrue(configConnectString.IsEF || configConnectString.IsADO);
            Assert.IsTrue(configConnectString.ConnectionStringType != ConnectionStringSafe.ConnectionStringTypes.Empty
                && configConnectString.ConnectionStringType != ConnectionStringSafe.ConnectionStringTypes.Invalid);
        }

        /// <summary>
        /// Attribute-based connection string nanems
        /// </summary>
        [TestMethod()]
        public void Data_ConnectionStringDatabase()
        {
            var result = TypeExtension.DefaultString;
            var configManager = new ConfigurationManagerLocal();
            var configConnectString = new ConnectionStringSafe();

            configConnectString = configManager.ConnectionString(this.GetAttributeValue<ConnectionStringName>(ConnectionStringName.DefaultConnectionName));
            result = configConnectString.ToEF(typeof(EntityWithConnectString));
            Assert.IsTrue(result != TypeExtension.DefaultString);
            Assert.IsTrue(configConnectString.IsValid);
            Assert.IsTrue(configConnectString.IsEF || configConnectString.IsADO);
            Assert.IsTrue(configConnectString.ConnectionStringType != ConnectionStringSafe.ConnectionStringTypes.Empty
                && configConnectString.ConnectionStringType != ConnectionStringSafe.ConnectionStringTypes.Invalid);
        }

        /// <summary>
        /// Tests attributes        
        /// </summary>
        [ConnectionStringName(testValue)]
        internal class ClassWithConnectString
        {            
        }

        /// <summary>
        /// Tests attributes        
        /// </summary>
        [ConnectionStringName(testValue)]
        internal class EntityWithConnectString : EntityInfo<EntityWithConnectString>
        {
        }
    }
}
