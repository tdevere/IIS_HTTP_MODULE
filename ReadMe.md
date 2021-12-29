# Example Http Module for IIS
## Scenario: Avoid Account Lockout Under Basic Authentication

### Configuration 

* The IIS Application pool should be configured with Inegrated Pipeline. If not, the registration of the Http Module must be moved to system.Web element of web.config.
* IIS Authentication should be configured to use Basic Auth for this example
* Edit the Application Web.Configuration file to include the following
~~~
  <system.webServer>
	  <modules>
		  <add name="HttpModuleBasicAuthCheck" type="IIS_HTTP_MODULE.HttpModuleBasicAuthCheck"/>
	  </modules>
  </system.webServer>
~~~

* Add the complied module IIS_HTTP_MODULE.dll to the Application Bin folder for the Web Application
* Perform an IISReset

### Validation

* Use Fiddler to capture a trace when calling the web site
* Reissue and edit the request
* Modify the Basic Auth header value to avoid sending the password component in the new request. https://www.base64encode.org/
* The module is set to abort the request if the basic header lacks a password
* We don't validate it's correct, instead we Abort only when we lack the valid domain\username:password as a complete request.

