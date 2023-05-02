# Joel's Smartwyre Developer Test / SOLID Exercise

## Solution

Visual Studio 2022 .NET 7.0 Solution

### Projects

1. [Smartwyre.DeveloperTest](Smartwyre.DeveloperTest)
    * Library containing the business logic
1. [Smartwyre.DeveloperTest.Runner](Smartwyre.DeveloperTest.Runner)
    * Console application for running `Smartwyre.DeveloperTest`
1. [Smartwyre.DeveloperTest.Tests](Smartwyre.DeveloperTest.Tests)
    * Xunit test cases

## Requirements

> # Smartwyre Developer Test Instructions
> 
> In the 'RebateService.cs' file you will find a method for calculating a rebate. At a high level the steps for calculating a rebate are:
> 
>  1. Lookup the rebate that the request is being made against.
>  2. Lookup the product that the request is being made against.
>  2. Check that the rebate and request are valid to calculate the incentive type rebate.
>  3. Store the rebate calculation.
> 
> What we'd like you to do is refactor the code with the following things in mind:
> 
>  - Adherence to SOLID principles
>  - Testability
>  - Readability
>  - In the future we will add many more incentive types. Determining the incentive type should be made as easy and intuitive as possible for developers who will edit this in the future.
> 
> We’d also like you to 
>  - Add some unit tests to the Smartwyre.DeveloperTest.Tests project to show how you would test the code that you’ve produced 
>  - Run the RebateService from the Smartwyre.DeveloperTest.Runner console application accepting inputs
> 
> The only specific 'rules' are:
> 
> - The solution should build
> - The tests should all pass
> 
> You are free to use any frameworks/NuGet packages that you see fit. You should plan to spend around 1 hour completing the exercise.


Completed:

- [x] Met requirements
- [x] Prove functionality validity through unit tests
- [x] Illustrated SOLID principles


## SOLID principles in code

Principles:
* [`S`ingle responsibility](#single-responsibility-principle)
* [`O`pen closed](#open-closed-principle)
* [`L`iskov substitution](#liskov-substitution-principle)
* [`I`nterface segregation](#interface-segregation-principle)
* [`D`ependency inversion](#dependency-inversion-principle)

### Single responsibility principle

1. [RebateCalculatorFactory class](Smartwyre.DeveloperTest/Services/RebateCalculators/RebateCalculatorFactory.cs)
    * Only responsible for picking the Rebate Calculated for the supplied rebate and product
1. Rebate Calculator classes
    * Only responsible for calculating rebates
        * [AmountPerUom class](Smartwyre.DeveloperTest/Services/RebateCalculators/AmountPerUom.cs)
        * [FixedCashAmount class](Smartwyre.DeveloperTest/Services/RebateCalculators/FixedCashAmount.cs)
        * [FixedRate class](Smartwyre.DeveloperTest/Services/RebateCalculators/FixedRate.cs)
1. [CommandLine class](Smartwyre.DeveloperTest.Runner/CommandLine.cs)
    * Only responsible processing console application command line arguments and commands
1. [StartUp class](Smartwyre.DeveloperTest.Runner/StartUp.cs)
    * Only responsible initializing application as it starts


### Open closed principle


### Liskov substitution principle


### Interface segregation principle


### Dependency inversion principle


