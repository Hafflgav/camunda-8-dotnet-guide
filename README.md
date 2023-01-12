![Compatible with: Camunda Platform 8](https://img.shields.io/badge/Compatible%20with-Camunda%20Platform%208-0072Ce)
# DotNet Guide to Camunda 8
> This code was used in a blogpost. I recommend taking a look at the blogpost first before diving into this repository.
> 
> For the blogpost: [Click here!](https://camunda.com/blog/2022/11/camunda-platform-8-dotnet-developers/) 

### Useful information
- This project's target framework is **.Net Core 3.1**
- We are using [zeebe-client-csharp](https://github.com/camunda-community-hub/zeebe-client-csharp) as nuget package in our camunda-8-access-layer project
- You need to enroll for a C8 client to connect this client with Camunda 8. You can do so following [this link](https://accounts.cloud.camunda.io/signup). 
- Make sure to **update** the **Camunda 8 secrets** hen running the project on your own. This needs to be done in the `ZeebeClient` class. 

You can find these implementations in the project: 
1. [Process diagram](./DotNet-Camunda8-getting-started/Resources) 
2. [ZeebeClient implementation](./DotNet-Camunda8-getting-started/ZeebeClient.cs)
3. [Blood alcohol approximator](./blood-alcohol-approximator/BloodAlcoholApproximator.cs)
4. [Unit Tests](./test-bac-approximator/UnitTest.cs)