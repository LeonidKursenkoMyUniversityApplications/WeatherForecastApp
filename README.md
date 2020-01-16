# WeatherForecastApp
A program that uses a developed component to demonstrate weather forecasts.

This is one of my practical work on the subject of component-oriented programming. As a result of the practical work, 
a program has been created that uses the developed component to demonstrate weather forecasts. 

This application has a multi-layer architecture. Weather data is stored in the 'weather' database, which must be hosted on the MSSQL server.
The database can be restored from the script file 'mssql_weather.sql', the connection term is specified in 'App.Config' in the main project
'PL.WeatherApp'. The database contains a table showing the paths to the image files and for the correct operation of the program they can
need correction. The data access layer to the data can handle the database through ADO.NET or Entity Framework.
The program uses an IoC container and MEF to write loosely linked code.

The program was created during my fourth-year studies in 2017.
