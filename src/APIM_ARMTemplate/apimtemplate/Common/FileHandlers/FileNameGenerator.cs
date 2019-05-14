﻿using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Create;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common
{
    public class FileNameGenerator
    {

        public FileNames GenerateFileNames(string apimServiceName)
        {
            // generate useable object with file names for consistency throughout project
            return new FileNames()
            {
                apiVersionSets = $@"/{apimServiceName}-apiVersionSets.template.json",
                products = $@"/{apimServiceName}-products.template.json",
                loggers = $@"/{apimServiceName}-loggers.template.json",
                backends = $@"/{apimServiceName}-backends.template.json",
                namedValues = $@"/{apimServiceName}-namedValues.template.json",
                authorizationServers = $@"/{apimServiceName}-authorizationServers.template.json",
                linkedMaster = $@"/{apimServiceName}-master.template.json",
                parameters = $@"/{apimServiceName}-parameters.json"
            };
        }

        public string GenerateCreatorAPIFileName(string apiName, bool isSplitAPI, bool isInitialAPI, string apimServiceName)
        {
            if (isSplitAPI == true)
            {
                return isInitialAPI == true ? $@"/{apimServiceName}-{apiName}-initial.api.template.json" : $@"/{apimServiceName}-{apiName}-subsequent.api.template.json";
            }
            else
            {
                return $@"/{apimServiceName}-{apiName}.api.template.json";
            }
        }

        public string GenerateExtractorAPIFileName(string singleAPIName, string apimServiceName)
        {
            return singleAPIName == null ? $@"{apimServiceName}-apis.template.json" : $@"{apimServiceName}-{singleAPIName}-api.template.json";
        }
    }

    public class FileNames
    {
        public string apiVersionSets { get; set; }
        public string products { get; set; }
        public string loggers { get; set; }
        public string authorizationServers { get; set; }
        public string backends { get; set; }
        public string namedValues { get; set; }
        public string parameters { get; set; }
        // linked property outputs 1 master template
        public string linkedMaster { get; set; }
    }
}
