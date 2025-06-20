using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudCode;
using UnityEngine;

namespace Networking.Networking
{
    public class CloudCodeProcessor
    {
        private readonly ICloudCodeService _cloudCodeService;

        public CloudCodeProcessor(ICloudCodeService cloudCodeService)
        {
            _cloudCodeService = cloudCodeService ?? throw new ArgumentNullException(nameof(cloudCodeService));
        }

        public async Task<T> CallCloudCodeWithResponse<T>(string endpoint, Dictionary<string, object> arguments = null)
        {
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentException("Endpoint cannot be null or empty", nameof(endpoint));

            try
            {
                var result = await _cloudCodeService.CallEndpointAsync(endpoint, arguments);
            
                if (string.IsNullOrEmpty(result))
                    throw new InvalidOperationException("Received empty response from cloud code endpoint");

                var response = JsonUtility.FromJson<T>(result);
                if (response == null)
                    throw new InvalidOperationException("Failed to deserialize response");
                return response;
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                throw new Exception($"Error calling cloud code endpoint '{endpoint}'", ex);
            }
        }
        public async Task CallCloudCode(string endpoint, Dictionary<string, object> arguments = null)
        {
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentException("Endpoint cannot be null or empty", nameof(endpoint));

            try
            {
                var result = await _cloudCodeService.CallEndpointAsync(endpoint, arguments);
            
                if (string.IsNullOrEmpty(result))
                    throw new InvalidOperationException("Received empty response from cloud code endpoint");
            }
            catch (Exception ex) when (ex is not InvalidOperationException)
            {
                throw new Exception($"Error calling cloud code endpoint '{endpoint}'", ex);
            }
        }
        
        public async Task<string> GetAchievementsByCategory(string category, string saveData)
        {
            return await _cloudCodeService.CallModuleEndpointAsync<string>(
                "PlayerAchievements",
                "GetAchievementsByCategory",
                new Dictionary<string, object>()
                {
                    {"category", category},
                    {"saveData", saveData},
                });
        }
    }
}