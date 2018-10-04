//-----------------------------------------------------------------------
// <copyright file="ExceptionLoggerTests.cs" company="Genesys Source">
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
using Framework.Test.Data;
using Genesys.Extensions;
using Genesys.Framework.Activity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Framework.Test
{
    /// <summary>
    /// Tests code first ExceptionLogger functionality
    /// </summary>
    [TestClass()]
    public partial class ExceptionLoggerTests
    {
        /// <summary>
        /// Tests code first ExceptionLogger saving to the database
        /// </summary>
        [TestMethod()]
        public void Activity_ExceptionLogger()
        {
            var reader = new ExceptionLogReader();
            var writer = new ExceptionLogWriter();
            var log = new ExceptionLog();
            var preSaveCount = TypeExtension.DefaultInteger;
            var postSaveCount = TypeExtension.DefaultInteger;

            Tables.DropMigrationHistory();

            preSaveCount = reader.GetAll().Count();
            log = writer.Save(log);
            postSaveCount = reader.GetAll().Count();
            Assert.IsTrue(log.ExceptionLogId != TypeExtension.DefaultInteger);
            Assert.IsTrue(postSaveCount == preSaveCount + 1);
        }
    }
}