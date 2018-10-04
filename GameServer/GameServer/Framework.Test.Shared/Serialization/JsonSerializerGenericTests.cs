//-----------------------------------------------------------------------
// <copyright file="JsonSerializerGenericTests.cs" company="Genesys Source">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Genesys.Extensions;
using Genesys.Extras.Serialization;
using Genesys.Extras.Text;
using System.Runtime.Serialization.Json;

namespace Framework.Test
{
    public class PersonInfo
    {
        public string FirstName { get; set; } = TypeExtension.DefaultString;
        public string LastName { get; set; } = TypeExtension.DefaultString;
        public DateTime BirthDate { get; set; } = TypeExtension.DefaultDate;
    }

    [TestClass()]
    public class JsonSerializerGenericTests
    {
        private const string testPhrase = "Services up and running...";
        private const string testPhraseSerialized = "\"Services up and running...\"";
        private const string testPhraseMutableSerialized = "{\"Value\":\"Services up and running...\"}";

        [TestMethod()]
        public void Serialization_Json_ValueTypes()
        {
            // Immutable string class
            var data1= TypeExtension.DefaultString;
            var Testdata1= "TestDataHere";
            ISerializer<object> Serialzer1 = new JsonSerializer<object>();
            data1= Serialzer1.Serialize(Testdata1);
            Assert.IsTrue(Serialzer1.Deserialize(data1).ToString() == Testdata1);
            
            var data = TypeExtension.DefaultString;
            StringMutable TestData = "TestDataHere";
            var Serialzer = new JsonSerializer<StringMutable>();
            data = Serialzer.Serialize(TestData);
            Assert.IsTrue(Serialzer.Deserialize(data).ToString() == TestData.ToString());
        }

        [TestMethod()]
        public void Serialization_Json_ReferenceTypes()
        {
            // Collections, etc
            var ItemL = new List<int> { 1, 2, 3 };
            var Serializer = new JsonSerializer<List<int>>();
            var SerializedDataL = Serializer.Serialize(ItemL);
            Assert.IsTrue(ItemL.Count == Serializer.Deserialize(SerializedDataL).Count);
        }

        [TestMethod()]
        public void Serialization_Json_PersonInfo()
        {
            // Collections, etc
            var personObject = new PersonInfo() { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1977, 11, 20) };
            var personObjectSerialized = TypeExtension.DefaultString;
            var personDefaultWebAPI = "{\"BirthDate\":\"\\/Date(248860800000-0800)\\/\",\"FirstName\":\"John\",\"LastName\":\"Doe\"}";
            var personISO8601 = "{\"FirstName\":\"John\",\"MiddleName\":\"Michelle\",\"LastName\":\"Doe\",\"BirthDate\":\"1977-11-20T00:00:00\",\"Id\":-1,\"Key\":\"00000000-0000-0000-0000-000000000000\"}";
            var personISO8601F = "{\"FirstName\":\"John\",\"MiddleName\":\"Michelle\",\"LastName\":\"Doe\",\"BirthDate\":\"1977-11-20T00:00:00.000\",\"Id\":-1,\"Key\":\"00000000-0000-0000-0000-000000000000\"}";
            var personJsonReSerialized = TypeExtension.DefaultString;
            var personJsonDeserialized = new PersonInfo();
            var serializer = new JsonSerializer<PersonInfo>();

            // stringISODate -> object -> string
            serializer = new JsonSerializer<PersonInfo>();
            personJsonDeserialized = serializer.Deserialize(personISO8601F);
            Assert.IsTrue(personJsonDeserialized.FirstName == "John");
            Assert.IsTrue(personJsonDeserialized.LastName == "Doe");
            Assert.IsTrue(personJsonDeserialized.BirthDate == new DateTime(1977, 11, 20));
            personJsonReSerialized = serializer.Serialize(personJsonDeserialized);
            Assert.IsTrue(personJsonReSerialized.Length > 0);

            // ISO8601 (no milliseconds) - Should fail
            serializer = new JsonSerializer<PersonInfo>();
            personJsonDeserialized = serializer.Deserialize(personISO8601);
            Assert.IsFalse(personJsonDeserialized.FirstName == "John");
            Assert.IsFalse(personJsonDeserialized.LastName == "Doe");
            Assert.IsFalse(personJsonDeserialized.BirthDate == new DateTime(1977, 11, 20));

