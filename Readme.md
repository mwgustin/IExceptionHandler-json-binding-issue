Json Payload Serialization doesn't obey ExceptionHandler when environment set to 'Production'.

Implementing a custom DefaultExceptionHandler, I noticed a discrepancy between how the ASP Json Bindings handle things in 'Development' vs 'Production'.


To run in 'Development' mode: 
```bash
dotnet run
```

To run in 'Production' mode:
```bash
dotnet run --launch-profile prod
```

The following results in the testing.http file show that in Development mode, the bad json makes it to the Exception Handler, but in Production dit does not. 

```bash
@HostAddress = http://localhost:5114

###
# Good message
# Returns 200 always
POST {{ HostAddress }}/test
Content-Type: application/json

{
  "message": "test message"
}

###
# Exception thrown inside the endpoint
# Returns 200 in Development and Production
GET {{ HostAddress }}/bad

###
# Bad json. Doesn't make it to the exception handler
# Returns 200 in Development.  Returns 400 in Production.
POST {{ HostAddress }}/test
Content-Type: application/json

"bad json"


```