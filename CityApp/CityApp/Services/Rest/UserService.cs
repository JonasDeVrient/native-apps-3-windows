﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CityApp.DataModel;
using CityApp.DataModel.Responses;
using CityApp.Views;
using Newtonsoft.Json;
using Windows.Security.Cryptography.Certificates;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace CityApp.Services.Rest
{
    public class UserService
    {
        private static UserService _us;
        public static UserService us
        {
            get
            {
                if (_us == null) { _us = new UserService(); }
                return _us;
            }
            set { _us = value; }
        }
        public int LoggedUser { get; internal set; }


        // Used for a workaround for the ssl certificate
        private readonly HttpBaseProtocolFilter _httpFilter;
        private readonly HttpClient _httpClient;

        // REST endpoint url
        private const string _apiUrl = "https://localhost:44321/api/users";
        //private const string _apiUrl = "https://cityapprest.azurewebsites.net/api/users";

        public UserService()
        {
            _httpFilter = new HttpBaseProtocolFilter();
            _httpFilter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Expired);
            _httpFilter.IgnorableServerCertificateErrors.Add(ChainValidationResult.Untrusted);
            _httpFilter.IgnorableServerCertificateErrors.Add(ChainValidationResult.InvalidName);

            _httpClient = new HttpClient(_httpFilter);
        }


        public async Task<User> RegisterUser(User user)
        {
            var userJson = JsonConvert.SerializeObject(user);

            var userPostReady = new HttpStringContent(userJson, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

            var response = await _httpClient.PostAsync(new Uri(_apiUrl), userPostReady);

            // No authorization is needed for a post user request
            _httpClient.DefaultRequestHeaders.Clear();

            await response.Content.ReadAsStringAsync();

            await AuthenticateAsync(new LogInCredentials() { Username = user.Username, Password = user.Password });

            return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
        }

        public async Task<string> AuthenticateAsync(LogInCredentials user)
        {
            var userJson = JsonConvert.SerializeObject(user);
            var userPostReady = new HttpStringContent(userJson, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

            var response = await _httpClient.PostAsync(new Uri(_apiUrl + "/authenticate"), userPostReady);

            if (response.IsSuccessStatusCode)
            {
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(await response.Content.ReadAsStringAsync());
                await StorageService.StoreUserToken(loginResponse.Token);
                await StorageService.StoreUserId(loginResponse.UserId);
                UserResponse uType = await GetUser();
                StorageService.UserType = (int)uType.UserType;
                if ((int)uType.UserType == 0)
                {
                    NavigationRoot.Instance.IsOwner = true;
                }
                else
                {
                    NavigationRoot.Instance.IsOwner = false;
                }
                return "Succesvol ingelogd";
            }
            else
            {
                return response.Content.ToString();
            }
        }
        public static async Task<string> LogOutUserAsync()
        {
            //StorageService.RemoveUserCredentials();
            await StorageService.ClearStoredUser();
            await StorageService.StoreUserToken("");
            StorageService.UserType = -1;
            NavigationRoot.Instance.IsOwner = false;
            return "ok";
        }
        public async Task<UserResponse> GetUser()
        {
            var token = await StorageService.RetrieveUserToken();
            var userId = await StorageService.RetrieveUserId();

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var json = await _httpClient.GetStringAsync(new Uri($"{_apiUrl}/{userId}"));

            return JsonConvert.DeserializeObject<UserResponse>(json);
        }

        public async Task<string> AddCompanyToSubscription(Company company)
        {
            var token = await StorageService.RetrieveUserToken();
            var userId = await StorageService.RetrieveUserId();

            var companyJson = JsonConvert.SerializeObject(company);

            var companyPostReady = new HttpStringContent(companyJson, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");

            var response = await _httpClient.PostAsync(new Uri($"{_apiUrl}/{userId}/companies"), companyPostReady);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            //return JsonConvert.DeserializeObject<List<Company>>(await response.Content.ReadAsStringAsync());
            return "succes";
        }
    }
}