            // Default: ISO8601F (with milliseconds) - Should work
            serializer = new JsonSerializer<PersonInfo>();
            personJsonDeserialized = serializer.Deserialize(personISO8601F);
            Assert.IsTrue(personJsonDeserialized.FirstName == "John");
            Assert.IsTrue(personJsonDeserialized.LastName == "Doe");
            Assert.IsTrue(personJsonDeserialized.BirthDate == new DateTime(1977, 11, 20));
            personJsonReSerialized = serializer.Serialize(personJsonDeserialized);
            Assert.IsTrue(personJsonReSerialized.Length > 0);

            // object -> string -> object
            personObjectSerialized = serializer.Serialize(personObject);
            Assert.IsTrue(personObjectSerialized.Length > 0);
            personObject = serializer.Deserialize(personObjectSerialized);
            Assert.IsTrue(personObject.FirstName == "John");
            Assert.IsTrue(personObject.LastName == "Doe");
            Assert.IsTrue(personObject.BirthDate == new DateTime(1977, 11, 20));

            // stringNONISODate (default date) -> object -> string
            DataContractJsonSerializer defaultSerializer = new DataContractJsonSerializer(typeof(PersonInfo));
            serializer = new JsonSerializer<PersonInfo>();
            serializer.DateTimeFormatString = defaultSerializer.DateTimeFormat;
            personJsonDeserialized = serializer.Deserialize(personDefaultWebAPI);
            Assert.IsTrue(personJsonDeserialized.FirstName == "John");
            Assert.IsTrue(personJsonDeserialized.LastName == "Doe");
            Assert.IsTrue(personJsonDeserialized.BirthDate == new DateTime(1977, 11, 20));
            personJsonReSerialized = serializer.Serialize(personJsonDeserialized);
            Assert.IsTrue(personJsonReSerialized.Length > 0);
        }

        [TestMethod()]
        public void Serialization_Json_String()
        {
            var serializer = new JsonSerializer<string>();

            Assert.IsTrue(testPhraseSerialized == serializer.Serialize(testPhrase));
            Assert.IsTrue(testPhrase == serializer.Deserialize(testPhraseSerialized));
        }

        [TestMethod()]
        public void Serialization_Json_StringMutable()
        {
            StringMutable testPhraseMutable = testPhrase;
            var result = TypeExtension.DefaultString;
            StringMutable resultMutable = TypeExtension.DefaultString;
            var serializerMutable = new JsonSerializer<StringMutable>();
           
            // Serialization            
            testPhraseMutable = testPhrase;
            result = serializerMutable.Serialize(testPhraseMutable);
            Assert.IsTrue(result == testPhraseMutableSerialized);

            // Deserialization
            resultMutable = serializerMutable.Deserialize(testPhraseMutableSerialized);
            Assert.IsTrue(resultMutable == testPhrase);
        }

        [TestMethod()]
        public void Serialization_Json_StringToStringMutable()
        {
            StringMutable testPhraseMutable = testPhrase;
            var result = TypeExtension.DefaultString;
            StringMutable resultMutable = TypeExtension.DefaultString;
            var serializerMutable = new JsonSerializer<StringMutable>();
            var serializer = new JsonSerializer<string>();

            // string Mutable can be serialized as string, then deserialized as string after transport 
            //  So that consumers don't need to know original was StringMutable
            result = serializer.Serialize(testPhraseMutable);
            Assert.IsTrue(testPhraseSerialized == result);

            // StringMutable serialize -> string deserialize
            result = serializerMutable.Deserialize(testPhraseSerialized); // Not supported scenario, should default ot empty string
            Assert.IsTrue(result == TypeExtension.DefaultString); 

            result = serializerMutable.Deserialize(testPhraseMutableSerialized);
            Assert.IsTrue(result == testPhrase);
            resultMutable = serializerMutable.Deserialize(testPhraseMutableSerialized);
            Assert.IsTrue(resultMutable == testPhrase);
        }
    }
}
