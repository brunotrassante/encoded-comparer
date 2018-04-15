# encoded-comparer

## Try it at
https://encoded-comparer.azurewebsites.net

## Using the API
- Documentaition is directly provided by Swagger

## Architecture Patterns and Decisions

The Architecture proposed tries to map the problem and business rules to rich entity classes that protects the manipulation of its proporties from external elements. Making sure you can't avoid passing throught every required rule before persisting your data.

Once we have those protected classes, that are always in a valid state, the necessity of a lighter path to retrive the itens from database becomes essential. Command and Query Responsibility Segregation comes to fix this problem. Creating different path to inputing data, using the rich domain classes, and to retrive data, with light DTOs.

Lastly, I used the notification pattern in the domain layer classes. Dealing with "expected" validetion errors without throwing exceptions. Preserving the server from the expensive interuptions and giving the final user, in just one massage, all errors he have to correct. 


In a nutshell:

- Anti-corruption Domain Entities
- CQRS
- Notification Pattern

> PS.: The solution proposed is obviously a huge over-architecture for the problem. But I intended to show some patterns I usually use when dealing with more complex real life problems.

## 3rd Party Tools  
- [Dapper](https://github.com/StackExchange/Dapper) for data access
- [Swagger](https://swagger.io/) for documentation
- [Azure Webapp](https://azure.microsoft.com/en-us/services/app-service/web/) to publish

## Next steps to improve  
- Central error logging tool (like [elmah](https://elmah.io/))
- APM tool to help monitoring  (like [new relic](https://newrelic.com/) or [Azure AppInsights](https://azure.microsoft.com/pt-br/services/application-insights/))
- Continuous Integration Server
- Static code validation (like [Sonarqube](https://www.sonarqube.org/))
- Performance and Load Test

## Cosidatations: 
- Some business rules were made up to give the Entities more things to validate, like the max Id lenth and data in Left and Right can oly be inserted once.
- Since a JSON can start with array, number, string, false, null or true since RFC 7159. I did not tried to validate in the base64 input if the JSON is valid. 
