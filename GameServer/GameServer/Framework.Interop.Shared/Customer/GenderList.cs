//-----------------------------------------------------------------------
// <copyright file="CustomerModel.cs" company="Genesys Source">
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
using System;
using Genesys.Extensions;
using System.Collections.Generic;
using Genesys.Framework.Data;

namespace Framework.Customer
{
    /// <summary>
    /// Customer screen model for binding and transport
    /// </summary>
    /// <remarks></remarks>
    public class GenderList : IGenderList
    {
        /// <summary>
        /// ISO 5218 Standard for Gender values
        /// </summary>
        public struct GenderData
        {
            /// <summary>
            /// Default. Not set
            /// </summary>
            public static KeyValuePair<int, string> NotSet { get; } = new KeyValuePair<int, string>(-1, "Not Set");

            /// <summary>
            /// Unknown gender
            /// </summary>
            public static KeyValuePair<int, string> NotKnown { get; } = new KeyValuePair<int, string>(0, "Not Known");

            /// <summary>
            /// Male gender
            /// </summary>
            public static KeyValuePair<int, string> Male { get; } = new KeyValuePair<int, string>(1, "Male");

            /// <summary>
            /// Femal Gender
            /// </summary>
            public static KeyValuePair<int, string> Female { get; } = new KeyValuePair<int, string>(2, "Female");

            /// <summary>
            /// Not applicable or do not want to specify
            /// </summary>
            public static KeyValuePair<int, string> NotApplicable { get; } = new KeyValuePair<int, string>(9, "Not Applicable");
        }

        /// <summary>
        /// List of Genders, bindable to int Id and string Name
        /// </summary>
        public List<KeyValuePair<int, string>> Genders
        {
            get
            {
                return GenderList.GetAll();
            }
        }

        /// <summary>
        /// List of Genders, bindable to int Id and string Name
        /// </summary>
        public static List<KeyValuePair<int, string>> GetAll()
        {
            return new List<KeyValuePair<int, string>>() { GenderData.NotSet, GenderData.Male, GenderData.Female };
        }
    }
}
