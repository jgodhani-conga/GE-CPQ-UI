using AutomationProject1.Main.resources;
using MongoDB.Driver.Core.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gehc_cpq_ui_master.Main.utils
{
    public class URLGenerator
    {
        public readonly string baseURL;
        public string proposalListPageURL { get; private set; } 
        public string proposalDetailsPageURL { get; private set; }
        public URLGenerator()
        {
            baseURL = Config.baseURL;
            this.proposalListPageURL = baseURL + "/o/Apttus_Proposal__Proposal__c/home";
            this.proposalDetailsPageURL = "/r/Apttus_Proposal__Proposal__c/{proposalId}/view";


        }
        public void generateProposalDetailsPageURLWithId(string Id)
        {
            this.proposalDetailsPageURL = baseURL + proposalDetailsPageURL.Replace("{proposalId}",Id);
        }

        public string GenerateProposalDetailsPageURL(string proposalId)
        {
            return baseURL + proposalDetailsPageURL.Replace("{proposalId}", proposalId);
        }
    }
}
