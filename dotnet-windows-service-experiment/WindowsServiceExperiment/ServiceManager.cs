using System;
using System.Collections;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace WindowsServiceExperiment
{
    public class ServiceManager
    {
        private readonly string _serviceName;
        private readonly Assembly _serviceAssembly;

        public ServiceManager(string serviceName, Assembly serviceAssembly)
        {
            _serviceName = serviceName;
            _serviceAssembly = serviceAssembly;
        }

        public bool IsInstalled()
        {
            using (var serviceController = new ServiceController(_serviceName))
            {
                try
                {
                    var dummy = serviceController.Status;
                }
                catch
                {
                    return false;
                }

                return true;
            }
        }

        public bool IsRunning()
        {
            using (var serviceController = new ServiceController(_serviceName))
            {
                return serviceController.Status == ServiceControllerStatus.Running;
            }
        }

        public void Start()
        {
            using (var serviceController = new ServiceController(_serviceName))
            {
                if (serviceController.Status == ServiceControllerStatus.Running)
                {
                    throw new Exception("Service already running");
                }

                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(10));
            }
        }

        public void Stop()
        {
            using (var serviceController = new ServiceController(_serviceName))
            {
                if (serviceController.Status == ServiceControllerStatus.Stopped)
                {
                    throw new Exception("Service already stopped");
                }

                serviceController.Stop();
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
            }
        }

        private AssemblyInstaller GetAssemblyInstaller()
        {
            var assemblyInstaller = new AssemblyInstaller(_serviceAssembly, null)
                {
                    UseNewContext = true
                };
            return assemblyInstaller;
        }

        public void Install()
        {
            using (var assemblyInstaller = GetAssemblyInstaller())
            {
                var state = new Hashtable();
                try
                {
                    assemblyInstaller.Install(state);
                    assemblyInstaller.Commit(state);
                }
                catch
                {
                    try
                    {
                        assemblyInstaller.Rollback(state);
                    }
                    catch
                    {
                    }

                    throw;
                }
            }
        }

        public void Uninstall()
        {
            using (var assemblyInstaller = GetAssemblyInstaller())
            {
                var state = new Hashtable();
                assemblyInstaller.Uninstall(state);
            }
        }
    }
}