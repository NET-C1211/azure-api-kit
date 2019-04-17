﻿using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Create
{
    public class DiagnosticTemplateCreator
    {
        public DiagnosticTemplateResource CreateAPIDiagnosticTemplateResource(APIConfig api, string[] dependsOn)
        {
            // create diagnostic resource with properties
            DiagnosticTemplateResource diagnosticTemplateResource = new DiagnosticTemplateResource()
            {
                name = $"[concat(parameters('ApimServiceName'), '/{api.name}/{api.diagnostic.name}')]",
                type = ResourceTypeConstants.APIDiagnostic,
                apiVersion = "2018-06-01-preview",
                properties = new DiagnosticTemplateProperties()
                {
                    alwaysLog = api.diagnostic.alwaysLog,
                    loggerId = api.diagnostic.loggerId,
                    sampling = api.diagnostic.sampling,
                    frontend = api.diagnostic.frontend,
                    backend = api.diagnostic.backend,
                    enableHttpCorrelationHeaders = api.diagnostic.enableHttpCorrelationHeaders
                },
                dependsOn = dependsOn
            };
            return diagnosticTemplateResource;
        }
    }
}
