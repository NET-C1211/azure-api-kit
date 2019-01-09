﻿using System.IO;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates
{
    public class PolicyTemplateCreator
    {
        private TemplateCreator templateCreator;
        private FileReader fileReader;

        public PolicyTemplateCreator(TemplateCreator templateCreator, FileReader fileReader)
        {
            this.templateCreator = templateCreator;
            this.fileReader = fileReader;
        }

        public async Task<Template> CreateAPIPolicyAsync(CreatorConfig creatorConfig)
        {
            Template policyTemplate = this.templateCreator.CreateEmptyTemplate();

            // add parameters
            policyTemplate.parameters = new Dictionary<string, TemplateParameterProperties>
            {
                { "ApimServiceName", new TemplateParameterProperties(){ type = "string" } }
            };

            List<TemplateResource> resources = new List<TemplateResource>();
            // create policy resource with properties
            PolicyTemplateResource policyTemplateResource = new PolicyTemplateResource()
            {
                name = "[concat(parameters('ApimServiceName'), '/apipolicy')]",
                type = "Microsoft.ApiManagement/service/apis/policies",
                apiVersion = "2018-06-01-preview",
                properties = new PolicyTemplateProperties()
                {
                    contentFormat = "rawxml",
                    policyContent = await this.fileReader.RetrieveLocationContentsAsync(creatorConfig.api.policy)
                }
            };
            resources.Add(policyTemplateResource);

            policyTemplate.resources = resources.ToArray();
            return policyTemplate;
        }

        public async Task<Template> CreateOperationPolicyAsync(KeyValuePair<string, OperationsConfig> policyPair)
        {
            Template policyTemplate = this.templateCreator.CreateEmptyTemplate();

            // add parameters
            policyTemplate.parameters = new Dictionary<string, TemplateParameterProperties>
            {
                { "ApimServiceName", new TemplateParameterProperties(){ type = "string" } }
            };

            List<TemplateResource> resources = new List<TemplateResource>();
            // create policy resource with properties
            PolicyTemplateResource policyTemplateResource = new PolicyTemplateResource()
            {
                name = $"[concat(parameters('ApimServiceName'), '/{String.Concat("operationpolicy-", policyPair.Key)}')]",
                type = "Microsoft.ApiManagement/service/apis/operations/policies",
                apiVersion = "2018-06-01-preview",
                properties = new PolicyTemplateProperties()
                {
                    contentFormat = "rawxml",
                    policyContent = await this.fileReader.RetrieveLocationContentsAsync(policyPair.Value.policy)
                }
            };
            resources.Add(policyTemplateResource);

            policyTemplate.resources = resources.ToArray();
            return policyTemplate;
        }

        public async Task<List<Template>> CreateOperationPolicies(CreatorConfig creatorConfig)
        {
            List<Template> policyTemplates = new List<Template>();
            foreach (KeyValuePair<string, OperationsConfig> pair in creatorConfig.api.operations)
            {
                policyTemplates.Add(await this.CreateOperationPolicyAsync(pair));
            }
            return policyTemplates;
        }
    }
}
