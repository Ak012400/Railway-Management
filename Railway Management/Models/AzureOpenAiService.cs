using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.Identity.Client;
using OpenAI.Chat;
using Railway_Management.Services;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using OpenAI;
using System.Text.RegularExpressions;
using Azure.Identity;
using Microsoft.EntityFrameworkCore;
using OpenAIClient = Azure.AI.OpenAI.OpenAIClient;
using System.Net.Http.Headers;
using System.Text.Json;
using OpenAI.RealtimeConversation;
using NuGet.Common;
using System.Numerics;

class AzureOpenAiService 
{

    private static readonly string Managed_Identity_Key = "f9c4c5dc-290c-4ce6-aca1-b65f70ff0ed6";
    private static readonly string Managed_Identity_ = "95010247-ed08-45e9-add2-c1fcf4ee7e6c";
    private static readonly string endpointReal = "https://arunk-m5wqdad2-eastus2.openai.azure.com/";
    private static readonly string ApiKey = "FFuquKuLa6uzsUnMbx6zMYa6IpC1Om5gbCPdYZ6RLj5VLCwwtWATJQQJ99BAACHYHv6XJ3w3AAAAACOGxM6L";
    private static readonly string subscription= "73cf14a9-c3db-4f7c-ae46-bce4844c0b94";

    public static async Task<string> GetResultAsync(string? userBusinessJustification)
    {
        try
        {
           

            

            

            //string endpoint = "https://arunk-m5wqdad2-eastus2.openai.azure.com/openai/deployments/gpt-4o/chat/completions?api-version=2024-02-15-preview";
            var miClientID = Managed_Identity_; 
            var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = subscription });

            var openAIClient = new OpenAIClient(new Uri(endpointReal), credential);

            var completionsOptions = new CompletionsOptions
            {
                Prompts = {
                        $"You have to give the precise and updated knowledge with kindly. this is user asked question--("+userBusinessJustification+")"
                       },
                MaxTokens = 300,
                NucleusSamplingFactor = 0.5f,
                FrequencyPenalty = 0f,
                PresencePenalty = 0f,
                GenerationSampleCount = 1,
            };

            Response<Completions> completionsResponse = await openAIClient.GetCompletionsAsync("gpt-35-turbo", completionsOptions);

           
           


           

            return completionsResponse.ToString();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public static async Task<string> GetResult2Async(string prompt)
    {
        var endpoint = "https://arunk-m5wqdad2-eastus2.openai.azure.com/";
    
        // Azure OpenAI API Key
        var apiKey = "FFuquKuLa6uzsUnMbx6zMYa6IpC1Om5gbCPdYZ6RLj5VLCwwtWATJQQJ99BAACHYHv6XJ3w3AAAAACOGxM6L";

        // Request Body
        var requestBody = new
        {
            messages = new[]
            {
                new { role = "system", content = "You are a helpful assistant." },
                new { role = "user", content = "Hello, how are you?" }
            },
            max_tokens = 100,
            temperature = 0.7
        };

        // Serialize Request Body
        var jsonRequest = JsonSerializer.Serialize(requestBody);

        using (var client = new HttpClient())
        {
            // Add Authorization Header
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            // Prepare Content
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Make POST Request
            var response = await client.PostAsync(endpoint, content);

            // Get Response
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseBody);
            return responseBody;
        }
    }

    public static async Task<string> GetAiResult3(string prompt)
    {
        string endpoint = "";
        string apiKey = "";

       
        var credential = new DefaultAzureCredential();

        
        var client = new OpenAIClient(new Uri(endpoint), credential);

        
        string deploymentName = "arunk-m5wqdad2-eastus2";

        
        var response = await client.GetCompletionsAsync(deploymentName, new CompletionsOptions
        {
            Prompts = { "Tell me a joke about programmers." },
            MaxTokens = 50, // Limit tokens for response
        });
        List<string> result = new List<string>();
        // Display the response
        foreach (var choice in response.Value.Choices)
        {
            Console.WriteLine(choice.Text);
            result.Add(choice.Text);
        }
        return result.ToString();
    }

    public async static Task<string> GetValueOfAi(String values)
    {

        try
        {
          
            string tenantId = "";
            string clientId = "";
            string clientSecret = "";

            
            string openAiEndpoint = "https://arunk-m5wqdad2-eastus2.openai.azure.com/"; // Replace with your Azure OpenAI resource name
            string deploymentName = "arunk-m5wqdad2-eastus2"; // Replace with your deployment name

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);

          
            var client = new OpenAIClient(new Uri(openAiEndpoint), credential);

           
            var completionsOptions = new CompletionsOptions
            {
                Prompts = { "What is the capital of France?" },
                MaxTokens = 50
            };

            var response = await client.GetCompletionsAsync(deploymentName, completionsOptions);

            Console.WriteLine("Response:");
            foreach (var choice in response.Value.Choices)
            {
                Console.WriteLine(choice.Text.Trim());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return "null";
    }

    ////public async static GetAiResult4(string result)
    ////{
    //OpenAIClient client_ = new OpenAIClient(new Uri(endpointReal), new AzureKeyCredential(ApiKey));

    //var chatCompletions = new ChatCompletionOptions()
    //{
    //    Messages =
    //        {

    //        }
    //}
    ////}
    ///

    public async static Task<string> GetAPIResult6(string result)
    {
        string apiKey = "FFuquKuLa6uzsUnMbx6zMYa6IpC1Om5gbCPdYZ6RLj5VLCwwtWATJQQJ99BAACHYHv6XJ3w3AAAAACOGxM6L"; // Replace with your OpenAI API Key

        // Initialize OpenAI API client
        var openAiClient = new OpenAIClient(apiKey);

        // Deployment/Model name from Azure
        string deploymentName = "YOUR_DEPLOYMENT_NAME"; // Replace with your deployment name

        // User's input prompt
        string prompt = "Hello, how are you?";

        //try
        //{
        //    // Get completion result


        //    // Display the response
        //    Console.WriteLine("Response from OpenAI:");
        //    Console.WriteLine(completionResult.Choices[0].Text.Trim());
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine("Error occurred: " + ex.Message);
        //}
        return null;
    }
    public async static Task<String> GetApiResult5(string Result)
    {
        string endpoint = "https://arunk-m64myhxh-swedencentral.openai.azure.com/";
        string apiKey = "6QOTjUJF4nX6ZK0N5zdqlU5CzUqmvZgvc3x0LBm5uGsOkbGeCz2jJQQJ99BAACfhMk5XJ3w3AAAAACOGkeyx"; // api secrets

        // Initialize the OpenAI client with API key
        var openAIClient = new OpenAIClient(new Uri(endpointReal), new AzureKeyCredential(ApiKey));

        // Set up the completions options
        CompletionsOptions completionsOptions = new()
        {
            // This will correspond to the custom name you chose for your deployment when you deployed a model.  
            // Use a gpt-35-turbo-instruct deployment.  
    
            Prompts = { "" },
            Temperature = (float)1,
            MaxTokens = 100,
            NucleusSamplingFactor = (float)0.5,
            FrequencyPenalty = (float)0,
            PresencePenalty = (float)0,

            GenerationSampleCount = 1,
        };

        // Get the completions response (await the async method)
        Response<Completions> completionsResponse = await openAIClient.GetCompletionsAsync("davinci-002", completionsOptions);


        // Retrieve the completion text
        string completion = completionsResponse.Value.Choices[0].Text.Trim();

        // Output the result
        Console.WriteLine(completion);
        return null;
    }


}