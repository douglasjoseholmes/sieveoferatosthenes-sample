# sieveoferatosthenes-sample
A graphical implementation of the Sieve of Eratosthenes using .NET for the Universal Windows Platform.

## Getting Started

This project was constructed in Windows 10 through Microsoft Visual Studio 2017. It is intended to run on any platform capable of side-running UWP applications.

### Prerequisites

* Windows 10, November Update (10.0; Build 10586)
* Windows 10 Anniversary Update or Later recommended

### Installing

* Clone the full contents of your repository to a readable/writable area of your computer.

If using Visual Studio 2017 or later:

* Launch Visual Studio 2017, and open the "SieveOfEratosthenes.sln" solution file.
* Compile the solution, then click the "Run" button under the architecture of your choice.

Otherwise:

* Navigate to "~/SieveOfEratosthenesUWP/AppPackages/SieveOfEratosthenesUWP_1.0.0.0_Test"
* Right-click the "Add-AppDevPackage.ps1" file and click "Run in Powershell"
* Follow any instructions provided by the prompt that appears, if necessary
* Once it indicates the app has been installed successfully, you should find the application in your Windows menu.

## Uninstall

You can uninstall the app by finding the app in your start menu, right clicking it, and selecting "Uninstall".

## Usage

* Click on the "Sieve Of Eratosthenes - UWP" tile to execute the application.
* As the instructions indicate, enter the range for which you which to see the sieve generate tiles in the "Min" and "Max" input fields, respectively.
* "Min" should never go below 2.
* "Max" should never go below "Min", or 2 if min is not specified.
* "Start" will start a timer that executes a step of the algorithm every 1 second.
* "Start" will be replaced by "Step" after it's clicked, you can use this to speed up the algorithm if you want.
* "Reset" will take all the primes generated, move them to the left, and stop the timer.
* "Quick Solve" lets the algorithm run at full speed, which should generate primes instantaneously, but there is no Toast notification generated for Quick Solve.
* While the algorithm is running (not with Quick Solve) the "Primes" list box should always scroll down to the last prime generated.
* There is a "Copy to Clipboard" button for the unresolved list and the primes list, and an "Export" button below it which will use the range specified.
* If "Min" or "Max" are left empty they default to 2 if Min, or whatever Min's value is if Max.
* There are LiveTile updates, so if you drag the app into the "Tiles" section of your start menu and click "Start", it will provide updates on the progress of the algorithm.
* The Toast Notification on completion of the algorithm should allow you to copy or save primes, just like in the app.


## Built With

* [.NET Framework 4.5](https://en.wikipedia.org/wiki/.NET_Framework) - The framework this application is built upon
* [Universal Windows Platform](https://en.wikipedia.org/wiki/Universal_Windows_Platform) - The application architecture employed
* [Visual Studio 2017](https://www.visualstudio.com/downloads/) - The IDE used to hold together and execute this code
* [NuGet](https://www.nuget.org/) - Dependency Management
* [Telerik UWP UI](https://github.com/telerik/UI-For-UWP) - Used for the input fields

## Contributing

At the moment this repository is private to prevent additional review from outside sources.

## Versioning

The current version is the initial release build (1.0.0.0).

## Authors

* **Douglas Jose Holmes** - [Repository located here](https://github.com/douglasjoseholmes/)

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

* Microsoft, for providing the framework, IDE, and help articles detailing the processes by which to use the IDE
* [Procore](https://www.procore.com/), for providing the inspiration and motivation for the project
