#DotNetOpenAuth.WebAPI
*DotNetOpenAuth for ASP.NET WebAPI (OpenID/OAuth/OAuth2)*

[![Build status](https://ci.appveyor.com/api/projects/status/71ev9i45h6ah4wcf?svg=true)](https://ci.appveyor.com/project/DavidChristiansen/dotnetopenauth-webapi)


##Purpose

This project was developed to look into ways we can bring OAuth to the world of ASP.NET WebAPI.

##Details

I have included the following in the source

**DotNetOpenAuth.WebAPI**

- Code required for the magic to happen

**DotNetOpenAuth.WebAPI.ClientSample**

- A sample client demonstrating communicating with a WebAPI resource via JavaScript as well as C#. Simply check/uncheck the checkbox to switch between the client modes

**DotNetOpenAuth.WebAPI.HostSample**

- A sample authorisation server and WebAPI host utilising the functionality exposed in DotNetOpenAuth.WebAPI

###Requirements

This sample uses Framework v4.5.1, MVC5 and requires SQL Express

###Getting Started

1. Launch the solution in visual studio
2. Start debugging both the client and host sample projects
3. Navigate to the Host sample (http://localhost:49810/)
4. Ensure you have SQL Express installed and running
5. Click the **Create Database** button
6. Navigate to the client sample (http://localhost:18529/)
7. Start clicking buttons and enjoy

##Feedback

As always feedback is always welcome.  Raise an issue if you have problems.
