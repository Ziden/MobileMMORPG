//-----------------------------------------------------------------------
// <copyright file="MyApplication.cs" company="Genesys Source">
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
using Genesys.Extras.Configuration;
using System.Data.SqlClient;

namespace Framework.Test.Data
{
    /// <summary>
    /// Utility methods for table
    /// </summary>
    public class Tables
    {
        /// <summary>
        /// Removes EF code-first migration history table
        /// </summary>
        /// <param name="database"></param>
        /// <param name="schema"></param>
        public static void DropMigrationHistory(string database = "[FrameworkData]", string schema = "[Activity]")
        {
            // Must remove __MigrationHistory for EF Code First objects to auto-create their tables
            var configuration = new ConfigurationManagerLocal();

            try
            {
                using (var connection = new SqlConnection(configuration.ConnectionStringValue("DefaultConnection")))
                {
                    using (var command = new SqlCommand("Drop Table " + database + "." + schema + ".[__MigrationHistory]", connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException)
            {
                // Ignore connection errors
            }
        }
    }
}
