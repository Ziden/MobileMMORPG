//-----------------------------------------------------------------------
// <copyright file="CustomerTypeModel.cs" company="Genesys Source">
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
using Genesys.Framework.Name;
using Genesys.Extensions;

namespace Framework.Customer
{
    /// <summary>
    /// Customer Type view/http transport model, mainly for a key/value of Id/Name
    /// </summary>
    public class CustomerTypeModel : NameIdModel, ICustomerType
    {
        /// <summary>
        /// Common customer type keys used as an Id for the table column CustomerType.CustomerTypeKey
        /// </summary>
        public struct Types
        {
            /// <summary>
            /// Default/No Type
            /// </summary>
            public static Guid None { get; set; } = TypeExtension.DefaultGuid;

            /// <summary>
            /// Standard customer
            /// </summary>
            public static Guid Standard { get; set; } = new Guid("BF3797EE-06A5-47F2-9016-369BEB21D944");

            /// <summary>
            /// Premium active customer
            /// </summary>
            public static Guid Premium { get; set; } = new Guid("36B08B23-0C1D-4488-B557-69665FD666E1");

            /// <summary>
            /// Lifetime status
            /// </summary>
            public static Guid Lifetime { get; set; } = new Guid("51A84CE1-4846-4A71-971A-CB610EEB4848");
        }
    }
}
