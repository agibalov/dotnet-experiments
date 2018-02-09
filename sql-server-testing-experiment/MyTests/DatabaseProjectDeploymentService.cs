using System;
using System.Collections.Generic;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;

namespace MyTests
{
    public class DatabaseProjectDeploymentService
    {
        private readonly string _sqlProjFilePath;
        private readonly string _publishProfileFilePath;

        public DatabaseProjectDeploymentService(string sqlProjFilePath, string publishProfileFilePath)
        {
            _sqlProjFilePath = sqlProjFilePath;
            _publishProfileFilePath = publishProfileFilePath;
        }

        public void DeployDatabase()
        {
            using (var projectCollection = new ProjectCollection())
            {
                var databasedProject = projectCollection.LoadProject(_sqlProjFilePath);
                databasedProject.SetProperty("SqlPublishProfilePath", _publishProfileFilePath);

                var buildSucceeded = databasedProject.Build(new[] { "Build", "Publish" }, new List<ILogger> { new ConsoleLogger() });
                if (!buildSucceeded)
                {
                    throw new Exception("Database deployment failed");
                }
            }
        }
    }
}