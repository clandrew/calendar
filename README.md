# Calendar
Simple Windows Forms-based graphical calendar application for keeping track days of the week, with of short notes attached to the days.

Features:
* Robustly handles date and time constraints including leap years
* Fast to start up and shut down
* No browser or log in, small memory footprint and executable size
* Print support

**Important:** This application doesn't try to solve the problem of syncing your calendar data across locations or across devices. In fact it doesn't connect to the internet. It's up to the user to decide how to manage this.

Looks like this:

![Image](https://raw.githubusercontent.com/clandrew/calendar/main/Images/Screenshot.PNG "Image")

## Interface

On the calendar, today is highlighted in blue. The current day is queried from the system time.

**Previous Month:** Click this button to navigate to the previous month.

**Next Month:** Click this button to navigate to the next month.

**Add/Edit notes:** Right click on any cell in the calendar, choose Edit(...) in the context menu, and edit the text in the pop-up dialog. To confirm, click OK.


![Image](https://raw.githubusercontent.com/clandrew/calendar/main/Images/Screenshot2.PNG "Image")


![Image](https://raw.githubusercontent.com/clandrew/calendar/main/Images/Screenshot3.PNG "Image")

## Build
The project is in C#, built with Visual Studio 2019. The Any CPU (x64 or x86) build configuration is supported.

## System requirements
This application relies on .NET Framework 4.7.2. It has been tested on Windows 10 on an x86-64 based computer. It is likely to work versions of Windows 7 and later that have x86-64 application compatibility and support that .NET Framework version.
