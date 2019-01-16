﻿using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates
{
    public class OpenAPISpecReader
    {
        public OpenApiDocument ConvertToOpenAPISpec(string json)
        {
            OpenApiStringReader reader = new OpenApiStringReader();
            OpenApiDocument doc = reader.Read(json, out var diagnostic);
            return doc;
        }

        public OpenApiDocument ConvertLocalFileToOpenAPISpec(string jsonFile)
        {
            JObject jObject = JObject.Parse(File.ReadAllText(jsonFile));
            string json = JsonConvert.SerializeObject(jObject);
            OpenApiDocument document = ConvertToOpenAPISpec(json);
            return document;
        }

        public async Task<OpenApiDocument> ConvertRemoteURLToOpenAPISpecAsync(Uri uriResult)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(uriResult);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                OpenApiDocument document = ConvertToOpenAPISpec(json);
                return document;
            }
            else
            {
                return new OpenApiDocument();
            }
        }

        public async Task<OpenApiDocument> ConvertOpenAPISpecToDoc(string openApiSpecFileLocation)
        {
            Uri uriResult;
            bool isUrl = Uri.TryCreate(openApiSpecFileLocation, UriKind.Absolute, out uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (isUrl)
            {
                return await this.ConvertRemoteURLToOpenAPISpecAsync(uriResult);
            } else
            {
                return this.ConvertLocalFileToOpenAPISpec(openApiSpecFileLocation);
            }
        }
    }
}
