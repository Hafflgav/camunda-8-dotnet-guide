﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Zeebe.Client;
using Zeebe.Client.Api.Responses;
using Zeebe.Client.Api.Worker;
using Zeebe.Client.Impl.Builder;
using blood_alcohol_approximator;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DotNet_Camunda8_getting_started 
{
    class ZeebeClient
    {
        private static readonly String _ClientID = "vGMj5t5PK1LObVszf5mswW6QtFr5-xk6";
        private static readonly String _ClientSecret = "TXNWoTDYqRRYbs7QUleD_HSEgKJ.TGNr3.6PJIrtzzpM~7LaACOeEdeViLd8AfBD";
        private static readonly String _ContactPoint = "04322dc6-3d72-4d72-8dfc-484189a06927.bru-2.zeebe.camunda.io:443";
        private static readonly String _BpmnFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../Resources", "BallmerPeakProcess.bpmn");
        private static readonly String _JobType = "approx_bac"; 

        public static IZeebeClient zeebeClient;

        static async Task Main(string[] args)
        {
            zeebeClient = CamundaCloudClientBuilder
                                .Builder()
                                .UseClientId(_ClientID)
                                .UseClientSecret(_ClientSecret)
                                .UseContactPoint(_ContactPoint)
                                .Build();

            // Deploy Process and start and Instance
            string bpmnProcessId = await DeployProcess(_BpmnFile);
            long processInstanceKey = await StartProcessInstance(bpmnProcessId);            

            // Starting the Job Worker
            using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
            {
                zeebeClient.NewWorker()
                         .JobType(_JobType)
                         .Handler(TriggerApproximation)
                         .MaxJobsActive(5)
                         .Name(Environment.MachineName)
                         .AutoCompletion()
                         .PollInterval(TimeSpan.FromSeconds(1))
                         .Timeout(TimeSpan.FromSeconds(10))
                         .Open();

                signal.WaitOne();
            }
        }

        /// <summary>
        /// Deploying a BPMN Process Model to Camunda 8 SaaS
        /// </summary>
        /// <param name=processDefinitionKey">Key of the Process Definition</param>
        private async static Task<string> DeployProcess(String bpmnFile)
        {
            var deployRespone = await zeebeClient.NewDeployCommand()
                .AddResourceFile(bpmnFile)
                .Send();
            Console.WriteLine("Process Definition has been deployed!");

            var bpmnProcessId = deployRespone.Processes[0].BpmnProcessId;
            return bpmnProcessId;
        }

        /// <summary>
        /// Starting a Process Instance on Camunda 8 SaaS
        /// </summary>
        /// <param name=bpmnProcessId">Key of the Process Definition</param>
        private async static Task<long> StartProcessInstance(string bpmnProcessId)
        {
            var processInstanceResponse = await zeebeClient.NewCreateProcessInstanceCommand()
                .BpmnProcessId(bpmnProcessId)
                .LatestVersion()
                .Send();
              
            Console.WriteLine("Process Instance has been started!");

            var processInstanceKey = processInstanceResponse.ProcessInstanceKey;
            return processInstanceKey;
        }

        /// <summary>
        /// Business Logic to calculate the blood alcohol concentration called by the Job Worker 
        /// </summary>
        /// <param name=jobClient">The client with access to all job-related operation</param>
        /// <param name=job">Job Object</param>
        private static void TriggerApproximation(IJobClient jobClient, IJob job)
        {
            JObject jsonObject = JObject.Parse(job.Variables);
            string gender = (string)jsonObject["gender"];
            int weight = (int)jsonObject["weight"];

            Console.WriteLine("Working on Task");
            Person person = new Person(weight, gender);
            double grammsAlcohol = BloodAlcoholApproximator.Approximate(person);
            Dictionary<String, double> suggestedDrinks = BloodAlcoholApproximator.SuggestDrinks(grammsAlcohol);

            jobClient.NewCompleteJobCommand(job.Key)
                    .Variables(JsonConvert.SerializeObject(suggestedDrinks))
                    .Send()
                    .GetAwaiter()
                    .GetResult();
            Console.WriteLine("Completed the fetched Task");
        }  
    }
}
