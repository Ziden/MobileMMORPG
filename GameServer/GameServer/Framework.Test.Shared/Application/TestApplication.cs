//-----------------------------------------------------------------------
// <copyright file="TestApplication.cs" company="Genesys Source">
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
using Genesys.Extras.Net;
using System;
using System.Threading.Tasks;

namespace Framework.Test
{
    /// <summary>
    /// Global application information
    /// </summary>
    public class TestApplication : ITestApplication
    {
        /// <summary>
        /// Persistent ConfigurationManager class, automatically loaded with this project .config files
        /// </summary>
        public IConfigurationManager ConfigurationManager { get; set; } = new ConfigurationManagerLocal();

        /// <summary>
        /// MyWebService
        /// </summary>
        public Uri MyWebService { get { return new Uri(ConfigurationManager.AppSettingValue("MyWebService"), UriKind.RelativeOrAbsolute); } }

        /// <summary>
        /// Entry point Screen (Typically login screen)
        /// </summary>
        public Uri StartupUri { get; } = TypeExtension.DefaultUri;

        /// <summary>
        /// Home dashboard screen
        /// </summary>
        public Uri HomePage { get; } = TypeExtension.DefaultUri;

        /// <summary>
        /// Error screen
        /// </summary>
        public Uri ErrorPage { get; } = TypeExtension.DefaultUri;

        /// <summary>
        /// Constructor
        /// </summary>
        public TestApplication() : base()
        {
            OnObjectInitialize();
        }

        /// <summary>
        /// Loads config data
        /// </summary>
        /// <returns></returns>
        public async Task LoadDataAsync()
        {
            await Task.Delay(1);
            ConfigurationManager = new ConfigurationManagerLocal();
        }

        /// <summary>
        /// Wakes up any sleeping processes, and MyWebService chain
        /// </summary>
        /// <returns></returns>
        public virtual async Task WakeServicesAsync()
        {
            if (MyWebService.ToString() == TypeExtension.DefaultString)
            {
                HttpRequestGetString Request = new HttpRequestGetString(MyWebService.ToString())
                {
                    ThrowExceptionWithEmptyReponse = false
                };
                await Request.SendAsync();
            }
        }

        /// <summary>
        /// Can this screen go back
        /// </summary>
        public bool CanGoBack { get; } = TypeExtension.DefaultBoolean;

        /// <summary>
        /// Can this screen go forward
        /// </summary>
        public bool CanGoForward { get; } = TypeExtension.DefaultBoolean;

        /// <summary>
        /// Current loaded page
        /// </summary>
        public Uri CurrentPage { get; } = TypeExtension.DefaultUri;

        /// <summary>
        /// Navigates back to previous screen
        /// </summary>
        public void GoBack() { }

        /// <summary>
        /// Navigates forward to next screen
        /// </summary>
        public void GoForward() { }

        /// <summary>
        /// Navigates to a page via Uri.
        /// Typically in WPF apps
        /// </summary>
        /// <param name="destinationPageUrl">Destination page Uri</param>
        public bool Navigate(Uri destinationPageUrl) { return true; }

        /// <summary>
        /// Navigates to a page via Uri.
        /// Typically in WPF apps
        /// </summary>
        /// <param name="destinationPageUrl">Destination page Uri</param>
        /// <param name="dataToPass">Data to be passed to the destination page</param>
        public bool Navigate<TModel>(Uri destinationPageUrl, TModel dataToPass) { return true; }
        
        /// <summary>
        /// New model to load
        /// </summary>
        public event ObjectInitializeEventHandler ObjectInitialize;

        /// <summary>
        /// OnObjectInitialize()
        /// </summary>
        protected async void OnObjectInitialize()
        {
            await this.LoadDataAsync();
            await this.WakeServicesAsync();
            ObjectInitialize?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Workflow beginning. No custom to return.
        /// </summary>
        /// <param name="sender">Sender of event</param>
        /// <param name="e">Event arguments</param>
        public delegate void ObjectInitializeEventHandler(object sender, EventArgs e);        
        }
}
