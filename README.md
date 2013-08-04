self-manageable-windows-service-experiment
==========================================

Proof of concept application to check if it's possible to have single executable that can act as both:

1. Regular Windows Service
2. Manager for this Windows Service

I have already built something similar here: https://github.com/loki2302/dotnet-windows-service-experiment, but that approach had a major issue handling dependencies (libraries, app.config): when service gets installed, the system only installs the executable while all its libraries are ignored. This seems to be pretty weired, because I firmly believe that it's a king of _very_ basic use case, when the app (service) has dependencies.

This new approach doesn't do any installation activities. Instead, it uses WMI (`Win32_Service`) to register the executable as Windows Service without any extra magic. Here are the details: http://msdn.microsoft.com/en-us/library/windows/desktop/gg196691(v=vs.85).aspx

* `SelfManageableWindowsServiceExperiment.exe` prints the lists of available commands.
* `SelfManageableWindowsServiceExperiment.exe status` tells if service is installed and running.
* `SelfManageableWindowsServiceExperiment.exe install` registers the service. Fails if this service already registered.
* `SelfManageableWindowsServiceExperiment.exe uninstall` unregisters the service. Fails if this service is not registered.
* `SelfManageableWindowsServiceExperiment.exe start` starts the service. Fails if this service is not installed. Fails if this service already running.
* `SelfManageableWindowsServiceExperiment.exe stop` stops the service. Fails if this service is not running. Fails if this service not installed.
* `SelfManageableWindowsServiceExperiment.exe install-and-start` idempotently install and start the service. If service is already installed, it just starts it. If service is already running, does nothing. Fails only in case of internal issues (like permissions, unexpected service state, etc).
* `SelfManageableWindowsServiceExperiment.exe stop-and-uninstall` idempotently stop and uninstall the service. If service is not running, it just uninstalls it. If service is not installed, does nothing. Fails only in case of internal issues (like permissions, unexpected service state, etc).
