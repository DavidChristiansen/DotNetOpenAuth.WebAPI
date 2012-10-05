#DotNetOpenAuth.WebAPI.40
*DotNetOpenAuth for ASP.NET WebAPI (OpenID/OAuth/OAuth2)*


**Please note that this project is still in development and should be considered unstable-ish*


##Purpose

This project was developed to look into ways we can bring OAuth to the world of ASP.NET WebAPI.

##Details

I have included the following in the source

**/[DotNetOpenAuth.WebAPI](https://github.com/DavidChristiansen/DotNetOpenAuth.WebAPI.40/tree/master/source/DotNetOpenAuth.WebAPI)**

- Code required for the magic to happen

**/[DotNetOpenAuth.WebAPI.ClientSample.MVC3](https://github.com/DavidChristiansen/DotNetOpenAuth.WebAPI.40/tree/master/source/DotNetOpenAuth.WebAPI.ClientSample.MVC3)**

- A sample client demonstrating communicating with a WebAPI resource via JavaScript as well as C#. Simply check/uncheck the checkbox to switch between the client modes

**/[DotNetOpenAuth.WebAPI.HostSample](https://github.com/DavidChristiansen/DotNetOpenAuth.WebAPI.40/tree/master/source/DotNetOpenAuth.WebAPI.HostSample)**

- A sample authorisation server and WebAPI host utilising the functionality exposed in DotNetOpenAuth.WebAPI

###Getting Started

1. Launch the solution in visual studio
2. Start debugging both the client and host sample projects
3. Navigate to the Host sample (http://localhost:49810/)
4. Ensure you have SQL Express installed and running
5. Click the **Create Database** button
6. Navigate to the client sample (http://localhost:49907/)
7. Start clicking buttons and enjoy

##Feedback

As I mentioned earlier, this is still 'work in progress' but nevertheless a functional stub that I have put together. It has not been fully tested as yet. As always feedback is always welcome.

You can get me via either github, on the [DotNetOpenAuth forum](https://groups.google.com/forum/#!forum/dotnetopenid) or [dotnetopenauth@davedoes.net](mailto:dotnetopenauth@davedoes.net)