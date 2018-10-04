//-----------------------------------------------------------------------
// <copyright file="CustomerSearchModel.cs" company="Genesys Source">
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
using Genesys.Framework.Data;
using System;
using System.Collections.Generic;

namespace Framework.Customer
{
    /// <summary>
    /// Customer Search Results
    /// </summary>    
    public class CustomerSearchModel : EntityModel<CustomerSearchModel>, ICustomerSearch<CustomerModel>
    {
        private int maxResults = 25;

        /// <summary>
        /// FirstName of customers
        /// </summary>     
        public string FirstName { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// MiddleName of customer
        /// </summary>
        public string MiddleName { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// LastName of customer
        /// </summary>
        public string LastName { get; set; } = TypeExtension.DefaultString;

        /// <summary>
        /// BirthDate of customer
        /// </summary>
        public DateTime BirthDate { get; set; } = TypeExtension.DefaultDate;

        /// <summary>
        /// BirthDate of customer
        /// </summary>
        public int GenderId { get; set; } = GenderList.GenderData.NotSet.Key;

        /// <summary>
        /// Type of customer
        /// </summary>
        public Guid CustomerTypeKey { get; set; } = CustomerTypeModel.Types.None;

        /// <summary>
        /// Search results
        /// </summary>
        public List<CustomerModel> Results { get; set; } = new List<CustomerModel>();

        /// <summary>
        /// Maximum number of results to return
        ///  Will not accept negative number, flips back to default (25)
        /// </summary>
        public int MaxResults
        {
            get => maxResults;
            set => maxResults = value > 0 ? value : maxResults;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public CustomerSearchModel()
                : base()
        {
            this.CustomerTypeKey = CustomerTypeModel.Types.Standard;
        }

    }
}